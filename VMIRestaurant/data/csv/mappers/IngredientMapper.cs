using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VMIRestaurant.common;
using VMIRestaurant.domain.restaurant;
using VMIRestaurant.extensions;

namespace VMIRestaurant.data.csv.mappers
{
    public class IngredientMapper : ICsvEntityMapper
    {
        public IEntity MapToEntity(IEnumerable<string> dataFrom)
        {
            var enumerable = dataFrom.ToList();
            
            return new Ingredient
            {
                Id = int.Parse(enumerable[0]),
                Name = enumerable[1],
                Stock = double.Parse(enumerable[2]),
                UnitMeasure = enumerable[3].ToEnum<UnitMeasure>(),
                AmountInPortion = double.Parse(enumerable[4], CultureInfo.InvariantCulture)
            };
        }
    }
}
