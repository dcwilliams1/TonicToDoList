using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Data.Models;

namespace TodoApp.Pages.Todo
{
    public class IndexModel : PageModel
    {
        private readonly TodoContext _context;

        public IndexModel(TodoContext context)
        {
            _context = context;
        }

        [BindProperty]
        public List<ToDo> ToDo { get; set; } = new();
        [BindProperty]
        public string Title { get; set; }
        [BindProperty]
        public string Description { get; set; }
        [BindProperty]
        public string DueDate { get; set; }

        public async Task OnGetAsync()
        {
            ToDo = await _context.ToDos.ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync(string? Title, string? Description, string? DueDate, int? Id, bool? IsCompleted)
        {
            if (!string.IsNullOrEmpty(Title))
            {
                var newItem = new ToDo { Title = Title, IsCompleted = false };
                _context.ToDos.Add(newItem);
                await _context.SaveChangesAsync();
            }
            else if (Id.HasValue)
            {
                var item = await _context.ToDos.FindAsync(Id.Value);
                if (item != null)
                {
                    item.IsCompleted = IsCompleted ?? false;
                    item.Description = Description ?? string.Empty;
                    DateTime.TryParse(DueDate, out var dueDate);
                    item.DueDate = dueDate;
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToPage();
        }
    }
}
