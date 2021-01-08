using System.Collections.Generic;
using VMIRestaurant.domain.restaurant;

namespace VMIRestaurant.domain
{
    public interface IRestaurant
    {
        (int, IList<Dish>) ProcessOrder(Order order);
        double GetIngredientStock(int ingredientId);
        void ClearIngredientStock(string ingredientName);
        void AddIngredient(Ingredient ingredient);
        void AddDish(Dish dish);
    }
}