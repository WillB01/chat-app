using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Controllers.Search
{
    public class SearchController : Controller
    {
        [HttpGet]
        public string[] Friends()
        {
            var countries = new string[] { "hej", "kewl" };
            return countries;
        }
    }
}
