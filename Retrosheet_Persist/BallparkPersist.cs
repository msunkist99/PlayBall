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
    public class BallparkPersist
    {
        public static void CreateBallpark(BallparkDTO ballparkDTO)
        {
            // ballpark instance of Ballpark class in Retrosheet_Persist.Retrosheet
            var ballpark = convertToEntity(ballparkDTO);

            // entity data model
            var dbCtx = new retrosheetDB();

            dbCtx.Ballparks.Add(ballpark);
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

        private static Ballpark convertToEntity(BallparkDTO ballparkDTO)
        {
            var ballpark = new Retrosheet_Persist.Ballpark();

            ballpark.record_id = ballparkDTO.RecordID;
            ballpark.ballpark_id = ballparkDTO.ID;
            ballpark.name = ballparkDTO.Name;
            ballpark.aka = ballparkDTO.AKA;
            ballpark.city = ballparkDTO.City;
            ballpark.state = ballparkDTO.State;
            ballpark.start_date = ballparkDTO.StartDate;
            ballpark.end_date = ballparkDTO.EndDate;
            ballpark.league = ballparkDTO.League;
            ballpark.notes = ballparkDTO.Notes;

            return ballpark; 
        }
    }
}
