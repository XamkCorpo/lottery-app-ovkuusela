using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Lottopeli
{
    class Lotto
    {
        public static HashSet<int> numbers;
        public static HashSet<int> randomNumbers;

        static readonly Random rnum = new Random();

        public static int voitot;
        public static int kirrokset;
        public static int cursorLeft;
        public static int cursorTop;

        static void Main()
        {
            Console.Clear();

            // Piilotetaan kursori
            ThreadStart thread = new ThreadStart(HideCursor);
            Thread thr = new Thread(thread);
            thr.Start();

            // Määritetään muuttujat
            numbers = new HashSet<int>();
            randomNumbers = new HashSet<int>();
            voitot = 0;
            kirrokset = 0;
            cursorLeft = 0;
            cursorTop = 0;

            // Aloitus
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("LOTTOPELI");
            Console.ResetColor();
            Thread.Sleep(2000);
            while (Console.KeyAvailable)
                Console.ReadKey(true);
            AddNumbers();
            Console.Clear();
            Thread.Sleep(1000); 
            while (Console.KeyAvailable)
                Console.ReadKey(true);
            Console.WriteLine("Paina mitä tahansa nappia aloittaaksesi. ");
            while (true)
            {
                if (Console.KeyAvailable) break;
            }
            Console.Clear();

            // Pelinäkymä
            string num = string.Join(" ", numbers);
            Console.WriteLine("Sinun numerot: " + num);

            Thread.Sleep(2000);
            while (Console.KeyAvailable)
                Console.ReadKey(true);

            // Looppi joka pyörii jatkuvasti kunnes pelaaja poistuu
            while (true)
            {
                kirrokset++;

                cursorLeft = Console.CursorLeft;
                cursorTop = Console.CursorTop;
                Console.WriteLine("\nVoitot: " + voitot);
                Console.WriteLine("Pelatut kierrokset: " + kirrokset);
                Console.WriteLine("\n\nPaina mitä tahansa nappia lopettaaksesi. ");
                Console.SetCursorPosition(cursorLeft, cursorTop);

                CompareNumbers();

                if (Console.KeyAvailable) break;
            }

            // Loppunäkymä
            Console.Clear();
            Console.WriteLine("Pelaa uudelleen: Y \nPoistu: N");
            while (true)
            {
                var key = Console.ReadKey(intercept: true);
                if (key.Key.Equals(ConsoleKey.Y))
                {
                    Main();
                }
                else if (key.Key.Equals(ConsoleKey.N))
                {
                    Environment.Exit(0);
                }
            }
        }

        static void HideCursor()
        {
            while (true)
            {
                Console.CursorVisible = false;
            }
        }

        // Verrataan random lukua pelaajan lukuun
        static void CompareNumbers()
        {
            AddRandomRumbers();

            var points = 0;

            foreach (var item in numbers)
            {
                if (randomNumbers.Contains(item))
                {
                    points++;
                }
            }

            string rng = string.Join(" ", randomNumbers);

            cursorLeft = Console.CursorLeft;
            cursorTop = Console.CursorTop;
            Console.WriteLine("Voittonumerot: " + rng.PadRight(30));
            Console.SetCursorPosition(cursorLeft, cursorTop);

            if (points >= 7) voitot += 1000000;
            else if (points >= 6) voitot += 10000;
            else if (points >= 5) voitot += 100;
            else if (points >= 4) voitot += 10;
            else if (points >= 3) voitot += 1;

            Thread.Sleep(50);
        }

        // Lisää random lukuja listaan
        static void AddRandomRumbers()
        {
            randomNumbers.Clear();

            for (int i = 0; i < 7; i++)
            {
                var temp = rnum.Next(1, 41);
                bool added = randomNumbers.Add(temp);
                if (!added) i--;
            }
        }

        // Lisää pelaajan luvut listaan
        static void AddNumbers()
        {
            for (int i = 0; i < 7; i++)
            {
                var temp = GetNumber();
                bool added = numbers.Add(temp);
                if (!added)
                {
                    Console.Write("\rNumero " + temp + " on jo valittu. Kirjoita toinen numero. ");
                    while (true)
                    {
                        if (Console.KeyAvailable) break;
                    }        
                    i--;
                }
            }
        }

        // Hankitaan pelaajan luku
        static int GetNumber()
        {
            Console.Clear();
            Console.WriteLine("Syötä 7 lottonumeroa (1-40) ");

            var newNum = "";

            while (true)
            {
                var pressedKey = Console.ReadKey(intercept: true);

                if (int.TryParse(pressedKey.KeyChar.ToString(), out int num))
                {
                    // Pakotetaan input olemaan luku 1-40
                    if(newNum == "" || int.Parse(newNum) < 40 && newNum.Length <= 1)
                    {
                        newNum += num;
                        Console.Write(num);
                        if(int.Parse(newNum) >= 40)
                        {
                            newNum = "40";
                            Console.Write("\r" + newNum);
                        }
                        else if (int.Parse(newNum) == 0)
                        {
                            newNum = "1";
                            Console.Write("\r"+ newNum);
                        }
                    }
                }
                // Hoitaa backspacen painamisen
                else if (pressedKey.Key.Equals(ConsoleKey.Backspace) && Console.CursorLeft > 0)
                {
                    if(newNum.Length > 0) newNum = newNum.Remove(newNum.Length-1, 1);
                    Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                    Console.Write(" ");
                    Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                }
                // Poistutaan loopista enterillä
                else if (pressedKey.Key.Equals(ConsoleKey.Enter) && newNum != "")
                {
                    break;
                }
            }

            // Muokataan input luku integeriksi
            return int.Parse(newNum);
        }
 
    }



}
