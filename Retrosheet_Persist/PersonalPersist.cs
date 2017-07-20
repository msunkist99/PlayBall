using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Retrosheet_ReferenceData.Model;

namespace Retrosheet_Persist
{
    public class PersonalPersist
    {
        public static void CreatePersonal(PersonalDTO personalDTO)
        {
            // ballpark instance of Player class in Retrosheet_Persist.Retrosheet
            var personal = convertToEntity(personalDTO);

            // entity data model
            var dbCtx = new retrosheetDB();

            dbCtx.Personals.Add(personal);
            try
            {
                dbCtx.SaveChanges();
            }
            catch (Exception e)
            {
                string text;
                text = e.Message;
            }
        }

        private static Personal convertToEntity(PersonalDTO personalDTO)
        {
            var personal = new Personal();

            personal.record_id = personalDTO.RecordID;
            personal.person_id = personalDTO.PersonID;
            personal.last_name = personalDTO.LastName;
            personal.first_name = personalDTO.FirstName;
            personal.career_date = personalDTO.CareerDate;
            personal.role = personalDTO.Role;

            return personal;
        }
    }
}