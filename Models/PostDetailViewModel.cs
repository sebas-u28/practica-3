using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using practica.Integration.Exchange;

namespace practica.Models
{
    public class PostDetailViewModel
    {
        public Post Post { get; set; } = new();
        public User? User { get; set; }
        public List<Comment> Comments { get; set; } = new();
    }
}
