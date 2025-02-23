using ChessGame.Models;
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
        private Button[,] tabla = new Button[8, 8];
        private Figura[,] tablaFigura = new Figura[8, 8];
        public MainWindow()
        {
            InitializeComponent();
            string[] crneFigure = { "crni_top.png", "crni_konj.png", "crni_lovac.png", "crna_kraljica.png", "crni_kralj.png", "crni_pijun.png" };
            string[] beleFigure = { "beli_top.png", "beli_konj.png", "beli_lovac.png", "bela_kraljica.png", "beli_kralj.png", "beli_pijun.png" };


            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (i != 0 && i != 1 && i != 6 && i != 7)
                    {
                        Button dugme = new Button();
                        dugme.Name = $"Button{i}{j}";
                        if ((i + j) % 2 != 0)
                        {
                            dugme.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFC8EDF"));
                        }
                        else
                        {
                            dugme.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("white"));
                        }
                        Grid.SetRow(dugme, i);
                        Grid.SetColumn(dugme, j);
                        Tabla.Children.Add(dugme);
                        tabla[i, j] = dugme;

                        dugme.Click += Button_Click;
                    }
                    else
                    {
                        tabla[i, j]=NadjiDugmeUGridu(i, j);
                    }
                }
            }
            PostaviPocetneFigure();
        }
        private Button NadjiDugmeUGridu(int red, int kolona)
        {
            foreach (UIElement element in Tabla.Children)
            {
                if (element is Button dugme &&
                    Grid.GetRow(dugme) == red &&
                    Grid.GetColumn(dugme) == kolona)
                {
                    return dugme;
                }
            }
            return null;
        }

        private bool PostaviPocetneFigure()
        {
            tablaFigura[0, 0] = new Top("crna", 0, 0);
            tablaFigura[0, 7] = new Top("crna", 0, 7);
            tablaFigura[7, 0] = new Top("bela", 7, 0);
            tablaFigura[7, 7] = new Top("bela", 7, 7);
            for (int i = 0; i < 8; i++)
            {
                tablaFigura[1, i] = new Pijun("crna", 1, i);
            }
            for (int i = 0; i < 8; i++)
            {
                tablaFigura[6, i] = new Pijun("bela", 6, i);
            }
            tablaFigura[0, 1] = new Konj("crna", 0, 1);
            tablaFigura[0, 2] = new Lovac("crna", 0, 2);
            tablaFigura[0, 3] = new Kraljica("crna", 0, 3);
            tablaFigura[0, 4] = new Kralj("crna", 0, 4);
            tablaFigura[0, 5] = new Lovac("crna", 0, 5);
            tablaFigura[0, 6] = new Konj("crna", 0, 6);
            tablaFigura[7, 1] = new Konj("bela", 7, 1);
            tablaFigura[7, 2] = new Lovac("bela", 7, 2);
            tablaFigura[7, 3] = new Kraljica("bela", 7, 3);
            tablaFigura[7, 4] = new Kralj("bela", 7, 4);
            tablaFigura[7, 5] = new Lovac("bela", 7, 5);
            tablaFigura[7, 6] = new Konj("bela", 7, 6);

            return true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button.Content as Image !=null)
            {
                MessageBox.Show("Kliknuto na figuru");
            }
        }
    }
}