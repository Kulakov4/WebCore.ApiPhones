using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCore.ApiPhones.Controllers
{
    [ApiController]
    [Route("api/v1/")]
    public class SwaggerController : Controller
    {
        /// <summary>
        /// Перенаправление на swagger
        /// </summary>
        /// <returns>объект</returns>
        [HttpGet]
        [Route("swagger")]
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }
    }
}
