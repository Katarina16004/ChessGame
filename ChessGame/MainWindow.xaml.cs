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
        private List<(int, int)> poslednjaObojenaPolja = new List<(int, int)>();
        private Figura poslednjaFigura = null; // poslednja izabrana
        private List<(int,int)> poljaZaJelo=new List<(int, int)>();
        private List<Figura> pojedeneFigure=new List<Figura>(); 
        public MainWindow()
        {
            InitializeComponent();

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
            int red = Grid.GetRow(button);
            int kolona = Grid.GetColumn(button);
            Figura polje = tablaFigura[red, kolona];
            if (polje != null) // ako je kliknuto na figuru
            {
                if(poljaZaJelo.Contains((red,kolona))) //jedemo figuru
                {
                    if (poslednjaFigura.ValidanPotez(red, kolona, tablaFigura))
                    {
                        if (tablaFigura[red, kolona].GetType().Name!="Pijun")
                            pojedeneFigure.Add(tablaFigura[red, kolona]); //necemo da cuvamo pijuna
                        tablaFigura[red, kolona] = null; //pojedemo figuru
                        PremestiFiguru(red, kolona);
                        
                        poljaZaJelo.Clear();

                        ResetujBoje();
                        IzbaciSvojuFiguru(red, kolona);
                        poslednjaFigura = null;
                        return;
                    }
                }
                // prikazivanje moguceg kretanja
                ResetujBoje();
                poslednjaFigura = polje;
                poslednjaObojenaPolja.Clear();
                poslednjaFigura.Red = red;
                poslednjaFigura.Kolona = kolona;
                
                //MessageBox.Show($"{red}{kolona} dugme");
                //MessageBox.Show($"{poslednjaFigura.GetSlika()} se nalazi na {poslednjaFigura.Red} redu i {poslednjaFigura.Kolona} koloni");
               
                List<(int, int)> moguciPotezi = poslednjaFigura.MoguciPotezi(tablaFigura);

                foreach (var (r, k) in moguciPotezi)
                {
                    if (tablaFigura[r, k] == null)
                    {
                        tabla[r, k].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("lightBlue"));
                    }
                    else
                    {
                        tabla[r, k].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("red"));
                        poljaZaJelo.Add((r, k));
                    }
                    poslednjaObojenaPolja.Add((r, k));
                }
            }
            else  // ako je null, znaci da je polje na koje treba da se skoci ili da je prazno polje
            {
                if(poslednjaFigura!=null) // ako smo pre toga kliknuli na neku figuru, znaci da zelimo da je pomerimo na to polje koje smo kliknuli
                {
                    //proveravamo da li je polje validno
                    if(poslednjaFigura.ValidanPotez(red,kolona,tablaFigura))
                    {

                        PremestiFiguru(red, kolona);
                        ResetujBoje();
                        IzbaciSvojuFiguru(red, kolona);
                        poslednjaFigura = null;
                    }
                }
            }
        }
        private void ResetujBoje()
        {
            foreach(var (r,k) in poslednjaObojenaPolja)
            {
                if ((r + k) % 2 != 0)
                    tabla[r,k].Background= new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFC8EDF"));
                else
                    tabla[r,k].Background= new SolidColorBrush((Color)ColorConverter.ConvertFromString("white"));
            }
            poslednjaObojenaPolja.Clear();
        }
        private void PremestiFiguru(int red, int kolona)
        {
            tablaFigura[poslednjaFigura.Red, poslednjaFigura.Kolona] = null;

            tabla[poslednjaFigura.Red, poslednjaFigura.Kolona].Content = null;

            Image figuraSlika = new Image
            {
                Source = new BitmapImage(new Uri(poslednjaFigura.GetSlika(), UriKind.Relative)),
                Width = 50,
                Height = 50,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            tabla[red, kolona].Content = figuraSlika;
            tablaFigura[red, kolona] = poslednjaFigura;
        }
        private void IzbaciSvojuFiguru(int red, int kolona)
        {
            Image img = (Image)tabla[red, kolona].Content;
            BitmapImage imgSource = (BitmapImage)img.Source;
            string imgPath = imgSource.UriSource.ToString();
            if ((imgPath.Contains("crni_pijun.png") && red == 7) || (imgPath.Contains("beli_pijun.png") && red == 0))
            {
                List<Figura> naseFigue= new List<Figura>();
                foreach (Figura f in pojedeneFigure)
                {
                    if (f.Boja == poslednjaFigura.Boja)
                        naseFigue.Add(f);
                }
                poljaZaJelo.Clear(); //PROVERITI KASNIJE
                VratiFiguruWindow vratiFiguruWindow = new VratiFiguruWindow(naseFigue);
                vratiFiguruWindow.ShowDialog();
                poslednjaFigura=vratiFiguruWindow.odabranaFigura;
                pojedeneFigure.Remove(poslednjaFigura);
                //MessageBox.Show($"{pojedeneFigure.Count()}");
                
                PremestiFiguru(red, kolona);
            }
        }
    }
}