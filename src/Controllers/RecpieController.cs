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

    // GET api/Recepie/5
    [HttpGet("{id}")]
    public IActionResult GetRecpie([FromQuery] int id)
    {

      var result = _appDbContext.Recepies.Where(x => x.Id == id).Include(i => i.Ingridients).FirstOrDefault();

      return Ok(result);
    }
    [Route("~/api/Recpie/SortNew")]
    [HttpGet]
    public IActionResult SortNew()
    {
      var result = _appDbContext.Recepies.ToList().OrderByDescending(x => x.DateTime);

      return Ok(result);
    }

    [Route("~/api/Recpie/SortRated")]
    [HttpGet]
    public IActionResult SortRated()
    {
      var result = _appDbContext.Recepies.ToList().OrderByDescending(x => x.Rating);

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
    [Route("~/api/Recpie/PostNew")]
    [HttpPost]
    public async Task PostNew([FromBody]NewRecpieViewModel newRecpieViewModel)
    {

      if (ModelState.IsValid)
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

        Recepie recepie = new Recepie
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

    }

    [Route("~/api/Recpie/PostRating")]
    [HttpPost]
    public async Task PostRating([FromBody]VoteViewModel voteViewModel)
    {
      var userId = _caller.Claims.Single(c => c.Type == "id");
      var customer = await _appDbContext.Customers.Include(c => c.Identity).SingleAsync(c => c.Identity.Id == userId.Value);

      var userRecepieRating = _appDbContext.UserRecpieVotes.Where(x => x.RecpieId == voteViewModel.id.ToString()
      && x.UserId == customer.Identity.Id).FirstOrDefault();
      var recpie = _appDbContext.Recepies.Where(x => x.Id == voteViewModel.id).FirstOrDefault();

      if (customer == null || voteViewModel.rating > 5 || customer.Identity.Id == recpie.UserId)
      {
        return;
      }
      else
      {

        if (userRecepieRating == null)
        {
          UserRecpieVote userRecpieVote = new UserRecpieVote
          {
            RecpieId = voteViewModel.id.ToString(),
            UserId = customer.Identity.Id,
            Score = voteViewModel.rating
          };
          recpie.Rating = recpie.Rating + voteViewModel.rating;
          recpie.TotalVotes = recpie.TotalVotes + 1;

          _appDbContext.Update(recpie);
          _appDbContext.Add(userRecpieVote);
          _appDbContext.SaveChanges();
        }
        else
        {
          recpie.Rating = recpie.Rating - userRecepieRating.Score + voteViewModel.rating;
          userRecepieRating.Score = voteViewModel.rating;

          _appDbContext.Update(recpie);
          _appDbContext.Update(userRecepieRating);
          _appDbContext.SaveChanges();
        }
      }

    }

    // DELETE api/<controller>/5
    [HttpDelete("{id}")]
    public async Task<List<Recepie>> Delete([FromQuery] int deleteId)
    {
      var userId = _caller.Claims.Single(c => c.Type == "id");
      var customer = await _appDbContext.Customers.Include(c => c.Identity).SingleAsync(c => c.Identity.Id == userId.Value);
      var recpie = _appDbContext.Recepies.Where(x => x.Id == deleteId).FirstOrDefault<Recepie>();

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
