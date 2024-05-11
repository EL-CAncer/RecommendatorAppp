using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RecommendatorAppp.Models
{
    public class Reviews
    {
        public int Id { get; set; }

        [StringLength(100, MinimumLength = 2)]
        [RegularExpression("([a-zA-Z0-9 .&'-]+)", ErrorMessage = "The field Title should only include letters and number.")]
        [DataType(DataType.Text)]
        [Required]
        public string Title { get; set; }

        [StringLength(500, MinimumLength = 2)]
        [DataType(DataType.MultilineText)]
        [Required]
        public string Description { get; set; }

        [Range(1, 5)]
        public int Grade { get; set; }
        public int Name { get; set; }
        public DateTime Date { get; set; }

        [DisplayName("Select Service")]
        public int ServiceId { get; set; }
        
        public virtual Services Service { get; set; }

        public string UserId { get; set; }

        public IdentityUser User { get; set; }
        //public Services Services { get; internal set; }
        //public int? ServicesId { get; internal set; }
    }
}