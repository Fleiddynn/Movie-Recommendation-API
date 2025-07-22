using WebApplication1.Entitites;

namespace WebApplication1.DbContexts.UserData
{
    public interface IUserRepository
    {
        public Task<List<User>> GetUsersAsync();
        public Task<User?> GetUserByIdAsync(Guid id);
        public Task<User> Create(User user);
        public Task<User> Update(User user);
        public Task<User?> Delete(Guid id);
    }
}
