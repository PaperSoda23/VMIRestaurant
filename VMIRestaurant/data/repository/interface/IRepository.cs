using System.Collections.Generic;
using VMIRestaurant.common;

namespace VMIRestaurant.data.repository.@interface
{
    public interface IRepository<T> where T : IEntity
    {
        List<T> GetAll();
        T FindById(int id);
        T Add(T entity);
        T Update(int id, T entity);
        T Delete(int id);
        bool Exists(int id);
    }
}