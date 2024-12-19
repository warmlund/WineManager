using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WineManager.EntityModels;

namespace WineManager.Web.Pages
{
    public class WinesModel : PageModel
    {
        private WineManagerContext _db;
        public WineManagerContext WineDb { get { return _db; } }
        public IEnumerable<Wine>? Wines { get; set; }

        [BindProperty]
        public Wine? Wine { get; set; }

        [BindProperty]
        public List<int> SelectedWines { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public WinesModel(WineManagerContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
            @ViewData["Title"] = "Wine Manager - Wines";
            ReloadWines();
        }

        public IActionResult OnPost()
        {
            if (Wine != null && !WineAlreadyExists(Wine))
            {
                Wine.Producer = _db.Producers.Where(p => p.ProducerName == Wine.ProducerName).FirstOrDefault();
                Console.WriteLine(Wine.Producer.ProducerName);

                _db.Wines.Add(Wine);
                _db.SaveChanges();
                ReloadWines();
                @ViewData["Title"] = "Wine Manager - Bajs";
                return RedirectToPage("/index");
            }

            else
            {
                // Reload the wine list
                ReloadWines();

                // Set the error message to display on the page
                ErrorMessage = WineAlreadyExists(Wine)
                   ? "Wine already exists."
                   : "Invalid input.";

                return Page();
            }
        }

        public IActionResult OnPostDeleteSelected()
        {
            if (SelectedWines != null && SelectedWines.Count > 0)
            {
                var winesToDelete = _db.Wines
                    .Where(w => SelectedWines.Contains(w.WineId))
                    .ToList();

                if (winesToDelete.Any())
                {
                    _db.Wines.RemoveRange(winesToDelete);
                    _db.SaveChanges();
                }
            }

            // Reload the wine list and return the page
            ReloadWines();
            return Page();
        }

        private bool WineAlreadyExists(Wine wine)
        {
            return _db.Wines.Any(x => x.WineName.Trim() == wine.WineName.Trim());
        }

        private void ReloadWines() => Wines = _db.Wines.OrderBy(n => n.WineName).ThenBy(n => n.BottleSize);
    }
}
