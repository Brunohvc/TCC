using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Login
{
    [Serializable]
    public class Login
    {
        public User user;
        public string login;
        public string email;
        public string password;
        public string message;
        public Boolean isLoginUsed;
    }
}
