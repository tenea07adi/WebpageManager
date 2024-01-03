using System.Security.Cryptography;
using System.Text;

namespace CommonLogic.Helpers
{
    public static class CryptographyHelper
    {
        private static readonly string _defaultAlphabet = "QWERTYUIOPLKJHGFDSAZXCVBNMqwertyuioplkjhgfdsazxcvbnm1234567890,./;'[]";

        public static bool IsCorrectPassword(string password, string passwrdHash, string passwordSalt)
        {
            string newHash = HashPassword(password, passwordSalt);

            return newHash == passwrdHash;
        }

        public static string HashPassword(string password, string passwordSalt)
        {
            byte[] byteSalt = Encoding.ASCII.GetBytes(passwordSalt);

            var pbkdf2 = new Rfc2898DeriveBytes(password, byteSalt, 100000);

            byte[] byteHash = pbkdf2.GetBytes(20);

            string hash =  Convert.ToBase64String(byteHash);

            return hash;
        }

        public static string GeneratePasswordSalt()
        {
            return GenerateRandomString(10);
        }

        public static string GenerateRandomString(int length)
        {
            return GenerateRandomString(_defaultAlphabet, length);
        }

        public static string GenerateRandomString(string alphabet, int length)
        {
            string result = string.Empty;

            Random rnd = new Random();

            for(int i = 0; i < length; i++)
            {
                result += alphabet[rnd.Next(0, alphabet.Length)];
            }

            return result;
        }
    }
}
