using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using EndOfOrder.Story;
using TileBuilder;
using TileBuilder.Files;

namespace EndOfOrder
{
    public class RoomView : Grid, IRoomView
    {
        private UnitCoord _roomLocation = new UnitCoord();

        private Dictionary<string, Brush> _brushesByName = new Dictionary<string, Brush>();

        private IResource _tileMap;

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
                        Background = LookUpTileBackground(tile.Background),
                    };

                    Grid.SetColumn(border, x);
                    Grid.SetRow(border, y);

                    Children.Add(border);
                }
            }
        }


        /// <summary>
        /// Look up a tile background with the given tile name (<paramref name="a_resourceName"/>).
        /// </summary>
        /// <param name="a_resourceName">Resource name.</param>
        /// <returns>Tile background brush.</returns>
        private Brush LookUpTileBackground(string a_resourceName)
        {
            if (a_resourceName == null)
                return null;

            if (_brushesByName.ContainsKey(a_resourceName))
                return _brushesByName[a_resourceName];

            var resource = Game.ResourceFinder.FindBackground(a_resourceName);

            if (resource == null)
                return null;

            var brush = CreateBrush(resource);

            if (brush != null)
                _brushesByName[a_resourceName] = brush;

            return brush;
        }

        /// <summary>
        /// Create a brush from the given resource (<paramref name="a_resource"/>).
        /// </summary>
        /// <param name="a_resource">Resource.</param>
        /// <returns>Brush.</returns>
        private Brush CreateBrush(IResource a_resource)
        {
            using (var stream = a_resource.Open())
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();

                var imageBrush = new ImageBrush
                {
                    ImageSource = bitmap,
                };

                return imageBrush;
            }
        }

        public void HandleKey(Key a_key)
        {
            if (a_key == Key.W)
                Game.World.GotoRoom(_roomLocation.X, _roomLocation.Y - 1);
            else if (a_key == Key.S)
                Game.World.GotoRoom(_roomLocation.X, _roomLocation.Y + 1);
            else if (a_key == Key.A)
                Game.World.GotoRoom(_roomLocation.X - 1, _roomLocation.Y);
            else if (a_key == Key.D)
                Game.World.GotoRoom(_roomLocation.X + 1, _roomLocation.Y);
        }

    }
}