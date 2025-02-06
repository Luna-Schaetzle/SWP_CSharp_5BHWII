using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUIBasics.Models
{
    public class Basket
    {
        [Key]
        public int BasketId { get; set; }
        public User user { get; set; }
        public Article article { get; set; }


    }
}