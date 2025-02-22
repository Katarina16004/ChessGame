using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChessGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            string[] crneFigure = { "crni_top.png", "crni_konj.png", "crni_lovac.png", "crna_kraljica.png", "crni_kralj.png", "crni_pijun.png" };
            string[] beleFigure = { "beli_top.png", "beli_konj.png", "beli_lovac.png", "bela_kraljica.png", "beli_kralj.png", "beli_pijun.png" };

            for (int i = 2; i < 6; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Button dugme = new Button();
                    dugme.Name =$"Button{i}{j}";
                    if ((i + j) % 2 != 0)
                    {
                        dugme.Background= new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFC8EDF"));
                    }
                    else
                    {
                        dugme.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("white"));
                    }
                    Grid.SetRow(dugme, i);
                    Grid.SetColumn(dugme, j);
                    Tabla.Children.Add(dugme);
                }
            }
        }
    }
}