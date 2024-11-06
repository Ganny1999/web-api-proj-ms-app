﻿using System.ComponentModel.DataAnnotations;

namespace product_api_ms.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string Discription {  get; set; }
        [Required]
        [Range(1,1000)]
        public int Price {  get; set; }
        [Range(1,5)]
        public int rating { get; set; }
    }
}
