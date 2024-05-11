using RecommendatorAppp.Models;

namespace RecommendatorAppp.Controllers
{
    internal class SearchServicesViewModel
    {
        public SearchServicesViewModel()
        {
        }

        public IEnumerable<Services> ServicesList { get; set; }
        public object SearchText { get; set; }
    }
}