namespace TileBuilder.Contracts.Ui
{
    public interface ICharacter
    {
        /// <summary>
        /// Character sprite image.
        /// </summary>
        string Sprite { get; }

        /// <summary>
        /// Character sprite frame.
        /// </summary>
        int SpriteFrame { get; set; }

        /// <summary>
        /// Position of this character in the room.
        /// </summary>
        UnitCoord Position { get; set; }
    }

}