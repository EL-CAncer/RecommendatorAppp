﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendatorAppp.Models
{
    public class Services
    {
        public Services()
        {
            Reviews = new HashSet<Reviews>();
        }

        public int Id { get; set; }

        [StringLength(100, MinimumLength = 2)]
        [RegularExpression("([a-zA-Z0-9 .&'-]+)", ErrorMessage = "The field Name should only include letters and number.")]
        [DataType(DataType.Text)]
        [Required]
        public string Name { get; set; }
        
        [Range(0, 1000)]
        [DataType(DataType.Currency)]
        [Required]
        [Precision(18, 2)]
        public decimal Price { get; set; }

        [StringLength(255, MinimumLength = 2)]
        [DataType(DataType.MultilineText)]
        [Required]
        public string Description { get; set; }

        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }


        [DisplayName("Select Category")]
        public int CategoriesId { get; set; }

        public virtual Categories Category { get; set; }

        public virtual ICollection<ServiceInformation> ServiceInformation { get; set; }
        public virtual ICollection<Reviews> Reviews { get; set; }
        public bool IsPizzaOfTheWeek { get; internal set; }
    }
}
