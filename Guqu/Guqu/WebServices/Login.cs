﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guqu.WebServices
{
    class CloudLogin
    {

        public CloudLogin() {
            //empty constructor 
        }

        private static bool googleDriveLogin() {

            return false;
        }

        private static bool oneDriveLogin()
        {
            //these are also login params, should move to login class
            try
            {
                await InitializeAPI.oneDriveClient.AuthenticateAsync();
                Console.Write("This succedded");
            }
            catch (OneDriveException e)
            {
                Console.Write(e);

            }

            return false;
        }

        private static bool boxLogin()
        {

            return false;
        }



    }
}