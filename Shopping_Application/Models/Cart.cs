namespace Shopping_Application.Models
{
    public class Cart
    {
        public Guid UserId { get; set; }
        public string Description { get; set; }
        public virtual IList<CartDetail> CartDetails { get; set;}
        public virtual User User { get; set; }
    }
}
