﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name ="Category Name")]
        public string Name { get; set; }
        [Display(Name = "Is Active?")]
        public bool Status { get; set; } = true;
    }
}
