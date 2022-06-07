using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labatron
{
    class Program
    {
        public static string format
        {
            get
            {
                return "0.00";
            }
        }

        [STAThread]
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    Reaction reaction = new Reaction();
                }
                catch
                {
                    Console.Clear();
                    Console.WriteLine("Sorry, try again");
                    Console.ReadKey();
                }
            }
        }
    }
}


















