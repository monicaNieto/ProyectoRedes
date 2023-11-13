using System;

namespace Servidor
{
    [Serializable]
    public class User
    {
        public string id;
        public string userName;
        public string password;

        public User(string id, string userName, string password)
        {
            this.id = id;
            this.userName = userName;
            this.password = password;
        }
    }
}
