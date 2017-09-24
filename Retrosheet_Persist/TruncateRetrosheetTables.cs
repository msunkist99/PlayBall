using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace Retrosheet_Persist
{
    public class TruncateRetrosheetTables
    {
        public static void TruncateAllTables()
        {
            var dbCtx = new retrosheetDB();

            try
            {
                dbCtx.Database.ExecuteSqlCommand("TRUNCATE TABLE Game_Info");
                dbCtx.Database.ExecuteSqlCommand("TRUNCATE TABLE Reference_Data");

                
                dbCtx.Database.ExecuteSqlCommand("TRUNCATE TABLE Admin_Info");
                dbCtx.Database.ExecuteSqlCommand("TRUNCATE TABLE Ballpark");
                dbCtx.Database.ExecuteSqlCommand("TRUNCATE TABLE Batter_Adjustment");
                dbCtx.Database.ExecuteSqlCommand("TRUNCATE TABLE Ejection");
                dbCtx.Database.ExecuteSqlCommand("TRUNCATE TABLE Game_Comment");
                dbCtx.Database.ExecuteSqlCommand("TRUNCATE TABLE Game_Data");
                
                dbCtx.Database.ExecuteSqlCommand("TRUNCATE TABLE Game_Information");
                dbCtx.Database.ExecuteSqlCommand("TRUNCATE TABLE Game_Suspension");
                dbCtx.Database.ExecuteSqlCommand("TRUNCATE TABLE Personnel");
                dbCtx.Database.ExecuteSqlCommand("TRUNCATE TABLE Pitcher_Adjustment");
                dbCtx.Database.ExecuteSqlCommand("TRUNCATE TABLE Play");
                dbCtx.Database.ExecuteSqlCommand("TRUNCATE TABLE Player");
                dbCtx.Database.ExecuteSqlCommand("TRUNCATE TABLE Protest");
               
               
                dbCtx.Database.ExecuteSqlCommand("TRUNCATE TABLE Replay");
                dbCtx.Database.ExecuteSqlCommand("TRUNCATE TABLE Starting_Player");
                dbCtx.Database.ExecuteSqlCommand("TRUNCATE TABLE Substitute_Player");
                dbCtx.Database.ExecuteSqlCommand("TRUNCATE TABLE Substitute_Umpire");
                dbCtx.Database.ExecuteSqlCommand("TRUNCATE TABLE Team");
                dbCtx.Database.ExecuteSqlCommand("TRUNCATE TABLE Team_Batting_Order_Adjustment");
                
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
        }


    }
}
