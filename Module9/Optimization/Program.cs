using System;
using System.Security.Cryptography;

namespace Optimization
{
    class Program
    {
        /// <summary>
        /// 1. Change Array.Copy -> Buffer.BlockCopy
        /// 2. If possible to change decryption method PasswordDeriveBytes works faster than Rfc2898DeriveBytes
        /// 3. Program is not much large -> using ProfileOptimization method does not have sense 
        /// ProfileOptimization.SetProfileRoot(path); ProfileOptimization.StartProfile("profile");
        /// 4. Reduce method size: remove unnecessary variables
        /// 5. using const values instead of simple declare
        /// </summary>
        static void Main(string[] args)
        {
            var watchSlow = System.Diagnostics.Stopwatch.StartNew();
            for (int i = 0; i < 100000; i++)
            {
                var pwdSlow = GeneratePasswordHashUsingSalt_Slow("1234567890asdasdasdasdadde23", new byte[20] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 1, 1, 1, 1, 1, 1, 1, 1, 1 });
            }
            watchSlow.Stop();
            Console.Write(watchSlow.ElapsedTicks);

            GC.Collect();

            var watchFast = System.Diagnostics.Stopwatch.StartNew();
            for (int i = 0; i < 100000; i++)
            {
                var pwdFast = GeneratePasswordHashUsingSalt_Fast("1234567890asdasdasdasdadde23", new byte[20] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 1, 1, 1, 1, 1, 1, 1, 1, 1 });
            }
            watchFast.Stop();
            Console.Write(watchFast.ElapsedTicks);
        }

        public static string GeneratePasswordHashUsingSalt_Fast(string passwordText, byte[] salt)
        {
            const int iterate = 10000;
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Buffer.BlockCopy(salt, 0, hashBytes, 0, 16);
            Buffer.BlockCopy(hash, 0, hashBytes, 16, 20);
            return Convert.ToBase64String(hashBytes);
        }

        public static string GeneratePasswordHashUsingSalt_Slow(string passwordText, byte[] salt)
        {
            int iterate = 10000;
            var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            var passwordHash = Convert.ToBase64String(hashBytes);

            return passwordHash;
        }
    }
}
