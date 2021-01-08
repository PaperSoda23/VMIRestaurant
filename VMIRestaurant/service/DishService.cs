using System;
using System.Collections.Generic;
using System.Linq;
using VMIRestaurant.data.repository.@interface;
using VMIRestaurant.domain.restaurant;

namespace VMIRestaurant.service
{
    public class DishService
    {
        private readonly IDishRepository _dishRepository;
        private readonly IIngredientRepository _ingredientRepository;

        public DishService(IDishRepository dishRepository, IIngredientRepository ingredientRepository)
        {
            _dishRepository = dishRepository;
            _ingredientRepository = ingredientRepository;
        }
        /// <summary>
        /// adds a dish to the menu of a restaurant
        /// 'add all ingredients when dish is added'? -> dish can't be added if all ingredients it requires does not exist
        /// 'recipe can only have one portion of an ingredient'
        /// </summary>
        /// <param name="dish">dish to be added</param>
        /// <exception cref="ArgumentException">dish with a name already exists or when not all dish ingredients exist for a dish</exception>
        public void AddDish(Dish dish)
        {
            var allDishIngredientsExist = new Func<Dish, bool>(d => 
                d.IngredientIds
                    .Select(ingredientId => ingredientId)
                    .All(ingredientId => _ingredientRepository.Exists(ingredientId)));

            if (!allDishIngredientsExist(dish))
                throw new ArgumentException($"not all dish ingredients exist in a {dish}");

            if (_dishRepository.ExistsByName(dish.Name))
                throw new ArgumentException($"dish with name '{dish.Name}' already exists");
            
            dish.IngredientIds = dish.IngredientIds.Distinct().ToList(); // 'recipe can only have one portion of an ingredient'

            _dishRepository.Add(dish);
        }

        public IEnumerable<Ingredient> GetDishIngredients(Dish dish)
        {
            return dish.IngredientIds
                .Select(ingredientId => _ingredientRepository
                    .FindById(ingredientId));
        }
    }
}