using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JanuaryProject.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public int PaintingId { get; set; }
        public virtual Painting Painting { get; set; }
        public DateTime DateAdded { get; set; }
    }
}