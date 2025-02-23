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

        public Figura(string boja, int red, int kolona)
        {
            Boja = boja;
            Red = red;
            Kolona = kolona;
        }
        public abstract bool ValidanPotez(int novaPozicijaRed, int novaPozicijaKolona, Figura[,] tabla);
    }
}
