namespace MultiTenancyDemo.Entities
{
    public class Countries : ICommonEntity
    {
        public int Id { get; set; }
        public string CountryName { get; set; } = null!;
        public string CountryCode { get; set; } = null!;
    }
}
