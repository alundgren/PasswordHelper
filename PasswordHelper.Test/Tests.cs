using System;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography;

namespace PasswordHelper.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void VerifyByteArrayMutability()
        {
            var b1 = new byte[] { 42 };
            var a1 = ByteArray.FromBytes(b1);
            a1.Bytes[0] = 43;
            Assert.AreEqual(b1[0], 42);

        }

        [TestMethod]
        public void VerifyByteArrayRoundTrippingAndFormatTolerance()
        {
            var knownByteSequence = new byte[] {55, 254, 0, 255, 42, 8, 10};
            const string knownHex = "37fe00ff2a080a"; //note, not case sensetive
            const string knownBase64 = "N/4A/yoICg=="; //note: case sensitive
   
            //FromHex
            ByteArray.FromHex(knownHex).Bytes.Is(knownByteSequence);
            ByteArray.FromHex(knownHex.ToLowerInvariant()).Bytes.Is(knownByteSequence);
            ByteArray.FromHex(knownHex.ToLowerInvariant().Replace("-", "")).Bytes.Is(knownByteSequence);
            ByteArray.FromHex(knownHex.Replace("-", "")).Bytes.Is(knownByteSequence);
            //ToHex
            ByteArray.FromBytes(knownByteSequence).ToHex().Is(knownHex);
            ByteArray.FromHex("37-FE-00-FF-2A-08-0A").ToHex().Is(knownHex);

            //Base64
            ByteArray.FromBase64(knownBase64).Bytes.Is(knownByteSequence);
            ByteArray.FromBytes(knownByteSequence).ToBase64().Is(knownBase64);
        }

        [TestMethod]
        public void VerifyConcatBytesArrays()
        {
            ByteArray.Concat(
                    ByteArray.FromBytes(new byte[] { 1, 2, 3 }), 
                    ByteArray.FromBytes(new byte[] { 255, 254, 253 }))
                .Bytes.Is<byte>(1, 2, 3, 255, 254, 253);
            ByteArray.Concat(
                    ByteArray.FromBytes(new byte[] { }),
                    ByteArray.FromBytes(new byte[] { 255, 254, 253 }))
                .Bytes.Is<byte>(255, 254, 253);
            ByteArray.Concat(
                    ByteArray.FromBytes(new byte[] { 1, 2, 3 }),
                    ByteArray.FromBytes(new byte[] { }))
                .Bytes.Is<byte>(1, 2, 3);
            ByteArray.Concat(
                    ByteArray.FromBytes(new byte[] { }),
                    ByteArray.FromBytes(new byte[] { }))
                .Bytes.Is<byte>();
        }

        [TestMethod]
        public void TestHashPassword()
        {
            const string clearTextPassword = "correct horse battery staple";
            var helper = new PasswordAssistant();
            var hash = helper.CreateHashFromClearTextPassword(clearTextPassword);

            //Correct password works
            Assert.IsTrue(helper.IsPasswordCorrect("correct horse battery staple", hash));

            //Wrong password doesnt work
            Assert.IsFalse(helper.IsPasswordCorrect("correct horsebattery staple", hash));
            Assert.IsFalse(helper.IsPasswordCorrect("correct horse bättery staple", hash));
            Assert.IsFalse(helper.IsPasswordCorrect("correct horse battery staple ", hash));
            Assert.IsFalse(helper.IsPasswordCorrect("correct horse battery staplE", hash));

            //Wrong salt doesnt work
            var h2 = hash.Clone();
            h2.Salt = PasswordUtils.CreateRandomBytes(new DefaultPasswordConfiguration().SaltByteCount);
            Assert.IsFalse(helper.IsPasswordCorrect("correct horse battery staple", h2));

            //Generated password works
            var genP = helper.CreatePassword();
            var genH = helper.CreateHashFromClearTextPassword(genP);
            Assert.IsTrue(helper.IsPasswordCorrect(genP, genH));
            Assert.AreEqual(new DefaultPasswordConfiguration().PasswordGenerationDefaultLength, genP.Length);
        }

        [TestMethod]
        public void PasswordGenerationTimeSanityCheck()
        {
            var h = new PasswordAssistant();
            var w = Stopwatch.StartNew();
            Enumerable.Range(1, 10000).Select(_ => h.CreatePassword()).Distinct().Count().Is(10000);
            w.Stop();
            w.ElapsedMilliseconds.Is(e => e < 1000);
        }
    }
}
