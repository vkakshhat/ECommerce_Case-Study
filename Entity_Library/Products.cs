namespace Entity_Library
{
    public class Products
    {
        public int Product_id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int StockQuantity { get; set; }


        public Products() { }

        public Products(int product_id, string name, decimal price, string description, int stockQuantity)
        {
            Product_id = product_id;
            Name = name;
            Price = price;
            Description = description;
            StockQuantity = stockQuantity;
        }
    }
}
