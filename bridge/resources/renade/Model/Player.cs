using System;

namespace renade
{
    public class Player
    {
        public readonly string Login;
        public readonly string SocialClubName;
        public readonly string Email;
        public readonly string Password;
        public readonly long RegDate;

        public Player(string login, string socialClubName, string email, string password, long regDate)
        {
            Login = login;
            SocialClubName = socialClubName;
            Email = email;
            Password = password;
            RegDate = regDate;
        }

        public override string ToString()
        {
            return String.Format("Player - Social club name: {0}; Login: {1}; Email: {2}; Registration date: {3}", 
                SocialClubName, Login, Email, DateTimeOffset.FromUnixTimeMilliseconds(RegDate).ToLocalTime());
        }
    }
} 
