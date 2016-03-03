namespace TileBuilder.Contracts.Ui
{
    public interface IRoomView
    {
        /// <summary>
        /// Show the given room (<paramref name="a_room"/>) in this view.
        /// </summary>
        /// <param name="a_room">Room to show.</param>
        void ShowRoom(IRoom a_room);

        /// <summary>
        /// Show the given character in room (<paramref name="a_character"/>) in this view.
        /// </summary>
        /// <param name="a_character">Character.</param>
        void ShowCharacter(ICharacter a_character);

        /// <summary>
        /// Update the position of the given character in the room (<paramref name="a_character"/>) in this view.
        /// </summary>
        /// <param name="a_character">Character.</param>
        void UpdateCharacter(ICharacter a_character);
    }
}