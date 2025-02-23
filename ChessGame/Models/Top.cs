using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Models
{
    public class Top:Figura
    {
        public Top(string boja, int red, int kolona) : base(boja, red, kolona) { }

        public override bool ValidanPotez(int novaPozicijaRed, int novaPozicijaKolona, Figura[,] tabla)
        {
            int pomakRed = novaPozicijaRed - Red;
            int pomakKolona = novaPozicijaKolona - Kolona;

            if (pomakKolona == 0)
            {
                // vertikalno
                int korak = -1;   //dole
                
                if(pomakRed>0)
                {
                    korak = 1;   //gore
                }
                for (int i = Red + korak; i != novaPozicijaRed; i += korak)
                {
                    if (tabla[i, Kolona] != null)
                    {
                        return false; // ako je na putu do tog polja figura, potez nije validan
                    }
                }
                //ne moze da preskoci tu figuru, ali moze da je pojede, ukoliko mu je to ciljano polje
                return tabla[novaPozicijaRed, novaPozicijaKolona] == null || tabla[novaPozicijaRed, novaPozicijaKolona].Boja != Boja;
            }
            else if (pomakRed == 0)
            {
                // horizontalno
                int korak = -1;  //levo

                if (pomakKolona > 0)
                {
                    korak = 1;   //desno
                }
                for (int i = Kolona + korak; i != novaPozicijaKolona; i += korak)
                {
                    if (tabla[Red, i] != null)
                    {
                        return false;
                    }
                }
                return tabla[novaPozicijaRed, novaPozicijaKolona] == null || tabla[novaPozicijaRed, novaPozicijaKolona].Boja != Boja;
            }

            return false;  
        }
    }
}
