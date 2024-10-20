namespace Entity_Library
{
    public class Orders
    {
        public int Order_id { get; set; }
        public int Customer_id { get; set; }
        public DateTime Order_date { get; set; } = DateTime.Now;
        public decimal Total_price { get; set; }
        public string Shipping_address { get; set; }

        public Orders() { }

        public Orders(int order_id, int customer_id, decimal total_price, string shipping_address)
        {
            Order_id = order_id;
            Customer_id = customer_id;
            Total_price = total_price;
            Shipping_address = shipping_address;
        }
    }
}
