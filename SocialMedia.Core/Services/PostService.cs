using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Exceptions;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.Core.Services
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;

        public  PostService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Post> GetPost(int id)
        {
            return await _unitOfWork.PostRepository.GetById(id);
        }

        public PageList<Post> GetPosts(PostQueryFilter filters)
        {
            
            var post = _unitOfWork.PostRepository.GetAll();

            if(filters.UserId != null)
            {
                post = post.Where(x => x.UserId == filters.UserId);
            }
            if (filters.Date != null)
            {
                post = post.Where(x => x.Date.ToShortDateString() == filters.Date?.ToShortDateString());
            }
            if (filters.Description != null)
            {
                post = post.Where(x => x.Description.ToLower().Contains(filters.Description.ToLower()));
            }

            var pagePosts = PageList<Post>.Create(post, filters.PageNumber, filters.PageSize);

            return pagePosts;
        }

        public async Task InsertPost(Post post)
        {
            var user =  await _unitOfWork.UserRepository.GetById(post.UserId);            
            if (user == null)
            {
                throw new BusinessException("User doesn´t exist");
            }

            var userPost = await _unitOfWork.PostRepository.GetPostsByUser(post.UserId);
            if (userPost.Count() < 10)
            {
                var lastPost = userPost.LastOrDefault();
                if ((DateTime.Now - lastPost.Date).TotalDays < 7)
                {
                    throw new BusinessException("You are not able to publish the post");
                }
            }

            if (post.Description.Contains("Sexo"))
            {
                throw new BusinessException("Content not allowed");
            }

            await _unitOfWork.PostRepository.Add(post);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> UpdatePost(Post post)
        {
            _unitOfWork.PostRepository.Update(post);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePost(int id)
        {
            await _unitOfWork.PostRepository.Delete(id);
            return true;
        }        
    }
}
