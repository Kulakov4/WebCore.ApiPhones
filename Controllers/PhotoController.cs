using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCore.ApiPhones.Interfaces;
using WebCore.ApiBase.Infrastructure.ActionResult;
using System.Net;

namespace WebCore.ApiPhones.Controllers
{
    [ApiController]
    [Route("api/v1/")]
    public class PhotoController : Controller
    {
        private readonly IPhoto photoService;

        public PhotoController(IPhoto service)
        {
            photoService = service;
        }

        /// <summary>
        /// получить фотографию
        /// </summary>
        /// <param name="id">ид физлица</param>
        /// <returns>фотография</returns>
        [HttpGet]
        [Route("photo/{id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(SuccessResult<string>), (int)HttpStatusCode.OK)]

        public async Task<IActionResult> PhotoByldAsync(string id)
        {
            var photo = await photoService.GetPhoto(id);
            if (photo != null)
            {
                return new JsonResult(new SuccessResult<string>(Convert.ToBase64String(photo)));
            }

            return NotFound();
        }
    }
}
