using System;

namespace renade
{
    class PlayerCouldNotBeCreatedException : Exception
    {
        public PlayerCouldNotBeCreatedException(string login, string socialClubName, string email)
            : base(string.Format("Failed to create player with login: {0}; social club name: {1}; email: {2}", login, socialClubName, email)) { }
    }
}