using RecommendatorAppp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendatorAppp.ViewModels
{
    public class SearchServicesViewModel
    {
        [Required]
        [DisplayName("Serach")]
        public string SearchTextt{ get; set; }

        //public IEnumerable<string> SearchListExamples { get; set; }

        public IEnumerable<Services> ServicesList { get; set; }

    }
}
