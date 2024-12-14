using Microsoft.AspNetCore.Mvc.RazorPages;
using WineManager.EntityModels;

namespace WineManager.Web.Pages
{
    public class ProducersModel : PageModel
    {
        private WineManagerContext _db;
        public IEnumerable<Producer>? Producers { get; set; }

        public ProducersModel(WineManagerContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
            @ViewData["Title"] = "Wine Manager - Producers";

            Producers = _db.Producers
                .OrderBy(c => c.ProducerName)
                .ThenBy(c => c.Region)
                .ThenBy(c => c.Country);    
        }
    }
}
