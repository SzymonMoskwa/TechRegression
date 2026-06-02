using Microsoft.AspNetCore.Mvc;
using TechRegression.Data;
using TechRegression.Models;

namespace TechRegression.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: /Comments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Author,Content,ArticleId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.CreatedAt = DateTime.Now;

                // zapis od bazy
                _context.Add(comment);
                await _context.SaveChangesAsync();

                // przekierowanie z powrotem do artykułu
                return RedirectToAction("Details", "Articles", new { id = comment.ArticleId });
            }

            return RedirectToAction("Details", "Articles", new { id = comment.ArticleId });
        }
    }
}