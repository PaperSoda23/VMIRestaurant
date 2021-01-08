using System;
using System.Collections.Generic;
using System.Linq;
using VMIRestaurant.data.repository.@interface;
using VMIRestaurant.domain.restaurant;

namespace VMIRestaurant.service
{
    public class IngredientService
    {
        private readonly IIngredientRepository _ingredientRepository;

        public IngredientService(IIngredientRepository ingredientRepository)
        {
            _ingredientRepository = ingredientRepository;
        }
        
        public double GetIngredientStock(int ingredientId)
        {
            return _ingredientRepository.FindById(ingredientId).Stock;
        }

        public bool AllIngredientsExist(IEnumerable<Ingredient> ingredients)
        {
            return ingredients
                .All(ingredient => _ingredientRepository.Exists(ingredient.Id));
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
    }
}