using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Reaction.Models
{
    public class Group
    {
        public Group()
        {
           
        }

        public enum VisibleGroup
        {
            Public,
            Private
        }

        [Key]
        public int GroupId { get; set; }
        [Required(ErrorMessage = "Enter a name")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        [Required]
        public VisibleGroup Visibility { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

        public virtual ICollection<Profile> Profiles { get; set; }
    }
}