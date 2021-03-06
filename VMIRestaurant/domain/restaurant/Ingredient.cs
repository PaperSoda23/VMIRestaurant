﻿using VMIRestaurant.common;

namespace VMIRestaurant.domain.restaurant
{
    public class Ingredient : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Stock { get; set; }
        public UnitMeasure UnitMeasure { get; set; }
        public double AmountInPortion { get; set; }

        public override string ToString() => $"[Ingredient]=Id:{Id} Name:{Name}, stock:{Stock}, in portion:{AmountInPortion}";
    }
}