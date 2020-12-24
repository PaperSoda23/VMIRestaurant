using System;
using System.Collections.Generic;
using System.Linq;
using VMIRestaurant.common;
using VMIRestaurant.domain.restaurant;

namespace VMIRestaurant.data.csv.mapper
{
    public class OrderMapper : ICsvEntityMapper
    {
        public IEntity MapToEntity(IEnumerable<string> dataFrom)
        {
            var enumerable = dataFrom.ToList();

            return new Order
            {
                Id = int.Parse(enumerable[0]),
                Date = DateTime.Parse(enumerable[1]),
                DishIds = enumerable[2].Split(" ").Select(int.Parse).ToList()
            };
        }
    }
}