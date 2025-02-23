using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Models
{
    public class Konj:Figura
    {
        public Konj(string boja, int red, int kolona) : base(boja, red, kolona) { }

        public override bool ValidanPotez(int novaPozicijaRed, int novaPozicijaKolona, Figura[,] tabla)
        {
            
            int pomakRed = Math.Abs(novaPozicijaRed - Red);
            int pomakKolona = Math.Abs(novaPozicijaKolona - Kolona);

            if ((pomakRed == 2 && pomakKolona == 1) || (pomakRed == 1 && pomakKolona == 2))
            {
                return tabla[novaPozicijaRed, novaPozicijaKolona] == null || tabla[novaPozicijaRed, novaPozicijaKolona].Boja != Boja;
            }

            return false;
        }
        public override List<(int, int)> MoguciPotezi(Figura[,] tabla) //vraca nam polja koja cemo u mainu da obojimo
        {
            List<(int, int)> potezi = new List<(int, int)>();
            int[] pomaciRed = { -2, -2, -1, -1, 1, 1, 2, 2 };
            int[] pomaciKolona = { -1, 1, -2, 2, -2, 2, -1, 1 };

            for (int i = 0; i < 8; i++)
            {
                int noviRed = Red + pomaciRed[i];
                int novaKolona = Kolona + pomaciKolona[i];

                if (noviRed >= 0 && noviRed < 8 && novaKolona >= 0 && novaKolona < 8)
                {
                    if (ValidanPotez(noviRed, novaKolona, tabla))
                    {
                        potezi.Add((noviRed, novaKolona));
                    }
                }
            }
            return potezi;
        }
    }
}
