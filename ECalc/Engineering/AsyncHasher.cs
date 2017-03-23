using DamienG.Security.Cryptography;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ECalc.Engineering
{
    public enum HashAlgorithms
    {
        MD5, SHA1, SHA256, SHA512, CRC32, CRC64
    }

    public class AsyncHasher
    {
        protected HashAlgorithm _hashAlgorithm;
        protected byte[] _hash;

        public AsyncHasher(HashAlgorithms algorithm)
        {
            switch (algorithm)
            {
                case HashAlgorithms.MD5:
                    _hashAlgorithm = MD5.Create();
                    break;
                case HashAlgorithms.SHA1:
                    _hashAlgorithm = SHA1.Create();
                    break;
                case HashAlgorithms.SHA256:
                    _hashAlgorithm = SHA256.Create();
                    break;
                case HashAlgorithms.SHA512:
                    _hashAlgorithm = SHA512.Create();
                    break;
                case HashAlgorithms.CRC32:
                    _hashAlgorithm = new Crc32();
                    break;
                case HashAlgorithms.CRC64:
                    _hashAlgorithm = new Crc64Iso();
                    break;
            }
        }

        public static string HashString(byte[] hash)
        {
            if (hash == null) return null;
            StringBuilder hex = new StringBuilder();
            foreach (byte b in hash)
                hex.AppendFormat("{0:x2}", b);

            return hex.ToString();
        }

        public Task<byte[]> ComputeHash(string input)
        {
            return  Task.Run(() =>
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                return _hashAlgorithm.ComputeHash(inputBytes);
            });
        }

        public Task<byte[]> ComputeHash(Stream stream, CancellationToken ct, IProgress<double> progress = null)
        {
            return Task.Run(() =>
            {

                var canceled = false;
                _hash = null;

                byte[] readAheadBuffer, buffer;
                var buffersize = 4096 * 4;
                int readAheadBytesRead, bytesRead;
                long size, totalBytesRead = 0;

                size = stream.Length;
                readAheadBuffer = new byte[buffersize];
                readAheadBytesRead = stream.Read(readAheadBuffer, 0, readAheadBuffer.Length);

                totalBytesRead += readAheadBytesRead;

                do
                {
                    if (ct.IsCancellationRequested)
                    {
                        canceled = true;
                        break;
                    }

                    bytesRead = readAheadBytesRead;
                    buffer = readAheadBuffer;

                    readAheadBuffer = new byte[buffersize];
                    readAheadBytesRead = stream.Read(readAheadBuffer, 0, readAheadBuffer.Length);

                    totalBytesRead += readAheadBytesRead;

                    if (readAheadBytesRead == 0)
                        _hashAlgorithm.TransformFinalBlock(buffer, 0, bytesRead);
                    else
                        _hashAlgorithm.TransformBlock(buffer, 0, bytesRead, buffer, 0);

                    if (progress != null)
                    {
                        double val = totalBytesRead / (double)size;
                        progress.Report(val);
                    }

                }
                while (readAheadBytesRead != 0);

                if (canceled) return null;
                return _hashAlgorithm.Hash;
            });
        }
    }
}
