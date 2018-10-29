using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularASPNETCore2WebApiAuth.ViewModels
{
  public class NewRecpieViewModel
  {
    public string Title { get; set; }
    public string Content { get; set; }
    public string[] Ingredients { get; set; }
    public string File { get; set; }
    public string FileName { get; set;}
    public string FileType { get; set; }
  }

}
