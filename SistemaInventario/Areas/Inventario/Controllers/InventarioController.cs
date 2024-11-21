using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repositorio.Irepositorio;
using SistemaInventario.Modelos.ViewModels;
using SistemaInventario.Utilidades;
using System.Security.Claims;

namespace SistemaInventario.Areas.Inventario.Controllers
{
    [Area("Inventario")]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Inventario)]
    public class InventarioController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        [BindProperty]
        public InventarioVM InventarioVM { get; set; }

        public InventarioController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NuevoInventario()
        {
            InventarioVM = new InventarioVM()
            {
                Inventario = new Modelos.Inventario(),
                BodegaLista = _unidadTrabajo.Inventario.ObtenerTodosDropdownList("Bodega")
            };

            InventarioVM.Inventario.Estado = false;
            //Obtener el Id del usuario desde la sesión
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            InventarioVM.Inventario.UsuarioAplicacionId = claim.Value;
            InventarioVM.Inventario.FechaInicial = DateTime.Now;
            InventarioVM.Inventario.FechaFinal = DateTime.Now;

            return View(InventarioVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NuevoInventario(InventarioVM inventarioVM)
        {
            if (ModelState.IsValid)
            {
                inventarioVM.Inventario.FechaInicial = DateTime.Now;
                inventarioVM.Inventario.FechaFinal = DateTime.Now;
                await _unidadTrabajo.Inventario.Agregar(inventarioVM.Inventario);
                await _unidadTrabajo.Guardar();
                return RedirectToAction("DetalleInventario", new { id = inventarioVM.Inventario.Id });
            }
            inventarioVM.BodegaLista = _unidadTrabajo.Inventario.ObtenerTodosDropdownList("Bodega");
            return View(inventarioVM);
        }

        public async Task<IActionResult> DetalleInventario(int id)
        {
            InventarioVM = new InventarioVM();
            InventarioVM.Inventario = await _unidadTrabajo.Inventario.ObtenerPrimero(i => i.Id == id, incluirPropiedades: "Bodega");
            InventarioVM.inventarioDetalles = await _unidadTrabajo.InventarioDetalle.ObtenerTodos(d => d.InventarioId == id,
                                                                                                incluirPropiedades: "Producto,Producto.Marca");
            return View(InventarioVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DetalleInventario(int InventarioId, int productoId, int cantidadId)
        {
            InventarioVM = new InventarioVM();
            InventarioVM.Inventario = await _unidadTrabajo.Inventario.ObtenerPrimero(i => i.Id == InventarioId);
            var bodegaProducto = await _unidadTrabajo.BodegaProducto.ObtenerPrimero(b => b.ProductoId == productoId &&
                                                                                    b.BodegaId == InventarioVM.Inventario.BodegaId);
            var detalle = await _unidadTrabajo.InventarioDetalle.ObtenerPrimero(d => d.InventarioId ==  InventarioId && 
                                                                                     d.ProductoId == productoId);

            if (detalle == null)
            {
                InventarioVM.InventarioDetalle = new Modelos.InventarioDetalle();
                InventarioVM.InventarioDetalle.ProductoId = productoId;
                InventarioVM.InventarioDetalle.InventarioId = InventarioId;
                if (bodegaProducto != null)
                {
                    InventarioVM.InventarioDetalle.StockAnterior = bodegaProducto.Cantidad;

                }
                else
                {
                    InventarioVM.InventarioDetalle.StockAnterior = 0;
                }
                InventarioVM.InventarioDetalle.Cantidad = cantidadId;
                await _unidadTrabajo.InventarioDetalle.Agregar(InventarioVM.InventarioDetalle);
                await _unidadTrabajo.Guardar();
            }
            else
            {
                detalle.Cantidad += cantidadId;
                await _unidadTrabajo.Guardar();
            }
            return RedirectToAction("DetalleInvetario", new { id = InventarioId });
                                                                                    
        }

        public async Task<IActionResult> Mas(int id)
        {
            InventarioVM = new InventarioVM();
            var detalle = await _unidadTrabajo.InventarioDetalle.Obtener(id);
            InventarioVM.Inventario = await _unidadTrabajo.Inventario.Obtener(detalle.InventarioId);

            detalle.Cantidad++;
            await _unidadTrabajo.Guardar();
            return RedirectToAction("DetalleInventario",new {id = InventarioVM.Inventario.Id});
        }
        public async Task<IActionResult> Menos(int id)
        {
            InventarioVM = new InventarioVM();
            var detalle = await _unidadTrabajo.InventarioDetalle.Obtener(id);
            InventarioVM.Inventario = await _unidadTrabajo.Inventario.Obtener(detalle.InventarioId);
            if (detalle.Cantidad == 1)
            {
                _unidadTrabajo.InventarioDetalle.Remover(detalle);
                await _unidadTrabajo.Guardar();
            }
            else
            { 
                detalle.Cantidad--;
                await _unidadTrabajo.Guardar();
            }
            return RedirectToAction("DetalleInventario", new { id = InventarioVM.Inventario.Id });
            
        }


        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodo()
        {
            var todos = await _unidadTrabajo.BodegaProducto.ObtenerTodos(incluirPropiedades: "Bodega,Producto");
            return Json(new { data = todos });
        }

        [HttpGet]
        public async Task<IActionResult> BuscarProducto(string term)
        {
            if (!String.IsNullOrEmpty(term))
            {
                var listaProductos = await _unidadTrabajo.Producto.ObtenerTodos(p => p.Estado == true);
                var data = listaProductos.Where(x => x.NumeroSerie.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                                                     x.Descripcion.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList();
                return Ok(data);
            }
            return Ok();
        }

        #endregion
    }
}
