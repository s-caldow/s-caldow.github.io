using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Homepage.Models;
using Homepage.NewsAggregator;
using Microsoft.WindowsAzure.Storage.Blob.Protocol;

namespace Homepage.Controllers
{
    public class HomeController : Controller
    {
	    public int PageSize = 7;

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

	    public IActionResult NewsAggregator(int? page)
	    {
			RSSReader reader = new RSSReader();
		    var newsFeed = reader.GetNewsFeed();
		    if (!page.HasValue)
		    {
			    page = 1;
		    } 
		    ViewBag.page = page;
		    ViewBag.lastPage = Math.Ceiling((decimal)(newsFeed.Count / 5));
		    if (page > 1)
		    {
			    return View(newsFeed.Skip(((page - 1) * 5).GetValueOrDefault()).Take(PageSize));
			}
		    else
		    {
			    return View(newsFeed.Take(PageSize));
			}
			
	    }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
