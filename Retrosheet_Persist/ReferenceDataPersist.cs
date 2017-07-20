using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Validation;
using System.Diagnostics;
using Retrosheet_ReferenceData.Model;

namespace Retrosheet_Persist
{
    public class ReferenceDataPersist
    {
        public static void CreateReferenceData(ReferenceDataDTO referenceDataDTO)
        {
            // ballpark instance of Ballpark class in Retrosheet_Persist.Retrosheet
            var referenceData = convertToEntity(referenceDataDTO);

            // entity data model
            var dbCtx = new retrosheetDB();

            try
            {
                dbCtx.Reference_Data.Add(referenceData);
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

        private static Reference_Data convertToEntity(ReferenceDataDTO referenceDataDTO)
        {
            var referenceData = new Retrosheet_Persist.Reference_Data();

            referenceData.record_id = referenceDataDTO.RecordID;
            referenceData.ref_data_type = referenceDataDTO.ReferenceDataType;
            referenceData.ref_data_code = referenceDataDTO.ReferenceDataCode;
            referenceData.ref_data_desc = referenceDataDTO.ReferenceDataDescription;

            return referenceData;
        }
    }
}

