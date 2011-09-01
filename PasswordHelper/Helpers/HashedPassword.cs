using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordHelper
{
    public class HashedPassword
    {
        public ByteArray Salt { get; set; }
        public ByteArray Hash { get; set; }
        public Encoding Encoding { get; set; }
        public string HashAlgorithmName { get; set; }

        public override string ToString()
        {
            return Hash.ToHex();
        }

        public HashedPassword Clone()
        {
            return new HashedPassword
            {
                Salt = this.Salt.Clone(),
                Hash = this.Hash.Clone(),
                Encoding = this.Encoding,
                HashAlgorithmName = this.HashAlgorithmName
            };
        }
    }
}
