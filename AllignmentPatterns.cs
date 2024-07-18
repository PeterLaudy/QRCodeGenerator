// <copyright file="AllignmentPatterns.cs" project="QRCodeGenerator" author="Peter Laudy">
// Copyright © 2024 - Peter Laudy All rights reserved.
// </copyright>

namespace QRCodeGenerator
{
    /// <summary>
    /// Class which tells us where the allignment patterns are located for a given version of the QRCode.
    /// </summary>
    internal class AllignmentPatterns
    {
        #region Static code

        /// <summary>
        /// Initializes static members of the <see cref="AllignmentPatterns"/> class.
        /// </summary>
        static AllignmentPatterns()
        {
            Log.LogMessage();

            List<List<int>> list = new List<List<int>>();

            list.Add(new List<int>());

            list.Add(new List<int>());

            list.Add(new List<int>(new int[] { 6, 18 }));
            list.Add(new List<int>(new int[] { 6, 22 }));
            list.Add(new List<int>(new int[] { 6, 26 }));
            list.Add(new List<int>(new int[] { 6, 30 }));
            list.Add(new List<int>(new int[] { 6, 34 }));

            list.Add(new List<int>(new int[] { 6, 22, 38 }));
            list.Add(new List<int>(new int[] { 6, 24, 42 }));
            list.Add(new List<int>(new int[] { 6, 26, 46 }));
            list.Add(new List<int>(new int[] { 6, 28, 50 }));
            list.Add(new List<int>(new int[] { 6, 30, 54 }));
            list.Add(new List<int>(new int[] { 6, 32, 58 }));
            list.Add(new List<int>(new int[] { 6, 34, 62 }));

            list.Add(new List<int>(new int[] { 6, 26, 46, 66 }));
            list.Add(new List<int>(new int[] { 6, 26, 48, 70 }));
            list.Add(new List<int>(new int[] { 6, 26, 50, 74 }));
            list.Add(new List<int>(new int[] { 6, 30, 54, 78 }));
            list.Add(new List<int>(new int[] { 6, 30, 56, 82 }));
            list.Add(new List<int>(new int[] { 6, 30, 58, 86 }));
            list.Add(new List<int>(new int[] { 6, 34, 62, 90 }));

            list.Add(new List<int>(new int[] { 6, 28, 50, 72, 94 }));
            list.Add(new List<int>(new int[] { 6, 26, 50, 74, 98 }));
            list.Add(new List<int>(new int[] { 6, 30, 54, 78, 102 }));
            list.Add(new List<int>(new int[] { 6, 28, 54, 80, 106 }));
            list.Add(new List<int>(new int[] { 6, 32, 58, 84, 110 }));
            list.Add(new List<int>(new int[] { 6, 30, 58, 86, 114 }));
            list.Add(new List<int>(new int[] { 6, 34, 62, 90, 118 }));

            list.Add(new List<int>(new int[] { 6, 26, 50, 74, 98, 122 }));
            list.Add(new List<int>(new int[] { 6, 30, 54, 78, 102, 126 }));
            list.Add(new List<int>(new int[] { 6, 26, 52, 78, 104, 130 }));
            list.Add(new List<int>(new int[] { 6, 30, 56, 82, 108, 134 }));
            list.Add(new List<int>(new int[] { 6, 34, 60, 86, 112, 138 }));
            list.Add(new List<int>(new int[] { 6, 30, 58, 86, 114, 142 }));
            list.Add(new List<int>(new int[] { 6, 34, 62, 90, 118, 146 }));

            list.Add(new List<int>(new int[] { 6, 30, 54, 78, 102, 126, 150 }));
            list.Add(new List<int>(new int[] { 6, 24, 50, 76, 102, 128, 154 }));
            list.Add(new List<int>(new int[] { 6, 28, 54, 80, 106, 132, 158 }));
            list.Add(new List<int>(new int[] { 6, 32, 58, 84, 110, 136, 162 }));
            list.Add(new List<int>(new int[] { 6, 26, 54, 82, 110, 138, 166 }));
            list.Add(new List<int>(new int[] { 6, 30, 58, 86, 114, 142, 170 }));

            AllignmentPatterns.s_list = list;
        }

        /// <summary>
        /// The AP positions for all verions.
        /// </summary>
        private static List<List<int>> s_list;

        #endregion Static code

        #region Member variables

        /// <summary>
        /// Positions of the AP's for the given version.
        /// </summary>
        private List<int> m_positions;

        #endregion Member variables

        #region Constructors & destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AllignmentPatterns"/> class.
        /// </summary>
        /// <param name="version">The version for which we want to find the AP positions.</param>
        public AllignmentPatterns(int version)
        {
            Log.LogMessage();

            m_positions = AllignmentPatterns.s_list[version];
        }

        #endregion Constructors & destructors

        #region Properties

        /// <summary>
        /// Gets a list of the positions of the allignment patterns.
        /// </summary>
        public List<Point> AllAllignmentPatternPositions
        {
            get
            {
                List<Point> result = new List<Point>();
                foreach (int x in m_positions)
                {
                    foreach (int y in m_positions)
                    {
                        if (IsAPAtPos(x, y))
                        {
                            result.Add(new Point(x, y));
                        }
                    }
                }

                return result;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Check if there is an Allignment Pattern at the given position.
        /// </summary>
        /// <param name="x">The x coordinate of the position to check.</param>
        /// <param name="y">The y coordinate of the position to check.</param>
        /// <returns>True if an Allignment Pattern is located at the given position, false if not.</returns>
        public bool IsAPAtPos(int x, int y)
        {
            Log.LogMessage();

            foreach (int xpos in m_positions)
            {
                foreach (int ypos in m_positions)
                {
                    if ((xpos != 6) || (ypos != 6))
                    {
                        if ((xpos != 6) || (m_positions[this.m_positions.Count - 1] != ypos))
                        {
                            if ((m_positions[this.m_positions.Count - 1] != xpos) || (ypos != 6))
                            {
                                if ((Math.Abs(xpos - x) < 3) && (Math.Abs(ypos - y) < 3))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        #endregion Methods
    }
}