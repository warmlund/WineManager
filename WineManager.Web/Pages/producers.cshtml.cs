using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WineManager.EntityModels;

namespace WineManager.Web.Pages
{
    public class ProducersModel : PageModel
    {
        private WineManagerContext _db;
        public IEnumerable<Producer>? Producers { get; set; }

        [BindProperty]
        public Producer? Producer { get; set; }

        [BindProperty]
        public List<int> SelectedProducers { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public ProducersModel(WineManagerContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
            @ViewData["Title"] = "Wine Manager - Producers";
            ReloadProducers();
        }

        public IActionResult OnPost()
        {
            if (Producer != null && ModelState.IsValid && !ProducerAlreadyExists(Producer))
            {
                _db.Producers.Add(Producer);
                _db.SaveChanges();

                return RedirectToPage("/producers");
            }

            else
            {
                // Reload the producers list
                ReloadProducers();

                 // Set the error message to display on the page
                 ErrorMessage = ProducerAlreadyExists(Producer)
                    ? "Producer already exists."
                    : "Invalid input.";

                return Page();
            }
        }

        public IActionResult OnPostDeleteSelected()
        {
            if (SelectedProducers != null && SelectedProducers.Count > 0)
            {
                var producersToDelete = _db.Producers
                    .Where(p => SelectedProducers.Contains(p.ProducerId))
                    .ToList();

                if (producersToDelete.Any())
                {
                    _db.Producers.RemoveRange(producersToDelete);
                    _db.SaveChanges();
                }
            }

            // Reload the producers list and return the page
            ReloadProducers();
            return Page();
        }

        private bool ProducerAlreadyExists(Producer producer)
        {
            return _db.Producers.Any(x =>x.ProducerName.Trim() == producer.ProducerName.Trim());
        }

        private void ReloadProducers() => Producers = _db.Producers
                .OrderBy(c => c.ProducerName)
                .ThenBy(c => c.Region)
                .ThenBy(c => c.Country);

        public IActionResult OnGetProducersJson()
        {
            List<Producer> producers = _db.Producers
                .OrderBy(c => c.ProducerName)
                .ToList();
            return new JsonResult(producers);
        }

    }
}
