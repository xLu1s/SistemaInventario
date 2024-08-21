using Microsoft.EntityFrameworkCore;
using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repositorio.Irepositorio;
<<<<<<< HEAD
=======
using SistemaInventario.Modelos.Especificaciones;
>>>>>>> master
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public Repositorio(ApplicationDbContext Db)
        {
            _db = Db;
            this.dbSet = _db.Set<T>();
        }
        public async Task Agregar(T entidad)
        {
            await dbSet.AddAsync(entidad); //insert into Table

        }


        public async Task<T> Obtener(int id)
        {
            return await dbSet.FindAsync(id); //select * from Table where id = id
        }

        public async Task<T> ObtenerPrimero(Expression<Func<T, bool>> filtro = null, string incluirPropiedades = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;
            if (filtro != null)
            {
                query = query.Where(filtro); //select * from Table where filtro
            }
            if (incluirPropiedades != null)
            {
                foreach (var propiedad in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(propiedad); //select * from Table inner join propiedad
                }
            }
            if (!isTracking)
            {
                query = query.AsNoTracking(); //select * from Table without tracking
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> ObtenerTodos(Expression<Func<T, bool>> filtro = null, Func<IQueryable<T>, IOrderedQueryable> orderBy = null, string incluirPropiedades = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;
            if(filtro != null)
            {
                query = query.Where(filtro); //select * from Table where filtro
            }
            if(incluirPropiedades != null)
            {
                foreach(var propiedad in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(propiedad); //select * from Table inner join propiedad
                }
            }
            if(orderBy != null)
            {
                query = (IQueryable<T>)orderBy(query); //select * from Table order by
            }
            if(!isTracking)
            {
                query = query.AsNoTracking(); //select * from Table without tracking
            }
            return await query.ToListAsync();
        }

<<<<<<< HEAD
=======
        public PagedList<T> ObtenerTodosPaginado(Parametros parametros, Expression<Func<T, bool>> filtro = null, Func<IQueryable<T>, IOrderedQueryable> orderBy = null, string incluirPropiedades = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;
            if (filtro != null)
            {
                query = query.Where(filtro); //select * from Table where filtro
            }
            if (incluirPropiedades != null)
            {
                foreach (var propiedad in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(propiedad); //select * from Table inner join propiedad
                }
            }
            if (orderBy != null)
            {
                query = (IQueryable<T>)orderBy(query); //select * from Table order by
            }
            if (!isTracking)
            {
                query = query.AsNoTracking(); //select * from Table without tracking
            }
            return PagedList<T>.ToPagedList(query, parametros.PageNumber, parametros.PageSize);
        }

>>>>>>> master
        public void Remover(T entidad)
        {
            dbSet.Remove(entidad); //delete from Table where id = id
        }

        public void RemoverRango(IEnumerable<T> entidad)
        {
            dbSet.RemoveRange(entidad); //delete from Table where id in (id1, id2, id3)
        }
    }
}
