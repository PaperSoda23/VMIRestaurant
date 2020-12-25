using System;
using System.Collections.Generic;
using System.Linq;
using VMIRestaurant.data.repository.@interface;
using VMIRestaurant.domain.restaurant;

namespace VMIRestaurant.domain
{
    public class Restaurant : IRestaurant
    {
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IDishRepository _dishRepository;
        
        public Restaurant(IIngredientRepository ingredientRepository, IDishRepository dishRepository)
        {
            _ingredientRepository = ingredientRepository;
            _dishRepository = dishRepository;
        }

        public (int, List<Dish>) ProcessOrder(Order order)
        {
            if (!CanProcessWholeOrder(order))
                return (order.Id, new List<Dish>());

            var dishes = PrepareDishes(order);

            return (order.Id, dishes);
        }

        private bool CanProcessWholeOrder(Order order)
        {
            var orderIngredients = GetOrderIngredients(order);
            
            bool allIngredientsForOrderExist = AllIngredientsExist(orderIngredients);
            bool hasSufficientIngredientStock = HasSufficientIngredientStock(orderIngredients);
            
            return allIngredientsForOrderExist && hasSufficientIngredientStock;
        }

        private IList<Ingredient> GetOrderIngredients(Order order)
        {
            return order.DishIds
                .Select(dishId => _dishRepository.FindById(dishId))
                .SelectMany(dish => dish.IngredientIds)
                .Select(ingredientId => _ingredientRepository.FindById(ingredientId))
                .ToList();
        }

        private static bool HasSufficientIngredientStock(IEnumerable<Ingredient> ingredients)
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

        private List<Dish> PrepareDishes(Order order)
        {
            var dishes = GetOrderDishes(order);
            
            foreach (var dish in dishes)
                DecreaseStock(dish);

            return dishes;
        }
        
        private void DecreaseStock(Dish dish)
        {
            var ingredients = GetDishIngredients(dish);

            foreach (var ingredient in ingredients)
                if (ingredient.Stock - ingredient.AmountInPortion >= 0)
                    ingredient.Stock = Math.Round(ingredient.Stock - ingredient.AmountInPortion, 2);
                else 
                    throw new Exception($"whole order can't be processed");
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

        public void AddIngredient(Ingredient ingredient)
        {
            if (!_ingredientRepository.ExistsByName(ingredient.Name))
            {
                _ingredientRepository.Add(ingredient);
                return;
            }

            var existingIngredient = _ingredientRepository.FindByName(ingredient.Name);

            ingredient.Id = existingIngredient.Id;
            ingredient.Stock += existingIngredient.Stock;            

            _ingredientRepository.Update(existingIngredient.Id, ingredient);
        }

        public void ClearIngredientStock(string ingredientName)
        {
            // ingredient can't be deleted because dish may require it
            // ingredient stock can only be zeroed (stock -> 0)
            if (!_ingredientRepository.ExistsByName(ingredientName)) 
                throw new ArgumentException($"ingredient {ingredientName} does not exist");
                
            var existingIngredient = _ingredientRepository.FindByName(ingredientName);

            existingIngredient.Stock = 0;

            _ingredientRepository.Update(existingIngredient.Id, existingIngredient);
        }
        
        private bool AllIngredientsExist(IEnumerable<Ingredient> ingredients) => ingredients
            .All(ingredient => _ingredientRepository.Exists(ingredient.Id));
        
        private List<Dish> GetOrderDishes(Order order) => order
            .DishIds.Select(dishId => _dishRepository.FindById(dishId)).ToList();
        
        private IEnumerable<Ingredient> GetDishIngredients(Dish dish) => dish
            .IngredientIds.Select(ingredientId => _ingredientRepository.FindById(ingredientId));
        
        public double GetIngredientStock(int ingredientId) => _ingredientRepository.FindById(ingredientId).Stock;
    }
}
