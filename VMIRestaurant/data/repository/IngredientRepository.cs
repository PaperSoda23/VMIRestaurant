using System.Collections.Generic;
using System.Linq;
using VMIRestaurant.data.repository.@interface;
using VMIRestaurant.domain.restaurant;

namespace VMIRestaurant.data.repository
{
    public class IngredientRepository : InMemoryEntityRepository<Ingredient>, IIngredientRepository
    {
        public IngredientRepository(IEnumerable<Ingredient> entities) : base(entities) { }
        
        public bool ExistsByName(string ingredientName)
        {
            return Entities
                .FirstOrDefault(e => e.Value.Name.Equals(ingredientName))
                .Value != null;
        }

        public Ingredient FindByName(string ingredientName)
        {
            return Entities.FirstOrDefault(e => e.Value.Name.Equals(ingredientName)).Value;
        }
    }
}