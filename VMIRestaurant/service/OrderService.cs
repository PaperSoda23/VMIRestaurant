using System.Collections.Generic;
using System.Linq;
using VMIRestaurant.data.repository.@interface;
using VMIRestaurant.domain.restaurant;

namespace VMIRestaurant.service
{
    public class OrderService
    {
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IDishRepository _dishRepository;

        public OrderService(IIngredientRepository ingredientRepository, IDishRepository dishRepository)
        {
            _ingredientRepository = ingredientRepository;
            _dishRepository = dishRepository;
        }

        public IList<Ingredient> GetOrderIngredients(in Order order)
        {
            return order.DishIds
                .Select(dishId => _dishRepository.FindById(dishId))
                .SelectMany(dish => dish.IngredientIds)
                .Select(ingredientId => _ingredientRepository.FindById(ingredientId))
                .ToList();
        }

        public IList<Dish> GetOrderDishes(in Order order)
        {
            return order.DishIds
                .Select(dishId => _dishRepository
                    .FindById(dishId))
                .ToList();
        } 
    }
}