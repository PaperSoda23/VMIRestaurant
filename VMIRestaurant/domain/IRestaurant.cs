using System.Collections.Generic;
using VMIRestaurant.domain.restaurant;

namespace VMIRestaurant.domain
{
    public interface IRestaurant
    {
        (int, List<Dish>) ProcessOrder(Order order);
    }
}