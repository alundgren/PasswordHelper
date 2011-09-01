using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace PasswordHelper
{
    public interface IPasswordConfiguration
    {
        int SaltByteCount { get; }
        Encoding PasswordToBytesEncoding { get; }
        string HashAlgorithmName { get; }
        HashAlgorithm CreateHashAlgorithm(string name);
        string PasswordGenerationAlphabet { get; }
        int PasswordGenerationDefaultLength { get; }
    }
}
