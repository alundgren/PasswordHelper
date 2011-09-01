using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace PasswordHelper
{
    public static class PasswordUtils
    {
        //TODO: Would there be any point to having clearTextPassword be a SecureString since we both have to make it into bytes here and also likely store it anyways as the framework parsing the password field will be using a string
        public static ByteArray ComputeHash(string clearTextPassword, ByteArray salt, Encoding passwordToBytesEncoding, string hashAlgorithmName, Func<string, HashAlgorithm> createHashAlgorithm)
        {
            if (clearTextPassword == null)
                throw new ArgumentNullException("clearTextPassword");

            //Unclear if these are thead-safe so we trade performance for clarity
            using (var hash = createHashAlgorithm(hashAlgorithmName))
            {
                var passwordBytes = ByteArray.FromBytes(passwordToBytesEncoding.GetBytes(clearTextPassword));
                var combinedBytes = ByteArray.Concat(passwordBytes, salt);
                return ByteArray.FromBytes(hash.ComputeHash(combinedBytes.Bytes));
            }
        }

        public static ByteArray CreateRandomBytes(int byteCount)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var bytes = new byte[byteCount];
                rng.GetBytes(bytes); //Note that this method is threadsafe
                return ByteArray.FromBytes(bytes);
            }
        }
    }
}
