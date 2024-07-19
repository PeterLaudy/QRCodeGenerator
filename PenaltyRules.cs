// <copyright file="PenaltyRules.cs" project="QRCodeGenerator" author="Peter Laudy">
// Copyright © 2024 - Peter Laudy All rights reserved.
// </copyright>

namespace QRCodeGenerator
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// The first rule says that each sequence of 5 or more consecutive
    /// dark/light modules in a row or column gets a penalty of the length of
    /// the sequence minus 2.
    /// </remarks>
    internal static class PenaltyScoreRule1
    {
        public static int CalculatePenalty(BitArray bmp)
        {
            Log.LogMessage();

            var horPenalty = 0;

            for (int y = 0; y < bmp.Size; y++)
            {
                for (int x = 0; x < bmp.Size - 4; )
                {
                    var mod = bmp[x, y];
                    var i = 1;
                    while ((x + i < bmp.Size) && (mod == bmp[x + i, y]))
                    {
                        i++;
                    }

                    if (i >= 5)
                    {
                        horPenalty += i - 2;
                    }

                    x += i;
                }
            }

            var verPenalty = 0;

            for (int x = 0; x < bmp.Size; x++)
            {
                for (int y = 0; y < bmp.Size - 4; )
                {
                    var mod = bmp[x, y];
                    var i = 1;
                    while ((y + i < bmp.Size) && (mod == bmp[x, y + i]))
                    {
                        i++;
                    }

                    if (i >= 5)
                    {
                        verPenalty += i - 2; 
                    }

                    y += i;
                }
            }

            Log.LogInfo($"Rule1 horizontal penalty score is {horPenalty}");
            Log.LogInfo($"Rule1 vertical penalty score is {verPenalty}");
            Log.LogInfo($"Rule1 total penalty score is {horPenalty + verPenalty}");
            return horPenalty + verPenalty;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// The second rule says that each rectangular region of dark/light modules
    /// of size m×n gets a penalty of 3×(m - 1)×(n - 1).
    /// Just add a penalty of 3 for each 2×2 square of dark/light modules,
    /// including overlapping ones.
    /// </remarks>
    internal static class PenaltyScoreRule2
    {
        public static int CalculatePenalty(BitArray bmp)
        {
            Log.LogMessage();

            var penalty = 0;

            for (int x = 0; x < bmp.Size - 1; x++)
            {
                for (int y = 0; y < bmp.Size - 1; y++)
                {
                    var mod = bmp[x, y];
                    if ((mod == bmp[x + 1, y]) && (mod == bmp[x, y + 1]) && (mod == bmp[x + 1, y + 1]))
                    {
                        penalty++;
                    }
                }
            }

            Log.LogInfo($"Rule2 penalty score is {penalty}");
            return penalty * 3;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// The third rules says that each sequence of
    /// dark-light-dark-dark-dark-light-dark-light-light-light-light modules
    /// (⬛⬜⬛⬛⬛⬜⬛⬜⬜⬜⬜) or its reverse
    /// (⬜⬜⬜⬜⬛⬜⬛⬛⬛⬜⬛), found on any row or column, adds a
    /// penalty of 40. These are part of the large markers in the corners.
    /// </remarks>
    internal static class PenaltyScoreRule3
    {
        public static int CalculatePenalty(BitArray bmp)
        {
            Log.LogMessage();

            var horPenalty = 0;
            for (int y = 0; y < bmp.Size; y++)
            {
                for (int x = 0; x < bmp.Size - 10; x++)
                {
                    if (bmp[x, y] && !bmp[x + 1, y] && bmp[x + 2, y] &&
                        bmp[x + 3, y] && bmp[x + 4, y] && !bmp[x + 5, y] &&
                        bmp[x + 6, y] && !bmp[x + 7, y] && !bmp[x + 8, y] &&
                        !bmp[x + 9, y] && !bmp[x + 10, y])
                    {
                        horPenalty++;
                        x += 10;
                    }
                    if (!bmp[x, y] && !bmp[x + 1, y] && !bmp[x + 2, y] &&
                        !bmp[x + 3, y] && bmp[x + 4, y] && !bmp[x + 5, y] &&
                        bmp[x + 6, y] && bmp[x + 7, y] && bmp[x + 8, y] &&
                        !bmp[x + 9, y] && bmp[x + 10, y])
                    {
                        horPenalty++;
                        x += 10;
                    }
                }
            }

            var verPenalty = 0;
            for (int x = 0; x < bmp.Size; x++)
            {
                for (int y = 0; y < bmp.Size - 10; y++)
                {
                    if (bmp[x, y] && !bmp[x, y + 1] && bmp[x, y + 2] &&
                        bmp[x, y + 3] && bmp[x, y + 4] && !bmp[x, y + 5] &&
                        bmp[x, y + 6] && !bmp[x, y + 7] && !bmp[x, y + 8] &&
                        !bmp[x, y + 9] && !bmp[x, y + 10])
                    {
                        verPenalty++;
                        y += 10;
                    }
                    if (!bmp[x, y] && !bmp[x, y + 1] && !bmp[x, y + 2] &&
                        !bmp[x, y + 3] && bmp[x, y + 4] && !bmp[x, y + 5] &&
                        bmp[x, y + 6] && bmp[x, y + 7] && bmp[x, y + 8] &&
                        !bmp[x, y + 9] && bmp[x, y + 10])
                    {
                        verPenalty++;
                        y += 10;
                    }
                }
            }

            horPenalty *= 40;
            verPenalty *= 40;

            Log.LogInfo($"Rule3 horizontal penalty score is {horPenalty}");
            Log.LogInfo($"Rule3 vertical penalty score is {verPenalty}");
            Log.LogInfo($"Rule3 total penalty score is {horPenalty + verPenalty}");
            return horPenalty + verPenalty;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Compute the percentage of dark modules.
    /// If the percentage is greater than 50, round down to the nearest
    /// multiple of 5; otherwise, round it up.
    /// Subtract 50 and double the absolute value of the difference: that's
    /// our penalty for rule 4.
    /// </remarks>
    internal static class PenaltyScoreRule4
    {
        public static int CalculatePenalty(BitArray bmp)
        {
            var blackPenalty = 0;
            for (int x = 0; x < bmp.Size; x++)
            {
                for (int y = 0; y < bmp.Size; y++)
                {
                    if (bmp[x, y])
                    {
                        blackPenalty++;
                    }
                }
            }

            var penalty = (int) (Math.Floor(Math.Abs(((100 * blackPenalty) / (5.0 * bmp.Size * bmp.Size)) - 10.0)) * 5);
            Log.LogInfo($"Rule4 penalty score is {penalty} ({blackPenalty} of {bmp.Size * bmp.Size})");
            return penalty;
        }
    }
}