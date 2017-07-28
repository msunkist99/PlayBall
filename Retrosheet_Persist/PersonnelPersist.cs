using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Retrosheet_ReferenceData.Model;

namespace Retrosheet_Persist
{
    public class PersonnelPersist
    {
        public static void CreatePersonnel(PersonnelDTO personnelDTO)
        {
            // ballpark instance of Player class in Retrosheet_Persist.Retrosheet
            var personnel = convertToEntity(personnelDTO);

            // entity data model
            var dbCtx = new retrosheetDB();

            dbCtx.Personnels.Add(personnel);
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

        private static Personnel convertToEntity(PersonnelDTO personnelDTO)
        {
            var personnel = new Personnel();

            personnel.record_id = personnelDTO.RecordID;
            personnel.person_id = personnelDTO.PersonID;
            personnel.last_name = personnelDTO.LastName;
            personnel.first_name = personnelDTO.FirstName;
            personnel.career_date = personnelDTO.CareerDate;
            personnel.role = personnelDTO.Role;

            return personnel;
        }
    }
}