using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_ISBAN.Model;
using Microsoft.AspNetCore.Mvc;

namespace API_ISBAN.Controllers
{   
    [Route("api/[controller]")]
    [ApiController]
    public class BestStoriesController : Controller
    {
        [HttpGet]
        public IEnumerable<dynamic> Get()
        {
            return HackNewsWrapper.GetData();
        }
    }
}