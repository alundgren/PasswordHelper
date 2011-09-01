using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace PasswordHelper
{
    public class DefaultPasswordConfiguration : IPasswordConfiguration
    {
        public virtual int SaltByteCount
        {
            get { return 16; }
        }

        public virtual Encoding PasswordToBytesEncoding
        {
            get { return Encoding.UTF8; }
        }

        public string HashAlgorithmName
        {
            get { return "SHA256"; }
        }

        public HashAlgorithm CreateHashAlgorithm(string name)
        {
            return HashAlgorithm.Create(name);
        }

        /// <summary>
        /// Alphabet:
        /// a-z
        /// A-Z
        /// 0-9
        /// Special chars: ! ? $ % & # @
        /// Note: 
        /// 0, O, o, I, i, 1 are excluded to avoid confusion when reading a wierd font/handwritten note.
        /// 62 char alphabet: 5.95 bits of entropy / char
        /// </summary>
        public string PasswordGenerationAlphabet
        {
            get { return "abcdefghjkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ23456789!?$%&#@"; }
        }

        /// <summary>
        /// Somewhere around 80 bits of entropy was the recommendation at http://en.wikipedia.org/wiki/Password_strength
        /// as of 2011-09-01.
        /// </summary>
        public int PasswordGenerationDefaultLength
        {
            get { return 14; }
        }
    }
}
