// <copyright file="BitArray.cs" project="QRCodeGenerator" author="Peter Laudy">
// Copyright © 2024 - Peter Laudy All rights reserved.
// </copyright>

using SkiaSharp;

namespace QRCodeGenerator
{
    internal class Point
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }
    }
    
    internal class BitArray
    {
        private byte[,] data;

        public BitArray(SKBitmap bmp)
        {
            var topLeft = FindTopLeft(bmp);
            var topRight = FindTopRight(bmp);
            var bottomLeft = FindBottomLeft(bmp);

            var w = topRight.X - bottomLeft.X + 1;
            var h = bottomLeft.Y - topRight.Y + 1;

            var version = (w - 17) >> 2;
            if ((w != h) || (version < 1) || (version > 40))
            {
                throw new FormatException("This is not a valid QR-Code.");
            }

            Version = version;

            Size = 17 + version * 4;
            var xSize = (17 + version * 4 + 7) >> 3;
            data = new byte[Size, xSize];
            ClearArray();

            for (var x = 0; x < Size; x++)
            {
                for (var y = 0; y < Size; y++)
                {
                    bmp.GetPixel(x + topLeft.X, y + topLeft.Y).ToHsl(out _, out _, out var l);
                    this[x, y] = l < 50F;
                }
            }
        }

        public BitArray(int version)
        {
            if ((version < 1) || (version > 40))
            {
                throw new ArgumentOutOfRangeException($"{nameof(version)}", "Size must be in the range of 1-40.");
            }

            Version = version;

            Size = 17 + version * 4;
            var xSize = (17 + version * 4 + 7) >> 3;
            data = new byte[Size, xSize];
            ClearArray();
        }

        public void ClearArray()
        {
            for (int y = 0; y < Size; y++)
                for (int x = 0; x < (Size + 7) >> 3; x++)
                    data[y, x] = 0;
        }

        private static Point FindTopLeft(SKBitmap bmp)
        {
            var result = new Point(-1, -1);
            var l = 100F;
            while (l > 50F)
            {
                bmp.GetPixel(++(result.X), ++(result.Y)).ToHsl(out _, out _, out l);
            }

            while ((l <= 50F) && (result.X >= 0))
            {
                bmp.GetPixel(--(result.X), result.Y).ToHsl(out _, out _, out l);
            }

            (result.X)++;
            l = 0;

            while ((l <= 50F) && (result.Y >= 0))
            {
                bmp.GetPixel(result.X, --(result.Y)).ToHsl(out _, out _, out l);
            }

            (result.Y)++;

            return result;
        }

        private static Point FindTopRight(SKBitmap bmp)
        {
            var result = new Point(bmp.Width, -1);
            var l = 100F;
            while (l > 50F)
            {
                bmp.GetPixel(--(result.X), ++(result.Y)).ToHsl(out _, out _, out l);
            }

            while ((l <= 50F) && (result.X < bmp.Width))
            {
                bmp.GetPixel(++(result.X), result.Y).ToHsl(out _, out _, out l);
            }

            (result.X)--;
            l = 0;

            while ((l <= 50F) && (result.Y >= 0))
            {
                bmp.GetPixel(result.X, --(result.Y)).ToHsl(out _, out _, out l);
            }

            (result.Y)++;

            return result;
        }

        private static Point FindBottomLeft(SKBitmap bmp)
        {
            var result = new Point(-1, bmp.Height);
            var l = 100F;
            while (l > 50F)
            {
                bmp.GetPixel(++(result.X), --(result.Y)).ToHsl(out _, out _, out l);
            }

            while ((l <= 50F) && (result.X >= 0))
            {
                bmp.GetPixel(--(result.X), result.Y).ToHsl(out _, out _, out l);
            }

            (result.X)++;
            l = 0;

            while ((l <= 50F) && (result.Y < bmp.Height))
            {
                bmp.GetPixel(result.X, ++(result.Y)).ToHsl(out _, out _, out l);
            }

            (result.Y)--;

            return result;
        }

        /// <summary>
        /// Set the 3 large corner markers.
        /// </summary>
        /// <remarks>
        /// Make sure this is called when the bit array is clear.
        /// </remarks>
        public void Set3CornerMarkers()
        {
            // Top-left marker is 8 bit wide, making it easy to set it.
            data[0, 0] = 0b_01111111;
            data[1, 0] = 0b_01000001;
            data[2, 0] = 0b_01011101;
            data[3, 0] = 0b_01011101;
            data[4, 0] = 0b_01011101;
            data[5, 0] = 0b_01000001;
            data[6, 0] = 0b_01111111;
            data[7, 0] = 0;

            // Bottom-left marker is 8 also bit wide, making it easy to set it.
            data[Size - 1, 0] = 0b_01111111;
            data[Size - 2, 0] = 0b_01000001;
            data[Size - 3, 0] = 0b_01011101;
            data[Size - 4, 0] = 0b_01011101;
            data[Size - 5, 0] = 0b_01011101;
            data[Size - 6, 0] = 0b_01000001;
            data[Size - 7, 0] = 0b_01111111;
            data[Size - 8, 0] = 0;

            // Bottom-left marker is 8 also bit wide, making it easy to set it.
            data[Size - 1, 0] = 0b_01111111;
            data[Size - 2, 0] = 0b_01000001;
            data[Size - 3, 0] = 0b_01011101;
            data[Size - 4, 0] = 0b_01011101;
            data[Size - 5, 0] = 0b_01011101;
            data[Size - 6, 0] = 0b_01000001;
            data[Size - 7, 0] = 0b_01111111;
            data[Size - 8, 0] = 0;

            // Top-right marker is a bit harder, as we have to split it over 2 bytes.
            // Since the size is 17 + Version 4, there are two options:

            if (Version % 2 == 0)
            {
                // Version is even. First 7 bits are together on the last but one byte. Last bit is at the last byte. 

                var x = (Size - 9) >> 3;
                data[0, x] = 0b_11111100;
                data[1, x] = 0b_00000100;
                data[2, x] = 0b_01110100;
                data[3, x] = 0b_01110100;
                data[4, x] = 0b_01110100;
                data[5, x] = 0b_00000100;
                data[6, x] = 0b_11111100;
                data[7, x] = 0;

                x++;
                data[0, x] = 0b_00000001;
                data[1, x] = 0b_00000001;
                data[2, x] = 0b_00000001;
                data[3, x] = 0b_00000001;
                data[4, x] = 0b_00000001;
                data[5, x] = 0b_00000001;
                data[6, x] = 0b_00000001;
                data[7, x] = 0;
            }
            else
            {
                // Version is odd. First 3 bits are together on the last but one byte. Last 5 bits is at the last byte. 

                var x = (Size - 9) >> 3;
                data[0, x] = 0b_11000000;
                data[1, x] = 0b_01000000;
                data[2, x] = 0b_01000000;
                data[3, x] = 0b_01000000;
                data[4, x] = 0b_01000000;
                data[5, x] = 0b_01000000;
                data[6, x] = 0b_11000000;
                data[7, x] = 0;

                x++;
                data[0, x] = 0b_00011111;
                data[1, x] = 0b_00010000;
                data[2, x] = 0b_00010111;
                data[3, x] = 0b_00010111;
                data[4, x] = 0b_00010111;
                data[5, x] = 0b_00010000;
                data[6, x] = 0b_00011111;
                data[7, x] = 0;
            }
        }

        public bool this[int x, int y]
        {
            get
            {
                return 0 != (data[y, x >> 3] & (0x01 << (x & 0x07)));
            }
            set
            {
                if (value)
                {
                    data[y, x >> 3] |= (byte)(0x01U << (x & 0x07));
                }
                else
                {
                    data[y, x >> 3] &= (byte)~(0x01U << (x & 0x07));
                }
            }
        }

        public SKBitmap CreateBitmap(int pixelsPerBit)
        {
            // Create a bitmap, 8 pixels wider, to account for the quiet zone around the code.
            var result = new SKBitmap((Size + 8) * pixelsPerBit, (Size + 8) * pixelsPerBit, SKColorType.Gray8, SKAlphaType.Opaque);
            var canvas = new SKCanvas(result);
            canvas.Clear(SKColors.White);
            for (var x = 0; x < Size; x++)
            {
                for (var y = 0; y < Size; y++)
                {
                    result.SetPixel(x + 4, y + 4, this[x, y] ? SKColors.Black : SKColors.White);
                }
            }
           
            return result;
        }

        public int Version { get; private set; }

        public int Size { get; private set; }
    }
}