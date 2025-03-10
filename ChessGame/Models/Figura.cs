using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChessGame.Models
{
    public abstract class Figura
    {
        public string Boja { get; set; }
        public int Red { get; set; }
        public int Kolona { get; set; }
        public int PrethodniRed { get; set; }
        public int PrethodnaKolona { get; set; }

        public Figura(string boja, int red, int kolona)
        {
            Boja = boja;
            Red = red;
            Kolona = kolona;
            PrethodnaKolona = -1;
            PrethodniRed = -1;
        }
        public string GetSlika()
        {
            string bojaSlike = (Boja == "bela") ? "beli" : "crni";
            string imeFigure = this.GetType().Name.ToLower();
            return $"Resources/Figure/{bojaSlike}_{imeFigure}.png";
        }
        public abstract bool ValidanPotez(int novaPozicijaRed, int novaPozicijaKolona, Figura[,] tabla);
        public abstract List<(int, int)> MoguciPotezi(Figura[,] tabla);

        public bool ZastiticeKralja(int novaPozicijaRed, int novaPozicijaKolona, Figura[,] tabla, List<(int, int)> pozicijeKralja)
        {
            int index = (Boja == "bela") ? 0 : 1;

            Kralj kralj = (Kralj)tabla[pozicijeKralja[index].Item1, pozicijeKralja[index].Item2];
            Figura pretnja = MainWindow.GetPretecaFigura(index);

            // figura moze da pojede pretecu
            if (novaPozicijaRed == pretnja.Red && novaPozicijaKolona == pretnja.Kolona)
            {
                return true;
            }

            // figura moze da stane na putanju do kralja
            if (BlokiraPretnju(kralj, pretnja, novaPozicijaRed, novaPozicijaKolona))
            {
                return true;
            }

            return false;
        }

        private bool BlokiraPretnju(Kralj kralj, Figura pretnja, int novaPozicijaRed, int novaPozicijaKolona)
        {
            int dRed = Math.Sign(pretnja.Red - kralj.Red);
            int dKolona = Math.Sign(pretnja.Kolona - kralj.Kolona);

            if (dRed == 0 && dKolona == 0) 
                return false;

            int red = kralj.Red + dRed;
            int kolona = kralj.Kolona + dKolona;

            //MessageBox.Show($"BlokiraPretnju: Kralj [{kralj.Red},{kralj.Kolona}] → Pretnja [{pretnja.Red},{pretnja.Kolona}]");

            while (red >= 0 && red < 8 && kolona >= 0 && kolona < 8) 
            {
                //MessageBox.Show($"Provera polja [{red},{kolona}]");

                if (red == novaPozicijaRed && kolona == novaPozicijaKolona)
                {
                    //MessageBox.Show("Potez blokira pretnju!");
                    return true;
                }

                if (red == pretnja.Red && kolona == pretnja.Kolona)
                {
                    //MessageBox.Show("Dosli smo do pretnje, nema vise blokiranja");
                    break;
                }

                red += dRed;
                kolona += dKolona;
            }

           // MessageBox.Show("Potez NE blokira pretnju");
            return false;
        }


    }
}
