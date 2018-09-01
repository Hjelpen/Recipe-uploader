using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularASPNETCore2WebApiAuth.Models.Entities
{
  public class Comment
  {
      public int Id { get; set; }
      public string Content { get; set; }

      public DateTime Datetime { get; set; }

      public string AppUserId { get; set; }

  }
}
