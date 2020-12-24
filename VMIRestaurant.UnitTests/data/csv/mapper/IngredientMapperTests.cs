using System.Collections.Generic;
using VMIRestaurant.data.csv.mapper;
using VMIRestaurant.domain.restaurant;
using Xunit;

namespace VMIRestaurant.UnitTests.data.csv.mapper
{
    public class IngredientMapperTests
    {
        private readonly IngredientMapper _ingredientMapper;
        
        public IngredientMapperTests()
        {
            _ingredientMapper = new IngredientMapper();
        }

        [Fact]
        public void maps_ingredient()
        {
            var data = new List<string> {"1", "name", "10", "kg", "0.3"};
            
            var ingredient = (Ingredient) _ingredientMapper.MapToEntity(data);
            
            Assert.Equal(1, ingredient.Id);
            Assert.Equal("name", ingredient.Name);
            Assert.Equal(10, ingredient.Stock);
            Assert.Equal(UnitMeasure.Kg, ingredient.UnitMeasure);
            Assert.Equal(0.3, ingredient.AmountInPortion);
        }
    }
}