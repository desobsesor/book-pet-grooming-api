using System.Security.Cryptography;
using System.Text;

namespace BookPetGroomingAPI.Application.Common
{
    public static class SecurityUtils
    {
        //Generate a method  to generate a unique user using two char of firstNName and all lastName 
        public static string GenerateUniqueUsername(string firstName, string lastName)
        {
            string uniqueUsername = string.Concat(firstName.AsSpan(0, 2), lastName.Split(' ')[0]);
            return uniqueUsername;
        }

        /// <summary>
        /// Generates a secure password using RNGCryptoServiceProvider.
        /// </summary>
        /// <returns>A secure password string.</returns>
        public static string GenerateSecurePassword()
        {
            const int passwordLength = 12;
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()";

            StringBuilder password = new();
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] data = new byte[4 * passwordLength];
                rng.GetBytes(data);

                for (int i = 0; i < passwordLength; i++)
                {
                    uint randomIndex = BitConverter.ToUInt32(data, i * 4) % (uint)validChars.Length;
                    password.Append(validChars[(int)randomIndex]);
                }
            }

            return password.ToString();
        }
    }
}