using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularASPNETCore2WebApiAuth.Models.Entities
{
  public class UserFollower
  {
    public int Id { get; set; }

    public string IdentityId { get; set; }
    public AppUser Identity { get; set; }

    public string FollowerId { get; set; }
  }
}
