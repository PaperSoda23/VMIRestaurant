using VMIRestaurant.domain.restaurant;

namespace VMIRestaurant.data.repository.@interface
{
    public interface IDishRepository : IRepository<Dish>
    {
        bool ExistsByName(string dishName);
    }
}