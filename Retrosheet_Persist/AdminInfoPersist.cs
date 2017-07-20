using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Validation;
using System.Diagnostics;
using Retrosheet_EventData.Model;

namespace Retrosheet_Persist
{
    public class AdminInfoPersist
    {
        public static void CreateAdminInfo(AdminInfoDTO adminInfoDTO)
        {
            // ballpark instance of Player class in Retrosheet_Persist.Retrosheet
            var adminInfo = convertToEntity(adminInfoDTO);

            // entity data model
            var dbCtx = new retrosheetDB();

            dbCtx.Admin_Info.Add(adminInfo);

            try
            {
                dbCtx.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                                                validationError.PropertyName,
                                                validationError.ErrorMessage);
                    }
                }
            }
            catch (Exception e)
            {
                string text;
                text = e.Message;
            }
        }

        private static Admin_Info convertToEntity(AdminInfoDTO adminInfoDTO)
        {
            var adminInfo = new Admin_Info();

            adminInfo.record_id = adminInfoDTO.RecordID;
            adminInfo.game_id = adminInfoDTO.GameID;
            adminInfo.admin_info_type = adminInfoDTO.AdminInfoType;
            adminInfo.admin_inf_value = adminInfoDTO.AdminInfoValue;

            return adminInfo;
        }
    }
}
