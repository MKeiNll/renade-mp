using System;

namespace renade
{
    class FailedToCreatePlayerException : Exception
    {
        public FailedToCreatePlayerException(string login, string socialClubName, string email)
            : base(string.Format("Failed to create player with login: {0}; social club name: {1}; email: {2}", login, socialClubName, email)) { }
    }
}