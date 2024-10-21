namespace Entity_Library
{
    public class Order_Items
    {
        public int Order_item_id { get; set; }
        public int Order_id { get; set; }
        public int Product_id { get; set; }
        public int Quantity { get; set; }

        public Order_Items() { }

        public Order_Items(int order_item_id, int order_id, int product_id, int quantity)
        {
            Order_item_id = order_item_id;
            Order_id = order_id;
            Product_id = product_id;
            Quantity = quantity;
        }
    }
}
