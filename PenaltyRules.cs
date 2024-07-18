// <copyright file="PenaltyRules.cs" project="QRCodeGenerator" author="Peter Laudy">
// Copyright © 2024 - Peter Laudy All rights reserved.
// </copyright>

namespace QRCodeGenerator
{
    internal abstract class PenaltyScore
    {
        public abstract int CalculatePenalty(BitArray bmp);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// The first rule says that each sequence of 5 or more consecutive
    /// dark/light modules in a row or column gets a penalty of the length of
    /// the sequence minus 2.
    /// </remarks>
    internal class PenaltyScoreRule1 : PenaltyScore
    {
        public override int CalculatePenalty(BitArray bmp)
        {
            return 0;
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
    internal class PenaltyScoreRule2 : PenaltyScore
    {
        public override int CalculatePenalty(BitArray bmp)
        {
            return 0;
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
    internal class PenaltyScoreRule3 : PenaltyScore
    {
        public override int CalculatePenalty(BitArray bmp)
        {
            return 0;
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
    internal class PenaltyScoreRule4 : PenaltyScore
    {
        public override int CalculatePenalty(BitArray bmp)
        {
            return 0;
        }
    }
}