using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JanuaryProject.Models
{
    public class Artist
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual List<Painting> Paintings { get; set; }

        public Artist()
        {
            Paintings = new List<Painting>();
        }
    }
}