using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
    public class PhoneBookController : ControllerBase
    {
        private readonly IPhoneBook service;

        public PhoneBookController(IPhoneBook s)
        {
            service = s;
        }


        private IActionResult GetPhoneBook(IEnumerable<PhoneBookRaw> rawData)
        {
            if (rawData == null || rawData.Count() == 0)
            {
                return NotFound();
            }

            var phoneBookRecors = new List<PhoneBookRecord>();
            PhoneBookRecord phoneBookUser = null;
            Phone phone = null;
            Email email = null;
            foreach (var data in rawData)
            {
                if ((phoneBookUser == null) || (phoneBookUser.PhysicalId != data.PhysicalId))
                {
                    phoneBookUser = new PhoneBookRecord()
                    {
                        PhysicalId = data.PhysicalId,
                        LastName = data.LastName,
                        FirstName = data.FirstName,
                        Patronymic = data.Patronymic,
                        Post = data.Post,
                        UnitId = data.UnitId,
                        Unit = data.Unit,
                        Office = data.Office,
                        Phones = new List<Phone>(),
                        Emails = new List<Email>()
                    };
                    phoneBookRecors.Add(phoneBookUser);
                    phone = null;
                    email = null;
                }

                // Добавляем телефон к абоненту
                if ((phone == null) || (phone.Id != data.PhoneId))
                {
                    // Ищем такой телефон в списке телефонов абонента
                    var p = phoneBookUser.Phones.Find(phone => phone.Id == data.PhoneId);
                    // Если такого телефона ещё не было
                    if (p == null)
                    {
                        phone = new Phone()
                        {
                            Id = data.PhoneId,
                            PhoneNumber = data.PhoneNumber,
                            RawPhone = data.RawPhone
                        };
                        phoneBookUser.Phones.Add(phone);
                    }
                }

                // Добавляем почту к абоненту
                if (data.EmailId.HasValue && ( (email == null) || (email.Id != data.EmailId) ) )
                {
                    email = new Email()
                    {
                        Id = data.EmailId.Value,
                        email = data.email
                    };
                    phoneBookUser.Emails.Add(email);
                }
            }
            return new JsonResult(new SuccessResult<IEnumerable<PhoneBookRecord>>(phoneBookRecors));
        }

        /// <summary>
        /// Получить абонентов телефонного справочника по первым цифрам номера телефона (только цифры, без пробелов)
        /// </summary>
        /// <param name="phone">первые цифры номера телефона</param>
        /// <returns>объект</returns>
        [HttpGet]
        [Route("phone/{phone}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(SuccessResult<IEnumerable<PhoneBookRecord>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPhoneBookByPhone(string phone)
        {
            var rawData = await service.GetPhoneBookByPhone(phone);
            return GetPhoneBook(rawData);
        }

        /// <summary>
        /// Получить абонентов телефонного справочника по идентификатору подразделения
        /// </summary>
        /// <param name="unitID">идентификатор подразделения</param>
        /// <returns>объект</returns>
        [HttpGet]
        [Route("unit/{unitID}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(SuccessResult<IEnumerable<PhoneBookRecord>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPhoneBookByUnitID(string unitID)
        {
            var rawData = await service.GetPhoneBookByUnitID(unitID);
            return GetPhoneBook(rawData);
        }

        /// <summary>
        /// Получить абонентов телефонного справочника по первым буквам фамилии
        /// </summary>
        /// <param name="lastname">первые буквы фамилии</param>
        /// <returns>объект</returns>
        [HttpGet]
        [Route("lastname/{lastname}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(SuccessResult<IEnumerable<PhoneBookRecord>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPhoneBookByLastName(string lastname)
        {
            var rawData = await service.GetPhoneBookByLastName(lastname);
            return GetPhoneBook(rawData);
        }

        /// <summary>
        /// Получить всех абонентов телефонного справочника
        /// </summary>
        /// <returns>объект</returns>
        [HttpGet]
        [Route("all")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(SuccessResult<IEnumerable<PhoneBookRecord>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Index()
        {
            return await GetPhoneBookByLastName("");
        }
    }
}
