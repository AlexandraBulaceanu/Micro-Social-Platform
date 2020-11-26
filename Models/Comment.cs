using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Reaction.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public int Likes { get; set; }

        public DateTime Date { get; set; }

        //public string Username { get; set; }

        public int PostId { get; set; }

        public virtual Post Post { get; set; }
    }
}