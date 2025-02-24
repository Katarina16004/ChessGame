using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Models
{
    public class Kralj : Figura
    {
        public Kralj(string boja, int red, int kolona) : base(boja, red, kolona) { }

        public override bool ValidanPotez(int novaPozicijaRed, int novaPozicijaKolona, Figura[,] tabla)
        {
            int pomakRed = Math.Abs(novaPozicijaRed - Red);
            int pomakKolona = Math.Abs(novaPozicijaKolona - Kolona);

            if (pomakRed <= 1 && pomakKolona <= 1)
            {
                if (tabla[novaPozicijaRed, novaPozicijaKolona] == null || tabla[novaPozicijaRed, novaPozicijaKolona].Boja != Boja)
                {
                    return true;
                }
            }

            return false;
        }
        public override List<(int, int)> MoguciPotezi(Figura[,] tabla)
        {
            List<(int, int)> potezi = new List<(int, int)>();

            int[] pomaciRed = { -1, 0, 1, -1, 1, -1, 0, 1 };
            int[] pomaciKolona = { -1, -1, -1, 0, 0, 1, 1, 1 };

            for (int i = 0; i < 8; i++)
            {
                int noviRed = Red + pomaciRed[i];
                int novaKolona = Kolona + pomaciKolona[i];

                if (noviRed >= 0 && noviRed < 8 && novaKolona >= 0 && novaKolona < 8)
                {
                    if (tabla[noviRed, novaKolona] == null || tabla[noviRed, novaKolona].Boja != Boja)
                    {
                        potezi.Add((noviRed, novaKolona));
                    }
                }
            }

            return potezi;
        }
    }
}
