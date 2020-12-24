using System.Collections.Generic;
using VMIRestaurant.common;

namespace VMIRestaurant.data.csv.mapper
{
    public interface ICsvEntityMapper
    {
        IEntity MapToEntity(IEnumerable<string> dataFrom);
    }
}