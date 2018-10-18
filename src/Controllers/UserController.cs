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
    public async Task<bool> FollowUser([FromBody]FollowViewModel username)
    {

      //the User who wants to follow someone.
      var userId = _caller.Claims.Single(c => c.Type == "id");
      var customer = await _appDbContext.Customers.Include(c => c.Identity).SingleAsync(c => c.Identity.Id == userId.Value);

      //The user that should be followed.
      var FollowUser = await _appDbContext.Customers.Include(c => c.Identity).Where(x => x.Identity.Email == username.username).SingleAsync(c => c.Identity.Email == username.username);

      var checkFollow = _appDbContext.UserFollowers.Where(x => x.FollowerId == customer.Identity.Id).FirstOrDefault();
      if(checkFollow == null)
      {
        UserFollower userFollower = new UserFollower
        {
          AppUserId = customer.Identity.Id,
          FollowerId = FollowUser.Identity.Id,
        };

        _appDbContext.Add(userFollower);
        _appDbContext.SaveChanges();
        return true;
      }
      else
      {
        _appDbContext.Remove(checkFollow);
        _appDbContext.SaveChanges();
        return false;
      }

    }

    [Route("~/api/User/GetFollowedList")]
    [HttpGet]
    public async Task<List<FollowedUserViewModel>> GetFollowedList()
    {

      var userId = _caller.Claims.Single(c => c.Type == "id");
      var customer = await _appDbContext.Customers.Include(c => c.Identity).Include(u => u.Identity.UserFollowers).SingleAsync(c => c.Identity.Id == userId.Value);
      var userFollowrs = _appDbContext.UserFollowers.Where(x => x.AppUserId == customer.Identity.Id).Include(f => f.Follower).Include(o => o.Follower.Recepies).ToList();

      List<FollowedUserViewModel> FollowedList = new List<FollowedUserViewModel>();

      foreach(var item in userFollowrs)
      {
        foreach(var recpie in item.Follower.Recepies)
        {
          FollowedUserViewModel followedUserViewModel = new FollowedUserViewModel();
          followedUserViewModel.UserName = item.Follower.UserName.ToString();
          followedUserViewModel.Recepie = recpie;
          FollowedList.Add(followedUserViewModel);
        }
      }

      List<FollowedUserViewModel> sortedList = FollowedList.OrderByDescending(x => x.Recepie.DateTime).ToList();

      return sortedList;

    }
  }
}
