using CodinovaTask.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodinovaTask.Services
{
    public interface IUserService
    {
        /// <summary>
        /// This function is used to create users.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        UserDetails Create(UserDetails user, string password);

        /// <summary>
        /// This function is used to update users details.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        void Update(UserDetails user, string password = null);


        /// <summary>
        /// This function is used to delete users.
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);

        /// <summary>
        /// This function is used to Authenticate users.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        UserDetails Authenticate(string username, string password);


        /// <summary>
        /// This function is used to get all users.
        /// </summary>
        /// <returns></returns>
        IEnumerable<UserDetails> GetAllUsers();

        /// <summary>
        /// This function is used to get user details by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserDetails GetUserById(int id);
    }
}
