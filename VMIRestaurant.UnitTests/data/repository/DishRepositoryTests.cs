using System.Collections.Generic;
using VMIRestaurant.data.repository;
using VMIRestaurant.domain.restaurant;
using Xunit;

namespace VMIRestaurant.UnitTests.data.repository
{
    public class DishRepositoryTests
    {
        [Fact]
        public void checks_by_name_if_entity_exists()
        {
            var dishes = new List<Dish> {new Dish {Id=0, Name = "One"}, new Dish {Id=1, Name = "Two"}};
            
            var dishRepository = new DishRepository(dishes);
            
            Assert.True(dishRepository.ExistsByName("One"));
            Assert.False(dishRepository.ExistsByName("Three"));
        }
    }
}