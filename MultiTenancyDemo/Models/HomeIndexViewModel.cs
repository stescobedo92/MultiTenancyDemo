using MultiTenancyDemo.Entities;

namespace MultiTenancyDemo.Models
{
    public class HomeIndexViewModel
    {
        public IEnumerable<Product> Products { get; set; } = new List<Product>();
        public IEnumerable<Countries> Countries { get; set; } = new List<Countries>();

    }
}
