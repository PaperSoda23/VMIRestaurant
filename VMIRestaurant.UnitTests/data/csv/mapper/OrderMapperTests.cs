using System;
using System.Collections.Generic;
using VMIRestaurant.data.csv.mapper;
using VMIRestaurant.domain.restaurant;
using Xunit;

namespace VMIRestaurant.UnitTests.data.csv.mapper
{
    public class OrderMapperTests
    {
        private readonly OrderMapper _orderMapper;
        
        public OrderMapperTests()
        {
            _orderMapper = new OrderMapper();
        }

        [Fact]
        public void maps_order()
        {
            var data = new List<string> {"1", "2021-01-01 00:00:00" ,"1 2 3"};
            
            var order = (Order) _orderMapper.MapToEntity(data);
            
            Assert.Equal(1, order.Id);
            Assert.Equal(DateTime.Parse(data[1]), order.Date);
            Assert.Equal(new List<int> {1, 2, 3}, order.DishIds);
        }
    }
}
