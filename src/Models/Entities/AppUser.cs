
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace AngularASPNETCore2WebApiAuth.Models.Entities
{
    // Add profile data for application users by adding properties to this class
  public class AppUser : IdentityUser
  {
    // Extended Properties
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PictureUrl { get; set; }
    public string Bio { get; set; }

    public List<Recepie> Recepies { get; set; }
    public List<UserFollower> UserFollowers { get; set; }
    public List<UserRecpieVote> UserRecpieVotes { get; set; }

  }
}
