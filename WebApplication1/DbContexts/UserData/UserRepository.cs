using WebApplication1.Entitites;

namespace WebApplication1.DbContexts.UserData
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _context;
        public UserRepository(UserDbContext context)
        {
            _context = context;
        }

        public Task<List<User>> GetUsersAsync()
        {
            var users = _context.Users.ToList();
            return Task.FromResult(users);
        }

        public Task<User?> GetUserByIdAsync(int id)
        {
            var user = _context.Users.Find(id);
            return Task.FromResult(user);
        }

        public Task<User> Create(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return Task.FromResult(user);
        }

        public Task<User> Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
            return Task.FromResult(user);
        }

        public Task<User?> Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
            return Task.FromResult(user);
        }
    }
}
