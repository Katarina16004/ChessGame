using ChessGame.Models;
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
using System.Windows.Shapes;

namespace ChessGame
{
    /// <summary>
    /// Interaction logic for VratiFiguruWindow.xaml
    /// </summary>
    public partial class VratiFiguruWindow : Window
    {
        public List<Figura> PojedeneFigure { get; set; }
        public Figura odabranaFigura = null;
        public VratiFiguruWindow(List<Figura> pojedeneFigure)
        {
            InitializeComponent();
            PojedeneFigure= pojedeneFigure;
            ListaFigura.ItemsSource = PojedeneFigure.Select(figura => new
            {
                Ime = figura.GetType().Name,
                FiguraObjekat = figura
            }).ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var odabranaFiguraObjekat = ListaFigura.SelectedItem as dynamic;
            odabranaFigura = odabranaFiguraObjekat.FiguraObjekat;
            this.Close();
        }

        private void ListaFigura_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            button.IsEnabled = true;
        }
    }
}
