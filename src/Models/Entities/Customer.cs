using System.Collections.Generic;

namespace AngularASPNETCore2WebApiAuth.Models.Entities
{
    public class Customer
    {
        public string Id { get; set; }
        public string IdentityId { get; set; }
        public AppUser Identity { get; set; }  // navigation property
  }
}
