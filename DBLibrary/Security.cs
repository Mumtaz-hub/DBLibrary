using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DBLibrary
{
    public class Security
    {
        public static string HashSHA1(string value)
        {
            var sha1 = System.Security.Cryptography.SHA1.Create();
            var inputBytes = Encoding.ASCII.GetBytes(value);
            var hash = sha1.ComputeHash(inputBytes);

            var sb = new StringBuilder();
            for (var i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public static string ECODE_DECODE(string StrVal, string StrTo)
        {
            //StrS -> String to Encode OR Decode
            //StrTo -> Flag indicating what to do -(E)ncode, (D)ecode
            try
            {
                int IntLen, IntCnt, IntPos;
                string StrPass, StrChar;
                //StrChar = StrChar * 1;
                string StrECode, StrDCode;

                StrECode = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
                StrDCode = ")(*&^%$#@!";

                for (IntLen = 1; IntLen <= 52; IntLen++)
                {
                    StrDCode = StrDCode + Convert.ToChar(IntLen + 160);
                }

                StrPass = "";
                IntLen = StrVal.Trim().Length;

                for (IntCnt = 0; IntCnt < IntLen; IntCnt++)
                {
                    StrChar = StrVal.Substring(IntCnt, 1);
                    IntPos = (StrTo == "E") ? StrECode.IndexOf(StrChar, 1) : StrDCode.IndexOf(StrChar, 1);
                    StrPass = StrPass + ((StrTo == "E") ? StrDCode.Substring(IntPos, 1) : StrECode.Substring(IntPos, 1));
                }
                return StrPass;
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
                return "";
            }
        }
    }
}
