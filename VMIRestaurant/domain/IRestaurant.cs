using System.Collections.Generic;
using VMIRestaurant.domain.restaurant;

namespace VMIRestaurant.domain
{
    public interface IRestaurant
    {
        /// <summary>
        /// prepares dish or multiple dishes for an order
        /// </summary>
        /// <param name="order">order to be processed</param>
        /// <returns>int: order id</returns>
        /// <returns>list of prepared dishes</returns>
        (int, IList<Dish>) ProcessOrder(Order order);
        double GetIngredientStock(int ingredientId);
        void ClearIngredientStock(string ingredientName);
        void AddIngredient(Ingredient ingredient);
        void AddDish(Dish dish);
    }
}