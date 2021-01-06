using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Reaction.Models
{
    public class Profile
    {
        public enum Visible
        {
            Public,
            Private
        }

        [Key]
        public int ProfileId { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Username { get; set; }

        public string Email { get; set; }

        [Required]
        public Visible Visibility { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

        public virtual ICollection<Group> Groups { get; set; }

        public virtual ICollection<Profile> FriendRequests { get; set; }

        public virtual ICollection<Friend> Friends { get; set; }

        public IEnumerable<Visible> VisibleItems { get; set; }
    }
}

//TODO: add some other fields