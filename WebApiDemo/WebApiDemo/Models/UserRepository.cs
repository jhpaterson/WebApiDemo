using System.Collections.Generic;
using System.Linq;

namespace WebApiDemo.Models
{
    public class UserRepository : IUserRepository
    {
        static List<User> _users = new List<User>{
                                       new User{ID=1, FirstName="Fernando", LastName="Alonso"},
                                       new User{ID=2, FirstName="Kimi", LastName="Raikkonen"},
                                       new User{ID=3, FirstName="Michael", LastName="Schumacher"}
                                  };

        public IEnumerable<User> GetAll()
        {
            return _users;
        }

        public User GetById(int id)
        {
            return _users.FirstOrDefault(u => u.ID == id);
        }

        public int Save(User user)
        {
            if (user.ID == 0)
            {
                user.ID = nextID();
                _users.Add(user);
                return user.ID;
            }
            else
            {
                int index = _users.FindIndex(u => u.ID == user.ID);
                if (index < 0)
                {
                    _users.Add(user);
                    return user.ID;
                }
                else
                {
                    _users[index] = user;
                    return user.ID;
                }
            }
        }

        public void Delete(int id)
        {
            int index = _users.FindIndex(u => u.ID == id);
            if (index >= 0)
            {
                _users.RemoveAt(index);
            }
        }

        private int nextID()
        {
            return _users.Max(u => u.ID) + 1;
        }
    }
}