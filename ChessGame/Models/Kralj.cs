using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

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
        public bool DaLiJeKraljUgrozen(Figura[,] tabla) // da li je kralj trenutno ugrozen
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Figura figura = tabla[i, j];
                    if (figura != null && figura.Boja != this.Boja)
                    {
                        if (figura.ValidanPotez(Red, Kolona, tabla))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool ImaLiKraljSigurnoPolje(Figura[,] tabla) // da li su okolna polja slobodna
        {
            List<(int, int)> moguciPotezi = new List<(int, int)>
            {
                (Red - 1, Kolona - 1), (Red - 1, Kolona), (Red - 1, Kolona + 1),
                (Red, Kolona - 1), (Red, Kolona + 1),
                (Red + 1, Kolona - 1), (Red + 1, Kolona), (Red + 1, Kolona + 1)
            };

            foreach (var (r, k) in moguciPotezi)
            {
                if (r >= 0 && r < 8 && k >= 0 && k < 8)
                {
                    if (tabla[r, k] == null || (tabla[r, k] != null && tabla[r, k].Boja != this.Boja))
                    {
                        int originalRed = Red;
                        int originalKolona = Kolona;
                        Red = r;
                        Kolona = k;

                        bool kraljUgrozen = DaLiJeKraljUgrozen(tabla);

                        Red = originalRed;
                        Kolona = originalKolona;

                        if (!kraljUgrozen)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool DaLiJeMat(Figura[,] tabla)
        {
            if (!DaLiJeKraljUgrozen(tabla))
                return false;
            if (ImaLiKraljSigurnoPolje(tabla))
                return false;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Figura figura = tabla[i, j];
                    if (figura != null && figura.Boja != this.Boja)
                    {
                        if (figura.ValidanPotez(Red, Kolona, tabla))
                        {
                            for (int k = 0; k < 8; k++)
                            {
                                for (int g = 0; g < 8; g++)
                                {
                                    Figura nasaFigura = tabla[k, g];
                                    if (nasaFigura != null && nasaFigura.Boja == this.Boja)
                                    {
                                        if (nasaFigura.ValidanPotez(figura.Red, figura.Kolona, tabla))
                                        {
                                            return false; // ima ko da pojede pretecu figuru, tako da nije mat
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        public bool DaLiJePat(Figura[,] tabla)
        {
            if (DaLiJeKraljUgrozen(tabla))
                return false;
            if (ImaLiKraljSigurnoPolje(tabla))
                return false;

            // da li imamo neku drugu figuru kojom moze da se igra
            foreach (var figura in tabla)
            {
                if (figura != null && figura.Boja == this.Boja) 
                {
                    if (figura.MoguciPotezi(tabla).Any() && figura.GetType().Name!="Kralj")
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
