
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AngularASPNETCore2WebApiAuth.Data;
using AngularASPNETCore2WebApiAuth.Models.Entities;
using AngularASPNETCore2WebApiAuth.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
 

namespace AngularASPNETCore2WebApiAuth.Controllers
{
  [Authorize(Policy = "ApiUser")]
  [Route("api/[controller]/[action]")]
  public class DashboardController : Controller
  {
    private readonly ClaimsPrincipal _caller;
    private readonly ApplicationDbContext _appDbContext;
    private readonly IHostingEnvironment _hostingEnvironment;

    public DashboardController(UserManager<AppUser> userManager, ApplicationDbContext appDbContext, IHttpContextAccessor httpContextAccessor, IHostingEnvironment hostingEnvironment)
    {
      _caller = httpContextAccessor.HttpContext.User;
      _appDbContext = appDbContext;
      _hostingEnvironment = hostingEnvironment;
    }

    // GET api/dashboard/home
    [HttpGet]
    public async Task<List<Recepie>> Home()
    {
      var userId = _caller.Claims.Single(c => c.Type == "id");
      var customer = await _appDbContext.Customers.Include(c => c.Identity).Include(y => y.Identity.Recepies).SingleAsync(c => c.Identity.Id == userId.Value);

      List<Recepie> recepies = new List<Recepie>();
      foreach (var item in customer.Identity.Recepies)
      {
        recepies.Add(item);
      }

      return recepies.ToList();
    }

    [Route("~/api/Dashboard/GetProfile")]
    public async Task<UserViewModel> GetProfile()
    {

      var userId = _caller.Claims.Single(c => c.Type == "id");
      var customer = await _appDbContext.Customers.Include(c => c.Identity).SingleAsync(c => c.Identity.Id == userId.Value);
      var appuser = _appDbContext.Users.Where(x => x.Id == customer.IdentityId).FirstOrDefault();

      UserViewModel userViewModel = new UserViewModel();

      userViewModel.UserName = appuser.UserName;
      userViewModel.PictureUrl = appuser.PictureUrl;

      return userViewModel;
    }

    [Route("~/api/Dashboard/ProfilePicture")]
    [HttpPost]
    public async Task<string> SaveProfilePicture([FromBody]ProfilePictureViewModel profilePictureViewModel)
    {
      var userId = _caller.Claims.Single(c => c.Type == "id");
      var customer = await _appDbContext.Customers.Include(c => c.Identity).SingleAsync(c => c.Identity.Id == userId.Value);

      if(!string.IsNullOrEmpty(customer.Identity.PictureUrl))
      {
        var uploads = Path.Combine(_hostingEnvironment.ContentRootPath, "src");
        var pathToData = Path.GetFullPath(Path.Combine(uploads, "assets/profilepicture"));
        var filePath = Path.Combine(pathToData, customer.Identity.PictureUrl);

        System.IO.File.Delete(filePath);

        var profileUrl = SaveImage(profilePictureViewModel);
        customer.Identity.PictureUrl = profileUrl;
      }
      else
      {
        var profileUrl = SaveImage(profilePictureViewModel);
        customer.Identity.PictureUrl = profileUrl;
      }

      _appDbContext.SaveChanges();

      return customer.Identity.PictureUrl;
    }

    public string SaveImage(ProfilePictureViewModel profilePictureViewModel)
    {
      var fileUrl = "";

      if (profilePictureViewModel.File != null && profilePictureViewModel.File.Length > 0)
      {
        var uploads = Path.Combine(_hostingEnvironment.ContentRootPath, "src");
        var pathToData = Path.GetFullPath(Path.Combine(uploads, "assets/profilepicture"));
        var guid = Guid.NewGuid();
        fileUrl = guid + profilePictureViewModel.FileName;
        var filePath = Path.Combine(pathToData, fileUrl);

        using (var ms = new MemoryStream(profilePictureViewModel.File, 0, profilePictureViewModel.File.Length))
        {
          using (var stream = new FileStream(filePath, FileMode.Create))
          {
            ms.CopyTo(stream);
            stream.Dispose();
          }
          ms.Dispose();
        }

        return fileUrl;

      }

      return fileUrl;
    }
  }


}
