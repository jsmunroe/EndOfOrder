namespace TileBuilder
{
    public struct UnitSize
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="a_width">Unit width.</param>
        /// <param name="a_height">Unit height.</param>
        public UnitSize(int a_width, int a_height)
        {
            Width = a_width;
            Height = a_height;
        }

        /// <summary>
        /// Unit width.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Unit height.
        /// </summary>
        public int Height { get; set; }
    }
}