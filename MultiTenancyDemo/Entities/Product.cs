namespace MultiTenancyDemo.Entities
{
    public class Product : ITenatEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string TenatId { get; set; } = null!;
    }
}
