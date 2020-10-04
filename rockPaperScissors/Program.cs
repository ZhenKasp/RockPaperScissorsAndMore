using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace rockPaperScissors
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length >= 2 && args.Length % 2 != 0 && argsNotRepeat(args))
            {
                
                rockPaperScissors(args);
            }
            else
            {
                Console.WriteLine("Wrong params");
            }
        }

        static void rockPaperScissors(string[] args)
        {

            string pcMove = pcRandomMove(args);
            string key = HMAC(args, pcMove);
            Console.WriteLine("Available moves:");
            for (int i = 0; i < args.Length; i++)
            {
                Console.WriteLine(i + 1 + " - " + args[i]);
            }
            Console.WriteLine("0 - exit");
            navigationTurn(args, key, pcMove);
            Console.ReadLine();
        }

        static bool argsNotRepeat(string[] args)
        {
            string[] notRepeatArgs = args.Distinct().ToArray();
            if (notRepeatArgs.Length == args.Length)
            {
                return true;
            }
            return false;
        }

        static string HMAC(string[] args, string pcMove) 
        {
            string key = createRandomNumber();
            string hmac = GetHash(pcMove, key); 
            Console.WriteLine("HMAC: " + hmac);
            return key;
        }

        static void navigationTurn(string[] args, string key, string pcMove)
        {
            Console.WriteLine();
            Console.Write("Enter your move: ");
            string playerMove = Console.ReadLine();
            int userMove;
            if (!int.TryParse(playerMove, out userMove))
            {
                Console.WriteLine("\"{0}\" is not an integer", playerMove);
                Console.WriteLine();
                rockPaperScissors(args);
            }
            if (userMove <= args.Length && userMove > 0)
            {
                Console.WriteLine("Your move: " + args[userMove-1]);
                Console.WriteLine("Computer move: " + args[int.Parse(pcMove)]);
                printResult(args, pcMove, userMove);
                Console.WriteLine("HMAC key: " + key);
                Console.WriteLine("----------------------------------------------------------------------");
                Console.WriteLine();
                rockPaperScissors(args);
            }
            else if (userMove == 0)
            {
                Console.WriteLine("Exiting");
                exit();
            }
            else
            {
                Console.WriteLine("Wrong input");
                Console.WriteLine();
                rockPaperScissors(args);
                exit();
            }
        }

        static void exit()
        {
            Environment.Exit(0);
        }


        public static String GetHash(String text, String key)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();

            Byte[] textBytes = encoding.GetBytes(text);
            Byte[] keyBytes = encoding.GetBytes(key);

            Byte[] hashBytes;

            using (HMACSHA256 hash = new HMACSHA256(keyBytes))
                hashBytes = hash.ComputeHash(textBytes);

            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        static string createRandomNumber()
        {
            RandomNumberGenerator rng = RNGCryptoServiceProvider.Create();

            byte[] key = new byte[14];
            rng.GetBytes(key);
            string hmacKey = BitConverter.ToString(key).Replace("-", "");
            //Console.WriteLine(hmacKey);
            return hmacKey;
        }

        static string pcRandomMove(string[] args)
        {
            Random rnd = new Random();
            int pcMove = rnd.Next(0, args.Length);
            return pcMove.ToString();
        }

        static void printResult(string[] args, string pcMove, int userMove) 
        {
            int pc = int.Parse(pcMove) + 1;
            int user = userMove;
            int len = args.Length;
            if (user == pc)
                Console.WriteLine("Draw.");
            else if ((user - pc + len) % len <= len / 2)
                Console.WriteLine("You win!");
            else
               Console.WriteLine("You lost.");
        }
    }
}
