using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularASPNETCore2WebApiAuth.Models.Entities
{
  public class UserRecpieVote
  {
    public int Id { get; set; }

    public string UserId { get; set; }
    public string RecpieId { get; set; }

    public int Score { get; set; }

  }
}
