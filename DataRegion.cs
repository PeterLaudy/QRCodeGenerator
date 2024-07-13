// <copyright file="DataRegion.cs" project="QRCodeGenerator" author="Peter Laudy">
// Copyright © 2024 - Peter Laudy All rights reserved.
// </copyright>
using System.Collections.Generic;
using System.Drawing;

namespace QRCodeGenerator
{
    /// <summary>
    /// Class which tells us where the data bits are located in the QRCode bitmap.
    /// </summary>
    public class DataRegion
    {
        #region Member variables

        /// <summary>
        /// The alignment patterns for this code.
        /// </summary>
        private AllignmentPatterns allignmentPattern;

        /// <summary>
        /// The size in pixels of the QRCode.
        /// </summary>
        private int size;

        #endregion Member variables

        #region Constructors & destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataRegion"/> class.
        /// </summary>
        /// <param name="version">The version for which we want to find the AP positions.</param>
        public DataRegion(int version)
        {
            size = version * 4 + 17;
            allignmentPattern = new AllignmentPatterns(version);
        }

        #endregion Constructors & destructors

        #region Properties

        /// <summary>
        /// Gets a list of the positions of the alignment patterns.
        /// </summary>
        public List<Point> AllAllignmentPatternPositions
        {
            get
            {
                return allignmentPattern.AllAllignmentPatternPositions;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Check if there is a data bit at the given position.
        /// </summary>
        /// <param name="x">The x coordinate of the position to check.</param>
        /// <param name="y">The y coordinate of the position to check.</param>
        /// <returns>True if a data bit is located at the given position, false if not.</returns>
        public bool IsDataAtPos(int x, int y)
        {
            // Timing patterns
            if ((x == 6) || (y == 6))
            {
                return false;
            }

            // Top-left position detection pattern
            if ((x < 9) && (y < 9))
            {
                return false;
            }

            // Top-right position detection pattern
            if ((x > size - 9) && (y < 9))
            {
                return false;
            }

            // Bottom-left position detection pattern
            if ((x < 9) && (y > size - 9))
            {
                return false;
            }

            // Version information
            if (size >= 45)
            {
                // Top-right version information
                if ((x > size - 12) && (y < 7))
                {
                    return false;
                }

                // Bottom-left version information
                if ((x < 7) && (y > size - 12))
                {
                    return false;
                }
            }

            return !this.allignmentPattern.IsAPAtPos(x, y);
        }

        #endregion Methods
    }
}