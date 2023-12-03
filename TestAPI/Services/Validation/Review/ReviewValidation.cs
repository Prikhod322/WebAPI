﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using WebAPI.Logic;
using WebAPI.Models;
using WebAPI.Services.Validation.ReviewValidation;

namespace WebAPI.Services.Validation.ReviewValidation
{
    public class ReviewValidation : IReviewValidation
    {
        private readonly ApplicationDbContext dbcontext;
        public ReviewValidation(ApplicationDbContext context)
        {
            dbcontext = context;
        }
        public void Validate(Review review, ModelStateDictionary modelState)
        {
            if (!dbcontext.Review.Any(x => x.ID == review.ID) && dbcontext.Review.Any(x => x.UserID == review.UserID && x.GameID == review.GameID))
                modelState.AddModelError("AlreadyExists", "This review already exists");

            if (!dbcontext.User.Any(x => x.ID == review.UserID))
                modelState.AddModelError("UserNotExists", $"User with ID \"{review.UserID}\" not exists");

            if (!dbcontext.Game.Any(x => x.ID == review.GameID))
                modelState.AddModelError("GameNotExists", $"Game with ID \"{review.GameID}\" not exists");
        }
    }
}
