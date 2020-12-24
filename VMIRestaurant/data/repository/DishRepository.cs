using System.Collections.Generic;
using System.Linq;
using VMIRestaurant.data.repository.@interface;
using VMIRestaurant.domain.restaurant;

namespace VMIRestaurant.data.repository
{
    public class DishRepository : InMemoryEntityRepository<Dish>, IDishRepository
    {
        public DishRepository(IEnumerable<Dish> entities) : base(entities) { }
        
        public bool ExistsByName(string ingredientName)
        {
            return Entities
                .FirstOrDefault(e => e.Value.Name.Equals(ingredientName))
                .Value != null;
        }
    }
}