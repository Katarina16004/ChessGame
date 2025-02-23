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
                if (Boja == "bela")
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
                else if (Boja == "crna")
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
            else if (Math.Abs(pomakKolona) == 1 && (Boja == "bela" && pomakRed == 1 || Boja == "crna" && pomakRed == -1))
            {
                if (tabla[novaPozicijaRed, novaPozicijaKolona] != null && tabla[novaPozicijaRed, novaPozicijaKolona].Boja != Boja)
                {
                    return true; // uzimanje protivnicke figure
                }
            }

            return false; 
        }
        public override List<(int, int)> MoguciPotezi(Figura[,] figura)
        {
            throw new NotImplementedException();
        }
    }
}
