﻿using Microsoft.AspNetCore.Mvc;
using WebAPI.Logic;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    public class GameStatsControllers : Controller
    {
        [HttpGet("GetAllGameStats")]
        public IActionResult GetAllGameStats()
        {
            try
            {
                Validation.ValidateList(new ApplicationContext().GamesStats);

                using (ApplicationContext db = new ApplicationContext())
                {
                    return Ok(db.GamesStats.ToList());
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetGameStats/{id:int}")]
        public IActionResult GetGame(int id)
        {
            try
            {
                Validation.ValidateGameStatsID(id);

                using (ApplicationContext db = new ApplicationContext())
                {
                    Game game = db.Games.Where(x => x.ID == id).First();
                    return Ok(game);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteGameStats/{id:int}")]
        public IActionResult DeleteGame(int id)
        {
            try
            {
                Validation.ValidateGameStatsID(id);

                using (ApplicationContext db = new ApplicationContext())
                {
                    GameStats gameStats = db.GamesStats.Where(x => x.ID == id).First();
                    db.GamesStats.Remove(gameStats);
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("PostGameStats/{ownerID:int}/{gameID:int}")]
        public IActionResult PostGameStats(int ownerID, int gameID, int achGot = 0, float hoursPlayed =0)
        {
            try
            {
                Validation.ValidateUserID(ownerID);
                Validation.ValidateGameID(gameID);

                using (ApplicationContext db = new ApplicationContext())
                {
                    GameStats gameStats = new GameStats
                    {
                        Owner = db.Users.Where(x => x.ID == ownerID).First(),
                        Game = db.Games.Where(x => x.ID == ownerID).First(),
                        HoursPlayed= hoursPlayed,
                        AchievementsGot = achGot,
                        PurchasehDate = DateTime.UtcNow,
                    };

                    db.GamesStats.Add(gameStats);
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutAchievementsCount/{id:int}/{achGot:int}")]
        public IActionResult PutAchievementsGot(int id, int achGot)
        {
            try
            {
                Validation.ValidateGameStatsID(id);
                Validation.ValidateAchievementsGot(id, achGot);

                using (ApplicationContext db = new ApplicationContext())
                {
                    GameStats gameStats = db.GamesStats.Where(x => x.ID == id).First();
                    gameStats.AchievementsGot = achGot;
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutHoursPlayed/{id:int}/{hoursPlayed:float}")]
        public IActionResult PutHoursPlayed(int id, float hoursPlayed)
        {
            try
            {
                Validation.ValidateGameStatsID(id);
                Validation.ValidateHoursPlayed(id, hoursPlayed);

                using (ApplicationContext db = new ApplicationContext())
                {
                    GameStats gameStats = db.GamesStats.Where(x => x.ID == id).First();
                    gameStats.HoursPlayed = hoursPlayed;
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("PutGameLaunchDate/{id:int}")]
        public IActionResult PutGameLaunchDate(int id)
        {
            try
            {
                Validation.ValidateGameStatsID(id);

                using (ApplicationContext db = new ApplicationContext())
                {
                    GameStats gameStats = db.GamesStats.Where(x => x.ID == id).First();
                    gameStats.LastLaunchDate = DateTime.UtcNow;
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
