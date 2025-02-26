using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            string bojaSlike;
            if (Boja == "bela")
            {
                bojaSlike = "beli";
            }
            else
            {
                bojaSlike = "crni";
            }
            string imeFigure = this.GetType().Name.ToLower();
            return $"Resources/Figure/{bojaSlike}_{imeFigure}.png";
        }
        public abstract bool ValidanPotez(int novaPozicijaRed, int novaPozicijaKolona, Figura[,] tabla);
        public abstract List<(int, int)> MoguciPotezi(Figura[,] tabla);
    }
}
