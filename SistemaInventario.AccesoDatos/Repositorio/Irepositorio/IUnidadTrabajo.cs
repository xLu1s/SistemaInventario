﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio.Irepositorio
{
    public interface IUnidadTrabajo : IDisposable
    {

        IBodegaRepositorio Bodega { get; }
        ICategoriaRepositorio Categoria { get; }
        IMarcaRepositorio Marca { get; }
        IProductoRepositorio Producto { get; }
<<<<<<< HEAD
=======

        IUsuarioAplicacionRepositorio UsuarioAplicacion { get; }
>>>>>>> master
        Task Guardar();
    }
}
