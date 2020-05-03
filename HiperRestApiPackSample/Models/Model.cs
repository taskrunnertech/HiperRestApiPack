using HiperRestApiPack;
using System;
using System.Collections.Generic;

namespace HiperRestApiPackSample.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        [IgnoreField]
        public int SomeIgnoredField { get; set; }

        public List<Variant> Variants { get; set; }

    }
    public class Variant
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }

    public class OrderItem
    {
        public int Id { get; set; }
        public int VariantId { get; set; }
        public int Quantity { get; set; }
        public Variant Variant { get; set; }
    }

}
