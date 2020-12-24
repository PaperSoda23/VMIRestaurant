using System.Collections.Generic;
using VMIRestaurant.data.csv.mapper;
using VMIRestaurant.domain.restaurant;
using Xunit;

namespace VMIRestaurant.UnitTests.data.csv.mapper
{
    public class DishMapperTests
    {
        private readonly DishMapper _dishMapper;
        
        public DishMapperTests()
        {
            _dishMapper = new DishMapper();
        }

        [Fact]
        public void maps_dish()
        {
            var data = new List<string> {"1", "name", "1 2 3"};

            var dish = (Dish) _dishMapper.MapToEntity(data);
            
            Assert.Equal(1, dish.Id);
            Assert.Equal("name", dish.Name);
            Assert.Equal(new List<int> {1, 2, 3}, dish.IngredientIds);
        }
    }
}