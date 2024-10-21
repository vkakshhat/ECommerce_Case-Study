namespace Entity_Library
{
    public class Customers
    {
        public int Customer_id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public Customers() { }

        public Customers(int customer_id, string name, string email, string password)
        {
            Customer_id = customer_id;
            Name = name;
            Email = email;
            Password = password;
        }
    }
}
