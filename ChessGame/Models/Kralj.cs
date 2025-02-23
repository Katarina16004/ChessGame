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
        public override List<(int, int)> MoguciPotezi(Figura[,] figura)
        {
            throw new NotImplementedException();
        }
    }
}
