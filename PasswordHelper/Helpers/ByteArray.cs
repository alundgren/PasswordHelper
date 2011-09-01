using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordHelper
{
    /// <summary>
    /// Wrappter class of a byte array.
    /// 
    /// </summary>
    public class ByteArray : IEquatable<ByteArray>
    {
        private byte[] _bytes;

        protected ByteArray(byte[] bytes)
        {
            _bytes = bytes;
        }

        /// <summary>
        /// Note that base64 string are case sensitive
        /// </summary>
        public virtual string ToBase64()
        {
            return Convert.ToBase64String(_bytes);
        }

        /// <summary>
        /// Hex string, lowercase without '-' between letter pairs.
        /// </summary>
        public virtual string ToHex()
        {
            //http://stackoverflow.com/questions/623104/c-byte-to-hex-string
            return BitConverter.ToString(_bytes).Replace("-", "").ToLowerInvariant();
        }

        public virtual byte[] Bytes
        {
            get { return _bytes; }
            protected set { _bytes = value; }
        }

        public virtual ByteArray Clone()
        {
            var c = new byte[Bytes.Length];
            Array.Copy(Bytes, c, c.Length);
            return new ByteArray(c);
        }

        public static ByteArray Concat(ByteArray b1, ByteArray b2)
        {
            if (b1 == null)
                throw new ArgumentNullException("b1");
            if (b2 == null)
                throw new ArgumentNullException("b2");

            var a1 = b1.Bytes;
            var a2 = b2.Bytes;

            var aa = new Byte[a1.Length + a2.Length];
            Array.Copy(a1, 0, aa, 0, a1.Length);
            Array.Copy(a2, 0, aa, a1.Length, a2.Length);

            return new ByteArray(aa);
        }

        /// <summary>
        /// Hex string can be upper or lower case and with our without '-' between pairs of letters.
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static ByteArray FromHex(string hex)
        {
            if (hex == null)
                throw new ArgumentNullException("hex");
            if (hex.Length % 2 != 0)
                throw new FormatException("Should be a hex string");

            var s = hex.ToLowerInvariant().Replace("-", "");
            var n = s.Length;
            var bytes = new byte[n / 2];
            for (int i = 0; i < n; i += 2)
                bytes[i / 2] = Convert.ToByte(s.Substring(i, 2), 16);
            return new ByteArray(bytes);
        }

        /// <summary>
        /// Note that base64 string are case sensitive
        /// </summary>
        public static ByteArray FromBase64(string base64)
        {
            if (base64 == null)
                throw new ArgumentNullException("base64");
            return new ByteArray(Convert.FromBase64String(base64));
        }

        /// <summary>
        /// Makes a copy of the input byte[]
        /// </summary>
        public static ByteArray FromBytes(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException("bytes");
            var b = new ByteArray(bytes);
            return b.Clone();
        }

        public bool Equals(ByteArray other)
        {
            return Bytes.SequenceEqual(other.Bytes);
        }
        public override bool Equals(object obj)
        {
            var o = obj as ByteArray;
            return o != null && Equals(o);
        }
        public override int GetHashCode()
        {
            return Bytes.GetHashCode();
        }
    }
}
