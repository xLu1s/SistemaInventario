using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repositorio.Irepositorio;
using SistemaInventario.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    public class ProductoRepositorio : Repositorio<Producto>, IProductoRepositorio
    {
        private readonly ApplicationDbContext _db;

        public ProductoRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Producto producto)
        {
            var ProductoBD = _db.Productos.FirstOrDefault(b => b.Id == producto.Id);
            if (ProductoBD != null)
            {
                if(producto.ImagenUrl != null)
                {
                    ProductoBD.ImagenUrl = producto.ImagenUrl;
                }
                ProductoBD.NumeroSerie = producto.NumeroSerie;
                ProductoBD.Descripcion = producto.Descripcion;
                ProductoBD.Estado = producto.Estado;
                ProductoBD.Precio = producto.Precio;
                ProductoBD.Coste = producto.Coste;
                ProductoBD.CategoriaId = producto.CategoriaId;
                ProductoBD.MarcaId = producto.MarcaId;
                ProductoBD.PadreId = producto.PadreId;
                _db.SaveChanges();
            }
        }

        public IEnumerable<SelectListItem> ObtenerTodosDropdownLista(string obj)
        {
            if (obj == "Categoria")
            {
                return _db.Categorias.Where(c => c.Estado == true).Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString()
                });
            }
            if (obj == "Marca")
            {
                return _db.Marcas.Where(c => c.Estado == true).Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString()
                });
            }
            if (obj == "Producto")
            {
                return _db.Productos.Where(c => c.Estado == true).Select(c => new SelectListItem
                {
                    Text = c.Descripcion,
                    Value = c.Id.ToString()
                });
            }
            return null;
        }

    }
}
