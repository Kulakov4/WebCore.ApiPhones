using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebCore.ApiPhones.Interfaces;
using WebCore.ApiPhones.Models;

namespace WebCore.ApiPhones.Services
{
    /// <summary>
    /// Сервис для работы с Oracle
    /// </summary>
    public class OracleService : IPhoneBook, IPhoto, IUnit
    {
        private readonly ConnectionFactory _factory;
        private readonly string base_sql = @$"
select 
    distinct
    phy.idphysical PhysicalId, phy.lastname, phy.firstname, phy.patronymic, con.UnitID, con.Unit, con.post
    , c.Address || ', каб. ' || AU.TITLE Office
    , ph2.id_phone PhoneId,
    ph2.Phone || NVL2 (pht.abbreviation, ' (' || pht.abbreviation || ')', '') PhoneNumber,
    PH2.RAWPHONE,
    u.id_user EmailId,
    nvl2(u.username, u.username || '@rb.asu.ru', '') email
        
from table(Personnel.PersonnelPack2.GetContractPhysicals) phy
join Personnel.Contracts con on phy.idcontract = con.idcontract and con.COLLATERALAGREEMENT = 0
join cdb_dat_study_process.Auditorium au on AU.ИДЕНТИФИКАТОРПОДРАЗДЕЛЕНИЯ = con.UnitID
JOIN cdb_dat_study_process.Corps c ON AU.CORPS = C.ID_CORPS
join Personnel.Phones ph on PH.IDAUDITORIUM = AU.ID_AUDITORIUM
join Personnel.employeephones eph on eph.ИдентификаторФЛ = phy.IDPhysical and EPH.IDPHONE = PH.ID_PHONE

join Personnel.employeephones eph2 on eph2.ИдентификаторФЛ = phy.IDPhysical
join Personnel.Phones ph2 on EPH2.IDPHONE = PH2.ID_PHONE
JOIN Personnel.PhoneTypes pht ON PH2.IDPHONETYPE = PHT.ID_PHONETYPE

left join audit_base.users u on u.ИдентификаторФЛ = phy.IDPhysical
where (0=0)
order by phy.LastName, phy.FirstName, phy.Patronymic, Office, PhoneNumber";

        public OracleService(ConnectionFactory factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// получить не сгруппированные данные о абонентах телефонного справочника
        /// </summary>
        public async Task<IEnumerable<PhoneBookRaw>> GetPhoneBookRaw(string sql)
        {
            using (IDbConnection dbcon = _factory.CreateConnection())
            {
                var result = await dbcon.QueryAsync<PhoneBookRaw>(sql);
                return result;
            }
        }

        /// <summary>
        /// получить не сгруппированные данные о абонентах телефонного справочника по фамилии
        /// </summary>
        public async Task<IEnumerable<PhoneBookRaw>> GetPhoneBookByLastName(string lastName)
        {
            var sql = base_sql.Replace("0=0", $"phy.lastname like '{lastName}%'");
            return await GetPhoneBookRaw(sql);
        }

        /// <summary>
        /// получить не сгруппированные данные о абонентах телефонного справочника по номеру телефона
        /// </summary>
        public async Task<IEnumerable<PhoneBookRaw>> GetPhoneBookByPhone(string phone)
        {
            var s = @$"phy.idphysical in
                    (
                        select eph3.ИдентификаторФЛ
                        from
                        Personnel.employeephones eph3
                        join Personnel.Phones ph3 on EPH3.IDPHONE = PH3.ID_PHONE
                        where PH3.RAWPHONE like '{phone}%'
                    )";
            var sql = base_sql.Replace("0=0", s);
            return await GetPhoneBookRaw(sql);
        }

        /// <summary>
        /// получить не сгруппированные данные о абонентах телефонного справочника по идентификатору подразделения
        /// </summary>
        public async Task<IEnumerable<PhoneBookRaw>> GetPhoneBookByUnitID(string unitID)
        {
            var s = @$"con.UnitID = '{unitID}'";
            var sql = base_sql.Replace("0=0", s);
            return await GetPhoneBookRaw(sql);
        }

        /// <summary>
        /// получить фотографию
        /// </summary>
        /// <param name="id">ид физлица</param>
        public async Task<byte[]> GetPhoto(string id)
        {
            try
            {
                using (IDbConnection dbcon = _factory.CreateConnection())
                {
                    var result = await dbcon.QueryFirstAsync<byte[]>(@"select PHOTO
                    from Table(Personnel.PERSONNELPACK.GetPhotos)
                    where IDPhysical = :pid", new { pid = id });
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<IEnumerable<UnitClass>> GetUnits()
        {
            var sql = @"
select distinct CON.UNITID IDUnit, CON.UNIT
from table(Personnel.PersonnelPack2.GetContractPhysicals) phy
join Personnel.Contracts con on phy.idcontract = con.idcontract and con.COLLATERALAGREEMENT = 0
join cdb_dat_study_process.Auditorium au on AU.ИДЕНТИФИКАТОРПОДРАЗДЕЛЕНИЯ = con.UnitID
JOIN cdb_dat_study_process.Corps c ON AU.CORPS = C.ID_CORPS
join Personnel.Phones ph on PH.IDAUDITORIUM = AU.ID_AUDITORIUM
join Personnel.employeephones eph on eph.ИдентификаторФЛ = phy.IDPhysical and EPH.IDPHONE = PH.ID_PHONE
order by CON.UNIT";

            using (IDbConnection dbcon = _factory.CreateConnection())
            {
                var result = await dbcon.QueryAsync<UnitClass>(sql);
                return result;
            }
        }
    }
}
