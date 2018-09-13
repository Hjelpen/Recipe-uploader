using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularASPNETCore2WebApiAuth.ViewModels
{
  public class ProfilePictureViewModel
  {
    public byte[] File { get; set; }
    public string FileName { get; set; }
    public string FileType { get; set; }
  }
}
