using System.Collections.Generic;
using Moq;
using VMIRestaurant.common;
using VMIRestaurant.data.csv;
using VMIRestaurant.data.csv.mapper;
using Xunit;

namespace VMIRestaurant.UnitTests.data.csv
{
    public class CsvDataProviderTests
    {
        private class MockEntity : IEntity { public int Id { get; set; } }

        [Fact]
        public void provides_data()
        {
            var csvData = new List<List<string>> {new List<string> {"1"}, new List<string> {"2"}};

            var mockMapper = new Mock<ICsvEntityMapper>();
            mockMapper.Setup(m => m.MapToEntity(csvData[0])).Returns(new MockEntity {Id = 1});
            mockMapper.Setup(m => m.MapToEntity(csvData[1])).Returns(new MockEntity {Id = 2});

            var result = CsvDataProvider<MockEntity>.ProvideData(csvData, mockMapper.Object);
            
            Assert.Equal(result.Count, csvData.Count);
            Assert.IsType<MockEntity>(result[0]);
            Assert.Equal(1, result[0].Id);
            Assert.Equal(2, result[1].Id);
        }
    }
}