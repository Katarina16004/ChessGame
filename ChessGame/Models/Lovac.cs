using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Models
{
    public class Lovac : Figura
    {
        public Lovac(string boja, int red, int kolona) : base(boja, red, kolona) { }

        public override bool ValidanPotez(int novaPozicijaRed, int novaPozicijaKolona, Figura[,] tabla)
        {
            int pomakRed = Math.Abs(novaPozicijaRed - Red);
            int pomakKolona = Math.Abs(novaPozicijaKolona - Kolona);

            // dijagonalno
            if (pomakRed == pomakKolona)
            {
                int korakRed = -1;
                if (novaPozicijaRed > Red)
                    korakRed = 1;

                int korakKolona = -1;
                if (novaPozicijaKolona > Kolona)
                    korakKolona = 1;

                int i = Red + korakRed;
                int j = Kolona + korakKolona;

                while (i != novaPozicijaRed && j != novaPozicijaKolona)
                {
                    if (tabla[i, j] != null)
                    {
                        return false;
                    }
                    i = i + korakRed;
                    j = j + korakKolona;
                }

                return tabla[novaPozicijaRed, novaPozicijaKolona] == null || tabla[novaPozicijaRed, novaPozicijaKolona].Boja != Boja;
            }

            return false;
        }
        public override List<(int, int)> MoguciPotezi(Figura[,] figura)
        {
            throw new NotImplementedException();
        }
    }
}
