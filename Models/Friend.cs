using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Reaction.Models
{
    public class Friend
    {
        public Friend() { 
            
        }
        [Key]
        public int FriendId { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Profile> Friends { get; set; }
    }
}