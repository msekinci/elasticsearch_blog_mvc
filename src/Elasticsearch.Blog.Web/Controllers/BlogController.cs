using Elasticsearch.Blog.Web.Services;
using Elasticsearch.Blog.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Elasticsearch.Blog.Web.Controllers;

public class BlogController : Controller
{
    private readonly BlogService _blogService;

    public BlogController(BlogService blogService)
    {
        _blogService = blogService;
    }

    [HttpGet]
    public async Task<IActionResult> Search(string s = "")
    {
        ViewBag.SearchText = s;
        var blogList = await _blogService.Search(s);
        return View(blogList);
    }



    // GET
    public IActionResult Save()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Save(BlogCreateViewModel model)
    {
        var isSuccess = await _blogService.SaveAsync(model);

        if (!isSuccess)
        {
            TempData["result"] = "Error";
        }
        else
        {
            TempData["result"] = "Successful";
        }
        
        return RedirectToAction("Save");
    }
}