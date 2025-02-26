using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Models
{
    public class Pijun : Figura
    {
        public Pijun(string boja, int red, int kolona) : base(boja, red, kolona) { }

        public override bool ValidanPotez(int novaPozicijaRed, int novaPozicijaKolona, Figura[,] tabla)
        {
            int pomakRed = novaPozicijaRed - Red;
            int pomakKolona = novaPozicijaKolona - Kolona;

            if (pomakKolona == 0)
            {
                if (Boja == "crna")
                {
                    if (pomakRed == 1 && tabla[novaPozicijaRed, novaPozicijaKolona] == null)
                    {
                        return true; // jedan korak napred
                    }
                    else if (Red == 1 && pomakRed == 2 && tabla[novaPozicijaRed, novaPozicijaKolona] == null && tabla[Red + 1, Kolona] == null)
                    {
                        return true; // dva koraka napred na pocetku
                    }
                }
                else if (Boja == "bela")
                {
                    if (pomakRed == -1 && tabla[novaPozicijaRed, novaPozicijaKolona] == null)
                    {
                        return true; 
                    }
                    else if (Red == 6 && pomakRed == -2 && tabla[novaPozicijaRed, novaPozicijaKolona] == null && tabla[Red - 1, Kolona] == null)
                    {
                        return true;
                    }
                }
            }
            else if (Math.Abs(pomakKolona) == 1 && (Boja == "bela" && pomakRed == -1 || Boja == "crna" && pomakRed == 1))
            {
                if (tabla[novaPozicijaRed, novaPozicijaKolona] != null && tabla[novaPozicijaRed, novaPozicijaKolona].Boja != Boja)
                {
                    return true; // uzimanje protivnicke figure
                }
                if(tabla[novaPozicijaRed, novaPozicijaKolona] == null)  //an pasan
                {
                    if((Boja == "bela" && tabla[novaPozicijaRed+1,novaPozicijaKolona]!= null && tabla[novaPozicijaRed + 1, novaPozicijaKolona].Boja!=Boja)
                        || (Boja == "crna" && tabla[novaPozicijaRed - 1, novaPozicijaKolona] != null && tabla[novaPozicijaRed - 1, novaPozicijaKolona].Boja != Boja))
                    {
                        return true;
                    }
                }
            }

            return false; 
        }
        public override List<(int, int)> MoguciPotezi(Figura[,] tabla)
        {
            List<(int, int)> potezi = new List<(int, int)>();
            int smer= 1;
            int startniRed = 1;

            if (Boja == "bela")
            {
                smer = -1; 
                startniRed = 6;
            }

            if (Red + smer >= 0 && Red + smer < 8 && tabla[Red + smer, Kolona] == null)
                potezi.Add((Red + smer, Kolona));

            if (Red == startniRed && tabla[Red + smer, Kolona] == null && tabla[Red + 2 * smer, Kolona] == null)
                potezi.Add((Red + 2 * smer, Kolona));

            if (Kolona - 1 >= 0 && Red + smer >= 0 && Red + smer < 8 && tabla[Red + smer, Kolona - 1] != null && tabla[Red + smer, Kolona - 1].Boja != Boja)
                potezi.Add((Red + smer, Kolona - 1));

            if (Kolona + 1 < 8 && Red + smer >= 0 && Red + smer < 8 && tabla[Red + smer, Kolona + 1] != null && tabla[Red + smer, Kolona + 1].Boja != Boja)
                potezi.Add((Red + smer, Kolona + 1));
            //an pasan
            if (Boja == "bela" && Red==3)
            {
                if (Kolona - 1 >= 0 && tabla[Red, Kolona - 1] is Pijun protivnickiPijun && protivnickiPijun.Boja != Boja && protivnickiPijun.PrethodniRed==1 &&
                    protivnickiPijun.PrethodniRed == MainWindow.GetPoslednjiPotez()[0].Item1 && protivnickiPijun.PrethodnaKolona == MainWindow.GetPoslednjiPotez()[0].Item2)
                {
                    potezi.Add((Red - 1, Kolona - 1));
                }
                if (Kolona + 1 < 8 && tabla[Red, Kolona + 1] is Pijun protivnickiPijunn && protivnickiPijunn.Boja != Boja && protivnickiPijunn.PrethodniRed == 1
                    && protivnickiPijunn.PrethodniRed == MainWindow.GetPoslednjiPotez()[0].Item1 && protivnickiPijunn.PrethodnaKolona == MainWindow.GetPoslednjiPotez()[0].Item2)
                {
                    potezi.Add((Red - 1, Kolona + 1));
                }

            }
            else if (Boja == "crna" && Red==4)
            {
                if (Kolona - 1 >= 0 && tabla[Red, Kolona - 1] is Pijun protivnickiPijun && protivnickiPijun.Boja != Boja && protivnickiPijun.PrethodniRed == 6 &&
                    protivnickiPijun.PrethodniRed == MainWindow.GetPoslednjiPotez()[0].Item1 && protivnickiPijun.PrethodnaKolona == MainWindow.GetPoslednjiPotez()[0].Item2)
                {
                    potezi.Add((Red + 1, Kolona - 1));
                }
                else if (Kolona + 1 < 8 && tabla[Red, Kolona + 1] is Pijun protivnickiPijunn && protivnickiPijunn.Boja != Boja && protivnickiPijunn.PrethodniRed == 6 &&
                    protivnickiPijunn.PrethodniRed == MainWindow.GetPoslednjiPotez()[0].Item1 && protivnickiPijunn.PrethodnaKolona == MainWindow.GetPoslednjiPotez()[0].Item2)
                {
                    potezi.Add((Red + 1, Kolona + 1));
                }
            }
            return potezi;
        }
    }
}
