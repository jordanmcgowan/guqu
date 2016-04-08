using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Diagnostics;
using Guqu.WebServices;

namespace GuquMysql
{
    class ServerCommunicationController
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        //Constructor
        public ServerCommunicationController()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            server = "52.32.86.246";
            database = "guqu";
            uid = "guqu"; //try "root" if this doesn't work 
            password = "guqu123";
            string connectionString;
            connectionString = "SERVER=" + server + "; " + "DATABASE=" + database + "; " + "UID=" + uid + "; " + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }

        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        //Insert statement
        public void Insert(string tablename, string email, string hash, string salt)
        {
            string query = "";

            if (tablename == "users")
            {
                query = "INSERT INTO " + tablename + " (email, pass_hash, pass_salt, sign_up_date, last_login) VALUES('" + email + "', '" + hash + "', '" + salt + "', NOW(), NOW());"; //TODO: try catch if the query actually worked.
            }

            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        //public void InsertNewUserCloud(int userId, string cloudUsername, string token, int cloudId)
        public void InsertNewUserCloud(int userId, string token, int cloudId, string refreshToken)
        {
            var cloudType = "";
            if (cloudId == 1)
            {
                cloudType = "oneDrive";
            }
            else if (cloudId == 2)
            {
                cloudType = "googleDrive";
            }
            if (doesUserCloudExist(userId))
            {
                Console.WriteLine("Your user_cloud already registered in DB.");
            }
            else
            {
                //Stores user's cloud creds in the DB
                string query = "INSERT INTO user_clouds (user_id, cloud_id, cloud_token, refresh_token, custom_cloud_name) "
                    + "VALUES (" + userId + ", " + cloudId + ", '" + token + "', '" + refreshToken + "', '" + cloudType + "')";

                //open connection
                if (this.OpenConnection() == true)
                {
                    //create command and assign the query and connection from the constructor
                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    //Execute command
                    cmd.ExecuteNonQuery();

                    //close connection
                    this.CloseConnection();
                    Console.WriteLine("new user_cloud inserted.");
                }
            }

        }


        public Boolean doesUserCloudExist(int userId)
        {
            string query = "SELECT COUNT(*) AS user_cloud_count FROM user_clouds WHERE user_id = '" + userId + "';";
            //Open Connection
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                int doesExist = -1;

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    doesExist = Int32.Parse(dataReader["user_cloud_count"] + "");

                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                if (doesExist == 1)
                {
                    Console.WriteLine("User has 1 cloud service in DB");
                    return true;
                }
                else if (doesExist == 0)
                {
                    Console.WriteLine("User has 0 cloud services in DB");
                    return false;
                }
                else if (doesExist > 1)
                {
                    Console.WriteLine("User has 2+ cloud services in DB");
                    return true;
                }
                else
                {
                    Console.WriteLine("ERROR: Verification failed.");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("ERROR!!");
                return false;
            }

        }
        /*
        //Update statement
        public void Update()
        {
            string query = "UPDATE tableinfo SET name='Joe', age='22' WHERE name='John Smith'";

            //Open connection
            if (this.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;

                //Execute query
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        //Delete statement
        public void Delete()
        {
            string query = "DELETE FROM tableinfo WHERE name='John Smith'";

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }
        */

        //Select statement
        public User SelectUser(string email)
        {
            string query = "SELECT * FROM users WHERE email = '" + email + "';";
            //Console.WriteLine(query);
            User user = new User();

            //Create Command
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    user.User_id = Int32.Parse(dataReader["user_id"] + "");
                    user.Email = dataReader["email"] + "";
                    user.Sign_up_date = dataReader["sign_up_date"] + "";
                    user.Last_login = dataReader["last_login"] + "";
                    user.Pass_hash = dataReader["pass_hash"] + "";
                    user.Pass_salt = dataReader["pass_salt"] + "";
                    user.Failed_pass_attempts = Int32.Parse(dataReader["failed_pass_attempts"] + "");
                    user.Failed_pass_date = dataReader["failed_pass_date"] + "";
                    user.Failed_pass_date = dataReader["failed_pass_date"] + "";
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                if (user.Email != null)
                {
                    Console.WriteLine("Logged user: " + user.Email);
                    return user;
                }
                else
                {
                    Console.WriteLine("No results for that user in the DB");
                    return user;
                }
            }
            else
            {
                Console.WriteLine("Server connection not open");
                return null;
            }
        }

        public List<UserCloud> SelectUserClouds(int userId)
        {
            string query = "SELECT * FROM user_clouds WHERE user_id = " + userId + ";";
            //Console.WriteLine(query);

            List<UserCloud> cloudList = new List<UserCloud>();

            //Create Command
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();
                
                //Read the data and store them in the list
                while (dataReader.Read()) //TODO: gives only one row? or entire list?
                {
                    UserCloud userCloud = new UserCloud();

                    //TODO: Add token expiration to this
                    userCloud.User_id = Int32.Parse(dataReader["user_id"] + "");
                    userCloud.Cloud_id = Int32.Parse(dataReader["cloud_id"] + "");
                    userCloud.Cloud_token = dataReader["cloud_token"].ToString();
                    userCloud.Refresh_token = dataReader["refresh_token"].ToString();
                    userCloud.Custom_cloud_name = dataReader["custom_cloud_name"].ToString();

                    cloudList.Add(userCloud);
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                if (cloudList.Count != 0)
                {
                    //Console.WriteLine("");
                    return cloudList;
                }
                else
                {
                    Console.WriteLine("No userCloud found");
                    return cloudList;
                }
            }
            else
            {
                Console.WriteLine("Server connection not open");
                return null;
            }
        }


        /*
        //Count statement
        public int Count()
        {
            string query = "SELECT Count(*) FROM users";
            int Count = -1;

            //Open Connection
            if (this.OpenConnection() == true)
            {
                //Create Mysql Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //ExecuteScalar will return one value
                Count = int.Parse(cmd.ExecuteScalar() + "");


                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                return Count;
            }
            else
            {
                return Count;
            }
        }
        
        //Backup
        public void Backup()
        {
            try
            {
                DateTime Time = DateTime.Now;
                int year = Time.Year;
                int month = Time.Month;
                int day = Time.Day;
                int hour = Time.Hour;
                int minute = Time.Minute;
                int second = Time.Second;
                int millisecond = Time.Millisecond;

                //Save file to C:\ with the current date as a filename
                string path;
                path = "C:\\MySqlBackup" + year + "-" + month + "-" + day +
            "-" + hour + "-" + minute + "-" + second + "-" + millisecond + ".sql";
                StreamWriter file = new StreamWriter(path);


                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "mysqldump";
                psi.RedirectStandardInput = false;
                psi.RedirectStandardOutput = true;
                psi.Arguments = string.Format(@"-u{0} -p{1} -h{2} {3}",
                    uid, password, server, database);
                psi.UseShellExecute = false;

                Process process = Process.Start(psi);

                string output;
                output = process.StandardOutput.ReadToEnd();
                file.WriteLine(output);
                process.WaitForExit();
                file.Close();
                process.Close();
            }
            catch (IOException ex)
            {
                MessageBox.Show("Error , unable to backup!");
            }
        }
        

        //Restore
        public void Restore()
        {
        }

        */


        public Boolean emailExists(string email)
        {
            string query = "SELECT COUNT(*) AS emailcount FROM users WHERE email = '" + email + "';";
            //Open Connection
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                int doesExist = -1;

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    doesExist = Int32.Parse(dataReader["emailcount"] + "");

                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                if (doesExist == 1)
                {
                    return true;
                }
                else if (doesExist == 0)
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Verification failed.");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("ERROR!! (serverConnection -- emailExists()");
                return false;
            }
        }

        public void UpdateLastLogin(string email)
        {
            //open connection
            if (this.OpenConnection() == true)
            {
                string query = "UPDATE users SET last_login = NOW() WHERE email = '" + email + "';"; //TODO: try catch if the query actually worked.

                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }

        }

    }

}