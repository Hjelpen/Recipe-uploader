using AngularASPNETCore2WebApiAuth.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularASPNETCore2WebApiAuth.ViewModels
{
  public class UserViewModel
  {
    public string UserName { get; set; }
    public string PictureUrl { get; set; }
    public string Bio { get; set; }
    public bool FollowStatus { get; set; }
    public List<Recepie> Recepies { get; set; }
    public List<UserFollower> UserFollowers { get; set; }

  }
}
