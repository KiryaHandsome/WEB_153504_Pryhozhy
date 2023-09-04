namespace WEB_15354_Pryhozhy.Domain.Entities
{
    public class Pizza
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Calories { get; set; }
        public string? Image {  get; set; }
        public int CategoryId { get; set; }
        public string? Description { get; set; }
    }
}