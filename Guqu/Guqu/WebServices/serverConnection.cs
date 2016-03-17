using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Diagnostics;

namespace GuquMysql
{
    /*
    class ServerCommunicationController
    {
        static void Main(string[] args)
        {
            DBConnect dbc = new DBConnect();
            List<String>[] arr = new List<string>[3];
            //arr = dbc.Select();
            Debug.WriteLine("-------------------FETCH START----------------");
            foreach (List<String> li in arr)
            {
                foreach (String str in li)
                {
                    Debug.Write(str);
                }
                Debug.WriteLine("");
            }
        }
    }
    */

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
                query = "INSERT INTO " + tablename + " (email, pass_hash, pass_salt, sign_up_date, last_login) VALUES(\"" + email + "\", \"" + hash + "\", \"" + salt + "\", NOW(), NOW());";
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
        public List<string> Select(string tablename, string email)
        {
            string query = "SELECT * FROM " + tablename + " WHERE email = \"" + email + "\";";
            Console.WriteLine(query);

            //Create a list to store the result
            //List<string>[] list = new List<string>[8];
            //list[0] = new List<string>();
            List<string> list = new List<string>();
            //Create Command
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list.Add(dataReader["user_id"] + "");
                    list.Add(dataReader["email"] + "");
                    list.Add(dataReader["sign_up_date"] + "");
                    list.Add(dataReader["last_login"] + "");
                    list.Add(dataReader["pass_hash"] + "");
                    list.Add(dataReader["pass_salt"] + "");
                    list.Add(dataReader["failed_pass_attempts"] + "");
                    list.Add(dataReader["failed_pass_date"] + "");

                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                if (list.Count > 0)
                {
                    Console.WriteLine(list[1]);
                    return list;
                }
                else
                {
                    Console.WriteLine("No results");
                    return list;
                }
            }
            else
            {
                Console.WriteLine("Conn not open");
                return list;
            }
        }

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
        /*
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
            string query = "SELECT COUNT(*) AS emailcount FROM users WHERE email = \"" + email + "\";";
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
                Console.WriteLine("ERROR!!");
                return false;
            }
        }



    }

}