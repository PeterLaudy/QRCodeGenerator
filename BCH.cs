// <copyright file="BCH.cs" project="QRCodeGenerator" author="Peter Laudy">
// Copyright © 2024 - Peter Laudy All rights reserved.
// </copyright>
using System;

namespace QRCodeGenerator
{
    /// <summary>
    /// Class to calculate checksums according to the Reed Solomon algorithm.
    /// </summary>
    public class BCH
    {
        /// <summary>
        /// Calculate the BCH checksum for the given format data.
        /// </summary>
        /// <param name="data">The data to calculate the checksum over.</param>
        /// <returns>The calculated checksum.</returns>
        public static int FormatCheckSum(int data)
        {
            // Set the polynome to divide by.
            int polynome = Convert.ToInt32("10100110111", 2);

            // Raise the data to the power of 10 (calculations are done binary!).
            int result = data << 10;

            // Shift the divider to occcupy 15 bits.
            int divider = polynome << 4;

            // Set a mask to check the first bit set in the data.
            int mask = 1 << 14;

            while (result > polynome)
            {
                if ((mask & result) != 0)
                {
                    result ^= divider;
                }

                mask >>= 1;
                divider >>= 1;
            }

            return result + (data << 10);
        }

        /// <summary>
        /// Calculate the BCH checksum for the given version data.
        /// </summary>
        /// <param name="data">The data to calculate the checksum over.</param>
        /// <returns>The calculated checksum.</returns>
        public static int VersionCheckSum(int data)
        {
            // Set the polynome to divide by.
            int polynome = Convert.ToInt32("1111100100101", 2);

            // Raise the data to the power of 12 (calculations are done binary!).
            int result = data << 12;

            // Shift the divider to occcupy 18 bits.
            int divider = polynome << 5;

            // Set a mask to check the first bit set in the data.
            int mask = 1 << 17;

            while (result > polynome)
            {
                if ((mask & result) != 0)
                {
                    result ^= divider;
                }

                mask >>= 1;
                divider >>= 1;
            }

            return result + (data << 12);
        }
    }
}