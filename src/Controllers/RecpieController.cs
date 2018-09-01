using AngularASPNETCore2WebApiAuth.Data;
using AngularASPNETCore2WebApiAuth.Models.Entities;
using AngularASPNETCore2WebApiAuth.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AngularASPNETCore2WebApiAuth.Controllers
{
  [Route("api/[controller]")]
  public class RecpieController : Controller
  {

    private readonly ApplicationDbContext _appDbContext;
    private readonly UserManager<AppUser> _userManager;
    private readonly IHostingEnvironment _hostingEnvironment;
    private readonly ClaimsPrincipal _caller;

    public RecpieController(UserManager<AppUser> userManager, ApplicationDbContext appDbContext, IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
    {
      _userManager = userManager;
      _appDbContext = appDbContext;
      _hostingEnvironment = hostingEnvironment;
      _caller = httpContextAccessor.HttpContext.User;
    }

    // GET: api/recpie/home
    [HttpGet]
    public IActionResult Home()
    {
      var result = _appDbContext.Recepies.ToList();

      return Ok(result);

    }

    // GET api/Recpie/5
    [HttpGet("{id}")]
    public IActionResult GetRecpie([FromQuery] int id)
    {

      var result = _appDbContext.Recepies.Where(x => x.Id == id).Include(i => i.Ingridients);

      Recpie recepie = new Recpie();

      foreach (var item in result)
      {
        recepie.Content = item.Content;
        recepie.DateTime = item.DateTime;
        recepie.Id = item.Id;
        recepie.ImageName = item.ImageName;
        recepie.ImageUrl = item.ImageUrl;
        recepie.Ingridients = item.Ingridients;
        recepie.PostedBy = item.PostedBy;
        recepie.Rating = item.Rating;
        recepie.Title = item.Title;
      }

      return Ok(recepie);
    }
    [Route("~/api/Recpie/SortNew")]
    [HttpGet]
    public IActionResult SortNew()
    {
      var result = _appDbContext.Recepies.ToList().OrderByDescending(x => x.DateTime);

      return Ok(result);
    }

    [Route("~/api/Recpie/Search")]
    [HttpGet]
    public IActionResult Search([FromQuery]string searchQuery)
    {

      if (string.IsNullOrEmpty(searchQuery))
      {
        var emptyQuery = _appDbContext.Recepies.ToList();
        return Ok(emptyQuery);
      }

      var result = _appDbContext.Recepies.Where(x => x.Title.Contains(searchQuery)).ToList();

      return Ok(result);
    }

    // POST api/recepie/post
    [HttpPost]
    public async Task PostAsync([FromBody]NewRecpieViewModel newRecpieViewModel)
    {

      var userId = _caller.Claims.Single(c => c.Type == "id");
      var customer = await _appDbContext.Customers.Include(c => c.Identity).SingleAsync(c => c.Identity.Id == userId.Value);

      var uploads = Path.Combine(_hostingEnvironment.ContentRootPath, "src");
      var pathToData = Path.GetFullPath(Path.Combine(uploads, "assets"));
      var guid = Guid.NewGuid();
      var fileUrl = guid + newRecpieViewModel.FileName;
      var filePath = Path.Combine(pathToData, fileUrl);

      using (var ms = new MemoryStream(newRecpieViewModel.File, 0, newRecpieViewModel.File.Length))
      {
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
          ms.CopyTo(stream);
        }
      }

       Recpie recepie = new Recpie
      {
        UserId = customer.IdentityId,
        ImageUrl = fileUrl,
        ImageName = newRecpieViewModel.FileName,
        DateTime = DateTime.UtcNow,
        Title = newRecpieViewModel.Title,
        PostedBy = customer.Identity.UserName,
        Content = newRecpieViewModel.Content,
        TotalVotes = 0,
        Rating = 0
      };

      _appDbContext.Add(recepie);
      _appDbContext.SaveChanges();


      List<Ingridient> ingredient = new List<Ingridient>();

      foreach (var item in newRecpieViewModel.Ingredients)
      {
        Ingridient ingridientItem = new Ingridient();
        ingridientItem.Content = item;
        ingridientItem.RecepieId = recepie.Id;
        ingredient.Add(ingridientItem);
      }

      _appDbContext.AddRange(ingredient);
      _appDbContext.SaveChanges();
    }

    // PUT api/<controller>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody]string value)
    {
    }

    [HttpPost]
    public async Task PostRating([FromBody]int rating, int id)
    {
      var userId = _caller.Claims.Single(c => c.Type == "id");
      var customer = await _appDbContext.Customers.Include(c => c.Identity).SingleAsync(c => c.Identity.Id == userId.Value);

      if (customer == null || rating > 5)
      {
        return;
      }
      else
      {
        var userRecepieRating = _appDbContext.UserRecpieVotes.Where(x => x.RecpieId == id.ToString()
         && x.UserId == customer.Identity.Id).FirstOrDefault();
          var recpie = _appDbContext.Recepies.Where(x => x.Id == id).FirstOrDefault();

        if (userRecepieRating == null)
        {
          UserRecpieVote userRecpieVote = new UserRecpieVote
          {
            RecpieId = id.ToString(),
            UserId = customer.Identity.Id,
            Score = rating
          };
          recpie.Rating = recpie.Rating + rating;
          recpie.TotalVotes = recpie.TotalVotes + 1;

          _appDbContext.Update(recpie);
          _appDbContext.Add(userRecpieVote);
          _appDbContext.SaveChanges();
        }
        else
        {
          recpie.Rating = recpie.Rating - userRecepieRating.Score + rating;
          userRecepieRating.Score = rating;

          _appDbContext.Update(recpie);
          _appDbContext.Update(userRecepieRating);
          _appDbContext.SaveChanges();
        }
      }

    }

    // DELETE api/<controller>/5
    [HttpDelete("{id}")]
    public async Task<List<Recpie>> Delete([FromQuery] int deleteId)
    {
      var userId = _caller.Claims.Single(c => c.Type == "id");
      var customer = await _appDbContext.Customers.Include(c => c.Identity).SingleAsync(c => c.Identity.Id == userId.Value);
      var recpie = _appDbContext.Recepies.Where(x => x.Id == deleteId).FirstOrDefault<Recpie>();

      if (recpie.UserId == customer.Identity.Id)
      {
        var ingredients = _appDbContext.Ingridients.Where(x => x.RecepieId == deleteId).ToList();

        _appDbContext.Remove(recpie);
        _appDbContext.RemoveRange(ingredients);

        var uploads = Path.Combine(_hostingEnvironment.ContentRootPath, "src");
        var pathToData = Path.GetFullPath(Path.Combine(uploads, "assets"));
        var filePath = Path.Combine(pathToData, recpie.ImageUrl);
        System.IO.File.Delete(filePath);

        await _appDbContext.SaveChangesAsync();

      }

      var recpieList = _appDbContext.Recepies.ToList();

      return recpieList;
    }
  }
}
