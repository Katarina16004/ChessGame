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
        private List<Figura> pojedeneFigure=new List<Figura>(); //lista figura za vracanje na tablu
        private List<(int, int)> pozicijeKralja = new List<(int, int)> { (7, 4), (0, 4) }; //na prvom mestu beli, na drugom crni 
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
            pojedeneFigure.Add(tablaFigura[0, 0]);
            tablaFigura[0, 7] = new Top("crna", 0, 7);
            tablaFigura[7, 0] = new Top("bela", 7, 0);
            pojedeneFigure.Add(tablaFigura[7,0]);
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
            pojedeneFigure.Add(tablaFigura[0, 1]);
            tablaFigura[0, 2] = new Lovac("crna", 0, 2);
            pojedeneFigure.Add(tablaFigura[0, 2]);
            tablaFigura[0, 3] = new Kraljica("crna", 0, 3);
            pojedeneFigure.Add(tablaFigura[0, 3]);
            tablaFigura[0, 4] = new Kralj("crna", 0, 4);
            tablaFigura[0, 5] = new Lovac("crna", 0, 5);
            tablaFigura[0, 6] = new Konj("crna", 0, 6);
            tablaFigura[7, 1] = new Konj("bela", 7, 1);
            pojedeneFigure.Add(tablaFigura[7,1]);
            tablaFigura[7, 2] = new Lovac("bela", 7, 2);
            pojedeneFigure.Add(tablaFigura[7, 2]);
            tablaFigura[7, 3] = new Kraljica("bela", 7, 3);
            pojedeneFigure.Add(tablaFigura[7, 3]);
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
            poljaZaJelo.Clear();
            poslednjaFigura.Red = red;
            poslednjaFigura.Kolona = kolona;

            if(poslednjaFigura.GetType().Name=="Kralj")
            {
                if (poslednjaFigura.Boja == "bela")
                    pozicijeKralja[0] = (red, kolona);
                else
                    pozicijeKralja[1] = (red, kolona);
            }
            //MessageBox.Show($"Beli: {pozicijeKralja[0].Item1},{pozicijeKralja[0].Item2}\nCrni: {pozicijeKralja[1].Item1},{pozicijeKralja[1].Item2}");
            Kralj beliKralj = tablaFigura[pozicijeKralja[0].Item1, pozicijeKralja[0].Item2] as Kralj;
            Kralj crniKralj = tablaFigura[pozicijeKralja[1].Item1, pozicijeKralja[1].Item2] as Kralj;
            
            if (crniKralj.DaLiJeMat(tablaFigura))
            {
                MessageBox.Show("Crni kralj je u matu");
            }
            else if (beliKralj.DaLiJeMat(tablaFigura))
            {
                MessageBox.Show("Beli kralj je u matu");
            }
            else if (crniKralj.DaLiJePat(tablaFigura))
            {
                MessageBox.Show("Crni kralj je u patu");
            }
            else if (beliKralj.DaLiJePat(tablaFigura))
            {
                MessageBox.Show("Beli kralj je u patu");
            }
            else
            {
                if (crniKralj.DaLiJeKraljUgrozen(tablaFigura))
                    MessageBox.Show("Crni kralj je ugrozen");
                else if(beliKralj.DaLiJeKraljUgrozen(tablaFigura))
                    MessageBox.Show("Beli kralj je ugrozen");
            }

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
                //MessageBox.Show($"{pojedeneFigure.Count()}");
                
                PremestiFiguru(red, kolona);
            }
        }
    }
}