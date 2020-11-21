using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Reaction.Models
{
    public class Profile
    {
        [Key]
        public int ProfileId { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Username { get; set; }

        [Required]
        public bool Visibility { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}