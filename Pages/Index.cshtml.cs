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

        public async Task<IActionResult> OnPostAsync(int? Id, bool? IsCompleted)
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            
            if (!string.IsNullOrEmpty(Title))
            {
                var newItem = new ToDo { 
                    Title = Title, 
                    IsCompleted = false, 
                    Description = Description,
                    DueDate = DateTime.TryParse(DueDate, out var parsedDate) ? parsedDate : (DateTime?)null,
                    };
                _context.ToDos.Add(newItem);
                await _context.SaveChangesAsync();
            }
            /* else if (Id.HasValue)
            {
                var item = await _context.ToDos.FindAsync(Id.Value);
                if (item != null)
                {
                    item.IsCompleted = IsCompleted ?? false;
                    item.Description = Description ?? string.Empty;
                    item.DueDate = dueDate;
                    await _context.SaveChangesAsync();
                }
            } */

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdate(int id, bool isCompleted)
        {
            // Update the to-do item based on the ID and completion status
            var item = _context.ToDos.FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                item.IsCompleted = isCompleted;
            }
            await _context.SaveChangesAsync();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var item = await _context.ToDos.FindAsync(id);
            if (item != null)
            {
                _context.ToDos.Remove(item);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}
