using System;
using System.Collections.Generic;
using System.Linq;
using VMIRestaurant.domain.restaurant;
using VMIRestaurant.service;

namespace VMIRestaurant.domain
{
    public class Restaurant : IRestaurant
    {
        private readonly DishService _dishService;
        private readonly OrderService _orderService;
        private readonly IngredientService _ingredientService;
        
        public Restaurant(DishService dishService, OrderService orderService, IngredientService ingredientService)
        {
            _dishService = dishService;
            _orderService = orderService;
            _ingredientService = ingredientService;
        }

        public (int, IList<Dish>) ProcessOrder(Order order)
        {
            if (!CanProcessWholeOrder(order))
            {
                return (order.Id, new List<Dish>());
            }
            
            var dishes = PrepareDishes(order);

            return (order.Id, dishes);
        }
        
        private bool CanProcessWholeOrder(Order order)
        {
            var orderIngredients = _orderService.GetOrderIngredients(order);
            
            bool allIngredientsForOrderExist = _ingredientService.AllIngredientsExist(orderIngredients);
            bool hasSufficientIngredientStock = HasSufficientIngredientStockToProcessOrder(orderIngredients);
            
            return allIngredientsForOrderExist && hasSufficientIngredientStock;
        }
        /// <summary>
        /// checks if there will be enough ingredients to fulfill whole order
        /// </summary>
        /// <param name="ingredients"></param>
        /// <returns>true if enough ingredients in stock to make every dish in order, else false</returns>
        private static bool HasSufficientIngredientStockToProcessOrder(IEnumerable<Ingredient> ingredients)
        {
            return ingredients
                .GroupBy(ingredient => ingredient.Id)
                .Select(ingredientGroup => new
                {
                    Id = ingredientGroup.Key,
                    Ingredient = ingredientGroup.Select(e => e).First(),
                    TotalAmount = ingredientGroup.Sum(b => b.AmountInPortion)
                })
                .All(e => e.Ingredient.Stock >= e.TotalAmount);
        }

        private IList<Dish> PrepareDishes(Order order)
        {
            var dishes = _orderService.GetOrderDishes(order);

            foreach (var dish in dishes)
            {
                DecreaseStock(dish);
            }

            return dishes;
        }
        /// <summary>
        /// decreases ingredient stock when making a dish
        /// </summary>
        /// <param name="dish">dish to be made</param>
        /// <exception cref="Exception">when order decreased order stock is less than 0</exception>
        private void DecreaseStock(Dish dish)
        {
            var ingredients = _dishService.GetDishIngredients(dish);

            foreach (var ingredient in ingredients)
            {
                var ingredientStockAfterDecrease = Math.Round(ingredient.Stock - ingredient.AmountInPortion, 2);

                if (ingredientStockAfterDecrease < 0)
                {
                    throw new Exception($"whole order can't be processed");
                }
                
                ingredient.Stock = ingredientStockAfterDecrease;
            }
        }
        
        public double GetIngredientStock(int ingredientId) => _ingredientService.GetIngredientStock(ingredientId);

        public void ClearIngredientStock(string ingredientName) =>
            _ingredientService.ClearIngredientStock(ingredientName);

        public void AddIngredient(Ingredient ingredient) => _ingredientService.AddIngredient(ingredient);

        public void AddDish(Dish dish) => _dishService.AddDish(dish);
    }
}
