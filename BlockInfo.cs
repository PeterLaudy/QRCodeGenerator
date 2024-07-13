// <copyright file="BlockInfo.cs" project="QRCodeGenerator" author="Peter Laudy">
// Copyright © 2024 - Peter Laudy All rights reserved.
// </copyright>
using System;
using System.Collections.Generic;

namespace QRCodeGenerator
{
    /// <summary>
    /// Class which tells us how the data is divided into blocks.
    /// </summary>
    public class BlockInfo
    {
        #region Nested classes

        /// <summary>
        /// Structure to store the size of a data block.
        /// </summary>
        public class BlockSize
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="BlockSize"/> class.
            /// </summary>
            /// <param name="dw">The number of data code words in the block.</param>
            /// <param name="ecw">The number of error correction code words in the block.</param>
            public BlockSize(int dw, int ecw)
            {
                DataWords = dw;
                ErrorCorrectionWords = ecw;
            }

            /// <summary>
            /// Gets the number of data code words in the block.
            /// </summary>
            public int DataWords { get; private set; }

            /// <summary>
            /// Gets the number of error correction code words in the block.
            /// </summary>
            public int ErrorCorrectionWords { get; private set; }
        }
        
        #endregion Nested classes

        #region Static code

        /// <summary>
        /// The block info for error correction level L.
        /// </summary>
        private static List<List<BlockSize>> tableL;

        /// <summary>
        /// The block info for error correction level M.
        /// </summary>
        private static List<List<BlockSize>> tableM;

        /// <summary>
        /// The block info for error correction level Q.
        /// </summary>
        private static List<List<BlockSize>> tableQ;

        /// <summary>
        /// The block info for error correction level H.
        /// </summary>
        private static List<List<BlockSize>> tableH;

        /// <summary>
        /// Initializes static members of the <see cref="BlockInfo"/> class.
        /// </summary>
        static BlockInfo()
        {
            BlockInfo.tableL = new List<List<BlockSize>>();
            BlockInfo.tableM = new List<List<BlockSize>>();
            BlockInfo.tableQ = new List<List<BlockSize>>();
            BlockInfo.tableH = new List<List<BlockSize>>();

            // Version 0 does not exist.
            BlockInfo.tableL.Add(new List<BlockSize>());
            BlockInfo.tableM.Add(new List<BlockSize>());
            BlockInfo.tableQ.Add(new List<BlockSize>());
            BlockInfo.tableH.Add(new List<BlockSize>());

            // Version 1
            BlockInfo.AddBlockAndList(BlockInfo.tableL, 19, 7);
            BlockInfo.AddBlockAndList(BlockInfo.tableM, 16, 10);
            BlockInfo.AddBlockAndList(BlockInfo.tableQ, 13, 13);
            BlockInfo.AddBlockAndList(BlockInfo.tableH, 9, 17);

            // Version 2
            BlockInfo.AddBlockAndList(BlockInfo.tableL, 34, 10);
            BlockInfo.AddBlockAndList(BlockInfo.tableM, 28, 16);
            BlockInfo.AddBlockAndList(BlockInfo.tableQ, 22, 22);
            BlockInfo.AddBlockAndList(BlockInfo.tableH, 16, 28);

            // Version 3
            BlockInfo.AddBlockAndList(BlockInfo.tableL, 55, 15);
            BlockInfo.AddBlockAndList(BlockInfo.tableM, 44, 26);
            BlockInfo.AddBlocksAndList(BlockInfo.tableQ, 17, 18, 2);
            BlockInfo.AddBlocksAndList(BlockInfo.tableH, 13, 22, 2);

            // Version 4
            BlockInfo.AddBlockAndList(BlockInfo.tableL, 80, 20);
            BlockInfo.AddBlocksAndList(BlockInfo.tableM, 32, 18, 2);
            BlockInfo.AddBlocksAndList(BlockInfo.tableQ, 24, 26, 2);
            BlockInfo.AddBlocksAndList(BlockInfo.tableH, 9, 16, 4);

            // Version 5
            BlockInfo.AddBlockAndList(BlockInfo.tableL, 108, 26);
            BlockInfo.AddBlocksAndList(BlockInfo.tableM, 43, 24, 2);
            BlockInfo.AddBlocksAndList(BlockInfo.tableQ, 15, 18, 2);
            BlockInfo.AddBlocks(BlockInfo.tableQ, 16, 18, 2);
            BlockInfo.AddBlocksAndList(BlockInfo.tableH, 11, 22, 2);
            BlockInfo.AddBlocks(BlockInfo.tableH, 12, 22, 2);

            // Version 6
            BlockInfo.AddBlocksAndList(BlockInfo.tableL, 68, 20, 2);
            BlockInfo.AddBlocksAndList(BlockInfo.tableM, 27, 16, 2);
            BlockInfo.AddBlocksAndList(BlockInfo.tableQ, 19, 24, 2);
            BlockInfo.AddBlocksAndList(BlockInfo.tableH, 15, 28, 2);

            // Version 7
            BlockInfo.AddBlocksAndList(BlockInfo.tableL, 78, 20, 2);
            BlockInfo.AddBlocksAndList(BlockInfo.tableM, 31, 16, 4);
            BlockInfo.AddBlocksAndList(BlockInfo.tableQ, 14, 18, 2);
            BlockInfo.AddBlocks(BlockInfo.tableQ, 15, 18, 4);
            BlockInfo.AddBlocksAndList(BlockInfo.tableH, 13, 26, 4);
            BlockInfo.AddBlock(BlockInfo.tableH, 14, 26);

            // Version 8
            BlockInfo.AddBlocksAndList(BlockInfo.tableL, 97, 24, 2);
            BlockInfo.AddBlocksAndList(BlockInfo.tableM, 38, 22, 2);
            BlockInfo.AddBlocks(BlockInfo.tableM, 39, 22, 2);
            BlockInfo.AddBlocksAndList(BlockInfo.tableQ, 18, 22, 4);
            BlockInfo.AddBlocks(BlockInfo.tableQ, 19, 22, 2);
            BlockInfo.AddBlocksAndList(BlockInfo.tableH, 14, 26, 4);
            BlockInfo.AddBlocks(BlockInfo.tableH, 15, 26, 2);

            // Version 9
            BlockInfo.AddBlocksAndList(BlockInfo.tableL, 116, 30, 2);
            BlockInfo.AddBlocksAndList(BlockInfo.tableM, 36, 22, 3);
            BlockInfo.AddBlocks(BlockInfo.tableM, 37, 22, 2);
            BlockInfo.AddBlocksAndList(BlockInfo.tableQ, 16, 20, 4);
            BlockInfo.AddBlocks(BlockInfo.tableQ, 17, 20, 4);
            BlockInfo.AddBlocksAndList(BlockInfo.tableH, 12, 24, 4);
            BlockInfo.AddBlocks(BlockInfo.tableH, 12, 24, 4);

            // Version 10
            BlockInfo.AddBlocksAndList(BlockInfo.tableL, 86, 18, 2);
            BlockInfo.AddBlocks(BlockInfo.tableL, 96, 18, 2);
            BlockInfo.AddBlocksAndList(BlockInfo.tableM, 43, 26, 4);
            BlockInfo.AddBlocks(BlockInfo.tableM, 44, 26, 1);
            BlockInfo.AddBlocksAndList(BlockInfo.tableQ, 19, 24, 6);
            BlockInfo.AddBlocks(BlockInfo.tableQ, 20, 24, 2);
            BlockInfo.AddBlocksAndList(BlockInfo.tableH, 15, 28, 6);
            BlockInfo.AddBlocks(BlockInfo.tableH, 16, 28, 2);

            // Version 11
            BlockInfo.AddBlocksAndList(BlockInfo.tableL, 81, 20, 4);
            BlockInfo.AddBlockAndList(BlockInfo.tableM, 50, 30);
            BlockInfo.AddBlocks(BlockInfo.tableM, 51, 30, 4);
            BlockInfo.AddBlocksAndList(BlockInfo.tableQ, 22, 28, 4);
            BlockInfo.AddBlocks(BlockInfo.tableQ, 23, 28, 4);
            BlockInfo.AddBlocksAndList(BlockInfo.tableH, 12, 24, 3);
            BlockInfo.AddBlocks(BlockInfo.tableH, 13, 24, 8);

            // Version 12
            BlockInfo.AddBlocksAndList(BlockInfo.tableL, 92, 24, 2);
            BlockInfo.AddBlocks(BlockInfo.tableL, 93, 24, 2);
            BlockInfo.AddBlocksAndList(BlockInfo.tableM, 36, 22, 6);
            BlockInfo.AddBlocks(BlockInfo.tableM, 37, 22, 2);
            BlockInfo.AddBlocksAndList(BlockInfo.tableQ, 20, 26, 4);
            BlockInfo.AddBlocks(BlockInfo.tableQ, 21, 26, 6);
            BlockInfo.AddBlocksAndList(BlockInfo.tableH, 14, 28, 7);
            BlockInfo.AddBlocks(BlockInfo.tableH, 15, 28, 4);

            // Version 13
            BlockInfo.AddBlocksAndList(BlockInfo.tableL, 107, 26, 4);
            BlockInfo.AddBlocksAndList(BlockInfo.tableM, 37, 22, 8);
            BlockInfo.AddBlock(BlockInfo.tableM, 38, 22);
            BlockInfo.AddBlocksAndList(BlockInfo.tableQ, 20, 24, 8);
            BlockInfo.AddBlocks(BlockInfo.tableQ, 21, 24, 4);
            BlockInfo.AddBlocksAndList(BlockInfo.tableH, 11, 22, 12);
            BlockInfo.AddBlocks(BlockInfo.tableH, 12, 22, 4);

            // Version 14
            BlockInfo.AddBlocksAndList(BlockInfo.tableL, 115, 30, 3);
            BlockInfo.AddBlock(BlockInfo.tableL, 116, 30);
            BlockInfo.AddBlocksAndList(BlockInfo.tableM, 40, 24, 4);
            BlockInfo.AddBlocks(BlockInfo.tableM, 41, 24, 5);
            BlockInfo.AddBlocksAndList(BlockInfo.tableQ, 16, 20, 11);
            BlockInfo.AddBlocks(BlockInfo.tableQ, 17, 20, 5);
            BlockInfo.AddBlocksAndList(BlockInfo.tableH, 12, 24, 11);
            BlockInfo.AddBlocks(BlockInfo.tableH, 13, 24, 5);

            // Version 15
            BlockInfo.AddBlocksAndList(BlockInfo.tableL, 87, 22, 5);
            BlockInfo.AddBlock(BlockInfo.tableL, 88, 22);
            BlockInfo.AddBlocksAndList(BlockInfo.tableM, 41, 24, 5);
            BlockInfo.AddBlocks(BlockInfo.tableM, 42, 24, 5);
            BlockInfo.AddBlocksAndList(BlockInfo.tableQ, 24, 30, 5);
            BlockInfo.AddBlocks(BlockInfo.tableQ, 25, 20, 7);
            BlockInfo.AddBlocksAndList(BlockInfo.tableH, 12, 24, 11);
            BlockInfo.AddBlocks(BlockInfo.tableH, 13, 24, 7);

            // Version 16
            BlockInfo.AddBlocksAndList(BlockInfo.tableL, 98, 22, 5);
            BlockInfo.AddBlock(BlockInfo.tableL, 99, 22);
            BlockInfo.AddBlocksAndList(BlockInfo.tableM, 45, 28, 7);
            BlockInfo.AddBlocks(BlockInfo.tableM, 46, 28, 3);
            BlockInfo.AddBlocksAndList(BlockInfo.tableQ, 19, 24, 15);
            BlockInfo.AddBlocks(BlockInfo.tableQ, 20, 24, 2);
            BlockInfo.AddBlocksAndList(BlockInfo.tableH, 15, 30, 3);
            BlockInfo.AddBlocks(BlockInfo.tableH, 16, 30, 13);
        }

        /// <summary>
        /// Add a BlockSize to the last list in the given list.
        /// </summary>
        /// <param name="list">The list to add the BlockSize to the last list.</param>
        /// <param name="dw">The number of data words for this BlockSize.</param>
        /// <param name="ecw">The number of error correction words for this BlockSize.</param>
        private static void AddBlock(List<List<BlockSize>> list, int dw, int ecw)
        {
            list[list.Count - 1].Add(new BlockSize(dw, ecw));
        }

        /// <summary>
        /// Add a BlockSize multiple times to the last list in the given list.
        /// </summary>
        /// <param name="list">The list to add the BlockSize multiple times to the last list.</param>
        /// <param name="dw">The number of data words for this BlockSize.</param>
        /// <param name="ecw">The number of error correction words for this BlockSize.</param>
        /// <param name="count">The number of block with these sizes.</param>
        private static void AddBlocks(List<List<BlockSize>> list, int dw, int ecw, int count)
        {
            for (int i = 0; i < count; i++)
            {
                list[list.Count - 1].Add(new BlockSize(dw, ecw));
            }
        }

        /// <summary>
        /// Add a BlockSize to the last list in the given list.
        /// </summary>
        /// <param name="list">The list to add the BlockSize to the last list.</param>
        /// <param name="dw">The number of data words for this BlockSize.</param>
        /// <param name="ecw">The number of error correction words for this BlockSize.</param>
        private static void AddBlockAndList(List<List<BlockSize>> list, int dw, int ecw)
        {
            list.Add(new List<BlockSize>());
            list[list.Count - 1].Add(new BlockSize(dw, ecw));
        }

        /// <summary>
        /// Add a BlockSize multiple times to the last list in the given list.
        /// </summary>
        /// <param name="list">The list to add the BlockSize multiple times to the last list.</param>
        /// <param name="dw">The number of data words for this BlockSize.</param>
        /// <param name="ecw">The number of error correction words for this BlockSize.</param>
        /// <param name="count">The number of block with these sizes.</param>
        private static void AddBlocksAndList(List<List<BlockSize>> list, int dw, int ecw, int count)
        {
            list.Add(new List<BlockSize>());
            for (int i = 0; i < count; i++)
            {
                list[list.Count - 1].Add(new BlockSize(dw, ecw));
            }
        }

        /// <summary>
        /// Returns the minimal version of the QRCode, given the data size and error correction level to store in it.
        /// </summary>
        /// <param name="level">The error correction level to use.</param>
        /// <param name="dataWords">The number of data words to store in the QRCode.</param>
        /// <returns>The minimal version to use.</returns>
        public static int GetMinimalVersion(ErrorCorrectionLevel level, int dataWords)
        {
            for (int i = 1; i < 40; i++)
            {
                BlockInfo bi = new BlockInfo(i, level);
                if (bi.DataWords > dataWords)
                {
                    return i;
                }
            }

            return 0;
        }

        #endregion Static code

        #region Constructors & destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockInfo"/> class.
        /// </summary>
        /// <param name="version">The version for which we want to find the data block information.</param>
        /// <param name="level">The level of error correction for which we want to find the data block information.</param>
        public BlockInfo(int version, ErrorCorrectionLevel level)
        {
            switch (level)
            {
                case ErrorCorrectionLevel.LevelL:
                    BlockSizes = BlockInfo.tableL[version];
                    break;
                case ErrorCorrectionLevel.LevelM:
                    BlockSizes = BlockInfo.tableM[version];
                    break;
                case ErrorCorrectionLevel.LevelQ:
                    BlockSizes = BlockInfo.tableQ[version];
                    break;
                case ErrorCorrectionLevel.LevelH:
                    BlockSizes = BlockInfo.tableH[version];
                    break;
                default: throw new ArgumentOutOfRangeException("level");
            }
        }

        #endregion Constructors & destructors

        #region Properties

        /// <summary>
        /// Gets the block size for the QRCode.
        /// </summary>
        public List<BlockSize>? BlockSizes { get; private set; }

        /// <summary>
        /// Gets the number of data words for the QRCode.
        /// </summary>
        public int DataWords
        {
            get
            {
                int result = 0;
                foreach (BlockSize size in BlockSizes!)
                {
                    result += size.DataWords;
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the number of error correction words for the QRCode.
        /// </summary>
        public int ErrorCorrectionWords
        {
            get
            {
                int result = 0;
                foreach (BlockSize size in BlockSizes!)
                {
                    result += size.ErrorCorrectionWords;
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the total number of code words for the QRCode.
        /// </summary>
        public int TotalCodeWords
        {
            get
            {
                int result = 0;
                foreach (BlockSize size in BlockSizes!)
                {
                    result += size.DataWords + size.ErrorCorrectionWords;
                }

                return result;
            }
        }

        #endregion Properties
    }
}