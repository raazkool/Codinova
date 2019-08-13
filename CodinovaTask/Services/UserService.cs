using CodinovaTask.DataEntities;
using CodinovaTask.Helpers;
using CodinovaTask.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodinovaTask.Services
{
    public class UserService : IUserService
    {
        private CodinovaContextEntities _dbContext;

        public UserService(CodinovaContextEntities context)
        {
            _dbContext = context;
        }
        public UserDetails Create(UserDetails user, string password)
        {

            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (_dbContext.Users.Any(x => x.UserName == user.UserName))
                throw new AppException("Username \"" + user.UserName + "\" is already exist.");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return user;
        }

        public void Update(UserDetails user, string password = null)
        {
            var result = _dbContext.Users.Find(user.UserId);

            if (result == null)
            {
                throw new AppException("User not found.");
            }
            else
            {
                result.FirstName = user.FirstName;
                result.LastName = user.LastName;
                result.UserName = user.UserName;
                if (!string.IsNullOrWhiteSpace(password))
                {
                    byte[] passwordHash, passwordSalt;
                    CreatePasswordHash(password, out passwordHash, out passwordSalt);

                    result.PasswordHash = passwordHash;
                    result.PasswordSalt = passwordSalt;
                }
                _dbContext.Users.Update(result);
                _dbContext.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var result = _dbContext.Users.Find(id);
            if (result != null)
            {
                _dbContext.Users.Remove(result);
                _dbContext.SaveChanges();
            }
        }

        public UserDetails Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var result = _dbContext.Users.SingleOrDefault(x => x.UserName == username);
            if (result == null)
                return null;

            if (!VerifyPasswordHash(password, result.PasswordHash, result.PasswordSalt))
                return null;

            return result;
        }

        public IEnumerable<UserDetails> GetAllUsers()
        {
            return _dbContext.Users;
        }

        public UserDetails GetUserById(int id)
        {
            return _dbContext.Users.Find(id);
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null)
                throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }
            return true;
        }
    }
}
