using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiTenancyDemo.Data;
using MultiTenancyDemo.Entities;
using MultiTenancyDemo.Models;
using System.Diagnostics;

namespace MultiTenancyDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            return View(await BuildHomeIndexModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(Product product)
        {
            _dbContext.Add(product);
            await _dbContext.SaveChangesAsync();
            var modelo = await BuildHomeIndexModel();

            return View(modelo);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<HomeIndexViewModel> BuildHomeIndexModel()
        {
            var products = await _dbContext.Products.ToListAsync();
            var countries = await _dbContext.Countries.ToListAsync();

            var homeIndexViewModel = new HomeIndexViewModel();
            homeIndexViewModel.Products = products;
            homeIndexViewModel.Countries = countries;

            return homeIndexViewModel;
        }
    }
}
