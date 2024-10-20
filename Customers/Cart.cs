namespace Entity_Library
{
    public class Cart
    {
        public int Cart_id { get; set; }
        public int Customer_id { get; set; }
        public int Product_id { get; set; }
        public int Quantity { get; set; }

        public Cart() { }

        public Cart(int cart_id, int customer_id, int product_id, int quantity)
        {
            Cart_id = cart_id;
            Customer_id = customer_id;
            Product_id = product_id;
            Quantity = quantity;
        }
    }
}
