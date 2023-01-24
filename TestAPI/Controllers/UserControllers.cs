﻿using Microsoft.AspNetCore.Mvc;
using WebAPI.Logic;
using WebAPI.Models;
using static System.Net.Mime.MediaTypeNames;

namespace WebAPI.Controllers
{
    [ApiController]
    public class UserControllers : Controller
    {
        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            try
            {
                Validation.ValidateList(new ApplicationContext().Users);

                return Ok(new ApplicationContext().Users.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetUser/{id:int}")]
        public IActionResult GetReview(int id)
        {
            try
            {
                Validation.ValidateUserID(id);

                using (ApplicationContext db = new ApplicationContext())
                {
                    User user = db.Users.Where(x => x.ID == id).First();
                    return Ok(user);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteUser/{id:int}")]
        public IActionResult DeleteReview(int id)
        {
            try
            {
                Validation.ValidateUserID(id);

                using (ApplicationContext db = new ApplicationContext())
                {
                    User user = db.Users.Where(x => x.ID == id).First();
                    db.Users.Remove(user);
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("PostUser/{login}/{password}/{nickname}")]
        public IActionResult PostReview(string login, string password, string nickname, string? email = null)
        {
            try
            {
                Validation.ValidateNameLength(nickname);
                Validation.ValidateLogin(login);
                Validation.ValidatePassword(password);
                Validation.ValidateEmail(email);

                using (ApplicationContext db = new ApplicationContext())
                {
                    User user = new User
                    {
                        Login= login,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                        Nickame=nickname,
                        Email = email==null? DBNull.Value.ToString() :email,
                        AvatarURL = $"{S3Bucket.UserBucketUrl}{S3Bucket.DefaultLogoName}",
                        MoneyOnAccount =0,
                        CreationDate = DateTime.UtcNow,
                        LastLogInDate = DateTime.UtcNow,
                    };

                    db.Users.Add(user);
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutUserLogin/{id:int}/{login}")]
        public IActionResult PutUserLogin(int id, string login)
        {
            try
            {
                Validation.ValidateUserID(id);
                Validation.ValidateLogin(login);

                using (ApplicationContext db = new ApplicationContext())
                {
                    User user = db.Users.Where(x => x.ID == id).First();
                    user.Login = login;

                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutUserPassword/{id:int}/{password}")]
        public IActionResult PutUserPassword(int id, string password)
        {
            try
            {
                Validation.ValidateUserID(id);
                Validation.ValidatePassword(password);

                using (ApplicationContext db = new ApplicationContext())
                {
                    User user = db.Users.Where(x => x.ID == id).First();
                    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);

                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutUserNickname/{id:int}/{nickname}")]
        public IActionResult PutUserNickname(int id, string nickname)
        {
            try
            {
                Validation.ValidateUserID(id);
                Validation.ValidateNameLength(nickname);

                using (ApplicationContext db = new ApplicationContext())
                {
                    User user = db.Users.Where(x => x.ID == id).First();
                    user.Nickame = nickname;

                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutUserEmail/{id:int}/{email}")]
        public IActionResult PutUserEmail(int id, string email)
        {
            try
            {
                Validation.ValidateUserID(id);
                Validation.ValidateEmail(email);

                using (ApplicationContext db = new ApplicationContext())
                {
                    User user = db.Users.Where(x => x.ID == id).First();
                    user.Email = email;

                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutUserFriend/{id:int}/{friendID}")]
        public IActionResult PutUserFriend(int id, int friendID)
        {
            try
            {
                Validation.ValidateUserID(id);
                Validation.ValidateUserID(friendID);

                using (ApplicationContext db = new ApplicationContext())
                {
                    User user = db.Users.Where(x => x.ID == id).First();
                    User user2 = db.Users.Where(x => x.ID == friendID).First();

                    user.Friends.Add(user2);
                    user2.Friends.Add(user);

                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutGameStats/{id:int}/{gameStatsID:int}")]
        public IActionResult PutGameStat(int id, int gameStatsID)
        {
            try
            {
                Validation.ValidateUserID(id);
                Validation.ValidateGameStatsID(gameStatsID);

                using (ApplicationContext db = new ApplicationContext())
                {
                    User user = db.Users.Where(x => x.ID == id).First();
                    GameStats gameStats = db.GamesStats.Where(x => x.ID == gameStatsID).First();

                    user.GamesStats.Add(gameStats);
                    gameStats.Owner= user;

                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutUserMoneyOnAccount/{id:int}/{money:float}")]
        public IActionResult PutUserMoneyOnAccount(int id, float money)
        {
            try
            {
                Validation.ValidateUserID(id);
                Validation.ValidateMoneyOnAccount(money);

                using (ApplicationContext db = new ApplicationContext())
                {
                    User user = db.Users.Where(x => x.ID == id).First();
                    user.MoneyOnAccount = money;

                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutUserAvatar/{id:int}")]
        public IActionResult PutUserAvatar(int id, IFormFile logo)
        {
            try
            {
                Validation.ValidateUserID(id);

                using (ApplicationContext db = new ApplicationContext())
                {
                    Guid guid = Guid.NewGuid();

                    S3Bucket.AddObject(logo, S3Bucket.UserBucketPath, guid).Wait();
                    S3Bucket.DeleteObject(db.Users.Where(x => x.ID == id).First().AvatarURL, S3Bucket.UserBucketPath).Wait();

                    User user = db.Users.Where(x => x.ID == id).First();
                    user.AvatarURL = $"{S3Bucket.UserBucketUrl}{guid}";
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

