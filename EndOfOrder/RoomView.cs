using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using EndOfOrder.Story;
using TileBuilder;
using TileBuilder.Contracts;
using TileBuilder.Contracts.Ui;
using TileBuilder.Files;

namespace EndOfOrder
{
    public class RoomView : Grid, IRoomView
    {
        private UnitCoord _roomLocation = new UnitCoord();

        /// <summary>
        /// Constructor.
        /// </summary>
        public RoomView()
        {
            Loaded += OnLoaded;
        }

        /// <summary>
        /// When this view is loaded into the visual tree.
        /// </summary>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var finder = new ResourceFinder();

            Game.World.RegisterRoomView(this);
            Game.Load(finder);
        }

        /// <summary>
        /// Show the given room (<paramref name="a_room"/>) in this view.
        /// </summary>
        /// <param name="a_room">Room to show.</param>
        public void ShowRoom(IRoom a_room)
        {
            _roomLocation = a_room.Location;

            if (a_room.Size.Height != RowDefinitions.Count)
            {
                RowDefinitions.Clear();
                for (var y = 0; y < a_room.Size.Height; y++)
                    RowDefinitions.Add(new RowDefinition {Height = new GridLength(64)});
            }

            if (a_room.Size.Width != RowDefinitions.Count)
            {
                ColumnDefinitions.Clear();
                for (var x = 0; x < a_room.Size.Width; x++)
                    ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(64)});
            }

            Children.Clear();

            for (var y = 0; y < a_room.Size.Height; y++)
            {
                for (var x = 0; x < a_room.Size.Width; x++)
                {

                    var tile = a_room.GetTile(x, y);

                    var border = new Border
                    {
                        Visibility = Visibility.Visible,
                        Background = ResourceProvider.Current.GetBrush(tile.Background),
                    };

                    Grid.SetColumn(border, x);
                    Grid.SetRow(border, y);

                    Children.Add(border);
                }
            }
        }

        /// <summary>
        /// Show the given character in room (<paramref name="a_character"/>) in this view.
        /// </summary>
        /// <param name="a_character">Character.</param>
        public void ShowCharacter(ICharacter a_character)
        {
            Children.Add(new CharacterControl(a_character));
        }

        /// <summary>
        /// Update the position of the given character in the room (<paramref name="a_character"/>) in this view.
        /// </summary>
        /// <param name="a_character">Character.</param>
        public void UpdateCharacter(ICharacter a_character)
        {
            var control = Children.OfType<CharacterControl>().FirstOrDefault(i => ReferenceEquals(i.Character, a_character));

            control?.Update();
        }

    }
}