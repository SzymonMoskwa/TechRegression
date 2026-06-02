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
                try
                {
                    comment.CreatedAt = DateTime.Now;
                    _context.Add(comment);
                    await _context.SaveChangesAsync();

                    TempData["CommentStatus"] = "Success";
                    TempData["CommentMessage"] = "Komentarz opublikowany.";
                }
                catch (Exception)
                {
                    TempData["CommentStatus"] = "Error";
                    TempData["CommentMessage"] = "Wystąpił nieoczekiwany błąd.";
                }
            }
            else
            {
                TempData["CommentStatus"] = "Error";
                TempData["CommentMessage"] = "Nie udało się dodać komentarza. Sprawdź poprawność pól.";
            }

            return RedirectToAction("Details", "Articles", new { id = comment.ArticleId });
        }
    }
}