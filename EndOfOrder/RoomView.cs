using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using EndOfOrder.Story;
using TileBuilder;
using TileBuilder.Files;

namespace EndOfOrder
{
    public class RoomView : Grid
    {
        private int _positionX = 0;
        private int _positionY = 0;

        private IRoom _room;

        private Brush[] _backgrounds;

        private IResource _tileMap;

        /// <summary>
        /// Constructor.
        /// </summary>
        public RoomView()
        {
            var finder = new ResourceFinder();
            _backgrounds = new []
            {
                null,
                finder.FindBackground("Tiles.Floor.Blue"),
                finder.FindBackground("Tiles.Wall.Blue"),
            };

            Game.Load(finder);

            _tileMap = finder.FindTileMap("Map0.tm");
            var reader = new TileMapReader(_tileMap);

            _room = reader.ReadRoom(_positionX, _positionY);

            Loaded += OnLoaded;
        }

        /// <summary>
        /// When this view is loaded into the visual tree.
        /// </summary>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            LoadRoom();
        }

        /// <summary>
        /// Select the room at the given coordinates (<paramref name="a_roomX"/>, <paramref name="a_roomY"/>).
        /// </summary>
        /// <param name="a_roomX">X coordinate of the room to select.</param>
        /// <param name="a_roomY">Y coordinate of the room to select.</param>
        private void SelectRoom(int a_roomX, int a_roomY)
        {
            _positionX = a_roomX;
            _positionY = a_roomY;

            var reader = new TileMapReader(_tileMap);
            _room = reader.ReadRoom(_positionX, _positionY);

            LoadRoom();
        }

        /// <summary>
        /// Load the room.
        /// </summary>
        private void LoadRoom()
        {
            if (_room.Height != RowDefinitions.Count)
            {
                RowDefinitions.Clear();
                for (var y = 0; y < _room.Height; y++)
                    RowDefinitions.Add(new RowDefinition {Height = new GridLength(64)});
            }

            if (_room.Width != RowDefinitions.Count)
            {
                ColumnDefinitions.Clear();
                for (var x = 0; x < _room.Width; x++)
                    ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(64)});
            }

            Children.Clear();

            for (var y = 0; y < _room.Height; y++)
            {
                for (var x = 0; x < _room.Width; x++)
                {

                    var tile = _room.GetTile(x, y);

                    var border = new Border
                    {
                        Visibility = Visibility.Visible,
                        Background = _backgrounds[tile.Background]
                    };

                    Grid.SetColumn(border, x);
                    Grid.SetRow(border, y);

                    Children.Add(border);
                }
            }
        }

        public void HandleKey(Key a_key)
        {
            if (a_key == Key.W)
                SelectRoom(_positionX, _positionY - 1);
            else if (a_key == Key.S)
                SelectRoom(_positionX, _positionY + 1);
            else if (a_key == Key.A)
                SelectRoom(_positionX - 1, _positionY);
            else if (a_key == Key.D)
                SelectRoom(_positionX + 1, _positionY);
        }

    }
}