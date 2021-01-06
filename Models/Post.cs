using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Reaction.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public int Likes { get; set; }

        public DateTime Date { get; set; }

        //public int ProfileId { get; set; }

        //public virtual Profile Profile { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public int GroupId { get; set; }
        //public virtual Group Group { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}