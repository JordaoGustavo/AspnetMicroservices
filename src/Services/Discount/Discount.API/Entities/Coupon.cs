namespace Discount.API.Entities
{
    public class Coupon
    {
        public int Id { get; private set; }
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public int Amount { get; set; }

        public Coupon()
        {
        }

        public Coupon(string productName, string description, int amount)
        {
            ProductName = productName;
            Description = description;
            Amount = amount;
        }

        internal static Coupon Default()
        {
            return new Coupon("No Discount", "No Discount Desc", 0);
        }
    }
}
