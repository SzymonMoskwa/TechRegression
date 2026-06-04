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
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
            {
                return RedirectToAction("Login", "Admin");
            }
            // przekazanie listy kategorii do tagu <select> w html
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: /Articles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ArticleViewModel model)
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
            {
                return RedirectToAction("Login", "Admin");
            }

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

        // --- EDYCJA ARTYKUŁU (GET) ---
        public async Task<IActionResult> Edit(int? id)
        {
            // FIX: HttpContext.Session zamiast Context.Session
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true") return RedirectToAction("Login", "Admin");
            if (id == null) return NotFound();

            var article = await _context.Articles.FindAsync(id);
            if (article == null) return NotFound();

            // Przekazanie listy kategorii dokładnie tak samo jak w Create
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", article.CategoryId);
            return View(article);
        }

        // --- EDYCJA ARTYKUŁU (POST) ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,CategoryId,ImagePath,CreatedAt")] Article article)
        {
            // FIX: HttpContext.Session zamiast Context.Session
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true") return RedirectToAction("Login", "Admin");
            if (id != article.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Aktualizujemy cały obiekt w bazie (zachowując oryginalną ścieżkę ImagePath i datę CreatedAt)
                    _context.Update(article);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Articles.Any(e => e.Id == article.Id)) return NotFound();
                    else throw;
                }
                // Po udanej edycji wracamy do szczegółów tego artykułu
                return RedirectToAction("Details", new { id = article.Id });
            }

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", article.CategoryId);
            return View(article);
        }

        // --- USUWANIE ARTYKUŁU (GET) ---
        public async Task<IActionResult> Delete(int? id)
        {
            // FIX: HttpContext.Session zamiast Context.Session
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true") return RedirectToAction("Login", "Admin");
            if (id == null) return NotFound();

            var article = await _context.Articles
                .Include(a => a.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (article == null) return NotFound();

            return View(article);
        }

        // --- USUWANIE ARTYKUŁU (POST) ---
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // FIX: HttpContext.Session zamiast Context.Session
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true") return RedirectToAction("Login", "Admin");

            var article = await _context.Articles.FindAsync(id);
            if (article != null)
            {
                // OPCJONALNIE: Jeśli chcesz usuwać plik zdjęcia z dysku serwera przy kasowaniu wpisu:
                if (!string.IsNullOrEmpty(article.ImagePath))
                {
                    var absolutePath = Path.Combine(_webHostEnvironment.WebRootPath, article.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(absolutePath))
                    {
                        System.IO.File.Delete(absolutePath);
                    }
                }

                _context.Articles.Remove(article);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
