using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebCore.ApiBase.Infrastructure.ActionResult;
using WebCore.ApiPhones.Interfaces;
using WebCore.ApiPhones.Models;

namespace WebCore.ApiPhones.Controllers
{
    [ApiController]
    [Route("api/v1/")]
    public class UnitController : ControllerBase
    {
        private readonly IUnit service;

        public UnitController(IUnit s)
        {
            service = s;
        }

        /// <summary>
        /// Получить список всех отделов, в которых работают сотрудники. Сортировка по алфавиту
        /// </summary>
        /// <returns>объект</returns>
        [HttpGet]
        [Route("unit")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(SuccessResult<IEnumerable<PhoneBookRecord>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Index()
        {
            var unitList = await service.GetUnits();
            if ( unitList == null || (unitList.Count() == 0) )
                return NotFound();

            return new JsonResult(new SuccessResult<IEnumerable<UnitClass>>(unitList));
        }
    }
}
