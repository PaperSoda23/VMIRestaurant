using System;
using System.Collections.Generic;
using Moq;
using VMIRestaurant.data.repository.@interface;
using VMIRestaurant.domain;
using VMIRestaurant.domain.restaurant;
using Xunit;

namespace VMIRestaurant.UnitTests.domain
{
    public class RestaurantTests
    {
        private readonly Restaurant _restaurant;
        private readonly Mock<IDishRepository> _mockDishRepository =  new Mock<IDishRepository>();
        private readonly Mock<IIngredientRepository> _mockIngredientRepository =  new Mock<IIngredientRepository>();
        
        public RestaurantTests()
        {
            _restaurant = new Restaurant(_mockIngredientRepository.Object, _mockDishRepository.Object);
        }
        
        [Fact]
        public void processes_single_order()
        {
            var dish = new Dish {Id = 1, Name = "dish", IngredientIds = new List<int>{1, 2, 3}};
            var order = new Order {Id = 1, Date = DateTime.Parse("2021-01-01 00:00:00"), DishIds = new List<int> {1}};
            var ingredients = new List<Ingredient>
            {
                new Ingredient {Id = 1, Name = "I1", Stock = 10, AmountInPortion = 0.3, UnitMeasure = UnitMeasure.Kg},
                new Ingredient {Id = 2, Name = "I2", Stock = 7, AmountInPortion = 0.3, UnitMeasure = UnitMeasure.Kg},
                new Ingredient {Id = 3, Name = "I3", Stock = 5, AmountInPortion = 0.3, UnitMeasure = UnitMeasure.Kg}
            };
            _mockDishRepository.Setup(m => m.FindById(1)).Returns(dish);
            _mockIngredientRepository.Setup(m => m.FindById(1)).Returns(ingredients[0]);
            _mockIngredientRepository.Setup(m => m.FindById(2)).Returns(ingredients[1]);
            _mockIngredientRepository.Setup(m => m.FindById(3)).Returns(ingredients[2]);
            _mockIngredientRepository.Setup(m => m.Exists(1)).Returns(true);
            _mockIngredientRepository.Setup(m => m.Exists(2)).Returns(true);
            _mockIngredientRepository.Setup(m => m.Exists(3)).Returns(true);

            var (_, madeDishes) = _restaurant.ProcessOrder(order);
            
            Assert.Single(madeDishes);
            Assert.Equal(dish.Name, madeDishes[0].Name);
            Assert.Equal(dish.IngredientIds, madeDishes[0].IngredientIds);
            Assert.Equal(9.7, _restaurant.GetIngredientStock(1));
            Assert.Equal(6.7, _restaurant.GetIngredientStock(2));
            // Assert.NotSame(dishes[0], madeDishes[0]); should not expose inner ref to client
        }

        [Fact]
        public void processes_multiple_orders()
        {
            var dishes = new List<Dish> {new Dish {Id = 1, Name = "dish", IngredientIds = new List<int>{1, 1, 2}}};
            var orders = new List<Order>
            {
                new Order {Id = 1, Date = DateTime.Parse("2021-01-01 00:00:00"), DishIds = new List<int> {1}},
                new Order {Id = 2, Date = DateTime.Parse("2021-01-01 00:00:00"), DishIds = new List<int> {1}}
            };
            var ingredients = new List<Ingredient>
            {
                new Ingredient {Id = 1, Name = "I1", Stock = 10, AmountInPortion = 0.3, UnitMeasure = UnitMeasure.Kg},
                new Ingredient {Id = 2, Name = "I2", Stock = 7, AmountInPortion = 0.3, UnitMeasure = UnitMeasure.Kg}
            };
            _mockDishRepository.Setup(m => m.FindById(1)).Returns(dishes[0]);
            _mockIngredientRepository.Setup(m => m.FindById(1)).Returns(ingredients[0]);
            _mockIngredientRepository.Setup(m => m.FindById(2)).Returns(ingredients[1]);
            _mockIngredientRepository.Setup(m => m.Exists(1)).Returns(true);
            _mockIngredientRepository.Setup(m => m.Exists(2)).Returns(true);

            orders.ForEach(o => _restaurant.ProcessOrder(o));
            
            Assert.Equal(8.8, _restaurant.GetIngredientStock(1));
            Assert.Equal(6.4, _restaurant.GetIngredientStock(2));
        }

        [Fact]
        public void when_not_enough_ingredients_for_each_dish_wont_process_order_()
        {
            var dishes = new List<Dish>
            {
                new Dish {Id = 1, Name = "dish1", IngredientIds = new List<int>{1, 2, 3}},
                new Dish {Id = 2, Name = "dish2", IngredientIds = new List<int>{1, 2, 3}}
            };
            var order = new Order {Id = 1, Date = DateTime.Parse("2021-01-01 00:00:00"), DishIds = new List<int> {1,2}};
            var ingredients = new List<Ingredient>
            {
                new Ingredient {Id = 1, Name = "I1", Stock = 0.9, AmountInPortion = 0.3, UnitMeasure = UnitMeasure.Kg},
                new Ingredient {Id = 2, Name = "I2", Stock = 0.6, AmountInPortion = 0.3, UnitMeasure = UnitMeasure.Kg},
                new Ingredient {Id = 3, Name = "I3", Stock = 0.5, AmountInPortion = 0.3, UnitMeasure = UnitMeasure.Kg}

            };

            _mockDishRepository.Setup(m => m.FindById(1)).Returns(dishes[0]);
            _mockDishRepository.Setup(m => m.FindById(2)).Returns(dishes[1]);
            _mockIngredientRepository.Setup(m => m.FindById(1)).Returns(ingredients[0]);
            _mockIngredientRepository.Setup(m => m.FindById(2)).Returns(ingredients[1]);
            _mockIngredientRepository.Setup(m => m.FindById(3)).Returns(ingredients[2]);
            
            var (_, madeDishes) = _restaurant.ProcessOrder(order);
            
            Assert.Empty(madeDishes);
            Assert.Equal(0.9, _restaurant.GetIngredientStock(1));
            Assert.Equal(0.6, _restaurant.GetIngredientStock(2));
        }

        [Fact]
        public void adds_dish()
        {
            var newDish = new Dish {Name = "new", IngredientIds = new List<int> {1, 2}};
            _mockIngredientRepository.Setup(m => m.Exists(1)).Returns(true);
            _mockIngredientRepository.Setup(m => m.Exists(2)).Returns(true);
            
            _restaurant.AddDish(newDish);
            
            _mockDishRepository.Verify(m => m.Add(newDish), Times.Once);
        }
        
        [Fact]
        public void errors_when_adding_dish_with_not_existing_ingredients()
        {
            var newDish = new Dish {Name = "new", IngredientIds = new List<int> {1, 2}};
            _mockIngredientRepository.Setup(m => m.Exists(1)).Returns(true);
            _mockIngredientRepository.Setup(m => m.Exists(2)).Returns(false);
            
            var ex = Assert.Throws<ArgumentException>(() => _restaurant.AddDish(newDish));
            Assert.Contains("not all dish ingredients exist in a", ex.Message);
        }
        
        [Fact]
        public void errors_when_name_already_exists()
        {
            var newDish = new Dish {Name = "new", IngredientIds = new List<int> {1, 2}};
            _mockDishRepository.Setup(m => m.ExistsByName(newDish.Name)).Returns(true);
            _mockIngredientRepository.Setup(m => m.Exists(1)).Returns(true);
            _mockIngredientRepository.Setup(m => m.Exists(2)).Returns(true);
            
            var ex = Assert.Throws<ArgumentException>(() => _restaurant.AddDish(newDish));
            Assert.Equal("dish with name 'new' already exists", ex.Message);
        }

        [Fact]
        public void when_ingredient_not_exists_adds_new_ingredient()
        {
            var ingredient = new Ingredient {Name="Pepper", Stock = 20, UnitMeasure = UnitMeasure.Unit};
            _mockIngredientRepository.Setup(m => m.ExistsByName(ingredient.Name)).Returns(false);
            
            _restaurant.AddIngredient(ingredient);
            
            _mockIngredientRepository.Verify(m => m.Add(ingredient), Times.Once);
            _mockIngredientRepository.Verify(m => m.FindByName(ingredient.Name), Times.Never);
            _mockIngredientRepository.Verify(m => m.Update(It.IsAny<int>(), It.IsAny<Ingredient>()), Times.Never);
        }

        [Fact]
        public void when_ingredient_exists_adds_to_existing_ingredient_stock()
        {
            var existingIngredient = new Ingredient {Id = 1, Name="Pepper", Stock = 20, UnitMeasure = UnitMeasure.Unit};
            var addedIngredient = new Ingredient {Name="Pepper", Stock = 30, UnitMeasure = UnitMeasure.Unit};
            
            _mockIngredientRepository.Setup(m => m.FindByName(addedIngredient.Name)).Returns(existingIngredient);
            _mockIngredientRepository.Setup(m => m.ExistsByName(addedIngredient.Name)).Returns(true);

            addedIngredient.Stock = 50;
            
            _restaurant.AddIngredient(addedIngredient);
            
            _mockIngredientRepository.Verify(m => m.Add(It.IsAny<Ingredient>()), Times.Never);
            _mockIngredientRepository.Verify(m => m.FindByName(existingIngredient.Name), Times.Once);
            _mockIngredientRepository.Verify(m => m.Update(existingIngredient.Id, addedIngredient), Times.Once);
            Assert.Equal(existingIngredient.Id, addedIngredient.Id);
        }

        [Fact]
        public void when_ingredient_exists_clears_ingredient_stock()
        {
            var ingredientName = "name";
            var existingIngredient = new Ingredient {Id = 1, Stock = 20};
            _mockIngredientRepository.Setup(m => m.ExistsByName(ingredientName)).Returns(true);
            _mockIngredientRepository.Setup(m => m.FindByName(ingredientName)).Returns(existingIngredient);
            
            Assert.Equal(20, existingIngredient.Stock);
            _restaurant.ClearIngredientStock(ingredientName);
            
            Assert.Equal(0, existingIngredient.Stock);
            _mockIngredientRepository
                .Verify(m => m.Update(existingIngredient.Id, existingIngredient), Times.Once);
        }

        [Fact]
        public void errors_when_clearing_ingredient_stock_if_ingredient_not_exists()
        {
            var ingredientName = "name";
            _mockIngredientRepository.Setup(m => m.ExistsByName(ingredientName)).Returns(false);
            
            Assert.Throws<ArgumentException>(() => _restaurant.ClearIngredientStock(ingredientName));
            _mockIngredientRepository.Verify(m => m.FindByName(ingredientName), Times.Never);
        }
    }
}
