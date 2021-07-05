using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Hangman_Jan
{
    class Program
    {
        static string[] Wczytaniepliku(string path)
        {

            string[] countriesandcapitals = System.IO.File.ReadAllLines(path);
            return countriesandcapitals;
        }

        static int LosowaLinijka(int dlugosctablicy)
        {
            Random r = new Random();
            int losowalinijka = r.Next(0, dlugosctablicy);
            return losowalinijka;

        }
        static string[] Podziallinijki(string linijka)
        {
            string[] podzial = linijka.Split(" | ");
            return podzial;

        }
        static string DodanieLinijki(string imie, string date, string guessingtime, int guessingtries,string guessedword)
        {
            string linijkadopliku=imie+" | "+date+" | "+guessingtime + " | "+guessingtries + " | "+ guessedword;
            
            return linijkadopliku;
        }
        static string RysowanieWisielca(int lifepoints)
        {
            string wisielec = "";
            if (lifepoints==5)
            {
                 wisielec = "                           ";
            }
            else if (lifepoints==4)
            {
                 wisielec = @"     /            
    /              ";
            }
            else if (lifepoints == 3)
            {
                 wisielec = @"     / \           
    /   \          ";
            }
            else if (lifepoints == 2)
            {
                 wisielec = @"         |
         |           
        / \           
       /   \          ";
            }
            else if (lifepoints == 1)
            {
                 wisielec = @"          _______
          |            
          |            
         / \           
        /   \          ";
            }
            else if (lifepoints == 0)
            {
                 wisielec = @"         _______
         |      O     
         |     /|\    
        / \    / \    
       /   \          ";
            }
            return wisielec;
        }
        static void Main(string[] args)
        {

            //wczytanie pliku
            string sciezka = @"C:\Users\Admin\Desktop\projekt motorola\countries_and_capitals.txt";
            string sciezkazapisu= @"C:\Users\Admin\Desktop\projekt motorola\Results.txt";
            string[] countriesandcapitals = Wczytaniepliku(sciezka);
            if (!File.Exists(sciezkazapisu))
            {
                using StreamWriter file = File.CreateText(sciezkazapisu);
            }
            
            bool restart = true;
            while (restart==true)
            {
                string linijkadopliku = "";
                bool wygrana = false;
                int lifepoint = 5;
                int count = 0;
                string imie = "";
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                //losowanie numeru linijki
                int losowalinijka = 0;
                losowalinijka = LosowaLinijka(countriesandcapitals.Length);

                string linijka = countriesandcapitals[losowalinijka];

                //podział linijki na dwa słowa

                string[] podzial = Podziallinijki(linijka);
                //slowo kraj
                string country = podzial[0];
                //slowo stolica
                string capital = podzial[1].ToUpper();
                //stolica jako tablica ciagow znakow 
                char[] znaki = new char[capital.Length];

                //Console.WriteLine(country+'\n');
                //Console.WriteLine(capital);
                //Powitanie 
                Console.WriteLine("Hello! Let's play Hangman!");
                //wypelnienie stolicy znakiem _
                for (int i = 0; i < capital.Length; i++)
                {
                    znaki[i] = '_';
                    Console.Write(znaki[i] + " ");
                }
                //lista liter ktore są nietrafione
                List<char> not_in_word = new List<char>();
                //petla do zgadywania
                while (lifepoint > 0 | wygrana == false)
                {

                    if (lifepoint==1)
                    {
                        Console.WriteLine("Maybe this HINT help you!");
                        Console.WriteLine("The capital of " + country);
                    }

                    Console.WriteLine("would you like to guess a letter or whole word? l/w \n");
                    string decyzja = Console.ReadLine();
                    //zgadywanie pojedynczej litery
                    if (decyzja == "l")
                    {
                        Console.WriteLine("Type a letter:");
                        char letter = Console.ReadLine().ToCharArray()[0];
                        letter = Char.ToUpper(letter);
                        count = count + 1;
                        if (capital.Contains(letter) == true)
                        {

                            for (int i = 0; i < capital.Length; i++)
                            {
                                if (capital[i] == letter)
                                {
                                    znaki[i] = letter;
                                }

                            }
                            for (int j = 0; j < znaki.Length; j++)
                            {
                                Console.Write(znaki[j] + " ");
                            }
                            string rezultat = new string(znaki);
                            if (capital == rezultat)
                            {
                                stopwatch.Stop();
                                TimeSpan ts = stopwatch.Elapsed;
                                Console.WriteLine("You won! Congratulations!");
                                Console.WriteLine("You guessed the capital after " + count + " letters. It took you" + ts.ToString());
                                Console.WriteLine("You have " + lifepoint + " life points");
                                wygrana = true;
                                Console.WriteLine("Enter your name:");
                                imie = Console.ReadLine();
                                linijkadopliku = DodanieLinijki(imie, DateTime.Now.ToString(), ts.ToString(), count, capital);
                                using (StreamWriter file = File.AppendText(sciezkazapisu))
                                {
                                    file.WriteLine(linijkadopliku);
                                }
                                Console.WriteLine("Do you want to restart and play again? y/n");
                                char ponownagra = Console.ReadLine().ToCharArray()[0];
                                if (ponownagra == 'y')
                                {
                                    restart = true;
                                    break;
                                }
                                else if (ponownagra == 'n')
                                {
                                    restart = false;
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Choose one of the letters! If you want to play - y, and if you don't - n");
                                }
                            }
                        }
                        else
                        {
                            lifepoint = lifepoint - 1;
                            Console.WriteLine(RysowanieWisielca(lifepoint));
                            if (not_in_word.Contains(letter))
                            {
                                Console.WriteLine("You used these incorrect letters:");
                                not_in_word.ForEach(Console.WriteLine);
                                Console.WriteLine("You have " + lifepoint + " life points");
                            }
                            else
                            {
                                not_in_word.Add(letter);
                                Console.WriteLine("You used these incorrect letters:");
                                not_in_word.ForEach(Console.WriteLine);
                                Console.WriteLine("You have " + lifepoint + " life points");
                            }
                            if (lifepoint==0)
                            {
                                stopwatch.Stop();
                                TimeSpan ts = stopwatch.Elapsed;
                                Console.WriteLine("Unfortunately You lost!");
                                Console.WriteLine("Enter your name:");
                                imie = Console.ReadLine();
                                linijkadopliku = DodanieLinijki(imie, DateTime.Now.ToString(), ts.ToString(), count, capital);
                                using (StreamWriter file = File.AppendText(sciezkazapisu))
                                {
                                    file.WriteLine(linijkadopliku);
                                }
                                Console.WriteLine("Do you want to restart and play again? y/n");
                                char ponownagra = Console.ReadLine().ToCharArray()[0];
                                if (ponownagra == 'y')
                                {
                                    restart = true;
                                    break;
                                }
                                else if (ponownagra == 'n')
                                {
                                    restart = false;
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Choose one of the letters! If you want to play - y, and if you don't - n");
                                }
                            }

                        }

                    }
                    else if (decyzja == "w" & lifepoint >= 2)
                    {
                        Console.WriteLine("Enter solution: ");
                        string rozwiazanie = Console.ReadLine();
                        rozwiazanie = rozwiazanie.ToUpper();
                        if (capital == rozwiazanie)
                        {
                            wygrana = true;
                            stopwatch.Stop();
                            TimeSpan ts = stopwatch.Elapsed;
                            Console.WriteLine("You won! Congratulations");
                            Console.WriteLine("You guessed the capital after " + count + " letters. It took you" + ts.ToString());
                            Console.WriteLine("Enter your name:");
                            imie = Console.ReadLine();
                            linijkadopliku = DodanieLinijki(imie, DateTime.Now.ToString(), ts.ToString(), count, capital);
                            using (StreamWriter file = File.AppendText(sciezkazapisu))
                            {
                                file.WriteLine(linijkadopliku);
                            }
                            Console.WriteLine("You have " + lifepoint + " life points");
                            Console.WriteLine("Do you want to restart and play again? y/n");
                            char ponownagra = Console.ReadLine().ToCharArray()[0];
                            if (ponownagra == 'y')
                            {
                                restart = true;
                                break;
                            }
                            else if (ponownagra == 'n')
                            {
                                restart = false;
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Choose one of the letters! If you want to play - y, and if you don't - n");
                            }
                        }
                        else
                        {
                            lifepoint = lifepoint - 2;
                            Console.WriteLine(RysowanieWisielca(lifepoint));
                            Console.WriteLine("You have " + lifepoint + " life points");
                            if (lifepoint == 0)
                            {
                                stopwatch.Stop();
                                TimeSpan ts = stopwatch.Elapsed;
                                Console.WriteLine("Unfortunately You lost");
                                Console.WriteLine("Enter your name:");
                                imie = Console.ReadLine();
                                linijkadopliku = DodanieLinijki(imie, DateTime.Now.ToString(), ts.ToString(), count, capital);
                                using (StreamWriter file = File.AppendText(sciezkazapisu))
                                {
                                    file.WriteLine(linijkadopliku);
                                }
                                Console.WriteLine("Do you want to restart and play again? y/n");
                                char ponownagra = Console.ReadLine().ToCharArray()[0];
                                if (ponownagra == 'y')
                                {
                                    restart = true;
                                    break;
                                }
                                else if (ponownagra == 'n')
                                {
                                    restart = false;
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Choose one of the letters! If you want to play - y, and if you don't - n");
                                }
                            }
                            
                        }
                    }
                    else
                    {
                        Console.WriteLine("Choose l and guess a letter or w and guess a solution - You can't guess solution when You have only 1 life point!");
                    }
                }
            }

        }
    }
}
