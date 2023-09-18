namespace WEB_153504_Klimkovich.Domain.Entities
{
    public class Electronics
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; } = null!;
        public string Mime { get; set; } = null!;
    }

}
