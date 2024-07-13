// <copyright file="BitStream.cs" project="QRCodeGenerator" author="Peter Laudy">
// Copyright © 2024 - Peter Laudy All rights reserved.
// </copyright>
using System;
using System.IO;

namespace QRCodeGenerator
{
    /// <summary>
    /// Stream which allows writing of bits instead of bytes.
    /// </summary>
    public sealed class BitStream
    {
        /// <summary>
        /// Maximum number of bits to add to the stream.
        /// </summary>
        /// <remarks>
        /// The number of bits for a Kanji character is 13, which is the most to add in one time.
        /// </remarks>
        private const int MAX_BIT_LENGTH = 13;

        /// <summary>
        /// The stream in which the data is stored.
        /// </summary>
        private Stream stream = new MemoryStream();

        /// <summary>
        /// The temporary variable to store the bits.
        /// </summary>
        private byte tempData = 0;

        /// <summary>
        /// The number of bits in the temporary variable.
        /// </summary>
        private int tempDataLength = 0;

        /// <summary>
        /// Gets the length of the stream in bytes.
        /// </summary>
        public int Length
        {
            get
            {
                return (int)this.stream.Length;
            }
        }

        /// <summary>
        /// Add one bit of data to the stream.
        /// </summary>
        /// <param name="bit">The bit to add to the stream.</param>
        public void AddBit(bool bit)
        {
            tempData = (byte)((tempData << 1) + (bit ? 1 : 0));
            if (7 == tempDataLength++)
            {
                stream.WriteByte(tempData);
                tempData = 0;
                tempDataLength = 0;
            }
        }

        /// <summary>
        /// Add multiple bits to the stream.
        /// </summary>
        /// <param name="data">The bits to add.</param>
        /// <param name="length">The number of bits to add.</param>
        public void AddBits(int data, int length)
        {
            if (length > BitStream.MAX_BIT_LENGTH)
            {
                throw new ArgumentOutOfRangeException("length");
            }

            int mask = 1 << (length - 1);
            for (int i = 0; i < length; i++)
            {
                AddBit(0 != (data & mask));
                mask >>= 1;
            }
        }

        /// <summary>
        /// Add one byte to the stream.
        /// </summary>
        /// <param name="b">The byte to add to the stream.</param>
        public void WriteByte(byte b)
        {
            stream.WriteByte(b);
        }

        /// <summary>
        /// Write the temporary bits to the stream and fill with zero's.
        /// </summary>
        public void Flush()
        {
            while (0 != tempDataLength)
            {
                AddBit(false);
            }
        }

        /// <summary>
        /// Reset the stream (sets the pointer to the beginning of the stream).
        /// </summary>
        public void ResetStream()
        {
            stream.Seek(0, SeekOrigin.Begin);
        }

        /// <summary>
        /// Get one bit of data from the stream.
        /// </summary>
        /// <returns>The bit read from the stream.</returns>
        public bool GetBit()
        {
            if (0 == (tempDataLength % 8))
            {
                int i = stream.ReadByte();
                if (-1 == i)
                {
                    return false;
                }

                tempData = (byte)i;
                tempDataLength = 0;
            }

            return ((tempData >> (7 - tempDataLength++)) & 0x01) != 0;
        }

        /// <summary>
        /// Gets a multiple number of bits from the stream.
        /// </summary>
        /// <param name="length">The number of bits to read.</param>
        /// <returns>The data containing the requested number of bits.</returns>
        public int GetBits(int length)
        {
            if (length > BitStream.MAX_BIT_LENGTH)
            {
                throw new ArgumentOutOfRangeException("length");
            }

            int result = 0;
            for (int i = 0; i < length; i++)
            {
                result <<= 1;
                if (GetBit())
                {
                    result++;
                }
            }

            return result;
        }

        /// <summary>
        /// Read one byte of data from the stream.
        /// </summary>
        /// <returns>The byte read from the stream.</returns>
        public int ReadByte()
        {
            return stream.ReadByte();
        }

        /// <summary>
        /// Remove all data from the stream.
        /// </summary>
        public void Clear()
        {
            ResetStream();
            stream.SetLength(0);
        }
    }
}