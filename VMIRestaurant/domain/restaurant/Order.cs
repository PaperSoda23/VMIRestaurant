using System;
using System.Collections.Generic;
using VMIRestaurant.common;

namespace VMIRestaurant.domain.restaurant
{
    public class Order : IEntity
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public List<int> DishIds { get; set; }
        
        public override string ToString() => $"[Order]=Id:{Id} Date:{Date} Dishes:[{string.Join(",", DishIds)}]";
    }
}