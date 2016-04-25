using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MakeSoftware.Security.MD5;

namespace Bitcliq.BIR.Utils
{
    public class PasswordManager
    {

        private const int NUMCHARS = 5;
        private static char[] alphabetArray = { 'a', 'b', 'c', 'd', 'e', 
											    'f', 'g', 'h', 'i', 'j', 
											    'k', 'l', 'm', 'n', 'o', 
											    'p', 'q', 'r', 's', 't', 
											    'u', 'v', 'w', 'y', 'z', 
											    '0', '1', '2', '3', '4', 
											    '5', '6', '7', '8', '9' };


        public static string GenerateRandomMD5Password()
        {
            Random random = new Random();

            int n = 0;
            string input = "";

            for (int i = 0; i < NUMCHARS; i++)
            {
                n = random.Next(0, alphabetArray.Length - 1);

                input += alphabetArray[n];

            }

            return input;
        }



        public static string GenerateMD5Password(string input)
        {
            return MD5Generator.GenerateMD5Hash(input);
        }
    }

}