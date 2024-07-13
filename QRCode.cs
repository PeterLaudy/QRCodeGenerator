// <copyright file="QRCode.cs" project="QRCodeGenerator" author="Peter Laudy">
// Copyright © 2024 - Peter Laudy All rights reserved.
// </copyright>
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace QRCodeGenerator
{
    /// <summary>
    /// Class to create or read a QRCode.
    /// </summary>
    public class QRCode
    {
        #region Constant definitions

        /// <summary>
        /// The 15 Format Information bits need to be XOR-e before putting them into the matrix.
        /// </summary>
        private const int FORMAT_INFO_XOR_MASK = 0x5412;
        
        #endregion Constant definitions

        #region Static code

        /// <summary>
        /// Initializes static members of the <see cref="QRCode"/> class.
        /// </summary>
        static QRCode()
        {
            List<char> alphaNumericCharacters = new List<char>();
            for (char c = '0'; c <= '9'; c++)
            {
                alphaNumericCharacters.Add(c);
            }

            for (char c = 'A'; c <= 'Z'; c++)
            {
                alphaNumericCharacters.Add(c);
            }

            alphaNumericCharacters.Add(' ');
            alphaNumericCharacters.Add('$');
            alphaNumericCharacters.Add('%');
            alphaNumericCharacters.Add('*');
            alphaNumericCharacters.Add('+');
            alphaNumericCharacters.Add('-');
            alphaNumericCharacters.Add('.');
            alphaNumericCharacters.Add('/');
            alphaNumericCharacters.Add(':');

            QRCode.AlphaNumericCharacterSet = alphaNumericCharacters.AsReadOnly();
        }

        /// <summary>
        /// Gets or sets a list of the characters allowed in AlphaNumeric mode.
        /// </summary>
        private static ReadOnlyCollection<char> AlphaNumericCharacterSet { get; set; }

        #endregion Static code

        #region Member variables

        /// <summary>
        /// The version for this QRCode, Basically it represents the size.
        /// </summary>
        private int version;

        /// <summary>
        /// The stream containing the encoded data.
        /// </summary>
        private BitStream finalBitStream = new BitStream();

        /// <summary>
        /// The stream containing the encoded data per block.
        /// </summary>
        private List<BitStream> dataBlockBitStreams = new List<BitStream>();

        /// <summary>
        /// The stream containing the error correction data per block.
        /// </summary>
        private List<BitStream> errorBlockBitStreams = new List<BitStream>();

        /// <summary>
        /// Bitmap representation of the QRCode.
        /// </summary>
        private Bitmap? bitmap;

        /// <summary>
        /// The reference of the pattern used to mask the data.
        /// </summary>
        private int maskPatternReference;

        #endregion Member variables

        #region Constructors & destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="QRCode"/> class.
        /// </summary>
        /// <param name="message">The message for which a QRCode must be created.</param>
        public QRCode(string message)
        {
            WriteQRCode(message);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QRCode"/> class.
        /// </summary>
        /// <param name="bmp">The bitmap from which the QRCode must be decoded.</param>
        public QRCode(Bitmap bmp)
        {
            bitmap = bmp;
            ReadQRCode();
        }

        #endregion Constructors & destructors

        #region Properties

        /// <summary>
        /// Gets or sets the error correction level for this QRCode.
        /// </summary>
        public ErrorCorrectionLevel ErrorCorrection { get; protected set; }

        /// <summary>
        /// Gets or sets the version for this QRCode.
        /// </summary>
        /// <remarks>The version  must be in the range of 1-40.</remarks>
        public int Version
        {
            get
            {
                return version;
            }

            protected set
            {
                if ((value >= 1) && (value <= 40))
                {
                    version = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the version for this QRCode.
        /// </summary>
        /// <remarks>The version  must be in the range of 1-40.</remarks>
        public int MaskPatternReference
        {
            get
            {
                return maskPatternReference;
            }

            protected set
            {
                if ((value >= 1) && (value <= 7))
                {
                    maskPatternReference = value;
                }
            }
        }

        /// <summary>
        /// Gets  the number of data (and error correction) blocks in this code.
        /// </summary>
        public BlockInfo? BlockInfo { get; private set; } = null;

        /// <summary>
        /// Gets the format information, which consists of ErrorCorrection and Version.
        /// </summary>
        protected int FormatInformation { get; private set; }

        /// <summary>
        /// Gets the bitmap representing this QRCode.
        /// </summary>
        public Bitmap? Bitmap
        {
            get
            {
                return bitmap;
            }
        }

        /// <summary>
        /// Gets the decoded data from the QRCode.
        /// </summary>
        public string DecodedData { get; private set; } = string.Empty;

        #endregion Properties

        #region Methods

        #region Encoding of the code

        /// <summary>
        /// Call all steps to create a QRCode.
        /// </summary>
        /// <param name="message">The data which the QRCode must contain.</param>
        protected void WriteQRCode(string message)
        {
            ErrorCorrection = ErrorCorrectionLevel.LevelL;
            List<string> messages = AnalyzeData(message);
            EncodeData(messages);
            CodeErrorCorrection();
            StructureFinalMessage();
            PutModulesInMatrix();
            MaskMatrix();
            WriteFormatAndVersion();
        }

        /// <summary>
        /// Analyze the data to check what encoding mode is best.
        /// </summary>
        /// <param name="message">The data which the QRCode must contain.</param>
        /// <returns>A list of strings, in which the first character defines how to encode the string.</returns>
        private List<string> AnalyzeData(string message)
        {
            List<string> subMessages = new List<string>();

            while (!string.IsNullOrEmpty(message))
            {
                string subMessage = string.Empty;
                int mode = 1;
                while (!string.IsNullOrEmpty(message))
                {
                    bool nextSubMessage = false;
                    switch (mode)
                    {
                        case 1:
                            if ((message[0] >= '0') && (message[0] <= '9'))
                            {
                                subMessage += message[0];
                                message = message.Substring(1);
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(subMessage))
                                {
                                    mode <<= 1;
                                }
                                else
                                {
                                    nextSubMessage = true;
                                }
                            }

                            break;
                        case 2:
                            if ((message[0] >= '0') && (message[0] <= '9'))
                            {
                                nextSubMessage = true;
                            }
                            else
                            {
                                if (QRCode.AlphaNumericCharacterSet.Contains(message[0]))
                                {
                                    subMessage += message[0];
                                    message = message.Substring(1);
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(subMessage))
                                    {
                                        mode <<= 1;
                                    }
                                    else
                                    {
                                        nextSubMessage = true;
                                    }
                                }
                            }

                            break;
                        case 4:
                            if (!QRCode.AlphaNumericCharacterSet.Contains(message[0]))
                            {
                                subMessage += message[0];
                                message = message.Substring(1);
                            }
                            else
                            {
                                nextSubMessage = true;
                            }

                            break;
                    }

                    if (nextSubMessage)
                    {
                        break;
                    }
                }

                subMessages.Add(mode.ToString("X1") + subMessage);
            }

            int messageCount = 0;
            while (subMessages.Count != messageCount)
            {
                messageCount = subMessages.Count;
                List<string> tempList = new List<string>();
                string prevSubMessage = subMessages[0];
                for (int i = 1; i < subMessages.Count; i++)
                {
                    string subMessage = subMessages[i];
                    if (prevSubMessage[0] == subMessage[0])
                    {
                        prevSubMessage += subMessage.Substring(1);
                    }
                    else
                    {
                        if ((prevSubMessage[0] > subMessage[0]) && (subMessage.Length < 5))
                        {
                            prevSubMessage += subMessage.Substring(1);
                        }
                        else
                        {
                            if ((prevSubMessage[0] < subMessage[0]) && (prevSubMessage.Length < 6))
                            {
                                prevSubMessage += subMessage.Substring(1);
                                prevSubMessage = subMessage[0] + prevSubMessage.Substring(1);
                            }
                            else
                            {
                                tempList.Add(prevSubMessage);
                                prevSubMessage = subMessage;
                            }
                        }
                    }
                }

                tempList.Add(prevSubMessage);
                subMessages = tempList;
            }

            return subMessages;
        }

        #region EncodeData

        /// <summary>
        /// Encode the data from the input stream and write it to the output stream.
        /// </summary>
        /// <param name="messages">The data which the QRCode must contain.</param>
        private void EncodeData(List<string> messages)
        {
            foreach (string message in messages)
            {
                switch (message[0])
                {
                    case '1':
                        GetNumericDataFromInputStream(message.Substring(1));
                        break;
                    case '2':
                        GetAlphaNumericDataFromInputStream(message.Substring(1));
                        break;
                    case '4':
                        GetBinaryDataFromInputStream(message.Substring(1));
                        break;
                    case '8':
                        throw new NotImplementedException("Kanji encoding is not supported (yet).");
                }
            }

            // Write the terminating mode word.
            finalBitStream.AddBits(0, 4);
            finalBitStream.Flush();
            Version = BlockInfo.GetMinimalVersion(ErrorCorrection, finalBitStream.Length);

            BlockInfo = new BlockInfo(Version, ErrorCorrection);
            while (finalBitStream.Length < BlockInfo.DataWords)
            {
                // Padding words 1110-1100 and 0001-0001
                finalBitStream.WriteByte(0xEC);
                if (finalBitStream.Length < BlockInfo.DataWords)
                {
                    finalBitStream.WriteByte(0x11);
                }
            }

            finalBitStream.ResetStream();

            for (int i = 0; i < BlockInfo!.BlockSizes!.Count; i++)
            {
                dataBlockBitStreams.Add(new BitStream());
                errorBlockBitStreams.Add(new BitStream());
            }

            for (int i = 0; i < BlockInfo!.BlockSizes!.Count; i++)
            {
                while (dataBlockBitStreams[i].Length < BlockInfo!.BlockSizes![i].DataWords)
                {
                    dataBlockBitStreams[i].WriteByte((byte)this.finalBitStream.ReadByte());
                }
            }
        }

        /// <summary>
        /// Get the numeric data from the input stream, convert it and write it to the output stream.
        /// </summary>
        /// <param name="data">The data which the must be encoded in Numeric format.</param>
        private void GetNumericDataFromInputStream(string data)
        {
            // Write Mode indicator for numeric segment.
            finalBitStream.AddBits(1, 4);

            // Write length of the data.
            if (10 > Version)
            {
                finalBitStream.AddBits(data.Length, 10);
            }
            else
            {
                if (27 > Version)
                {
                    finalBitStream.AddBits(data.Length, 12);
                }
                else
                {
                    finalBitStream.AddBits(data.Length, 14);
                }
            }

            int digit;
            int number;
            for (int i = 0; i < data.Length - 2; i += 3)
            {
                number = 0;
                for (int j = 0; j < 3; j++)
                {
                    digit = (int)data[i + j] - (int)'0';
                    number = number * 10 + digit;
                }

                finalBitStream.AddBits(number, 10);
            }

            switch (data.Length % 3)
            {
                case 1:
                    digit = (int)data[data.Length - 1] - (int)'0';
                    finalBitStream.AddBits(digit, 4);
                    break;
                case 2:
                    digit = (int)data[data.Length - 2] - (int)'0';
                    number = digit * 10;
                    digit = (int)data[data.Length - 1] - (int)'0';
                    number += digit;
                    finalBitStream.AddBits(number, 7);
                    break;
            }
        }

        /// <summary>
        /// Get the alphanumeric data from the input stream, convert it and write it to the output stream.
        /// </summary>
        /// <param name="data">The data which the must be encoded in Alphanumeric format.</param>
        private void GetAlphaNumericDataFromInputStream(string data)
        {
            // Write Mode indicator for numeric segment.
            finalBitStream.AddBits(2, 4);

            // Write length of the data.
            if (10 > Version)
            {
                finalBitStream.AddBits(data.Length, 9);
            }
            else
            {
                if (27 > Version)
                {
                    finalBitStream.AddBits(data.Length, 11);
                }
                else
                {
                    finalBitStream.AddBits(data.Length, 13);
                }
            }

            int number;
            for (int i = 0; i < data.Length - 1; i += 2)
            {
                number = 0;
                number = QRCode.AlphaNumericCharacterSet.IndexOf(data[i]) * 45 + QRCode.AlphaNumericCharacterSet.IndexOf(data[i + 1]);
                finalBitStream.AddBits(number, 11);
            }

            if (0 != (data.Length & 0x01))
            {
                number = QRCode.AlphaNumericCharacterSet.IndexOf(data[data.Length - 1]);
                finalBitStream.AddBits(number, 6);
            }
        }

        /// <summary>
        /// Get the binary data from the input stream, convert it and write it to the output stream.
        /// </summary>
        /// <param name="data">The data which the must be encoded in Binary format.</param>
        private void GetBinaryDataFromInputStream(string data)
        {
            // Write Mode indicator for numeric segment.
            finalBitStream.AddBits(4, 4);

            // Write length of the data.
            if (10 > Version)
            {
                finalBitStream.AddBits(data.Length, 8);
            }
            else
            {
                finalBitStream.AddBits(data.Length, 16);
            }

            for (int i = 0; i < data.Length; i++)
            {
                int number = Encoding.UTF8.GetBytes(new char[] { data[i] })[0];
                finalBitStream.AddBits(number, 8);
            }
        }

        #endregion EncodeData

        /// <summary>
        /// Calculate the error correction code words.
        /// </summary>
        private void CodeErrorCorrection()
        {
            for (int i = 0; i < dataBlockBitStreams.Count; i++)
            {
                dataBlockBitStreams[i].ResetStream();
                int[] data = new int[this.dataBlockBitStreams[i].Length];
                for (int j = 0; j < dataBlockBitStreams[i].Length; j++)
                {
                    data[j] = dataBlockBitStreams[i].ReadByte();
                }

                int[] rs = ReedSolomon.CheckSum(BlockInfo!.BlockSizes![i].ErrorCorrectionWords, data);
                for (int j = 0; j < rs.Length; j++)
                {
                    errorBlockBitStreams[i].WriteByte((byte)rs[j]);
                }
            }
        }

        /// <summary>
        /// Put the data and error correction words in the correct order.
        /// </summary>
        private void StructureFinalMessage()
        {
            finalBitStream.Clear();
            for (int i = 0; i < BlockInfo!.BlockSizes!.Count; i++)
            {
                dataBlockBitStreams[i].ResetStream();
                errorBlockBitStreams[i].ResetStream();
            }

            while (finalBitStream.Length < BlockInfo!.DataWords)
            {
                for (int i = 0; i < BlockInfo!.BlockSizes!.Count; i++)
                {
                    int data = dataBlockBitStreams[i].ReadByte();
                    if (-1 != data)
                    {
                        finalBitStream.WriteByte((byte)data);
                    }
                }
            }

            while (finalBitStream.Length < BlockInfo!.TotalCodeWords)
            {
                for (int i = 0; i < BlockInfo!.BlockSizes!.Count; i++)
                {
                    int data = errorBlockBitStreams[i].ReadByte();
                    if (-1 != data)
                    {
                        finalBitStream.WriteByte((byte)data);
                    }
                }
            }
        }

        /// <summary>
        /// Put the bits in the bitmap.
        /// </summary>
        private void PutModulesInMatrix()
        {
            finalBitStream.ResetStream();
            DataRegion dataRegion = new DataRegion(Version);

            bitmap = new Bitmap(Version * 4 + 17, Version * 4 + 17, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.White);
            }

            for (int x = bitmap.Width - 1; x > 0; x -= 2)
            {
                if (6 == x)
                {
                    x--;
                }

                for (int y = bitmap.Height - 1; y >= 0; y--)
                {
                    if (dataRegion.IsDataAtPos(x, y))
                    {
                        bitmap.SetPixel(x, y, finalBitStream.GetBit() ? Color.Black : Color.White);
                    }

                    if (dataRegion.IsDataAtPos(x - 1, y))
                    {
                        bitmap.SetPixel(x - 1, y, finalBitStream.GetBit() ? Color.Black : Color.White);
                    }
                }

                x -= 2;
                if (6 == x)
                {
                    x--;
                }

                if (x > 0)
                {
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        if (dataRegion.IsDataAtPos(x, y))
                        {
                            bitmap.SetPixel(x, y, finalBitStream.GetBit() ? Color.Black : Color.White);
                        }

                        if (dataRegion.IsDataAtPos(x - 1, y))
                        {
                            bitmap.SetPixel(x - 1, y, finalBitStream.GetBit() ? Color.Black : Color.White);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Apply the mask to the bitmap.
        /// </summary>
        private void MaskMatrix()
        {
            MaskPatternReference = 7;
            MaskPattern mask = new MaskPattern(MaskPatternReference, Version);
            bitmap = mask.MaskBitmap(bitmap!);
        }

        /// <summary>
        /// Write the format and (if applicable) the version information to the bitmap.
        /// </summary>
        private void WriteFormatAndVersion()
        {
            int formatInformation = 0;
            switch (ErrorCorrection)
            {
                case ErrorCorrectionLevel.LevelM:
                    formatInformation = 0;
                    break;
                case ErrorCorrectionLevel.LevelL:
                    formatInformation = 1;
                    break;
                case ErrorCorrectionLevel.LevelH:
                    formatInformation = 2;
                    break;
                case ErrorCorrectionLevel.LevelQ:
                    formatInformation = 3;
                    break;
            }

            formatInformation <<= 3;
            formatInformation |= MaskPatternReference;
            
            FormatInformation = BCH.FormatCheckSum(formatInformation) ^ QRCode.FORMAT_INFO_XOR_MASK;

            int size = bitmap!.Width - 1;
            for (int i = 0; i < 6; i++)
            {
                EncodeFormatAndMask(8, i);
                EncodeFormatAndMask(size - i, 8);
                FormatInformation >>= 1;
            }

            EncodeFormatAndMask(8, 7);
            EncodeFormatAndMask(size - 6, 8);
            FormatInformation >>= 1;

            EncodeFormatAndMask(8, 8);
            EncodeFormatAndMask(size - 7, 8);
            FormatInformation >>= 1;

            EncodeFormatAndMask(7, 8);
            EncodeFormatAndMask(8, size - 6);
            FormatInformation >>= 1;

            for (int i = 5; i >= 0; i--)
            {
                EncodeFormatAndMask(i, 8);
                EncodeFormatAndMask(8, size - i);
                FormatInformation >>= 1;
            }

            if (Version >= 7)
            {
                int versionInfo = BCH.VersionCheckSum(Version);

                for (int i = 0; i < 18; i++)
                {
                    if ((versionInfo & 0x01) != 0)
                    {
                        bitmap.SetPixel(size - 10 + (i % 3), i / 3, Color.Black);
                        bitmap.SetPixel(i / 3, size - 10 + (i % 3), Color.Black);
                    }
                    else
                    {
                        bitmap.SetPixel(size - 10 + (i % 3), i / 3, Color.White);
                        bitmap.SetPixel(i / 3, size - 10 + (i % 3), Color.White);
                    }

                    versionInfo >>= 1;
                }
            }
        }

        /// <summary>
        /// Set the next Format and Mask bit in the bitmap.
        /// </summary>
        /// <param name="x">The x position of the pixel.</param>
        /// <param name="y">The y position of the pixel.</param>
        private void EncodeFormatAndMask(int x, int y)
        {
            if ((FormatInformation & 0x01) != 0)
            {
                bitmap!.SetPixel(x, y, Color.Black);
            }
            else
            {
                bitmap!.SetPixel(x, y, Color.White);
            }
        }

        #endregion Encoding of the code

        #region Decoding of the code

        /// <summary>
        /// Execute all steps needed to decode a QRCode.
        /// </summary>
        protected void ReadQRCode()
        {
            ReadFormatAndVersion();
            UnmaskMatrix();
            GettModulesFromMatrix();
            DestructureFinalMessage();
            DecodeErrorCorrection();
            DecodeData();
        }

        #region Read format and version

        /// <summary>
        /// Read the format and version information from the QRCode.
        /// </summary>
        private void ReadFormatAndVersion()
        {
            Version = (bitmap!.Width - 17) >> 2;

            FormatInformation = 0;
            for (int i = 0; i < 6; i++)
            {
                DecodeFormatAndVersion(i, 8);
            }

            DecodeFormatAndVersion(7, 8);
            DecodeFormatAndVersion(8, 8);
            DecodeFormatAndVersion(8, 7);

            for (int i = 5; i >= 0; i--)
            {
                DecodeFormatAndVersion(8, i);
            }

            FormatInformation ^= QRCode.FORMAT_INFO_XOR_MASK;
            FormatInformation >>= 10;
            MaskPatternReference = FormatInformation & 0x07;
            switch (FormatInformation >> 3)
            {
                case 0:
                    ErrorCorrection = ErrorCorrectionLevel.LevelM;
                    break;
                case 1:
                    ErrorCorrection = ErrorCorrectionLevel.LevelL;
                    break;
                case 2:
                    ErrorCorrection = ErrorCorrectionLevel.LevelH;
                    break;
                case 3:
                    ErrorCorrection = ErrorCorrectionLevel.LevelQ;
                    break;
            }
        }

        /// <summary>
        /// Decode one pixel of the format information from the QRCode.
        /// </summary>
        /// <param name="x">The x coordinate of the pixel to decode.</param>
        /// <param name="y">The y coordinate of the pixel to decode.</param>
        private void DecodeFormatAndVersion(int x, int y)
        {
            FormatInformation <<= 1;
            if (bitmap!.GetPixel(x, y).ToArgb() == Color.Black.ToArgb())
            {
                FormatInformation++;
            }
        }

        #endregion Read format and version

        /// <summary>
        /// Apply the mask to the matrix to retrieve the original data.
        /// </summary>
        private void UnmaskMatrix()
        {
            MaskPattern mask = new MaskPattern(MaskPatternReference, Version);
            bitmap = mask.UnmaskBitmap(bitmap!);
            bitmap!.Save(@"D:\16-3\Artwork\Final\tmp.png");
        }

        /// <summary>
        /// Get all data and error correction bits from the matrix.
        /// </summary>
        private void GettModulesFromMatrix()
        {
            DataRegion dataRegion = new DataRegion(Version);

            for (int x = bitmap!.Width - 1; x > 0; x -= 2)
            {
                if (6 == x)
                {
                    x--;
                }

                for (int y = bitmap!.Height - 1; y >= 0; y--)
                {
                    if (dataRegion.IsDataAtPos(x, y))
                    {
                        finalBitStream.AddBit(bitmap.GetPixel(x, y).ToArgb() == Color.Black.ToArgb());
                    }

                    if (dataRegion.IsDataAtPos(x - 1, y))
                    {
                        finalBitStream.AddBit(bitmap.GetPixel(x - 1, y).ToArgb() == Color.Black.ToArgb());
                    }
                }

                x -= 2;
                if (6 == x)
                {
                    x--;
                }

                if (x > 0)
                {
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        if (dataRegion.IsDataAtPos(x, y))
                        {
                            finalBitStream.AddBit(bitmap.GetPixel(x, y).ToArgb() == Color.Black.ToArgb());
                        }

                        if (dataRegion.IsDataAtPos(x - 1, y))
                        {
                            finalBitStream.AddBit(bitmap.GetPixel(x - 1, y).ToArgb() == Color.Black.ToArgb());
                        }
                    }
                }
            }

            finalBitStream.Flush();
        }

        /// <summary>
        /// Put the data decoded from the QRCode in separate streams.
        /// </summary>
        private void DestructureFinalMessage()
        {
            BlockInfo bi = new BlockInfo(Version, ErrorCorrection);
            for (int i = 0; i < bi.BlockSizes!.Count; i++)
            {
                dataBlockBitStreams.Add(new BitStream());
                errorBlockBitStreams.Add(new BitStream());
            }

            finalBitStream.ResetStream();
            bool done = false;
            while (!done)
            {
                done = true;
                for (int i = 0; i < bi.BlockSizes.Count; i++)
                {
                    if (dataBlockBitStreams[i].Length < bi.BlockSizes[i].DataWords)
                    {
                        done = false;
                        int data = finalBitStream.ReadByte();
                        if (-1 != data)
                        {
                            dataBlockBitStreams[i].WriteByte((byte)data);
                        }
                        else
                        {
                            done = true;
                            break;
                        }
                    }
                }
            }

            done = false;
            while (!done)
            {
                done = true;
                for (int i = 0; i < bi.BlockSizes.Count; i++)
                {
                    if (errorBlockBitStreams[i].Length < bi.BlockSizes[i].ErrorCorrectionWords)
                    {
                        done = false;
                        int data = finalBitStream.ReadByte();
                        if (-1 != data)
                        {
                            errorBlockBitStreams[i].WriteByte((byte)data);
                        }
                        else
                        {
                            done = true;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Check if there are errors and correct them if possible.
        /// </summary>
        private void DecodeErrorCorrection()
        {
            // TODO: Error correction is not our top priority right now.
        }

        /// <summary>
        /// Get the data from the stream.
        /// </summary>
        private void DecodeData()
        {
            // Assemble the blocks back into 1 stream.
            finalBitStream.Clear();
            for (int i = 0; i < dataBlockBitStreams.Count; i++)
            {
                dataBlockBitStreams[i].ResetStream();
                int data = dataBlockBitStreams[i].ReadByte();
                while (-1 != data)
                {
                    finalBitStream.WriteByte((byte)data);
                    data = dataBlockBitStreams[i].ReadByte();
                }
            }

            finalBitStream.ResetStream();

            List<string> messages = new List<string>();

            int mode = finalBitStream.GetBits(4);
            while (0 != mode)
            {
                switch (mode)
                {
                    case 1:
                        messages.Add(DecodeNumericStream());
                        break;
                    case 2:
                        messages.Add(DecodeAlphaNumericStream());
                        break;
                    case 4:
                        messages.Add(DecodeBinaryStream());
                        break;
                    case 8:
                        // TODO: Decode Kanji data
                        messages.Add(DecodeKanjiStream());
                        break;
                }

                mode = finalBitStream.GetBits(4);
            }

            string result = string.Empty;
            foreach (string message in messages)
            {
                result += message;
            }

            DecodedData = result;
        }

        /// <summary>
        /// Gets a Numeric encoded string from the stream.
        /// </summary>
        /// <returns>The string which was encoded in Numeric format.</returns>
        private string DecodeNumericStream()
        {
            string result = string.Empty;
            int length = 0;
            if (10 > Version)
            {
                length = 10;
            }
            else
            {
                if (27 > Version)
                {
                    length = 12;
                }
                else
                {
                    length = 14;
                }
            }

            length = finalBitStream.GetBits(length);

            for (int i = 0; i < length - 2; i += 3)
            {
                result += finalBitStream.GetBits(10).ToString("D3");
            }

            switch (length % 3)
            {
                case 1:
                    result += finalBitStream.GetBits(4).ToString();
                    break;
                case 2:
                    result += finalBitStream.GetBits(7).ToString("D2");
                    break;
            }

            return result;
        }

        /// <summary>
        /// Gets a Alpha Numeric encoded string from the stream.
        /// </summary>
        /// <returns>The string which was encoded in Alpha Numeric format.</returns>
        private string DecodeAlphaNumericStream()
        {
            string result = string.Empty;
            int length = 0;
            if (10 > Version)
            {
                length = 9;
            }
            else
            {
                if (27 > Version)
                {
                    length = 11;
                }
                else
                {
                    length = 13;
                }
            }

            length = finalBitStream.GetBits(length);

            for (int i = 0; i < length - 1; i += 2)
            {
                int data = finalBitStream.GetBits(11);
                result += QRCode.AlphaNumericCharacterSet[data / 45];
                result += QRCode.AlphaNumericCharacterSet[data % 45];
            }

            if (0 != (length & 0x01))
            {
                result += QRCode.AlphaNumericCharacterSet[this.finalBitStream.GetBits(6)];
            }

            return result;
        }

        /// <summary>
        /// Gets a Binary encoded string from the stream.
        /// </summary>
        /// <returns>The string which was encoded in Binary format.</returns>
        private string DecodeBinaryStream()
        {
            string result = string.Empty;
            int length = 0;
            if (10 > Version)
            {
                length = 8;
            }
            else
            {
                length = 16;
            }

            length = finalBitStream.GetBits(length);

            for (int i = 0; i < length; i++)
            {
                result += Encoding.UTF8.GetChars(new byte[] { (byte)this.finalBitStream.GetBits(8) })[0];
            }

            return result;
        }

        /// <summary>
        /// Gets a Kanji encoded string from the stream.
        /// </summary>
        /// <returns>The string which was encoded in Kanji format.</returns>
        private string DecodeKanjiStream()
        {
            string result = string.Empty;

            int length = 0;
            if (10 > Version)
            {
                length = 8;
            }
            else
            {
                if (27 > Version)
                {
                    length = 10;
                }
                else
                {
                    length = 12;
                }
            }

            length = finalBitStream.GetBits(length);

            for (int i = 0; i < length; i++)
            {
                int data = finalBitStream.GetBits(13);
                result += "¥";
            }

            return result;
        }

        #endregion Decoding of the code

        #endregion Methods
    }
}