using System.Collections.Generic;
using VMIRestaurant.common;

namespace VMIRestaurant.domain.restaurant
{
    public class Dish : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> IngredientIds { get; set; }

        public override string ToString() => $"[Dish]=Id:{Id} Name:{Name} Ingredients:[{string.Join(",", IngredientIds)}]";
    }
}