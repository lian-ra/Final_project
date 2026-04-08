using System;
using System.Collections.Generic;

public class Product
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public string Picture { get; set; }
}

public class OrderItem
{
    public int OrderItemId { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    
    public string ProductName { get; set; }
    public decimal Price { get; set; }
}

public class StoreOrder
{
    public int OrderId { get; set; }
    public string Username { get; set; }
    public int EventId { get; set; }
    public DateTime DatePurchased { get; set; }
    public decimal Total { get; set; }
    
    public List<OrderItem> Items { get; set; }
    public string ItemsSummary { get; set; }
}
