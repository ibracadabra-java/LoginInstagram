using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace LoginWithIAS.Models
{
    static class Helper
    {
        public const string AccountPathDirectory = "Accounts";
        public const string SessionExtension = ".bin";
        public static void CreateAccountDirectory()
        {
            if (!Directory.Exists(AccountPathDirectory))
                Directory.CreateDirectory(AccountPathDirectory);
        }
        public static string GetAccountPath(this string username) => $"{AccountPathDirectory}/{username}{SessionExtension}";
    }
}