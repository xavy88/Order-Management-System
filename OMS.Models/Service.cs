using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.Models
{
    public class Service
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name ="Service Name")]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        public string Description { get; set; }
        [DataType(DataType.ImageUrl)]
        [Display(Name ="Image")]
        public string ImageUrl { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        public bool Status { get; set; } = true;
    }
}
