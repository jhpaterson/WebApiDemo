using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiDemo.Models;
using WebApiDemo.Filters;

namespace WebApiDemo.Controllers
{
    public class UsersController : ApiController
    {
        // IUserRepository repository = new UserRepository();
        IUserRepository _repository;

        public UsersController(IUserRepository repository)
        {
            _repository = repository;
        }

        // GET api/users
        public IEnumerable<User> Get()
        {
            return _repository.GetAll();
        }

        // GET api/users ALTERNATIVE VERSION (more RESTful?)
        //public IEnumerable<string> Get()
        //{
        //    var users = _repository.GetAll();
        //    List<string> userUris = new List<string>();
        //    foreach (User user in users)
        //    {
        //        userUris.Add(Url.Link("DefaultApi", 
        //            new { id = user.ID }));
        //    }
        //    return userUris;
        //}

        // GET api/users/3
        public User GetById(int id)
        {
            User user = _repository.GetById(id);
            if (user == null)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format("No user with ID = {0}", id)),
                    ReasonPhrase = "User ID Not Found"
                };
                throw new HttpResponseException(resp);
            }
            return user;
        }

        // POST api/users
        // Note - if testing by submitting form data from Chrome Postman make sure to select
        // x-www-form-urlencoded as content type to ensure media type is supported
        [ValidateFilter]
        public HttpResponseMessage Post(User user)
        {
            _repository.Save(user);

            var response = Request.CreateResponse<User>(HttpStatusCode.Created, user);

            string uri = Url.Link("DefaultApi", new { id = user.ID });
            response.Headers.Location = new Uri(uri);
            return response;

        }

        // PUT api/users/3
        public HttpResponseMessage Put(int id, User user)
        {
            User userToUpdate = _repository.GetById(id);
            if (userToUpdate == null)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format("No user with ID = {0}", id)),
                    ReasonPhrase = "User ID To Update Not Found"
                };
                throw new HttpResponseException(resp);
            }

            userToUpdate.FirstName = user.FirstName;
            userToUpdate.LastName = user.LastName;
            _repository.Save(userToUpdate);

            var response = Request.CreateResponse<User>(HttpStatusCode.OK, userToUpdate);

            string uri = Url.Link("DefaultApi", new { id = userToUpdate.ID });
            response.Headers.Location = new Uri(uri);
            return response;
        }

        // DELETE api/users/3
        public HttpResponseMessage Delete(int id)
        {
            _repository.Delete(id);
            return new HttpResponseMessage(HttpStatusCode.NoContent);

        }

        // OPTIONS api/users
        [AcceptVerbs("OPTIONS")]
        [NotImplExceptionFilter]
        public HttpResponseMessage Options()
        {
            throw new NotImplementedException("This method is not supported");
        }
    }
}
