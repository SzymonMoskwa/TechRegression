using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechRegression.Data;
using TechRegression.Models;

namespace TechRegression.Controllers
{
    public class HomeController : Controller
    {
        public readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString, int? categoryId)
        {
            var articlesQuery = _context.Articles.Include(a => a.Category).AsQueryable();

            // filtrowanie po wyszukiwarce
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                searchString = searchString.Trim();
                articlesQuery = articlesQuery.Where(a =>
                    a.Title.Contains(searchString) ||
                    a.Content.Contains(searchString));

                ViewData["CurrentFilter"] = searchString;
            }

            // filtrowanie po kategorii
            if (categoryId.HasValue)
            {
                articlesQuery = articlesQuery.Where(a => a.CategoryId == categoryId.Value);
            }

            var articles = await articlesQuery.OrderByDescending(a => a.CreatedAt).ToListAsync();
            return View(articles);
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
