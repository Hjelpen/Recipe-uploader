using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AngularASPNETCore2WebApiAuth.Data;
using AngularASPNETCore2WebApiAuth.Models.Entities;
using AngularASPNETCore2WebApiAuth.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AngularASPNETCore2WebApiAuth.Controllers
{
  [Route("api/[controller]")]
  public class UserController : Controller
  {
    private readonly ApplicationDbContext _appDbContext;
    private readonly UserManager<AppUser> _userManager;
    private readonly ClaimsPrincipal _caller;

    public UserController(UserManager<AppUser> userManager, ApplicationDbContext appDbContext, IHttpContextAccessor httpContextAccessor)
    {
      _userManager = userManager;
      _appDbContext = appDbContext;
      _caller = httpContextAccessor.HttpContext.User;
    }

    // GET api/<controller>/5
    [HttpGet("{username}")]
    public async Task<UserViewModel> Get([FromQuery]string username)
    {
      //Get info for the user being viewed
      var customer = await _appDbContext.Customers.Include(c => c.Identity).Include(x => x.Identity.Recepies).SingleAsync(c => c.Identity.UserName == username);    

      UserViewModel userViewModel = new UserViewModel
      {
        PictureUrl = customer.Identity.PictureUrl,
        UserName = customer.Identity.UserName,
        Recepies = customer.Identity.Recepies,
        Bio = customer.Identity.Bio
      };

      return userViewModel;
    }

    [Route("~/api/User/FollowUser")]
    [HttpPost]
    public async Task<List<UserFollower>> FollowUser([FromBody]FollowViewModel username)
    {

      //the User who wants to follow someone.
      var userId = _caller.Claims.Single(c => c.Type == "id");
      var customer = await _appDbContext.Customers.Include(c => c.Identity).SingleAsync(c => c.Identity.Id == userId.Value);

      //The user that should be followed.
      var FollowUser = await _appDbContext.Customers.Where(x => x.Identity.Email == username.username).SingleAsync(c => c.Identity.Email == username.username);

      var checkFollow = customer.Identity.UserFollowers.Where(x => x.Identity.Id == FollowUser.Identity.Id);
      if(checkFollow == null)
      {
        UserFollower userFollower = new UserFollower
        {
          FollowerId = FollowUser.Identity.Id,
          IdentityId = customer.Identity.Id
        };

        _appDbContext.Add(userFollower);
      }
      else
      {
        _appDbContext.Remove(checkFollow);
      }

      _appDbContext.SaveChanges();

      return customer.Identity.UserFollowers.ToList();
    }
  }
}
