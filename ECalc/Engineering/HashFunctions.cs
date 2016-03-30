using DamienG.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ECalc.Engineering
{
    internal static class HashFunctions
    {
        public enum Algorithms
        {
            MD5, SHA1, SHA256, SHA512, CRC32, CRC64
        }

        private static string BytesToString(byte[] bytes)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++) sb.Append(bytes[i].ToString("X2"));
            return sb.ToString();
        }

        private static string MD5String(string input)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            return BytesToString(hash);
        }

        private static string SHA1String(string input)
        {
            SHA1 sha1 = SHA1.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hash = sha1.ComputeHash(inputBytes);

            return BytesToString(hash);
        }

        private static string SHA256String(string input)
        {
            SHA256 sha256 = SHA256.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hash = sha256.ComputeHash(inputBytes);

            return BytesToString(hash);
        }

        private static string SHA512String(string input)
        {
            SHA512 sha512 = SHA512.Create();

            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hash = sha512.ComputeHash(inputBytes);

            return BytesToString(hash);
        }

        private static string Crc32String(string input)
        {
            var crc32 = new Crc32();
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hash = crc32.ComputeHash(inputBytes);

            return BytesToString(hash);
        }

        private static string Crc64String(string input)
        {
            Crc64 crc64 = new Crc64Iso();
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hash = crc64.ComputeHash(inputBytes);

            return BytesToString(hash);
        }

        public static Task<string> HashString(Algorithms algorithm, string input)
        {
            return Task.Run(() =>
            {
                switch (algorithm)
                {
                    case Algorithms.MD5:
                        return MD5String(input);
                    case Algorithms.CRC32:
                        return Crc32String(input);
                    case Algorithms.CRC64:
                        return Crc64String(input);
                    case Algorithms.SHA1:
                        return SHA1String(input);
                    case Algorithms.SHA256:
                        return SHA256String(input);
                    case Algorithms.SHA512:
                        return SHA512String(input);
                    default:
                        return "";
                }
            });
        }

        public static Task<string> HashFile(Algorithms algorithm, string filename)
        {
            return Task.Run(() =>
            {
                HashAlgorithm hasher = null;
                byte[] hash = null;
                switch (algorithm)
                {
                    case Algorithms.MD5:
                        hasher = MD5.Create();
                        break;
                    case Algorithms.CRC32:
                        hasher = new Crc32();
                        break;
                    case Algorithms.CRC64:
                        hasher = new Crc64Iso();
                        break;
                    case Algorithms.SHA1:
                        hasher = SHA1.Create();
                        break;
                    case Algorithms.SHA256:
                        hasher = SHA256.Create();
                        break;
                    case Algorithms.SHA512:
                        hasher = SHA512.Create();
                        break;
                }
                using (var stream = File.OpenRead(filename))
                {
                     hash = hasher.ComputeHash(stream);
                }
                hasher.Dispose();
                hasher = null;
                return BytesToString(hash);
            });
        }

    }
}
