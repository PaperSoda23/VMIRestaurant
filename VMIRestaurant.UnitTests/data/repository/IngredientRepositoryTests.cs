using System.Collections.Generic;
using VMIRestaurant.data.repository;
using VMIRestaurant.domain.restaurant;
using Xunit;

namespace VMIRestaurant.UnitTests.data.repository
{
    public class IngredientRepositoryTests
    {
        [Fact]
        public void checks_by_name_if_ingredient_exists()
        {
            var ingredients = new List<Ingredient> {new Ingredient {Id=0, Name = "One"}};
            
            var ingredientRepository = new IngredientRepository(ingredients);
            
            Assert.True(ingredientRepository.ExistsByName("One"));
            Assert.False(ingredientRepository.ExistsByName("Three"));
        }

        [Fact]
        public void gets_ingredient_by_name()
        {
            var ingredients = new List<Ingredient> {new Ingredient {Id=0, Name = "ExistingName"}};
            
            var ingredientRepository = new IngredientRepository(ingredients);
            
            Assert.Equal(ingredientRepository.FindByName("ExistingName"), ingredients[0]);
            Assert.Null(ingredientRepository.FindByName("NotExitingName"));
        }
    }
}
