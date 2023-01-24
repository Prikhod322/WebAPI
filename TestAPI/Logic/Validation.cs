﻿using Amazon.Auth.AccessControlPolicy;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using WebAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Logic
{
    public static class Validation
    {
        private static int MinNameLength = 3;
        private static int MaxNameLength = 25;

        private static int MinLoginLength = 8;
        private static int MaxLoginength = 25;

        private static int MinPasswordLength = 10;
        private static int MaxPasswordLength = 50;

        private static int MaxReviewLength = 9999;
        private static float MaxGamePrice = 999;
        private static float MaxAchCount = 9999;

        public static void ValidateList<T>(DbSet<T> list) where T : class
        {
            if (list.Count() == 0)
                throw new Exception("Table contains no elements");
        }

        public static void ValidateGameID(int id)
        {
            ValidateList(new ApplicationContext().Games);

            if (new ApplicationContext().Games.Where(x => x.ID == id).FirstOrDefault() == null)
                throw new Exception($"Game with ID {id} do not exist");
        }

        public static void ValidateDeveloperID(int id)
        {
            ValidateList(new ApplicationContext().Developers);

            if (new ApplicationContext().Developers.Where(x => x.ID == id).FirstOrDefault() == null)
                throw new Exception($"Developer with ID {id} do not exist");
        }

        public static void ValidateDeveloperName(string name)
        {
            ValidateNameLength(name);

            if (new ApplicationContext().Developers.Where(x => x.Name.ToLower() == name.ToLower()).Any())
                throw new Exception($"Developer with name {name} already exists");
        }

        public static void ValidateGameName(string name)
        {
            ValidateNameLength(name);

            if (new ApplicationContext().Games.Where(x => x.Name.ToLower() == name.ToLower()).Any())
                throw new Exception($"Game with name {name} already exists");
        }

        public static void ValidateNameLength(string name)
        {
            if (name.Length < MinNameLength)
                throw new Exception($"Name should be at least {MinNameLength} symbols long");

            if (name.Length > MaxNameLength)
                throw new Exception($"Name should be shorter than {MaxNameLength} symbols");
        }

        public static void ValidateGamePrice(float price)
        {
            if (price < 0)
                throw new Exception($"Game price can't be negative");

            if (price > MaxGamePrice)
                throw new Exception($"Game price can't be more than {MaxGamePrice}");
        }

        public static void ValidateAchievementsCount(int achCount)
        {
            if (achCount < 0)
                throw new Exception($"Achievements count can't be negative");

            if (achCount > MaxAchCount)
                throw new Exception($"Achievements count can't be more than {MaxAchCount}");

        }

        public static void ValidateGameStatsID(int id)
        {
            ValidateList(new ApplicationContext().GamesStats);

            if (new ApplicationContext().GamesStats.Where(x => x.ID == id).FirstOrDefault() == null)
                throw new Exception($"Game stats with ID {id} do not exist");
        }

        public static void ValidateUserID(int id)
        {
            ValidateList(new ApplicationContext().Users);

            if (new ApplicationContext().Users.Where(x => x.ID == id).FirstOrDefault() == null)
                throw new Exception($"User with ID {id} do not exist");
        }

        public static void ValidateReviewID(int id)
        {
            ValidateList(new ApplicationContext().Reviews);

            if (new ApplicationContext().Reviews.Where(x => x.ID == id).FirstOrDefault() == null)
                throw new Exception($"Review with ID {id} do not exist");
        }

        public static void ValidateAchievementsGot(int id, int achCount)
        {
            GameStats gameStats = new ApplicationContext().GamesStats.Where(x => x.ID == id).First();

            if (achCount < 0)
                throw new Exception($"Achievements count can't be negative");

            if (achCount <= gameStats.AchievementsGot)
                throw new Exception($"New achievements count should be bigger than previous ({gameStats.AchievementsGot})");

            if (achCount > gameStats.Game.AchievementsCount)
                throw new Exception($"Achievements count can't be greater than achievements count of the game ({gameStats.Game.AchievementsCount})");
        }

        public static void ValidateHoursPlayed(int id, float hoursPlayed)
        {
            GameStats gameStats = new ApplicationContext().GamesStats.Where(x => x.ID == id).First();

            if (hoursPlayed < 0)
                throw new Exception($"Hours played can't be negative");

            if (hoursPlayed <= gameStats.HoursPlayed)
                throw new Exception($"Hours played number should be bigger than previous ({gameStats.HoursPlayed})");
        }

        internal static void ValidateReviewText(string? text)
        {
            if (text == null)
                return;

            if(text.Length > MaxReviewLength)
                throw new Exception($"Review can't be longer than {MaxReviewLength} symbols long");
        }

        internal static void ValidateReviewApproval(int id, bool isPositive)
        {
            if (isPositive == new ApplicationContext().Reviews.Where(x => x.ID == id).First().IsPositive)
                throw new Exception($"Review is already {(isPositive==true?"Positive":"Negative")}");
        }

        internal static void ValidateLogin(string login)
        {
            if (login.Length < MinLoginLength)
                throw new Exception($"Login should be at least {MinLoginLength} symbols long");

            if (login.Length > MaxLoginength)
                throw new Exception($"Login should be shorter than {MaxLoginength} symbols");

            if (new ApplicationContext().Users.Where(x => x.Login.ToLower() == login.ToLower()).Any())
                throw new Exception($"User with login {login} already exists");
        }

        internal static void ValidatePassword(string password)
        {
            if (password.Length < MinPasswordLength)
                throw new Exception($"Password should be at least {MinPasswordLength} symbols long");

            if (password.Length > MaxPasswordLength)
                throw new Exception($"Password should be shorter than {MaxPasswordLength} symbols");
        }

        internal static void ValidateMoneyOnAccount(float money)
        {
            if (money < 0)
                throw new Exception($"Balance can't be negative");
        }

        internal static void ValidateEmail(string? email)
        {
            if (email == null)
                return;

            if(new EmailAddressAttribute().IsValid(email)!)
                throw new Exception($"Email {email} is not valid");
        }
    }
}
