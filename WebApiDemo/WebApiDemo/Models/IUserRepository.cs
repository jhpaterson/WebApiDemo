using System.Collections.Generic;

namespace WebApiDemo.Models
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();

        User GetById(int id);

        int Save(User user);

        void Delete(int id);
    }
}
