// <copyright file="MaskPattern.cs" project="QRCodeGenerator" author="Peter Laudy">
// Copyright © 2024 - Peter Laudy All rights reserved.
// </copyright>
using System;
using System.Collections.Generic;
using System.Drawing;

namespace QRCodeGenerator
{
    /// <summary>
    /// Class to xor the bitmap with a pattern.
    /// </summary>
    public class MaskPattern
    {
        #region Nested classes

        /// <summary>
        /// Base class to handle masking of the data.
        /// </summary>
        private abstract class MaskBase
        {
            /// <summary>
            /// Tells us which pixels in the bitmap to xor.
            /// </summary>
            private DataRegion dataRegion;

            /// <summary>
            /// Initializes a new instance of the <see cref="MaskBase"/> class.
            /// </summary>
            /// <param name="version">The version of the QRCode for which this class is created.</param>
            public MaskBase(int version)
            {
                dataRegion = new DataRegion(version);
            }

            /// <summary>
            /// Check if the pixel at the given position must be inverted.
            /// </summary>
            /// <param name="x">The x position of the pixel.</param>
            /// <param name="y">The y position of the pixel.</param>
            /// <returns>True if the pixel must be inverted, false if not.</returns>
            public virtual bool MustInvert(int x, int y)
            {
                return dataRegion.IsDataAtPos(x, y);
            }

            /// <summary>
            /// Check if the pixel at the given position represents a data bit.
            /// </summary>
            /// <param name="x">The x position of the pixel.</param>
            /// <param name="y">The y position of the pixel.</param>
            /// <returns>True if the pixel represents a data bit, false if not.</returns>
            public bool IsDataAtPos(int x, int y)
            {
                return dataRegion.IsDataAtPos(x, y);
            }

            /// <summary>
            /// Gets a list of the positions of the alignment patterns.
            /// </summary>
            public List<Point> AllAllignmentPatternPositions
            {
                get
                {
                    return dataRegion.AllAllignmentPatternPositions;
                }
            }
        }

        /// <summary>
        /// Class to handle masking of the data for pattern reference 000.
        /// </summary>
        private class Mask000 : MaskBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Mask000"/> class.
            /// </summary>
            /// <param name="version">The version of the QRCode represented by the bitmap.</param>
            public Mask000(int version) : base(version)
            {
            }

            /// <summary>
            /// Check if the pixel at the given position must be inverted.
            /// </summary>
            /// <param name="x">The x position of the pixel.</param>
            /// <param name="y">The y position of the pixel.</param>
            /// <returns>True if the pixel must be inverted, false if not.</returns>
            public override bool MustInvert(int x, int y)
            {
                return base.MustInvert(x, y) & (0 == (x + y) % 2);
            }
        }

        /// <summary>
        /// Class to handle masking of the data for pattern reference 000.
        /// </summary>
        private class Mask001 : MaskBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Mask001"/> class.
            /// </summary>
            /// <param name="version">The version of the QRCode represented by the bitmap.</param>
            public Mask001(int version) : base(version)
            {
            }

            /// <summary>
            /// Check if the pixel at the given position must be inverted.
            /// </summary>
            /// <param name="x">The x position of the pixel.</param>
            /// <param name="y">The y position of the pixel.</param>
            /// <returns>True if the pixel must be inverted, false if not.</returns>
            public override bool MustInvert(int x, int y)
            {
                return base.MustInvert(x, y) & (0 == y % 2);
            }
        }

        /// <summary>
        /// Class to handle masking of the data for pattern reference 000.
        /// </summary>
        private class Mask010 : MaskBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Mask010"/> class.
            /// </summary>
            /// <param name="version">The version of the QRCode represented by the bitmap.</param>
            public Mask010(int version) : base(version)
            {
            }

            /// <summary>
            /// Check if the pixel at the given position must be inverted.
            /// </summary>
            /// <param name="x">The x position of the pixel.</param>
            /// <param name="y">The y position of the pixel.</param>
            /// <returns>True if the pixel must be inverted, false if not.</returns>
            public override bool MustInvert(int x, int y)
            {
                return base.MustInvert(x, y) & (0 == x % 3);
            }
        }

        /// <summary>
        /// Class to handle masking of the data for pattern reference 000.
        /// </summary>
        private class Mask011 : MaskBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Mask011"/> class.
            /// </summary>
            /// <param name="version">The version of the QRCode represented by the bitmap.</param>
            public Mask011(int version) : base(version)
            {
            }

            /// <summary>
            /// Check if the pixel at the given position must be inverted.
            /// </summary>
            /// <param name="x">The x position of the pixel.</param>
            /// <param name="y">The y position of the pixel.</param>
            /// <returns>True if the pixel must be inverted, false if not.</returns>
            public override bool MustInvert(int x, int y)
            {
                return base.MustInvert(x, y) & (0 == (x + y) % 3);
            }
        }

        /// <summary>
        /// Class to handle masking of the data for pattern reference 000.
        /// </summary>
        private class Mask100 : MaskBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Mask100"/> class.
            /// </summary>
            /// <param name="version">The version of the QRCode represented by the bitmap.</param>
            public Mask100(int version) : base(version)
            {
            }

            /// <summary>
            /// Check if the pixel at the given position must be inverted.
            /// </summary>
            /// <param name="x">The x position of the pixel.</param>
            /// <param name="y">The y position of the pixel.</param>
            /// <returns>True if the pixel must be inverted, false if not.</returns>
            public override bool MustInvert(int x, int y)
            {
                return base.MustInvert(x, y) & (0 == ((x / 3) + (y / 2)) % 2);
            }
        }

        /// <summary>
        /// Class to handle masking of the data for pattern reference 000.
        /// </summary>
        private class Mask101 : MaskBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Mask101"/> class.
            /// </summary>
            /// <param name="version">The version of the QRCode represented by the bitmap.</param>
            public Mask101(int version) : base(version)
            {
            }

            /// <summary>
            /// Check if the pixel at the given position must be inverted.
            /// </summary>
            /// <param name="x">The x position of the pixel.</param>
            /// <param name="y">The y position of the pixel.</param>
            /// <returns>True if the pixel must be inverted, false if not.</returns>
            public override bool MustInvert(int x, int y)
            {
                return base.MustInvert(x, y) & (0 == (x * y) % 2 + (x * y) % 3);
            }
        }

        /// <summary>
        /// Class to handle masking of the data for pattern reference 000.
        /// </summary>
        private class Mask110 : MaskBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Mask110"/> class.
            /// </summary>
            /// <param name="version">The version of the QRCode represented by the bitmap.</param>
            public Mask110(int version) : base(version)
            { 
            }

            /// <summary>
            /// Check if the pixel at the given position must be inverted.
            /// </summary>
            /// <param name="x">The x position of the pixel.</param>
            /// <param name="y">The y position of the pixel.</param>
            /// <returns>True if the pixel must be inverted, false if not.</returns>
            public override bool MustInvert(int x, int y)
            {
                return base.MustInvert(x, y) & (0 == ((x * y) % 2 + (x * y) % 3) % 2);
            }
        }

        /// <summary>
        /// Class to handle masking of the data for pattern reference 000.
        /// </summary>
        private class Mask111 : MaskBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Mask111"/> class.
            /// </summary>
            /// <param name="version">The version of the QRCode represented by the bitmap.</param>
            public Mask111(int version) : base(version)
            {
            }

            /// <summary>
            /// Check if the pixel at the given position must be inverted.
            /// </summary>
            /// <param name="x">The x position of the pixel.</param>
            /// <param name="y">The y position of the pixel.</param>
            /// <returns>True if the pixel must be inverted, false if not.</returns>
            public override bool MustInvert(int x, int y)
            {
                return base.MustInvert(x, y) & (0 == ((x * y) % 3 + (x + y) % 2) % 2);
            }
        }

        #endregion Nested classes

        #region Member variables

        /// <summary>
        /// The mask which xors the bitmap.
        /// </summary>
        private MaskBase mask;

        #endregion Member variables

        #region Constructors & destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MaskPattern"/> class.
        /// </summary>
        /// <param name="maskPatternReference">The value which defines which mask to use.</param>
        /// <param name="version">The version of the QRCode to mask.</param>
        public MaskPattern(int maskPatternReference, int version)
        {
            switch (maskPatternReference)
            {
                case 0:
                    mask = new Mask000(version);
                    break;
                case 1:
                    mask = new Mask001(version);
                    break;
                case 2:
                    mask = new Mask010(version);
                    break;
                case 3:
                    mask = new Mask011(version);
                    break;
                case 4:
                    mask = new Mask100(version);
                    break;
                case 5:
                    mask = new Mask101(version);
                    break;
                case 6:
                    mask = new Mask110(version);
                    break;
                case 7:
                    mask = new Mask111(version);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion Constructors & destructors

        #region Methods

        /// <summary>
        /// Mask the bitmap.
        /// </summary>
        /// <param name="bmp">The bitmap to mask.</param>
        /// <returns>The masked bitmap.</returns>
        public Bitmap MaskBitmap(Bitmap bmp)
        {
            Bitmap result = new Bitmap(bmp.Width, bmp.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            int size = result.Width - 1;
            using (Graphics g = Graphics.FromImage(result))
            using (Brush blackBrush = new SolidBrush(Color.Black))
            using (Pen blackPen = new Pen(blackBrush))
            {
                g.Clear(Color.White);
                g.DrawRectangle(blackPen, 0, 0, 6, 6);
                g.DrawRectangle(blackPen, size - 6, 0, 6, 6);
                g.DrawRectangle(blackPen, 0, size - 6, 6, 6);

                g.FillRectangle(blackBrush, 2, 2, 3, 3);
                g.FillRectangle(blackBrush, size - 4, 2, 3, 3);
                g.FillRectangle(blackBrush, 2, size - 4, 3, 3);

                foreach (Point p in mask.AllAllignmentPatternPositions)
                {
                    g.DrawRectangle(blackPen, p.X - 2, p.Y - 2, 4, 4);
                    result.SetPixel(p.X, p.Y, Color.Black);
                }
            }

            // Black pixel near the bottom left corner of the bitmap.
            result.SetPixel(8, size - 7, Color.Black);

            for (int i = 8; i < result.Width - 8; i += 2)
            {
                result.SetPixel(i, 6, Color.Black);
                result.SetPixel(6, i, Color.Black);
            }

            for (int x = 0; x < result.Width; x++)
            {
                for (int y = 0; y < result.Height; y++)
                {
                    if (mask.IsDataAtPos(x, y))
                    {
                        if (mask.MustInvert(x, y))
                        {
                            if (bmp.GetPixel(x, y).ToArgb() == Color.Black.ToArgb())
                            {
                                result.SetPixel(x, y, Color.White);
                            }
                            else
                            {
                                result.SetPixel(x, y, Color.Black);
                            }
                        }
                        else
                        {
                            result.SetPixel(x, y, bmp.GetPixel(x, y));
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Unmask to bitmap.
        /// </summary>
        /// <remarks>
        /// The non-data bits are converted to red pixels.
        /// </remarks>
        /// <param name="bmp">The bitmap to unmask.</param>
        /// <returns>The unmasked bitmap.</returns>
        public Bitmap UnmaskBitmap(Bitmap bmp)
        {
            Bitmap result = new Bitmap(bmp.Width, bmp.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.Clear(Color.White);
            }

            for (int x = 0; x < result.Width; x++)
            {
                for (int y = 0; y < result.Height; y++)
                {
                    if (mask.IsDataAtPos(x, y))
                    {
                        if (mask.MustInvert(x, y))
                        {
                            if (bmp.GetPixel(x, y).ToArgb() == Color.Black.ToArgb())
                            {
                                result.SetPixel(x, y, Color.White);
                            }
                            else
                            {
                                result.SetPixel(x, y, Color.Black);
                            }
                        }
                        else
                        {
                            result.SetPixel(x, y, bmp.GetPixel(x, y));
                        }
                    }
                    else
                    {
                        if (bmp.GetPixel(x, y).ToArgb() == Color.Black.ToArgb())
                        {
                            result.SetPixel(x, y, Color.FromArgb(128, 0, 0));
                        }
                        else
                        {
                            result.SetPixel(x, y, Color.FromArgb(255, 64, 64));
                        }
                    }
                }
            }

            return result;
        }

        #endregion Methods
    }
}