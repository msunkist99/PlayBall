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
    public class GameCommentPersist
    {
        public static void CreateGameComment(GameCommentDTO gameCommentDTO)
        {
            // ballpark instance of Player class in Retrosheet_Persist.Retrosheet
            var gameComment = convertToEntity(gameCommentDTO);

            // entity data model
            var dbCtx = new retrosheetDB();

            dbCtx.Game_Comment.Add(gameComment);
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

        private static Game_Comment convertToEntity(GameCommentDTO gameCommentDTO)
        {
            var gameComment = new Game_Comment();

            gameComment.record_id = gameCommentDTO.RecordID;
            gameComment.game_id = gameCommentDTO.GameID;
            gameComment.inning = gameCommentDTO.Inning;
            gameComment.game_team_code = gameCommentDTO.GameTeamCode;
            gameComment.sequence = gameCommentDTO.Sequence;
            gameComment.comment_sequence = gameCommentDTO.CommentSequence;
            gameComment.comment = gameCommentDTO.Comment;

            return gameComment;
        }
    }
}
