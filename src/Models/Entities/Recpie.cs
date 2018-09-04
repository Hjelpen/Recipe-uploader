using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularASPNETCore2WebApiAuth.Models.Entities
{
  public class Recepie
  {
    public int Id { get; set; }

    public string Title { get; set; }
    public string Content { get; set; }
    public string ImageUrl { get; set; }
    public string ImageName { get; set; }
    public List <Ingridient> Ingridients { get; set; }

    public int Rating { get; set; }
    public int TotalVotes { get; set; }
    public string PostedBy { get; set; }
    public DateTime DateTime { get; set; }

    [JsonIgnore]
    public string UserId { get; set; }
  }
}
