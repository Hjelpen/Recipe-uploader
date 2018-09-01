using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularASPNETCore2WebApiAuth.Models.Entities
{
  public class Ingridient
  {
    public int Id { get; set; }
    public string Content { get; set; }

    public int RecepieId { get; set; }
  }
}
