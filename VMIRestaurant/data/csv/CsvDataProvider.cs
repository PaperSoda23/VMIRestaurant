using System.Collections.Generic;
using System.Linq;
using VMIRestaurant.common;
using VMIRestaurant.data.csv.mappers;

namespace VMIRestaurant.data.csv
{
    public static class CsvDataProvider<T> where T : IEntity
    {
        public static List<T> ProvideData(
            IEnumerable<IEnumerable<string>> csvData,
            ICsvEntityMapper csvEntityMapper
        ) {
            return csvData
                .Select(dataLine => (T) csvEntityMapper.MapToEntity(dataLine))
                .ToList();
        }
    }
}