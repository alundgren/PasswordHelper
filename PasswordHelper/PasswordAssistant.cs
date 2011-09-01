using System;
using System.Collections.Generic;
using System.Linq;

namespace PasswordHelper
{
    public class PasswordAssistant
    {
        protected IPasswordConfiguration Config { get; set; }

        public PasswordAssistant(IPasswordConfiguration config)
        {
            Config = config;
        }

        public PasswordAssistant() : this(new DefaultPasswordConfiguration()) { }

        /// <summary>
        /// Typical usage is when a new password is first created.
        /// </summary>
        public virtual HashedPassword CreateHashFromClearTextPassword(string clearTextPassword)
        {
            var salt = PasswordUtils.CreateRandomBytes(Config.SaltByteCount);
            var enc = Config.PasswordToBytesEncoding;
            var algoName = Config.HashAlgorithmName;
            var hash = PasswordUtils.ComputeHash(clearTextPassword, salt, enc, algoName, Config.CreateHashAlgorithm);
            return new HashedPassword
            {
                Hash = hash,
                Salt = salt,
                Encoding = enc,
                HashAlgorithmName = algoName
            };
        }

        /// <summary>
        /// Typical scenario is on user login to verify that the user entered the correct password
        /// </summary>
        /// <param name="clearTextPassword"></param>
        /// <param name="storedHash"></param>
        /// <returns></returns>
        public virtual bool IsPasswordCorrect(string clearTextPassword, HashedPassword storedHash)
        {
            var hash = PasswordUtils.ComputeHash(
                clearTextPassword,
                storedHash.Salt,
                storedHash.Encoding,
                storedHash.HashAlgorithmName,
                Config.CreateHashAlgorithm);
            return hash.Equals(storedHash.Hash);
        }

        /// <summary>
        /// Alphabet can be changed by providing an IPasswordConfiguration.
        /// Default values are in DefaultPasswordConfiguration.
        /// </summary>
        /// <param name="passwordLength">Number of chars in the password</param>
        /// <returns>A password of length passwordLength</returns>
        public virtual string CreatePassword(int passwordLength)
        {
            var randomBytes = PasswordUtils.CreateRandomBytes(passwordLength);
            var source = Config.PasswordGenerationAlphabet;
            var n = source.Length;
            return new string(randomBytes
                .Bytes
                .Select(b => source[b % n])
                .ToArray());
        }

        /// <summary>
        /// Alphabet and password length can be changed by providing an IPasswordConfiguration.
        /// Default values are in DefaultPasswordConfiguration.
        /// </summary>
        /// <returns></returns>
        public string CreatePassword()
        {
            return CreatePassword(Config.PasswordGenerationDefaultLength);
        }
    }
}
