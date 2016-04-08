using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guqu.WebServices
{
    class UserCloud
    {
        private int user_cloud_id;
        public int User_cloud_id
        {
            get { return user_cloud_id; }
            set { user_cloud_id = value; }
        }

        private int user_id;
        public int User_id
        {
            get { return user_id; }
            set { user_id = value; }
        }

        private int cloud_id;
        public int Cloud_id
        {
            get { return cloud_id; }
            set { cloud_id = value; }
        }

        private string cloud_username;
        public string Cloud_username
        {
            get { return cloud_username; }
            set { cloud_username = value; }
        }

        private string cloud_token;
        public string Cloud_token
        {
            get { return cloud_token; }
            set { cloud_token = value; }
        }

        private string refresh_token;
        public string Refresh_token
        {
            get { return refresh_token; }
            set { refresh_token = value; }
        }

        private string custom_cloud_name;
        public string Custom_cloud_name
        {
            get { return custom_cloud_name; }
            set { custom_cloud_name = value; }
        }

        private DateTime token_exp_date;
        public DateTime Token_exp_date
        {
            get { return token_exp_date; }
            set { token_exp_date = value; }
        }
    }
}
