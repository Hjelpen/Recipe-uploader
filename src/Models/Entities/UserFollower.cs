using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularASPNETCore2WebApiAuth.Models.Entities
{
  public class UserFollower
  {
    public int UserFollowerId { get; set; }


    public string AppUserId { get; set; }
    public virtual AppUser AppUser { get; set; }

    public string FollowerId { get; set; }
    public virtual AppUser Follower { get; set; }

  }
}
