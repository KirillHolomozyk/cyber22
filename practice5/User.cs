using System;

namespace practice5
{
    public class User
    {
        public string Login;
        public string Password;
        public byte[] Salt;
        
        public User(string login, byte[] password, byte[] salt)
        {
            Login = login;
            Password = Convert.ToBase64String(password);
            Salt = salt;
        }
    }
}