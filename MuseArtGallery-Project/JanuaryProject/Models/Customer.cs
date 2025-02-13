using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JanuaryProject.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        public string UserId { get; set; }  // For authentication later
        public virtual List<Cart> Carts { get; set; }

        public Customer()
        {
            Carts = new List<Cart>();
        }
    }
}