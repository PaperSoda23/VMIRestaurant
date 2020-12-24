using System;
using System.Collections.Generic;
using System.Linq;
using VMIRestaurant.common;
using VMIRestaurant.data.repository.@interface;

namespace VMIRestaurant.data.repository
{
    public abstract class InMemoryEntityRepository<T> : IRepository<T> where T : IEntity
    {
        protected readonly Dictionary<int, T> Entities;
        
        protected InMemoryEntityRepository(IEnumerable<T> entities)
        {
            Entities = entities.ToDictionary(i => i.Id, i => i);
        }

        public List<T> GetAll() => Entities.Values.ToList();

        public T FindById(int id)
        {
            if (Entities.ContainsKey(id))
                return Entities[id];
            throw new ArgumentException($"entity of type ${typeof(T)} with id {id} not found");
        }

        public T Add(T entity)
        {
            if (Entities.ContainsKey(entity.Id))
                throw new ArgumentException($"entity with id {entity.Id} already exists");

            var maxCurrentId = 0;
            if (Entities.Count != 0) 
                maxCurrentId = Entities.Max(e => e.Key);

            entity.Id = maxCurrentId + 1;
            Entities.Add(entity.Id, entity);
            
            return entity;
        }

        public T Update(int id, T entity)
        {
            FindById(id);
            
            Entities[id] = entity;
            
            return entity;
        }

        public T Delete(int id)
        {
            var entity = FindById(id);
            
            Entities.Remove(id);
            
            return entity;
        }

        public bool Exists(int id) => Entities.ContainsKey(id);
    }
}