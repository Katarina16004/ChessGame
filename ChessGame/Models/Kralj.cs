using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        private bool prviPomeraj=true;

        public override bool ValidanPotez(int novaPozicijaRed, int novaPozicijaKolona, Figura[,] tabla)
        {
            int pomakRed = Math.Abs(novaPozicijaRed - Red);
            int pomakKolona = Math.Abs(novaPozicijaKolona - Kolona);

            if (pomakRed <= 1 && pomakKolona <= 1)
            {
                if (tabla[novaPozicijaRed, novaPozicijaKolona] == null || tabla[novaPozicijaRed, novaPozicijaKolona].Boja != Boja)
                {
                    if (!DaLiJeKraljSiguran(novaPozicijaRed, novaPozicijaKolona, tabla))
                    {
                        return false; 
                    }
                    if (prviPomeraj)
                        prviPomeraj = false;
                    return true;
                }
            }
            else if(pomakKolona==2 && pomakRed==0 && prviPomeraj) //mogucnost rokade
            {
                if (Boja == "bela" && Red == 7 && Kolona == 4)
                {
                    return DaLiMozeRokada(tabla, 7);
                }
                else if (Boja == "crna" && Red == 0 && Kolona == 4)
                {
                    return DaLiMozeRokada(tabla, 0);
                }
            }
            return false;
        }
        private bool DaLiMozeRokada(Figura[,] tabla, int red)
        {
            if (DaLiJeKraljUgrozen(tabla))
                return false;

            bool malaRokada = tabla[red, 7] is Top && tabla[red, 7].Boja == Boja && 
                tabla[red, 5] == null && tabla[red, 6] == null;

            bool velikaRokada = tabla[red, 0] is Top && tabla[red, 0].Boja == Boja &&
                tabla[red, 1] == null && tabla[red, 2] == null && tabla[red, 3] == null;

            return (malaRokada && Kolona == 4 && tabla[red, 7] != null) || (velikaRokada && Kolona == 4 && tabla[red, 0] != null);
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
                        if (!DaLiJeKraljSiguran(noviRed, novaKolona, tabla))
                        {
                            continue; 
                        }
                        potezi.Add((noviRed, novaKolona));
                    }
                }
            }
            //mogucnost rokade
            if (DaLiMozeRokada(tabla, Red) && prviPomeraj)
            {
                if (tabla[Red, 7] is Top && tabla[Red, 5] == null && tabla[Red, 6] == null) // mala
                {
                    potezi.Add((Red, 6));
                }
                if (tabla[Red, 0] is Top && tabla[Red, 1] == null && tabla[Red, 2] == null && tabla[Red, 3] == null) //velika
                {
                    potezi.Add((Red, 2));
                }
            }

            return potezi;
        }
        private bool DaLiJeKraljSiguran(int novaPozicijaRed, int novaPozicijaKolona, Figura[,] tabla) //na potencijalnom mestu
        {
            // privremeno premestimo kralja
            Figura orgFigura = tabla[novaPozicijaRed, novaPozicijaKolona];
            int orgRed = Red;
            int orgKol = Kolona;
            tabla[Red, Kolona] = null;
            tabla[novaPozicijaRed, novaPozicijaKolona] = this;
            tabla[novaPozicijaRed, novaPozicijaKolona].Red = novaPozicijaRed;
            tabla[novaPozicijaRed, novaPozicijaKolona].Kolona = novaPozicijaKolona;

            bool kraljUgrozen = DaLiJeKraljUgrozen(tabla);

            // vratimo stanje table
            tabla[novaPozicijaRed, novaPozicijaKolona] = orgFigura;
            tabla[orgRed, orgKol] = this;
            tabla[orgRed, orgKol].Red = orgRed;
            tabla[orgRed, orgKol].Kolona = orgKol;

            return !kraljUgrozen;
        }
        public bool DaLiJeKraljUgrozen(Figura[,] tabla) // da li je kralj trenutno ugrozen
        {
            List<Figura> preteceFigure = new List<Figura>(); 

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Figura figura = tabla[i, j];
                    if (figura != null && figura.Boja != this.Boja) 
                    {
                        if (figura.ValidanPotez(Red, Kolona, tabla))
                        {
                            preteceFigure.Add(figura);

                            if (Boja == "bela")
                            {
                                MainWindow.SetPretecaFigura(0, figura);
                            }
                            else
                            {
                                MainWindow.SetPretecaFigura(1, figura);
                            }
                        }
                    }
                }
            }

            int brojPretecihFigura = preteceFigure.Count; 

            if (brojPretecihFigura > 0)
            {
                if (Boja == "bela")
                {
                    MainWindow.SetUgrozenKralj(0, brojPretecihFigura);
                }
                else
                {
                    MainWindow.SetUgrozenKralj(1, brojPretecihFigura);
                }

                return true; //bar jedna ga ugrozava
            }
            else
            {
                if (Boja == "bela")
                {
                    MainWindow.SetUgrozenKralj(0, 0);
                    MainWindow.SetPretecaFigura(0, null);
                }
                else
                {
                    MainWindow.SetUgrozenKralj(1, 0);
                    MainWindow.SetPretecaFigura(1, null);
                }

                return false; //nije ugrozen
            }
        }


        public bool ImaLiKraljSigurnoPolje(Figura[,] tabla) // da li su okolna polja slobodna
        {
            foreach (var (r, k) in MoguciPotezi(tabla))
            {
                if (r >= 0 && r < 8 && k >= 0 && k < 8)
                {
                    if (tabla[r, k] == null || (tabla[r,k]!=null && tabla[r, k].Boja != this.Boja && !DaLiJeZasticenaProtFigura(tabla[r,k],tabla))) //slobodno polje ima ukoliko je prazno ili moze da pojede tu figuru
                    {
                        if (DaLiJeKraljSiguran(r, k, tabla))
                            return true;
                    }
                }
            }
            return false;
        }
        public bool DaLiJeZasticenaProtFigura(Figura figura, Figura[,] tabla)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Figura saveznik = tabla[i,j];
                    if(saveznik!=null && saveznik!=figura && saveznik.Boja==figura.Boja)
                    {
                        if (saveznik.ValidanPotez(figura.Red, figura.Kolona, tabla))
                            return true;
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

            int brojNapadaca = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Figura figura = tabla[i, j];
                    if (figura != null && figura.Boja != this.Boja)
                    {
                        if (figura.ValidanPotez(Red, Kolona, tabla))
                        {
                            brojNapadaca++;
                        }
                    }
                }
            }
            if (brojNapadaca >= 2)
                return true;

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
                                    if (nasaFigura != null && nasaFigura.Boja == this.Boja && nasaFigura.GetType().Name!="Kralj")
                                    {
                                        if (nasaFigura.ValidanPotez(figura.Red, figura.Kolona, tabla) && !DaLiJeZasticenaProtFigura(figura,tabla))
                                        {
                                            //MessageBox.Show($"{nasaFigura} moze da pojede {figura}");
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
                        //MessageBox.Show($"Mozemo odigrati {Boja} {figura}");
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
