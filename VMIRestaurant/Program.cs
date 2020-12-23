using System;
using System.Threading.Tasks;
using VMIRestaurant.data.csv;
using VMIRestaurant.data.csv.mappers;
using VMIRestaurant.domain.restaurant;

namespace VMIRestaurant
{
    internal static class Program
    {
        public static async Task Main(string[] args)
        {
            const string directoryCsvResource = @"C:\Users\Lukas\Desktop\DirtyRider\VMIRestaurant\VMIRestaurant\data\csv\resource";
            const string fileCsvIngredient = "ingredient.csv";
            const string fileCsvOrder = "order.csv";
            const string fileCsvDish = "dish.csv";
           
            var ingredientCsvData = await CsvFileReader.ReadData(directoryCsvResource, fileCsvIngredient);
            var ingredients = CsvDataProvider<Ingredient>.ProvideData(ingredientCsvData, new IngredientMapper());
            
            var orderCsvData = await CsvFileReader.ReadData(directoryCsvResource, fileCsvOrder);
            var orders = CsvDataProvider<Order>.ProvideData(orderCsvData, new OrderMapper());
            
            var dishCsvData = await CsvFileReader.ReadData(directoryCsvResource, fileCsvDish);
            var dishes = CsvDataProvider<Dish>.ProvideData(dishCsvData, new DishMapper());
            
            
            
            Console.WriteLine(ingredients[0].UnitMeasure);
            Console.WriteLine(dishes[0].Name);
            Console.WriteLine(orders[0].Date);
        }
    }
}
