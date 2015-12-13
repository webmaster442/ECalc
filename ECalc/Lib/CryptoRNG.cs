using System;
using System.Security.Cryptography;

namespace ECalc.Lib
{
    /// <summary>
    /// Based on: http://www.informit.com/guides/content.aspx?g=dotnet&seqNum=775
    /// </summary>
    public class CryptoRNG: IDisposable
    {
        private const int _BufferSize = 4066;

        private byte[] RandomBuffer;
        private int BufferOffset;
        private RNGCryptoServiceProvider rng;

        public CryptoRNG()
        {
            rng = new RNGCryptoServiceProvider();
            RandomBuffer = new byte[_BufferSize];
            BufferOffset = RandomBuffer.Length;
        }

        ~CryptoRNG()
        {
            Dispose(true);
        }

        private void FillBuffer()
        {
            rng.GetBytes(RandomBuffer);
            BufferOffset = 0;
        }

        public int Next()
        {
            if (BufferOffset >= RandomBuffer.Length)
            {
                FillBuffer();
            }
            int val = BitConverter.ToInt32(RandomBuffer, BufferOffset) & 0x7fffffff;
            BufferOffset += sizeof(int);
            return val;
        }

        public int Next(int maxValue)
        {
            return Next() % maxValue;
        }

        public int Next(int minValue, int maxValue)
        {
            if (maxValue < minValue)
            {
                throw new ArgumentOutOfRangeException("maxValue must be greater than or equal to minValue");
            }
            int range = maxValue - minValue;
            return minValue + Next(range);
        }

        protected virtual void Dispose(bool native)
        {
            if (rng != null)
            {
                rng.Dispose();
                rng = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
