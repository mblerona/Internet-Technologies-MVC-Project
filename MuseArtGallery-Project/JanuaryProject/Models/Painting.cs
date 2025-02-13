using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JanuaryProject.Models
{
    public class Painting
    {
        [Key]
        public int PaintingId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Dimensions { get; set; }
        [Required]
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public int ArtistId { get; set; }
        public virtual Artist Artist { get; set; }
        public Painting()
        {
            Carts = new List<Cart>();
        }
    }
}