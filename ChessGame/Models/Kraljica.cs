using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Models
{
    public class Kraljica : Figura
    {
        public Kraljica(string boja, int red, int kolona) : base(boja, red, kolona) { }

        public override bool ValidanPotez(int novaPozicijaRed, int novaPozicijaKolona, Figura[,] tabla)
        {
            int pomakRed = Math.Abs(novaPozicijaRed - Red);
            int pomakKolona = Math.Abs(novaPozicijaKolona - Kolona);

            if (pomakRed == 0 || pomakKolona == 0 || pomakRed == pomakKolona)
            {
                int korakRed = 0, korakKolona = 0;

                if (pomakRed != 0)
                {
                    if (novaPozicijaRed > Red)
                        korakRed = 1;
                    else
                        korakRed = -1;
                }

                if (pomakKolona != 0)
                {
                    if (novaPozicijaKolona > Kolona)
                        korakKolona = 1;
                    else
                        korakKolona = -1;
                }

                int i = Red + korakRed;
                int j = Kolona + korakKolona;

                // dijagonalno
                while ((pomakRed != 0 && i != novaPozicijaRed) || (pomakKolona != 0 && j != novaPozicijaKolona))
                {
                    if (tabla[i, j] != null)
                    {
                        return false;
                    }

                    if (pomakRed != 0)
                    {
                        i = i + korakRed;
                    }
                    if (pomakKolona != 0)
                    {
                        j = j + korakKolona;
                    }
                }

                return tabla[novaPozicijaRed, novaPozicijaKolona] == null || tabla[novaPozicijaRed, novaPozicijaKolona].Boja != Boja;
            }

            return false;
        }

        public override List<(int, int)> MoguciPotezi(Figura[,] tabla)  // miks lovca i topa
        {
            List<(int, int)> potezi = new List<(int, int)>();

            Top top = new Top(Boja, Red, Kolona);
            potezi.AddRange(top.MoguciPotezi(tabla));

            Lovac lovac = new Lovac(Boja, Red, Kolona);
            potezi.AddRange(lovac.MoguciPotezi(tabla));

            return potezi;
        }
    }
}
