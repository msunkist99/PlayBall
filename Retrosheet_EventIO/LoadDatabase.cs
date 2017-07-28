using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Retrosheet_Persist;
using Retrosheet_ReferenceData.Model;
using Retrosheet_EventData.Model;

namespace Retrosheet_DataFileIO
{
    public class LoadDatabase
    {    
        // constructor
        public LoadDatabase()
        {
        }

        public void TruncateDatabase()
        {
            TruncateRetrosheetTables.TruncateAllTables();
        }

        public void LoadDatabaseGameInformation()
        {
            Retrosheet_Queries.BuildGameInformationData();
        }

        public void LoadDatabaseBallparkData(string inputPathFile)
        {
            Console.WriteLine(inputPathFile);
            ReadWriteBallparkFile(inputPathFile);
        }

        public void LoadDatabaseReferenceData(string inputPathFile)
        {
            Console.WriteLine(inputPathFile);
            ReadWriteReferenceDataFile(inputPathFile);
        }

        public void LoadDatabaseTeamData(string inputPathFile)
        {
            Console.WriteLine(inputPathFile);
            ReadWriteTeamFile(inputPathFile);
        }

        public void LoadDatabasePersonnelData(string inputPathFile)
        {
            Console.WriteLine(inputPathFile);
            ReadWritePersonnelDataFile(inputPathFile);
        }

        public void LoadDatabaseEventData(string inputPath)
        {
            string[] files = Directory.GetFiles(inputPath);

            foreach(string fullPathFile in files)
            {
                if (fullPathFile.IndexOf("team") > -1)
                {
                    ReadWriteTeamFile(fullPathFile);
                }
            }

            string[] subdirectories = Directory.GetDirectories(inputPath);

            foreach (string subdirectory in subdirectories)
            {
                Console.WriteLine(subdirectory);
                string[] filenames = Directory.GetFiles(subdirectory);

                foreach (string fullPathFile in filenames)
                {
                    Console.WriteLine(fullPathFile);
                    if (fullPathFile.IndexOf("admininfo") > -1)
                    {
                        ReadWriteAdminInfoFile(fullPathFile);
                    }
                    else if (fullPathFile.IndexOf("com_ej") > -1)
                    {
                        ReadWriteEjectionFile( fullPathFile);
                    }
                    else if (fullPathFile.IndexOf("com_replay") > -1)
                    {
                        ReadWriteReplayFile( fullPathFile);
                    }
                    else if (fullPathFile.IndexOf("com_umpchange") > -1)
                    {
                        ReadWriteSubstituteUmpireFile( fullPathFile);
                    }
                    else if (fullPathFile.IndexOf("com") > -1)
                    {
                        ReadWriteGameCommentFile( fullPathFile);
                    }
                    else if (fullPathFile.IndexOf("gamedata") > -1)
                    {
                        ReadWriteGameDataFile( fullPathFile);
                    }
                    else if (fullPathFile.IndexOf("gameinfo") > -1)
                    {
                        ReadWriteGameInfoFile( fullPathFile);
                    }
                    else if (fullPathFile.IndexOf("players") > -1)
                    {
                        ReadWritePlayerFile( fullPathFile);
                    }
                    else if (fullPathFile.IndexOf("play") > -1)
                    {
                        ReadWritePlayFile( fullPathFile);
                    }
                    else if (fullPathFile.IndexOf("start") > -1)
                    {
                        ReadWriteStartingPlayerFile( fullPathFile);
                    }
                    else if (fullPathFile.IndexOf("sub") > -1)
                    {
                        ReadWriteSubstitutePlayerFile( fullPathFile);
                    }
                    else if (fullPathFile.IndexOf("badj") > -1)
                    {
                        ReadWriteBatterAdjustmentFile( fullPathFile);
                    }
                    else if (fullPathFile.IndexOf("padj") > -1)
                    {
                        ReadWritePitcherAdjustmentFile( fullPathFile);
                    }
                    //else if (fullPathFile.IndexOf("padj") > -1)
                    //{
                    //    ReadWritePitcherAdjustmentFile( fullPathFile);
                    //}
                }
            }
        }

        private static void ReadWriteSubstituteUmpireFile(string inputPathFile)
        {
            string[] columnValue;
            string textLine = null;

            using (StreamReader reader = new StreamReader(inputPathFile))
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
                    substituteUmpireDTO.ComSequence = Convert.ToInt16(columnValue[4]);
                    substituteUmpireDTO.FieldPosition = columnValue[8];
                    substituteUmpireDTO.UmpireID = columnValue[9];

                    SubstituteUmpirePersist.CreateSubstituteUmpire(substituteUmpireDTO);
                }
            }
        }

        private static void ReadWriteSubstitutePlayerFile(string inputPathFile)
        {
            string[] columnValue;
            string textLine = null;

            using (StreamReader reader = new StreamReader(inputPathFile))
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
                }
            }
        }

        private static void ReadWriteStartingPlayerFile(string inputPathFile)
        {
            string[] columnValue;
            string textLine = null;

            using (StreamReader reader = new StreamReader(inputPathFile))
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
                }
            }
        }

        private static void ReadWriteReplayFile(string inputPathFile)
        {
            string[] columnValue;
            string textLine = null;

            using (StreamReader reader = new StreamReader(inputPathFile))
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
                    replayDTO.ComSequence = Convert.ToInt16(columnValue[4]);
                    replayDTO.PlayerID = columnValue[8];
                    replayDTO.TeamID = columnValue[9];
                    replayDTO.UmpireID = columnValue[10];
                    replayDTO.BallparkID = columnValue[11];
                    replayDTO.Reason = columnValue[12];
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
                }
            }
        }

        private static void ReadWritePlayFile(string inputPathFile)
        {
            string[] columnValue;
            string textLine = null;

            using (StreamReader reader = new StreamReader(inputPathFile))
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
                    playDTO.Pitches = columnValue[10];
                    playDTO.EventSequence = columnValue[11];
                    playDTO.EventModifier = columnValue[12];
                    playDTO.EventRunnerAdvance = columnValue[13];

                    PlayPersist.CreatePlay(playDTO);
                }
            }
        }

        private static void ReadWritePitcherAdjustmentFile(string inputPathFile)
        {
            string[] columnValue;
            string textLine = null;

            using (StreamReader reader = new StreamReader(inputPathFile))
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
                }
            }
        }

        private static void ReadWriteGameInfoFile(string inputPathFile)
        {
            string[] columnValue;
            string textLine = null;

            using (StreamReader reader = new StreamReader(inputPathFile))
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
                }
            }
        }

        private static void ReadWriteGameDataFile(string inputPathFile)
        {
            string[] columnValue;
            string textLine = null;

            using (StreamReader reader = new StreamReader(inputPathFile))
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
                }
            }
        }

        private static void ReadWriteGameCommentFile(string inputPathFile)
        {
            string[] columnValue;
            string textLine = null;

            using (StreamReader reader = new StreamReader(inputPathFile))
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
                }
            }
        }

        private static void ReadWriteEjectionFile(string inputPathFile)
        {
            string[] columnValue;
            string textLine = null;

            using (StreamReader reader = new StreamReader(inputPathFile))
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
                    ejectionDTO.ComSequence = Convert.ToInt16(columnValue[4]);
                    ejectionDTO.PlayerID = columnValue[7];
                    ejectionDTO.JobCode = columnValue[8];
                    ejectionDTO.UmpireID = columnValue[9];
                    ejectionDTO.Reason = columnValue[10];

                    EjectionPersist.CreateEjection(ejectionDTO);
                }
            }
        }

        private static void ReadWriteBatterAdjustmentFile(string inputPathFile)
        {
            string[] columnValue;
            string textLine = null;

            using (StreamReader reader = new StreamReader(inputPathFile))
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
                }
            }
        }

        private static void ReadWriteAdminInfoFile(string inputPathFile)
        {
            string[] columnValue;
            string textLine = null;

            using (StreamReader reader = new StreamReader(inputPathFile))
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
                }
            }
        }

        private static void ReadWritePlayerFile(string inputPathFile)
        {
            string[] columnValue;
            string textLine = null;

            using (StreamReader reader = new StreamReader(inputPathFile))
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

                    PlayerPersist.CreatePlayer(playerDTO);
                }
            }
        }

        private static void ReadWriteTeamFile(string inputPathFile)
        {
            string[] columnValue;
            string textLine = null;

            using (StreamReader reader = new StreamReader(inputPathFile))
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
                        Console.WriteLine("The " + inputPathFile + " file could not be read:");
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }

                    columnValue = textLine.Split('|');

                    TeamDTO teamDTO = new TeamDTO();

                    teamDTO.RecordID = Guid.NewGuid();
                    teamDTO.ID = columnValue[0];
                    teamDTO.League = columnValue[1];
                    teamDTO.City = columnValue[2];
                    teamDTO.Name = columnValue[3];

                    TeamPersist.CreateTeam(teamDTO);
                    Console.WriteLine(textLine);
                }
            }
        }

        private static void ReadWriteReferenceDataFile(string inputPathFile)
        {
            string[] columnValue;
            string textLine = null;

            using (StreamReader reader = new StreamReader(inputPathFile))
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
                        Console.WriteLine("The " + inputPathFile + " file could not be read:");
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
                }
            }
        }

        private static void ReadWriteBallparkFile(string inputPathFile)
        {
            string[] columnValue;
            string textLine = null;
            string textLine1 = null;
            string textLine2 = null;
            int doubleQuoteIndex = 0;
            DateTime dateTime;

            using (StreamReader reader = new StreamReader(inputPathFile))
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
                        Console.WriteLine("The " + inputPathFile + " file could not be read:");
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
                }
            }
        }

        private static void ReadWritePersonnelDataFile(string inputPathFile)
        {
            string[] columnValue;
            string textLine = null;
            DateTime dateTime;

            using (StreamReader reader = new StreamReader(inputPathFile))
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
                        Console.WriteLine("The " + inputPathFile + " file could not be read:");
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }

                    // skip the header record in the input file
                    if (textLine.IndexOf("ref_data_type") == -1)
                    {
                        columnValue = textLine.Split('|');
                        PersonnelDTO personnelDTO = new PersonnelDTO();

                        personnelDTO.RecordID = Guid.NewGuid();
                        personnelDTO.LastName = columnValue[0];
                        personnelDTO.FirstName = columnValue[1];
                        personnelDTO.PersonID = columnValue[2];

                        if (DateTime.TryParse(columnValue[3], out dateTime))
                            personnelDTO.CareerDate = dateTime;
                        else
                        {
                            personnelDTO.CareerDate = DateTime.MaxValue; ;
                        }

                        if (columnValue[4] == "8" )
                        {
                            personnelDTO.Role = "M";  // manager
                        }
                        else if (columnValue[4] == "9")
                        {
                            personnelDTO.Role = "U"; // umpire
                        }
                        else
	                    {
                            personnelDTO.Role = null; // unknown
                        }

                        PersonnelPersist.CreatePersonnel(personnelDTO);
                    }
                }
            }
        }
    }
}
