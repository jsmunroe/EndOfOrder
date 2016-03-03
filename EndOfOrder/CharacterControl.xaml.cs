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
using TileBuilder.Contracts.Ui;

namespace EndOfOrder
{
    /// <summary>
    /// Interaction logic for CharacterControl.xaml
    /// </summary>
    public partial class CharacterControl : UserControl
    {

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <paramref name="a_character"/>
        public CharacterControl(ICharacter a_character)
        {
            Character = a_character;

            InitializeComponent();

            Sprite.SpriteSource = ResourceProvider.Current.GetImageSource(Character.Sprite);
            Update();
        }

        /// <summary>
        /// Character represented in this control.
        /// </summary>
        public ICharacter Character { get; }

        /// <summary>
        /// Update the control to represet the character.
        /// </summary>
        public void Update()
        {
            Sprite.Frame = Character.SpriteFrame;
            Grid.SetColumn(this, Character.Position.X);
            Grid.SetRow(this, Character.Position.Y);
        }
    }
}
