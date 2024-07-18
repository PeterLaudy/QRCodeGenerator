// <copyright file="ErrorCorrectionLevel.cs" project="QRCodeGenerator" author="Peter Laudy">
// Copyright © 2024 - Peter Laudy All rights reserved.
// </copyright>

namespace QRCodeGenerator
{
    /// <summary>
    /// Enum which defines the level of error correction used in the QRCode.
    /// </summary>
    public enum ErrorCorrectionLevel
    {
        /// <summary>
        /// Low level error correction.
        /// </summary>
        LevelL,

        /// <summary>
        /// Medium level error correction.
        /// </summary>
        LevelM,

        /// <summary>
        /// Medium to high level error correction.
        /// </summary>
        LevelQ,

        /// <summary>
        /// High level error correction.
        /// </summary>
        LevelH
    }
}