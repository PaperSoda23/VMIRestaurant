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
            // 'recipe can only have one portion of an ingredient'
            dish.IngredientIds = dish.IngredientIds.Distinct().ToList();
            // 'add all ingredients when dish is added'?
            // dish can't be added if all ingredients it requires does not exist
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