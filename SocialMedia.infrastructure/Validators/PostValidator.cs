using FluentValidation;
using SocialMedia.Core.DTOs;
using System;

namespace SocialMedia.Infrastructure.Validators
{
    public class PostValidator : AbstractValidator<PostDto>
    {
        public PostValidator()
        {
            RuleFor(post => post.Description)
                 .NotNull()
                 .Length(5, 500);

            RuleFor(post => post.Date)
                .NotNull()
                .GreaterThan(DateTime.Now);
        }
    }
}
