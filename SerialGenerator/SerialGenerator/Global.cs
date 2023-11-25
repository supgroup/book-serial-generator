﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialGenerator
{
    class Global
    {
        public const string APIKey = "1234";
        //public const string APIUri = "https://192.168.1.5:443/api/";
        //public const string APIUri = "https://localhost:443/api/";
        // public const string APIUri = "https://51.210.241.73:443/api/";
        public const string APIUri = "http://localhost:81/laracom/laravel10/api/";
        public static string ScannedImageLocation = "Thumb/Scan/scan.jpg";
        public const string TMPFolder = "Thumb";
        public const string TMPItemsFolder = "Thumb/items"; // folder to save items photos locally 
        public const string TMPCustomersFolder = "Thumb/customer"; // folder to save agents photos locally 
        public const string TMPUsersFolder = "Thumb/users"; // folder to save users photos locally 
        public const string TMPSettingFolder = "Thumb/setting"; // folder to save Logo photos locally 
        public static string rootofficeFolder = "Thumb/office";
        public static string rootpassengerFolder = "Thumb/passenger";
        public static string rootservicefilesFolder = "Thumb/servicefiles";
       
        

    }
}
