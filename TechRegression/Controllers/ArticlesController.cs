using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TechRegression.Data;
using TechRegression.Models;

namespace TechRegression.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ArticlesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: /Articles/Create
        public IActionResult Create()
        {
            // przekazanie listy kategorii do tagu <select> w html
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: /Articles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ArticleViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;

                // Upload zdjęć
                if (model.ImageFile is not null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "articles");

                    uniqueFileName = $"{Guid.NewGuid().ToString()}_{model.ImageFile.FileName}";

                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(fileStream);
                    }
                }

                // mapowanie danych z viewmodelu do modelu bazodanowego
                var article = new Article
                {
                    Title = model.Title,
                    Content = model.Content,
                    CategoryId = model.CategoryId.Value,
                    ImagePath = $"/uploads/articles/{uniqueFileName}",
                    CreatedAt = DateTime.Now
                };

                // zapis do bazy
                _context.Add(article);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .Include(a => a.Category)
                .Include(a => a.Comments)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (article is null)
            {
                return NotFound();
            }

            return View(article);
        }
    }
}
