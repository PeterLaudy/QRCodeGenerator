// <copyright file="MaskPattern.cs" project="QRCodeGenerator" author="Peter Laudy">
// Copyright © 2024 - Peter Laudy All rights reserved.
// </copyright>

namespace QRCodeGenerator
{
    /// <summary>
    /// Class to xor the bitmap with a pattern.
    /// </summary>
    internal class MaskPattern
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
        public BitArray MaskBitmap(BitArray bmp)
        {
            var result = new BitArray(bmp.Version);
            result.Set3CornerMarkers();

            foreach (Point p in mask.AllAllignmentPatternPositions)
            {
                for (var i = -2; i < 2; i++)
                {
                    result[p.X + i, p.Y - 2] = true;
                    result[p.X + 2, p.Y + i] = true;
                    result[p.X - i, p.Y + 2] = true;
                    result[p.X - 2, p.Y - i] = true;
                    result[p.X, p.Y] = true;
                }
            }

            // Black pixel near the bottom left corner of the bitmap.
            result[8, bmp.Size - 7] = true;

            for (int i = 8; i < result.Size- 8; i += 2)
            {
                result[i, 6] = true;
                result[6, i] = true;
            }

            for (int x = 0; x < result.Size; x++)
            {
                for (int y = 0; y < result.Size; y++)
                {
                    if (mask.IsDataAtPos(x, y))
                    {
                        if (mask.MustInvert(x, y))
                        {
                            result[x, y] = !bmp[x, y];
                        }
                        else
                        {
                            result[x, y] = bmp[x, y];
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
        public BitArray UnmaskBitmap(BitArray bmp)
        {
            BitArray result = new BitArray(bmp.Version);

            for (int x = 0; x < result.Size; x++)
            {
                for (int y = 0; y < result.Size; y++)
                {
                    if (mask.IsDataAtPos(x, y))
                    {
                        if (mask.MustInvert(x, y))
                        {
                            result[x, y] = !bmp[x, y];
                        }
                        else
                        {
                            result[x, y] = bmp[x, y];
                        }
                    }
                    else
                    {
                        result[x, y] = bmp[x, y];
                    }
                }
            }

            return result;
        }

        #endregion Methods
    }
}