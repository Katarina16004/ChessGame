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
        private List<(int, int)> poljaZaJelo = new List<(int, int)>();
        private List<Figura> pojedeneFigure = new List<Figura>(); //lista figura za vracanje na tablu
        private List<(int, int)> pozicijeKralja = new List<(int, int)> { (7, 4), (0, 4) }; //na prvom mestu beli, na drugom crni 
        private bool beliIgracNaRedu = true;
        private static List<(int, int)> poslednjaPomerenaFigura = new List<(int, int)> { (-1, -1) }; //prethodna pozicija poslednje pomerene figure
        private static List<Figura> pretecaFigura = new List<Figura>() { null, null }; //pamtimo koja je figura dala sah kralju //na prvom mestu beli, na drugom crni
        private static List<int> ugrozenKralj = new List<int>() { 0, 0 }; //pamtimo koliko njih ugrozava kralja //na prvom mestu beli, na drugom crni
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
            PostaviSlikuNaDugme();
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
        private bool PostaviSlikuNaDugme()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (tablaFigura[i, j] != null) //tu treba postaviti odgovarajucu sliku
                    {
                        Image Slika = new Image
                        {
                            Source = new BitmapImage(new Uri(tablaFigura[i, j].GetSlika(), UriKind.Relative)),
                            Width = 50,
                            Height = 50,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center
                        };
                        tabla[i, j].Content = Slika;
                    }
                    else
                        tabla[i, j].Content = null;
                }
            }
            return true;
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
        public void ResetujIgru()
        {
            poslednjaObojenaPolja.Clear();
            poslednjaFigura = null;
            poljaZaJelo.Clear();
            pojedeneFigure.Clear();
            pozicijeKralja = new List<(int, int)> { (7, 4), (0, 4) };
            beliIgracNaRedu = true;
            poslednjaPomerenaFigura = new List<(int, int)> { (-1, -1) };
            pretecaFigura = new List<Figura>() { null, null };
            ugrozenKralj = new List<int>() { 0, 0 };
            Array.Clear(tablaFigura, 0, tablaFigura.Length);

            PostaviPocetneFigure();
            PostaviSlikuNaDugme();
            naRedu.Text = "beli";
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int red = Grid.GetRow(button);
            int kolona = Grid.GetColumn(button);
            Figura polje = tablaFigura[red, kolona];
            if (polje != null) // ako je kliknuto na figuru
            {
                if(poljaZaJelo.Contains((red,kolona)) && poslednjaFigura.ValidanPotez(red, kolona, tablaFigura)) //jedemo figuru
                {
                    if (!KraljZasticen(poslednjaFigura, red, kolona)) //ukoliko je kralj napadnut mora da se zastiti
                    {
                        return;
                    }
                    else
                    {
                        //da li bi pomeranje te figure ugrozilo kralja
                        if (poslednjaFigura is not Kralj && UgrozavaKralja(poslednjaFigura, red, kolona))
                            return;
                    }

                    if (!PremestiFiguru(red, kolona)) //mozemo pojesti sve figure osim kralja
                    {
                        MessageBox.Show("Ne mozete uzeti kralja!", "Nevazeci potez", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                        
                    ResetujBoje();
                    if (ProveriStanjeIgre())
                    {
                        ResetujIgru();
                        return;
                    }

                    poljaZaJelo.Clear();

                    if (poslednjaFigura is Pijun)
                    {
                        ProveriIzbacivanjeSvojeFigure(red, kolona);
                    }
                    poslednjaFigura = null;
                    beliIgracNaRedu = !beliIgracNaRedu;
                    naRedu.Text = (beliIgracNaRedu == true) ? "beli" : "crni";
                    return;
                }
                /*
                if ((beliIgracNaRedu && polje.Boja == "bela") || (!beliIgracNaRedu && polje.Boja == "crna"))
                {
                    poslednjaFigura = polje;
                }
                else
                {
                    //MessageBox.Show("Nije tvoje vreme za potez!");
                    return;
                }*/

                // prikazivanje moguceg kretanja
                ResetujBoje();
                 poslednjaFigura = polje;
                poslednjaObojenaPolja.Clear();
                poslednjaFigura.Red = red;
                poslednjaFigura.Kolona = kolona;

                List<(int, int)> moguciPotezi = poslednjaFigura.MoguciPotezi(tablaFigura);
                

                foreach (var (r, k) in moguciPotezi)
                {
                    if (tablaFigura[r, k] == null)
                    {
                        tabla[r, k].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C3FDB8"));
                    }
                    else
                    {
                        tabla[r, k].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E55451"));
                        poljaZaJelo.Add((r, k));
                    }
                    poslednjaObojenaPolja.Add((r, k));
                }
            }
            else  // ako je null, znaci da je polje na koje treba da se skoci ili da je prazno polje
            {
                if(poslednjaFigura!=null) // ako smo pre toga kliknuli na neku figuru, znaci da zelimo da je pomerimo na to polje koje smo kliknuli
                {
                    if(poslednjaFigura.ValidanPotez(red,kolona,tablaFigura))
                    {
                        if (!KraljZasticen(poslednjaFigura, red, kolona))
                        {
                            return;
                        }
                        else
                        {
                            //da li bi pomeranje te figure ugrozilo kralja
                            if (poslednjaFigura is not Kralj && UgrozavaKralja(poslednjaFigura, red, kolona))
                                return;
                        }

                        PremestiFiguru(red, kolona);
                        ResetujBoje();
                        if (ProveriStanjeIgre())
                        {
                            ResetujIgru();
                            return;
                        }

                        if(poslednjaFigura is Pijun)
                        {
                            ProveriAnPasan(red,kolona);
                            ProveriIzbacivanjeSvojeFigure(red, kolona);
                        }
                        poslednjaFigura = null;
                        beliIgracNaRedu = !beliIgracNaRedu;
                        naRedu.Text = (beliIgracNaRedu==true) ? "beli" : "crni";
                    }
                }
            }
        }
        private bool UgrozavaKralja(Figura figura, int red, int kolona)
        {
            //privremeno pomerimo figuru
            int orgRed = figura.Red;
            int orgKol = figura.Kolona;
            tablaFigura[orgRed, orgKol] = null;
            tablaFigura[red, kolona] = figura;
            tablaFigura[red, kolona].Red = red;
            tablaFigura[red, kolona].Kolona = kolona;

            int index = figura.Boja == "bela" ? 0 : 1;
            Kralj kralj =new Kralj(figura.Boja,pozicijeKralja[index].Item1, pozicijeKralja[index].Item2); //sigurno je tu kralj
            bool ugrozen = kralj.DaLiJeKraljUgrozen(tablaFigura);

            //vracamo originalno stanje table
            tablaFigura[red, kolona] = null;
            tablaFigura[orgRed, orgKol] = figura;
            tablaFigura[orgRed, orgKol].Red = orgRed;
            tablaFigura[orgRed, orgKol].Kolona = orgKol;
            if (ugrozen)
            {
                pretecaFigura[index] = null; //funkcija kralja ce nam ovo promeniti, pa moramo da vratimo na staro
                ugrozenKralj[index] = 0;
                MessageBox.Show("Ugrozili biste kralja", "Nevazeci potez", MessageBoxButton.OK, MessageBoxImage.Warning);
                return true;
            }
            return false;
        }
        private bool KraljZasticen(Figura figura, int red, int kolona)
        {
            int index = figura.Boja == "bela" ? 0 : 1;
            string bojaKralja = figura.Boja == "bela" ? "Beli" : "Crni";

            if (ugrozenKralj[index] > 0)
            {
                if (figura is not Kralj)
                {
                    if (ugrozenKralj[index] == 1 && !figura.ZastiticeKralja(red, kolona, tablaFigura, pozicijeKralja))
                    {
                        MessageBox.Show($"{bojaKralja} kralj mora biti zasticen!", "Nevazeci potez", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return false;
                    }
                    else if (ugrozenKralj[index] > 1)
                    {
                        MessageBox.Show($"{bojaKralja} kralj mora biti zasticen! Pomerite kralja", "Nevazeci potez", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return false;
                    }
                }
            }
            return true;
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
        private bool PremestiFiguru(int red, int kolona)
        {
            if (tablaFigura[red, kolona] is not Kralj)
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
                poslednjaFigura.PrethodnaKolona = poslednjaFigura.Kolona;
                poslednjaFigura.PrethodniRed = poslednjaFigura.Red;
                poslednjaPomerenaFigura[0] = (poslednjaFigura.PrethodniRed, poslednjaFigura.PrethodnaKolona);

                poslednjaFigura.Red = red;
                poslednjaFigura.Kolona = kolona;

                if (poslednjaFigura is Kralj)
                {
                    if (poslednjaFigura.Boja == "bela")
                    {
                        int pomeraj = kolona - pozicijeKralja[0].Item2;
                        if (Math.Abs(pomeraj) == 2)
                        {
                            IzvrsiRokadu(red, pomeraj, poslednjaFigura.Boja);
                        }
                        pozicijeKralja[0] = (red, kolona);
                    }
                    else
                    {
                        int pomeraj = kolona - pozicijeKralja[1].Item2;
                        if (Math.Abs(pomeraj) == 2)
                        {
                            IzvrsiRokadu(red, pomeraj, poslednjaFigura.Boja);
                        }
                        pozicijeKralja[1] = (red, kolona);
                    }
                }
                return true;
            }
            return false;
            
        }
        public void IzvrsiRokadu(int red, int pomeraj, string boja)
        {
            Image topSlika;
            if(boja=="bela")
            {
                topSlika = new Image
                {
                    Source = new BitmapImage(new Uri("Resources/Figure/beli_top.png", UriKind.Relative)),
                    Width = 50,
                    Height = 50,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
            }
            else
            {
                topSlika = new Image
                {
                    Source = new BitmapImage(new Uri("Resources/Figure/crni_top.png", UriKind.Relative)),
                    Width = 50,
                    Height = 50,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
            }

            if (pomeraj == 2) //mala rokada
            {
                tablaFigura[red, 5] = tablaFigura[red, 7];
                tablaFigura[red, 7] = null;
                tabla[red, 7].Content = null;
                tabla[red, 5].Content = topSlika;
                tablaFigura[red, 5].Red = red;
                tablaFigura[red, 5].Kolona = 5;
            }
            else if (pomeraj == -2) //velika rokada
            {
                //pomeramo topa na [7,3]
                tablaFigura[red, 3] = tablaFigura[red, 0];
                tablaFigura[red, 0] = null;
                tabla[red, 0].Content = null;
                tabla[red, 3].Content = topSlika;
                tablaFigura[red, 3].Red = red;
                tablaFigura[red, 3].Kolona = 3;
            }
        }
        public void ProveriAnPasan(int red,int kolona)
        {
            if (poslednjaFigura.Boja == "bela")
            {
                if (tablaFigura[red + 1, kolona] != null && tablaFigura[red + 1, kolona] is Pijun
                        && tablaFigura[red + 1, kolona].Boja != poslednjaFigura.Boja)
                {
                    tablaFigura[red+1, kolona] = null;
                    tabla[red+1, kolona].Content = null;
                }
            }
            else
            {
                if (tablaFigura[red - 1, kolona] != null && tablaFigura[red - 1, kolona] is Pijun
                        && tablaFigura[red - 1, kolona].Boja != poslednjaFigura.Boja)
                {
                    tablaFigura[red-1, kolona] = null;
                    tabla[red-1, kolona].Content = null;
                }
            }

        }
        public bool ProveriStanjeIgre()
        {
            Kralj beliKralj = tablaFigura[pozicijeKralja[0].Item1, pozicijeKralja[0].Item2] as Kralj;
            Kralj crniKralj = tablaFigura[pozicijeKralja[1].Item1, pozicijeKralja[1].Item2] as Kralj;

            if (crniKralj.DaLiJeMat(tablaFigura))
            {
                MessageBox.Show("🎉 Crni kralj je u matu! \nPobedio je beli igrac 🎉", "Kraj partije", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }
            else if (beliKralj.DaLiJeMat(tablaFigura))
            {
                MessageBox.Show("🎉 Beli kralj je u matu! \nPobedio je crni igrac 🎉", "Kraj partije", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }
            else if (crniKralj.DaLiJePat(tablaFigura))
            {
                MessageBox.Show("Crni kralj je u patu \nNeresno", "Kraj partije", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }
            else if (beliKralj.DaLiJePat(tablaFigura))
            {
                MessageBox.Show("Beli kralj je u patu \nNeresno", "Kraj partije", MessageBoxButton.OK, MessageBoxImage.Information);
                
                return true;
            }
            else if (tablaFigura.Cast<Figura>().Count(figura => figura != null) == 3) //pravilo nedovoljno materijala za pobedu
            {
                foreach (var fig in tablaFigura)
                {
                    if (fig != null && fig is not Kralj  && (fig is Konj || fig is Lovac))
                    {
                        MessageBox.Show("Nedovoljno materijala za pobedu \nNeresno", "Kraj partije", MessageBoxButton.OK, MessageBoxImage.Information);
                        return true;
                    }
                }
            }
            else
            {
                if (crniKralj.DaLiJeKraljUgrozen(tablaFigura))
                {
                    MessageBox.Show("Crni kralj je ugrozen");
                }
                else if (beliKralj.DaLiJeKraljUgrozen(tablaFigura))
                {
                    MessageBox.Show("Beli kralj je ugrozen");
                }
            }
            return false;
        }
        private void ProveriIzbacivanjeSvojeFigure(int red, int kolona)
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
                poljaZaJelo.Clear();
                VratiFiguruWindow vratiFiguruWindow = new VratiFiguruWindow(naseFigue);
                vratiFiguruWindow.ShowDialog();

                Figura novaFigura = vratiFiguruWindow.odabranaFigura;
                if (novaFigura == null)
                    return;

                // postavimo novu figuru na tablu
                tablaFigura[red, kolona] = novaFigura;
                tabla[red, kolona].Content = new Image
                {
                    Source = new BitmapImage(new Uri(novaFigura.GetSlika(), UriKind.Relative)),
                    Width = 50,
                    Height = 50,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
            }
        }
        public static List<(int, int)> GetPoslednjiPotez()
        {
            return poslednjaPomerenaFigura;
        }
        public static void SetPretecaFigura(int i,Figura f)
        {
            pretecaFigura[i]= f;
        }
        public static Figura GetPretecaFigura(int i)
        {
            return pretecaFigura[i];
        }
        public static void SetUgrozenKralj(int i, int ugrozen)
        {
            ugrozenKralj[i] = ugrozen;
        }

        public static int GetUgrozenKralj(int i)
        {
            return ugrozenKralj[i];
        }
    }
}