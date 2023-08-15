using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant.Models.Entities;
using System.Collections.ObjectModel;

namespace Restaurant.Models.Repositories
{
    public class RepositoryBase<T> where T : BaseEntity
    {
        protected readonly AppDbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public RepositoryBase(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        public T GetById(int id)
        {
            return _dbSet.FirstOrDefault(entity => entity.Id == id);
        }

        public void Insert(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        /// <summary>
        ///     This method will actually remove the row from the database.
        /// </summary>
        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public List<T> GetAll()
        {
            return GetRecords().ToList();
        }

        public bool Any(Func<T, bool> expression)
        {
            return GetRecords().Any(expression);
        }

        protected IQueryable<T> GetRecords()
        {
            return _dbSet.AsQueryable<T>();
        }

        public ObservableCollection<T> ListToObsCollection(List<T> listObjects)
        {
            ObservableCollection<T> result = new ObservableCollection<T>();
            foreach (var obj in listObjects)
            {
                result.Add(obj);
            }
            return result;
        }
    }
}
