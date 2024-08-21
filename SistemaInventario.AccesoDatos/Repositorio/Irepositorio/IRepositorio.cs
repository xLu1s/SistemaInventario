﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio.Irepositorio
{
    public interface IRepositorio<T> where T : class
    {
        Task<T> Obtener(int id);

        Task<IEnumerable<T>> ObtenerTodos(
            Expression<Func<T,bool>> filtro = null,
            Func<IQueryable<T>,IOrderedQueryable> orderBy = null,
            string incluirPropiedades = null,
            bool isTracking = true
            );

        Task<T> ObtenerPrimero(Expression<Func<T, bool>> filtro = null,
            string incluirPropiedades = null,
            bool isTracking = true);

        Task Agregar(T entidad);
        void RemoverRango(IEnumerable<T> entidad);
        void Remover(T entidad);
    }
}
