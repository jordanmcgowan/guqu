using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guqu.WebServices
{
    public class User
    {
        private int user_id;
        public int User_id
        {
            get { return user_id; }
            set { user_id = value; }
        }

        private string email;
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        private string sign_up_date;
        public string Sign_up_date
        {
            get { return sign_up_date; }
            set { sign_up_date = value; }
        }

        private string last_login;
        public string Last_login
        {
            get { return last_login; }
            set { last_login = value; }
        }

        private string pass_hash;
        public string Pass_hash
        {
            get { return pass_hash; }
            set { pass_hash = value; }
        }

        private string pass_salt;
        public string Pass_salt
        {
            get { return pass_salt; }
            set { pass_salt = value; }
        }

        private int failed_pass_attempts;
        public int Failed_pass_attempts
        {
            get { return failed_pass_attempts; }
            set { failed_pass_attempts = value; }
        }

        //private DateTime failed_pass_date;
        //public DateTime Failed_pass_date
        private string failed_pass_date;
        public string Failed_pass_date
        {
            get { return failed_pass_date; }
            set { failed_pass_date = value; }
        }
    }
}
