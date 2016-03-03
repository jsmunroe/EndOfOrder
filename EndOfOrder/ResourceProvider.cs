using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TileBuilder;
using TileBuilder.Contracts;

namespace EndOfOrder
{
    public class ResourceProvider
    {
        #region Current

        private static ResourceProvider s_current = null;

        /// <summary>
        /// Current (readonly) - The current ResourceProvider instance.
        /// </summary>
        public static ResourceProvider Current
        {
            get
            {
                if (s_current == null)
                    s_current = new ResourceProvider();

                return s_current;
            }

        }

        #endregion

        private readonly Dictionary<string, Brush> _brushesByName = new Dictionary<string, Brush>();
        private readonly Dictionary<string, ImageSource> _imageSourcesByName = new Dictionary<string, ImageSource>();

        /// <summary>
        /// Private constructor.
        /// </summary>
        private ResourceProvider()
        {
            
        }

        /// <summary>
        /// Get an image source from the image with the given name (<paramref name="a_resourceName"/>).
        /// </summary>
        /// <param name="a_resourceName">Resource name.</param>
        /// <returns>Image source.</returns>
        public ImageSource GetImageSource(string a_resourceName)
        {
            if (a_resourceName == null)
                return null;

            if (_imageSourcesByName.ContainsKey(a_resourceName))
                return _imageSourcesByName[a_resourceName];

            var resource = Game.ResourceFinder.FindImage(a_resourceName);

            if (resource == null)
                return null;

            var imageSource = CreateImageSource(resource);

            if (imageSource != null)
                _imageSourcesByName[a_resourceName] = imageSource;

            return imageSource;
        }

        /// <summary>
        /// Get an brush from the image with the given name (<paramref name="a_resourceName"/>).
        /// </summary>
        /// <param name="a_resourceName">Resource name.</param>
        /// <returns>Brush source.</returns>
        public Brush GetBrush(string a_resourceName)
        {
            if (a_resourceName == null)
                return null;

            var resource = Game.ResourceFinder.FindImage(a_resourceName);

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
        private static Brush CreateBrush(IResource a_resource)
        {
            var bitmap = CreateImageSource(a_resource);
            var imageBrush = new ImageBrush
            {
                ImageSource = bitmap,
            };

            return imageBrush;
        }

        /// <summary>
        /// Create an image source from the given resource (<paramref name="a_resource"/>).
        /// </summary>
        /// <param name="a_resource">Resource.</param>
        /// <returns>Image brush.</returns>
        private static ImageSource CreateImageSource(IResource a_resource)
        {
            using (var stream = a_resource.Open())
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();

                return bitmap;
            }
        }
    }
}
