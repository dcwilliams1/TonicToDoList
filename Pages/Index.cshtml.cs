using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using TodoApp.Data;
using TodoApp.Data.Models;

namespace TodoApp.Pages.Todo
{
    public class IndexModel : PageModel
    {
        private readonly TodoContext _context;
        //private readonly ISession _session;
        public IndexModel(TodoContext context)
        {
            _context = context;
            //_session = PageModel.Session;
        }

        public List<ToDo> ToDo { get; set; } = new();

        [BindProperty]
        public string Title { get; set; } = string.Empty;
        [BindProperty]
        public string Description { get; set; } = string.Empty;
        [BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DueDate { get; set; } = DateTime.Today;

        public bool IsEditMode
        { 
            get
            {
                if (HttpContext is null)
                {
                    return false;
                }
                return Boolean.Parse(HttpContext.Session.GetString("IsEditMode") ?? "false");
            }

            set
            {
                HttpContext.Session.SetString("IsEditMode", value.ToString());
            }
        } 
        public string EditItemID { get; set; } = "0";

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
                    DueDate = DateTime.TryParse(DueDate.ToString(), out var parsedDate) ? parsedDate : (DateTime?)null,
                    };
                _context.ToDos.Add(newItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateCompleted(int id, bool isCompleted)
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

        public async Task<IActionResult> OnPostUpdateDetails(int id)
        {
            // Update the to-do item based on the ID and completion status
            var item = _context.ToDos.FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                item.Title = Title;
                item.Description = Description;
                item.DueDate = DueDate;
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

        public IActionResult OnPostToggleEditMode()
        {
            bool newEditMode = !IsEditMode;
            HttpContext.Session.SetString("IsEditMode", newEditMode.ToString());
            return new JsonResult(new { success = true });
        }
    }
}
