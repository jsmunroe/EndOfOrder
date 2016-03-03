using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EndOfOrder
{
    /// <summary>
    /// Interaction logic for SpriteControl.xaml
    /// </summary>
    public partial class SpriteControl : UserControl
    {
        public static readonly DependencyProperty SpriteSourceProperty = DependencyProperty.Register("SpriteSource", typeof(ImageSource), typeof(SpriteControl), new FrameworkPropertyMetadata(null, OnSpriteSourcePropertyChanged));

        public static readonly DependencyProperty FrameProperty = DependencyProperty.Register("Frame", typeof(int), typeof(SpriteControl), new FrameworkPropertyMetadata(0, OnFramePropertyChanged));

        /// <summary>
        /// Constructor.
        /// </summary>
        public SpriteControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sprite source.
        /// </summary>
        public ImageSource SpriteSource
        {
            get { return (ImageSource)GetValue(SpriteSourceProperty); }
            set { SetValue(SpriteSourceProperty, value); }
        }

        /// <summary>
        /// Frame number.
        /// </summary>
        public int Frame
        {
            get { return (int)GetValue(FrameProperty); }
            set { SetValue(FrameProperty, value); }
        }

        /// <summary>
        /// When the SpriteSourceProperty dependency property is changed.
        /// </summary>
        private static void OnSpriteSourcePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var control = obj as SpriteControl;

            if (control == null)
                return;

            var spriteBrush = new ImageBrush
            {
                ImageSource = (ImageSource)e.NewValue,
            };

            control.Sprite.Width = spriteBrush.ImageSource.Width;
            control.Sprite.Height = spriteBrush.ImageSource.Height;
            control.Sprite.Fill = spriteBrush;
        }


        /// <summary>
        /// When the FrameProperty dependency property is changed.
        /// </summary>
        private static void OnFramePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var control = obj as SpriteControl;

            if (control == null)
                return;

            var rowLength = (int)control.Sprite.Width / 32;

            if (rowLength == 0)
                return;

            if (e.NewValue is int == false)
                return;

            var frame = (int)e.NewValue;

            var x = frame % rowLength;
            var y = frame / rowLength;

            var margin = new Thickness
            {
                Left = -x * 32,
                Top = -y * 32
            };

            control.Sprite.Margin = margin;
        }

    }
}
