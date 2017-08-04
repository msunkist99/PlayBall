using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
////  need this for StreamReader
using System.IO;

using Retrosheet_Persist;
using Retrosheet_ReferenceData.Model;
using Retrosheet_EventData.Model;

namespace Retrosheet_Console
{
    class Program
    {
        static void xMain(string[] args)
        {
            //ReadWriteFiles();
            ReadWriteBallparkFile();
            ReadWriteReferenceDataFile();
            ReadWriteTeamFile();
            ReadWritePlayerFile();
            ReadWriteAdminInfoFile();
            ReadWriteBatterAdjustmentFile();
            ReadWriteEjectionFile();
            ReadWriteGameCommentFile();
            ReadWriteGameDataFile();
            ReadWriteGameInfoFile();
            ReadWritePitcherAdjustmentFile();
            ReadWritePlayFile();
            ReadWriteReplayFile();
            ReadWriteStartingPlayerFile();
            ReadWriteSubstitutePlayerFile();
            ReadWriteSubstituteUmpireFile();
            Console.ReadLine();
        }

        private static void ReadWriteSubstituteUmpireFile()
        {
            string[] columnValue;
            string textLine = null;

            using (StreamReader reader = new StreamReader(@"C:\users\mmr\documents\retrosheet\2016 Regular Season\Output\2016TOR\2016TOR_com_umpchange"))
            {
                while (!reader.EndOfStream)
                {
                    try
                    {
                        textLine = reader.ReadLine();
                    }
                    catch (Exception e)
                    {
                        // Let the user know what went wrong.
                        Console.WriteLine("The " + @"C:\users\mmr\documents\retrosheet\2016 Regular Season\Output\2016TOR\2016TOR_com_umpchange" + " file could not be read:");
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }

                    columnValue = textLine.Split('|');

                    var substituteUmpireDTO = new SubstituteUmpireDTO();

                    substituteUmpireDTO.RecordID = Guid.NewGuid();
                    substituteUmpireDTO.GameID = columnValue[0];
                    substituteUmpireDTO.Inning = Convert.ToInt16(columnValue[1]);
                    substituteUmpireDTO.Sequence = Convert.ToInt16(columnValue[3]);
                    substituteUmpireDTO.FieldPosition = columnValue[7];
                    substituteUmpireDTO.UmpireID = columnValue[8];

                    SubstituteUmpirePersist.CreateSubstituteUmpire(substituteUmpireDTO);

                    Console.WriteLine(textLine);
                }
            }
        }

        private static void ReadWriteSubstitutePlayerFile()
        {
            string[] columnValue;
            string textLine = null;

            using (StreamReader reader = new StreamReader(@"C:\users\mmr\documents\retrosheet\2016 Regular Season\Output\2016SLN\2016SLN_sub"))
            {
                while (!reader.EndOfStream)
                {
                    try
                    {
                        textLine = reader.ReadLine();
                    }
                    catch (Exception e)
                    {
                        // Let the user know what went wrong.
                        Console.WriteLine("The " + @"C:\users\mmr\documents\retrosheet\2016 Regular Season\Output\2016SLN\2016SLN_sub" + " file could not be read:");
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }

                    columnValue = textLine.Split('|');

                    var substitutePlayerDTO = new SubstitutePlayerDTO();

                    substitutePlayerDTO.RecordID = Guid.NewGuid();
                    substitutePlayerDTO.GameID = columnValue[0];
                    substitutePlayerDTO.Inning = Convert.ToInt16(columnValue[1]);
                    substitutePlayerDTO.GameTeamCode = Convert.ToInt16(columnValue[2]);
                    substitutePlayerDTO.Sequence = Convert.ToInt16(columnValue[3]);
                    substitutePlayerDTO.PlayerID = columnValue[5];
                    substitutePlayerDTO.BattingOrder = Convert.ToInt16(columnValue[8]);
                    substitutePlayerDTO.FieldPosition = Convert.ToInt16(columnValue[9]);

                    SubstitutePlayerPersist.CreateSubstitutePlayer(substitutePlayerDTO);

                    Console.WriteLine(textLine);
                }
            }
        }

        private static void ReadWriteStartingPlayerFile()
        {
            string[] columnValue;
            string textLine = null;

            using (StreamReader reader = new StreamReader(@"C:\users\mmr\documents\retrosheet\2016 Regular Season\Output\2016SLN\2016SLN_start"))
            {
                while (!reader.EndOfStream)
                {
                    try
                    {
                        textLine = reader.ReadLine();
                    }
                    catch (Exception e)
                    {
                        // Let the user know what went wrong.
                        Console.WriteLine("The " + @"C:\users\mmr\documents\retrosheet\2016 Regular Season\Output\2016SLN\2016SLN_start" + " file could not be read:");
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }

                    columnValue = textLine.Split('|');

                    StartingPlayerDTO startingPlayerDTO = new StartingPlayerDTO();

                    startingPlayerDTO.RecordID = Guid.NewGuid();
                    startingPlayerDTO.GameID = columnValue[0];
                    startingPlayerDTO.PlayerID = columnValue[2];
                    startingPlayerDTO.GameTeamCode = Convert.ToInt16(columnValue[4]);
                    startingPlayerDTO.BattingOrder = Convert.ToInt16(columnValue[5]);
                    startingPlayerDTO.FieldPosition = Convert.ToInt16(columnValue[6]);

                    StartingPlayerPersist.CreateStartingPlayer(startingPlayerDTO);

                    Console.WriteLine(textLine);
                }
            }
        }

        private static void ReadWriteReplayFile()
        {
            string[] columnValue;
            string textLine = null;

            using (StreamReader reader = new StreamReader(@"C:\users\mmr\documents\retrosheet\2016 Regular Season\Output\2016SLN\2016SLN_com_replay"))
            {
                while (!reader.EndOfStream)
                {
                    try
                    {
                        textLine = reader.ReadLine();
                    }
                    catch (Exception e)
                    {
                        // Let the user know what went wrong.
                        Console.WriteLine("The " + @"C:\users\mmr\documents\retrosheet\2016 Regular Season\Output\2016SLN\2016SLN_com_replay" + " file could not be read:");
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }

                    columnValue = textLine.Split('|');

                    ReplayDTO replayDTO = new ReplayDTO();

                    replayDTO.RecordID = Guid.NewGuid();
                    replayDTO.GameID = columnValue[0];
                    replayDTO.Inning = Convert.ToInt16(columnValue[1]);
                    replayDTO.GameTeamCode = Convert.ToInt16(columnValue[2]);
                    replayDTO.Sequence = Convert.ToInt16(columnValue[3]);
                    replayDTO.PlayerID = columnValue[7];
                    replayDTO.TeamID = columnValue[8];
                    replayDTO.UmpireID = columnValue[9];
                    replayDTO.BallparkID = columnValue[10];
                    replayDTO.Reason = columnValue[11];
                    if (columnValue[12] == "Y")
                    {
                        replayDTO.Reversed = true;
                    }
                    else
                    {
                        replayDTO.Reversed = false;
                    }
                    replayDTO.Initiated = columnValue[13];
                    replayDTO.ReplayChallengeTeamID = columnValue[14];
                    replayDTO.Type = columnValue[15];

                    ReplayPersist.CreateReplay(replayDTO);

                    Console.WriteLine(textLine);
                }
            }
        }

        private static void ReadWritePlayFile()
        {
            string[] columnValue;
            string textLine = null;

            using (StreamReader reader = new StreamReader(@"C:\users\mmr\documents\retrosheet\2016 Regular Season\Output\2016SLN\2016SLN_play"))
            {
                while (!reader.EndOfStream)
                {
                    try
                    {
                        textLine = reader.ReadLine();
                    }
                    catch (Exception e)
                    {
                        // Let the user know what went wrong.
                        Console.WriteLine("The " + @"C:\users\mmr\documents\retrosheet\2016 Regular Season\Output\2016SLN\2016SLN_play" + " file could not be read:");
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }

                    columnValue = textLine.Split('|');

                    PlayDTO playDTO = new PlayDTO();

                    playDTO.RecordID = Guid.NewGuid();
                    playDTO.GameID = columnValue[0];
                    playDTO.Inning = Convert.ToInt16(columnValue[1]);
                    playDTO.GameTeamCode = Convert.ToInt16(columnValue[2]);
                    playDTO.Sequence = Convert.ToInt16(columnValue[3]);
                    playDTO.PlayerID = columnValue[7];
                    playDTO.CountBalls = Convert.ToInt16(columnValue[8]);
                    playDTO.CountStrikes = Convert.ToInt16(columnValue[9]);
                    playDTO.EventSequence = columnValue[10];
                    playDTO.EventModifier = columnValue[11];
                    playDTO.EventRunnerAdvance = columnValue[12];

                    PlayPersist.CreatePlay(playDTO);

                    Console.WriteLine(textLine);
                }
            }
        }

        private static void ReadWritePitcherAdjustmentFile()
        {
            string[] columnValue;
            string textLine = null;

            using (StreamReader reader = new StreamReader(@"C:\users\mmr\documents\retrosheet\2016 Regular Season\Output\2016SEA\2016SEA_padj"))
            {
                while (!reader.EndOfStream)
                {
                    try
                    {
                        textLine = reader.ReadLine();
                    }
                    catch (Exception e)
                    {
                        // Let the user know what went wrong.
                        Console.WriteLine("The " + @"C:\users\mmr\documents\retrosheet\2016 Regular Season\Output\2016SEA\2016SEA_padj" + " file could not be read:");
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }

                    columnValue = textLine.Split('|');

                    PitcherAdjustmentDTO pitcherAdjustmentDTO = new PitcherAdjustmentDTO();

                    pitcherAdjustmentDTO.RecordID = Guid.NewGuid();
                    pitcherAdjustmentDTO.GameID = columnValue[0];
                    pitcherAdjustmentDTO.Inning = Convert.ToInt16(columnValue[1]);
                    pitcherAdjustmentDTO.GameTeamCode = Convert.ToInt16(columnValue[2]);
                    pitcherAdjustmentDTO.Sequence = Convert.ToInt16(columnValue[3]);
                    pitcherAdjustmentDTO.PlayerID = columnValue[5];
                    pitcherAdjustmentDTO.PitcherHand = columnValue[6];

                    PitcherAdjustmentPersist.CreatePitcherAdjustment(pitcherAdjustmentDTO);

                    Console.WriteLine(textLine);
                }
            }
        }

        private static void ReadWriteGameInfoFile()
        {
            string[] columnValue;
            string textLine = null;

            using (StreamReader reader = new StreamReader(@"C:\users\mmr\documents\retrosheet\2016 Regular Season\Output\2016SLN\2016SLN_gameinfo"))
            {
                while (!reader.EndOfStream)
                {
                    try
                    {
                        textLine = reader.ReadLine();
                    }
                    catch (Exception e)
                    {
                        // Let the user know what went wrong.
                        Console.WriteLine("The " + @"C:\users\mmr\documents\retrosheet\2016 Regular Season\Output\2016SLN\2016SLN_gameinfo" + " file could not be read:");
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }

                    columnValue = textLine.Split('|');

                    GameInfoDTO gameInfoDTO = new GameInfoDTO();

                    gameInfoDTO.RecordID = Guid.NewGuid();
                    gameInfoDTO.GameID = columnValue[0];
                    gameInfoDTO.GameInfoType = columnValue[2];
                    gameInfoDTO.GameInfoValue = columnValue[3];

                    GameInfoPersist.CreateGameInfo(gameInfoDTO);

                    Console.WriteLine(textLine);
                }
            }
        }

        private static void ReadWriteGameDataFile()
        {
            string[] columnValue;
            string textLine = null;

            using (StreamReader reader = new StreamReader(@"C:\users\mmr\documents\retrosheet\2016 Regular Season\Output\2016SLN\2016SLN_gamedata"))
            {
                while (!reader.EndOfStream)
                {
                    try
                    {
                        textLine = reader.ReadLine();
                    }
                    catch (Exception e)
                    {
                        // Let the user know what went wrong.
                        Console.WriteLine("The " + @"C:\users\mmr\documents\retrosheet\2016 Regular Season\Output\2016SLN\2016SLN_gamedata" + " file could not be read:");
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }

                    columnValue = textLine.Split('|');

                    GameDataDTO gameDataDTO = new GameDataDTO();

                    gameDataDTO.RecordID = Guid.NewGuid();
                    gameDataDTO.GameID = columnValue[0];
                    gameDataDTO.DataType = columnValue[2];
                    gameDataDTO.PlayerID = columnValue[3];
                    gameDataDTO.DataValue = columnValue[4];

                    GameDataPersist.CreateGameData(gameDataDTO);

                    Console.WriteLine(textLine);
                }
            }
        }

        private static void ReadWriteGameCommentFile()
        {
            string[] columnValue;
            string textLine = null;

            using (StreamReader reader = new StreamReader(@"C:\users\mmr\documents\retrosheet\2016 Regular Season\Output\2016SLN\2016SLN_com"))
            {
                while (!reader.EndOfStream)
                {
                    try
                    {
                        textLine = reader.ReadLine();
                    }
                    catch (Exception e)
                    {
                        // Let the user know what went wrong.
                        Console.WriteLine("The " + @"C:\users\mmr\documents\retrosheet\2016 Regular Season\Output\2016SLN\2016SLN_com" + " file could not be read:");
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }

                    columnValue = textLine.Split('|');

                    GameCommentDTO gameCommentDTO = new GameCommentDTO();

                    gameCommentDTO.RecordID = Guid.NewGuid();
                    gameCommentDTO.GameID = columnValue[0];
                    gameCommentDTO.Inning = Convert.ToInt16(columnValue[1]);
                    gameCommentDTO.GameTeamCode = Convert.ToInt16(columnValue[2]);
                    gameCommentDTO.Sequence = Convert.ToInt16(columnValue[3]);
                    gameCommentDTO.CommentSequence = Convert.ToInt16(columnValue[4]);
                    gameCommentDTO.Comment = columnValue[6];

                    GameCommentPersist.CreateGameComment(gameCommentDTO);
                    Console.WriteLine(textLine);
                }
            }
        }

        private static void ReadWriteEjectionFile()
        {
            string[] columnValue;
            string textLine = null;

            using (StreamReader reader = new StreamReader(@"C:\users\mmr\documents\retrosheet\2016 Regular Season\Output\2016SLN\2016SLN_com_ej"))
            {
                while (!reader.EndOfStream)
                {
                    try
                    {
                        textLine = reader.ReadLine();
                    }
                    catch (Exception e)
                    {
                        // Let the user know what went wrong.
                        Console.WriteLine("The " + @"C:\users\mmr\documents\retrosheet\2016 Regular Season\Output\2016SLN\2016SLN_com_ej" + " file could not be read:");
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }

                    columnValue = textLine.Split('|');

                    EjectionDTO ejectionDTO = new EjectionDTO();

                    ejectionDTO.RecordID = Guid.NewGuid();
                    ejectionDTO.GameID = columnValue[0];
                    ejectionDTO.Inning = Convert.ToInt16(columnValue[1]);
                    ejectionDTO.GameTeamCode = Convert.ToInt16(columnValue[2]);
                    ejectionDTO.Sequence = Convert.ToInt16(columnValue[3]);
                    ejectionDTO.PlayerID = columnValue[6];
                    ejectionDTO.JobCode = columnValue[7];
                    ejectionDTO.UmpireID = columnValue[8];
                    ejectionDTO.Reason = columnValue[9];

                    EjectionPersist.CreateEjection(ejectionDTO);
                    Console.WriteLine(textLine);
                }
            }
        }

        private static void ReadWriteBatterAdjustmentFile()
        {
            string[] columnValue;
            string textLine = null;

            using (StreamReader reader = new StreamReader(@"C:\users\mmr\documents\retrosheet\2016 Regular Season\Output\2016HOU\2016HOU_badj"))
            {
                while (!reader.EndOfStream)
                {
                    try
                    {
                        textLine = reader.ReadLine();
                    }
                    catch (Exception e)
                    {
                        // Let the user know what went wrong.
                        Console.WriteLine("The " + @"C:\users\mmr\documents\retrosheet\2016 Regular Season\Output\2016HOU\2016HOU_badj" + " file could not be read:");
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }

                    columnValue = textLine.Split('|');

                    BatterAdjustmentDTO batterAdjustmentDTO = new BatterAdjustmentDTO();

                    batterAdjustmentDTO.RecordID = Guid.NewGuid();
                    batterAdjustmentDTO.GameID = columnValue[0];
                    batterAdjustmentDTO.Inning = Convert.ToInt16(columnValue[1]);
                    batterAdjustmentDTO.GameTeamCode = Convert.ToInt16(columnValue[2]);
                    batterAdjustmentDTO.Sequence = Convert.ToInt16(columnValue[3]);
                    batterAdjustmentDTO.PlayerID = columnValue[5];
                    batterAdjustmentDTO.Bats = columnValue[6];

                    BatterAdjustmentPersist.CreateBatterAdjustment(batterAdjustmentDTO);
                    Console.WriteLine(textLine);
                }
            }
        }

        private static void ReadWriteAdminInfoFile()
        {
            string[] columnValue;
            string textLine = null;

            using (StreamReader reader = new StreamReader(@"C:\users\mmr\documents\retrosheet\2016 Regular Season\Output\2016SLN\2016SLN_admininfo"))
            {
                while (!reader.EndOfStream)
                {
                    try
                    {
                        textLine = reader.ReadLine();
                    }
                    catch (Exception e)
                    {
                        // Let the user know what went wrong.
                        Console.WriteLine("The " + @"C:\users\mmr\documents\retrosheet\2016 Regular Season\Output\2016SLN\2016SLN_admininfo" + " file could not be read:");
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }

                    columnValue = textLine.Split('|');

                    AdminInfoDTO adminInfoDTO = new AdminInfoDTO();

                    adminInfoDTO.RecordID = Guid.NewGuid();
                    adminInfoDTO.GameID = columnValue[0];
                    adminInfoDTO.AdminInfoType = columnValue[2];
                    adminInfoDTO.AdminInfoValue = columnValue[3];

                    AdminInfoPersist.CreateAdminInfo(adminInfoDTO);
                    Console.WriteLine(textLine);
                }
            }
        }

        private static void ReadWritePlayerFile()
        {
            string[] columnValue;
            string textLine = null;

            using (StreamReader reader = new StreamReader(@"C:\users\mmr\documents\retrosheet\2016 Regular Season\Output\2016SLN\2016SLN_players"))
            {
                while (!reader.EndOfStream)
                {
                    try
                    {
                        textLine = reader.ReadLine();
                    }
                    catch (Exception e)
                    {
                        // Let the user know what went wrong.
                        Console.WriteLine("The " + @"C:\users\mmr\documents\retrosheet\2016 Regular Season\Output\2016SLN\2016SLN_players" + " file could not be read:");
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }

                    columnValue = textLine.Split('|');

                    PlayerDTO playerDTO = new PlayerDTO();

                    playerDTO.RecordID = Guid.NewGuid();
                    playerDTO.PlayerID = columnValue[0];
                    playerDTO.LastName = columnValue[1];
                    playerDTO.FirstName = columnValue[2];
                    playerDTO.Throws = columnValue[3];
                    playerDTO.Bats = columnValue[4];
                    playerDTO.TeamID = columnValue[5];
                    playerDTO.Position = columnValue[6];

                    PlayerPersist.CreatePlayer (playerDTO);
                    Console.WriteLine(textLine);
                }
            }
        }

        private static void ReadWriteTeamFile()
        {
            string[] columnValue;
            string textLine = null;

            using (StreamReader reader = new StreamReader(@"C:\users\mmr\documents\retrosheet\2016 Regular Season\Output\TEAM2016_team"))
            {
                while (!reader.EndOfStream)
                {
                    try
                    {
                        textLine = reader.ReadLine();
                    }
                    catch (Exception e)
                    {
                        // Let the user know what went wrong.
                        Console.WriteLine("The " + @"C:\users\mmr\documents\retrosheet\2016 Regular Season\Output\2016SLN\TEAM2016_team" + " file could not be read:");
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }

                    columnValue = textLine.Split('|');

                    TeamDTO teamDTO = new TeamDTO();


                    teamDTO.RecordID = Guid.NewGuid();
                    teamDTO.TeamID = columnValue[0];
                    teamDTO.League = columnValue[1];
                    teamDTO.City = columnValue[2];
                    teamDTO.Name = columnValue[3];

                    TeamPersist.CreateTeam(teamDTO);
                    Console.WriteLine(textLine);
                }
            }
        }

        private static void ReadWriteReferenceDataFile()
        {
            string[] columnValue;
            string textLine = null;

            using (StreamReader reader = new StreamReader(@"C:\users\mmr\documents\retrosheet\reference_data.txt"))
            {
                while (!reader.EndOfStream)
                {
                    try
                    {
                        textLine = reader.ReadLine();
                    }
                    catch (Exception e)
                    {
                        // Let the user know what went wrong.
                        Console.WriteLine("The " + @"C:\users\mmr\documents\retrosheet\reference_data.txt" + " file could not be read:");
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }

                    // skip the header record in the input file
                    if (textLine.IndexOf("ref_data_type") == -1)
                    {
                        columnValue = textLine.Split('|');
                        ReferenceDataDTO referenceDataDTO = new ReferenceDataDTO();

                        referenceDataDTO.RecordID = Guid.NewGuid();
                        referenceDataDTO.ReferenceDataType = columnValue[0];
                        referenceDataDTO.ReferenceDataCode = columnValue[1];
                        referenceDataDTO.ReferenceDataDescription = columnValue[2];
                        ReferenceDataPersist.CreateReferenceData(referenceDataDTO);
                    }
                    Console.WriteLine(textLine);

                }
            }
        }

        private static void ReadWriteBallparkFile()
        {
            string[] columnValue;
            string textLine = null;
            string textLine1 = null;
            string textLine2 = null;
            int doubleQuoteIndex = 0;
            DateTime dateTime;

            using (StreamReader reader = new StreamReader(@"C:\users\mmr\documents\retrosheet\ballparkcodes.txt"))
            {
                while (!reader.EndOfStream)
                {
                    try
                    {
                        textLine = reader.ReadLine();
                    }
                    catch (Exception e)
                    {
                        // Let the user know what went wrong.
                        Console.WriteLine("The " + @"C:\users\mmr\documents\retrosheet\ballparkcodes.txt" + " file could not be read:");
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }

                    // skip the header record in the input file
                    if (textLine.IndexOf("PARKID") == -1)
                    {
                        doubleQuoteIndex = textLine.IndexOf("\"");

                        // there are notes available on the baseballcodes record which may contain 
                        // commas within the double quotes - leave the comma in the notes.
                        if (doubleQuoteIndex > -1)
                        {
                            textLine1 = textLine.Substring(0, doubleQuoteIndex - 1);
                            textLine2 = textLine.Substring(doubleQuoteIndex + 1);
                            textLine1 = textLine1.Replace(",", "|");
                            textLine2 = textLine2.Replace("\"", "");
                            textLine = textLine1 + "|" + textLine2;
                        }
                        else
                        {
                            textLine = textLine.Replace(",", "|");
                        }
                        columnValue = textLine.Split('|');

                        BallparkDTO ballparkDTO = new BallparkDTO();

                        //Retrosheet_Persist.Ballpark newBallpark = new Retrosheet_Persist.Ballpark();

                        ballparkDTO.RecordID = Guid.NewGuid();
                        ballparkDTO.ID = columnValue[0];
                        ballparkDTO.Name = columnValue[1];
                        ballparkDTO.AKA = columnValue[2];
                        ballparkDTO.City = columnValue[3];
                        ballparkDTO.State = columnValue[4];

                        if (DateTime.TryParse(columnValue[5], out dateTime))
                        {
                            ballparkDTO.StartDate = dateTime;
                        }
                        else
                        {
                            ballparkDTO.StartDate = DateTime.MinValue;
                        }

                        if (DateTime.TryParse(columnValue[6], out dateTime))
                        {
                            ballparkDTO.EndDate = dateTime;
                        }
                        else
                        {
                            ballparkDTO.EndDate = DateTime.MaxValue;
                        }

                        ballparkDTO.League = columnValue[7];
                        ballparkDTO.Notes = columnValue[8];

                        BallparkPersist.CreateBallpark(ballparkDTO);
                    }
                    Console.WriteLine(textLine);
                }
            }
        }

        /*
        static void ReadWriteFiles()
        {
            string[] columnValue;
            string textLine = null;
            string textLine1 = null;
            string textLine2 = null;
            string record_id = null;
            int doubleQuoteIndex = 0;

            string connectionString = "Data Source=MMR-PC\\SQLEXPRESS;Initial Catalog=retrosheet;Integrated Security=True";

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            if (connection.State == System.Data.ConnectionState.Open)
            {

            }


            Ballpark ballpark = new Ballpark();
            //var db = new retrosheetEntities();

            using (StreamReader reader = new StreamReader(@"C:\users\mmr\documents\retrosheet\ballparkcodes.txt"))
            {
                while (!reader.EndOfStream)
                {
                    try
                    {
                        textLine = reader.ReadLine();
                    }
                    catch (Exception e)
                    {
                        // Let the user know what went wrong.
                        Console.WriteLine("The " + @"C:\users\mmr\documents\retrosheet\ballparkcodes.txt" + " file could not be read:");
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }

                    // skip the header record in the input file
                    if (textLine.IndexOf("PARKID") == -1)
                    {
                        doubleQuoteIndex = textLine.IndexOf("\"");

                        // there are notes available on the baseballcodes record which may contain 
                        // commas within the double quotes - leave the comma in the notes.
                        if (doubleQuoteIndex > -1)
                        {
                            textLine1 = textLine.Substring(0, doubleQuoteIndex - 1);
                            textLine2 = textLine.Substring(doubleQuoteIndex + 1);
                            textLine1 = textLine1.Replace(",", "|");
                            textLine2 = textLine2.Replace("\"", "");
                            textLine = textLine1 + "|" + textLine2;
                        }
                        else
                        {
                            textLine = textLine.Replace(",", "|");
                        }
                        columnValue = textLine.Split('|');

                        record_id = Guid.NewGuid();
                        ballpark.ballpark_id = FormatColumnValue(columnValue[0]);
                        ballpark.name = FormatColumnValue(columnValue[1]);
                        ballpark.aka = FormatColumnValue(columnValue[2]);
                        ballpark.city = FormatColumnValue(columnValue[3]);
                        ballpark.state = FormatColumnValue(columnValue[4]);

                        DateTime dateTime;
                        if (DateTime.TryParse(columnValue[5], out dateTime))
                        {
                            ballpark.start_date = dateTime;
                        }
                        else
                        {
                            ballpark.start_date = null;
                        }

                        if (DateTime.TryParse(columnValue[6], out dateTime))
                        {
                            ballpark.end_date = dateTime;
                        }
                        else
                        {
                            ballpark.end_date = null;
                        }

                        ballpark.league = FormatColumnValue(columnValue[7]);
                        ballpark.notes = FormatColumnValue(columnValue[8]);

                        /*
                        string sqlInsert = "insert into ballpark(" +
                                           "record_id, " +
                                           "ballpark_id, " +
                                           "name, " +
                                           "aka, " +
                                           "city, " +
                                           "state, " +
                                           "start_date, " +
                                           "end_date, " +
                                           "league, " +
                                           "notes) values (" +
                                           singleQuote + ballpark.record_id + singleQuote +
                                           "'" + ballpark.record_id + "', "+
                                           ballpark.ballpark_id + ", " +
                                           ballpark.name + ", " +
                                           ballpark.aka + ", " +
                                           ballpark.city + ", " +
                                           ballpark.state + ", " +
                                           "'" + ballpark.start_date + "', " +
                                           "'" + ballpark.end_date + "', " +
                                           ballpark.league + ", " +
                                           ballpark.notes + ")";
                        */

                        /*
                        string sqlInsert = "insert into ballpark(" +
                                           "record_id, " +
                                           "ballpark_id, " +
                                           "name, " +
                                           "aka, " +
                                           "city, " +
                                           "state, " +
                                           "start_date, " +
                                           "end_date, " +
                                           "league, " +
                                           "notes) values (" +
                                          singleQuote + record_id + singleQuote + comma +
                                          FormatColumnValue(columnValue[0]) + comma +
                                          FormatColumnValue(columnValue[1]) + comma +
                                          FormatColumnValue(columnValue[2]) + comma +
                                          FormatColumnValue(columnValue[3]) + comma +
                                          FormatColumnValue(columnValue[4]) + comma +
                                          FormatColumnValue(columnValue[5]) + comma +
                                          FormatColumnValue(columnValue[6]) + comma +
                                          FormatColumnValue(columnValue[7]) + comma +
                                          FormatColumnValue(columnValue[8]) +  ")";
                        

                        SqlCommand command = new SqlCommand(sqlInsert, connection);
                        command.ExecuteNonQuery();

                        //db.Ballpark.Add(ballpark);
                        //db.SaveChanges();

                    }
                    Console.WriteLine(textLine);
                }
                connection.Close();

            }

            Console.ReadLine();
        }

        static string FormatColumnValue(string inputString)
        {
            inputString = inputString.Replace("'", "''");


            return singleQuote + inputString + singleQuote; 
        }
        */
    }
}
