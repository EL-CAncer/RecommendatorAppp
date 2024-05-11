using RecommendatorAppp.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecommendatorAppp.Models
{
    public class Information
    {
        public Information()
        {
            ServiceInformation = new HashSet<ServiceInformation>();
        }

        public int Id { get; set; }

        [StringLength(100, MinimumLength = 2)]
        [RegularExpression("([a-zA-Z0-9 .&'-]+)", ErrorMessage = "The field Name should only include letters and number.")]
        [DataType(DataType.Text)]
        [Required]
        public string Name { get; set; }

        public virtual ICollection<ServiceInformation> ServiceInformation { get; set; }
        public virtual ICollection<Services> Services{ get; set; }

    }
}