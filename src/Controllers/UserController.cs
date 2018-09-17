using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AngularASPNETCore2WebApiAuth.Controllers
{
  [Route("api/[controller]")]
  public class UserController : Controller
  {

    // GET api/<controller>/5
    [HttpGet("{username}")]
    public string Get(string username)
    {

      return "value";
    }
  }
}
