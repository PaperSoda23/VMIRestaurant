using VMIRestaurant.domain.restaurant;

namespace VMIRestaurant.data.repository.@interface
{
    public interface IIngredientRepository : IRepository<Ingredient>
    {
        bool ExistsByName(string dishName);
        Ingredient FindByName(string dishName);
    }
}