using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SocialMediaContext _Context;
        private readonly IRepository<Post> _postRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Comment> _CommentRepository;

        public UnitOfWork(SocialMediaContext context)
        {
            _Context = context;
        }

        public IRepository<Post> PostRepository => _postRepository ?? new BaseRepository<Post>(_Context);

        public IRepository<User> UserRepository => _userRepository ?? new BaseRepository<User>(_Context);

        public IRepository<Comment> CommentRepository => _CommentRepository ?? new BaseRepository<Comment>(_Context);

        public void Dispose()
        {
            if(_Context != null)
            {
                _Context.Dispose();
            }
        }

        public void SaveChanges()
        {
            _Context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _Context.SaveChangesAsync();
        }
    }
}
