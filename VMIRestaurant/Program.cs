using System;
using System.IO;
using System.Threading.Tasks;
using VMIRestaurant.data.csv;
using VMIRestaurant.data.csv.mapper;
using VMIRestaurant.data.repository;
using VMIRestaurant.domain;
using VMIRestaurant.domain.restaurant;
using VMIRestaurant.helper;

namespace VMIRestaurant
{
    internal static class Program
    {
        public static async Task Main(string[] args)
        {
            string projectRootDir = DirectoryHelper.GetProjectRootDir();
            string directoryCsvResource = Path.Combine(projectRootDir, @"data\csv\resource");
            const string fileCsvIngredient = "ingredient.csv";
            const string fileCsvOrder = "order.csv";
            const string fileCsvDish = "dish.csv";
            
            var ingredientCsvData = await CsvFileReader.ReadData(directoryCsvResource, fileCsvIngredient);
            var ingredients = CsvDataProvider<Ingredient>.ProvideData(ingredientCsvData, new IngredientMapper());
            
            var orderCsvData = await CsvFileReader.ReadData(directoryCsvResource, fileCsvOrder);
            var orders = CsvDataProvider<Order>.ProvideData(orderCsvData, new OrderMapper());
            
            var dishCsvData = await CsvFileReader.ReadData(directoryCsvResource, fileCsvDish);
            var dishes = CsvDataProvider<Dish>.ProvideData(dishCsvData, new DishMapper());

            
            var ingredientRepo = new IngredientRepository(ingredients);
            var dishRepo = new DishRepository(dishes);
            
            IRestaurant restaurant = new Restaurant(
                ingredientRepo,
                dishRepo
            );


            Console.WriteLine("Before:");
            ingredientRepo.GetAll().ForEach(Console.WriteLine);
            dishRepo.GetAll().ForEach(Console.WriteLine);
            Console.WriteLine();
            
            orders.ForEach(order =>
            {
                var (orderId, processedOrder) =  restaurant.ProcessOrder(order);
                Console.WriteLine($"Order ID: {orderId}");
                processedOrder.ForEach(Console.WriteLine);
            });
            Console.WriteLine();

            Console.WriteLine("After:");
            ingredientRepo.GetAll().ForEach(Console.WriteLine);
        }
    }
}
