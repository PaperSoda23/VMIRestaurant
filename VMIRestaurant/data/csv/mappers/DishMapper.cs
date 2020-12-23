using System.Collections.Generic;
using System.Linq;
using VMIRestaurant.common;
using VMIRestaurant.domain.restaurant;

namespace VMIRestaurant.data.csv.mappers
{
    public class DishMapper : ICsvEntityMapper
    {
        public IEntity MapToEntity(IEnumerable<string> dataFrom)
        {
            
            var enumerable = dataFrom.ToList();

            return new Dish
            {
                Id = int.Parse(enumerable[0]),
                Name = enumerable[1],
                IngredientIds = enumerable[2].Split(" ").Select(int.Parse).ToList()
            };
        }
    }
}