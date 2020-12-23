using VMIRestaurant.common;

namespace VMIRestaurant.domain.restaurant
{
    public class Ingredient : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Stock { get; set; }
        public UnitMeasure UnitMeasure { get; set; }
        public float AmountInPortion { get; set; }
    }
}