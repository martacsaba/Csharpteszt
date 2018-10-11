using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assynchron
{
     class Program
    {
        static void Main(string[] args)
        {
            ThreadTeszt();

            Console.ReadKey();
            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }

        public delegate DateTime muvelet(int ciklus, string uzenet);

        private static void ThreadTeszt()
        {
            muvelet am = (ciklus, uzenet) => 
            {
                Console.WriteLine("--> Művelet kezd");
                for (int i = 1; i <= ciklus; i++)
                {
                    Console.WriteLine("---> {0} ciklus",i);
                }
                Console.WriteLine("--> Művelet vége");
                return DateTime.Now;
            };

            AsyncCallback Callback = ar =>
            {
                var eredmeny = am.EndInvoke(ar);
                Console.WriteLine("Hetedik példa (asszinkron indítás 07) végzett, eredmény: {0}", eredmeny);
            };

            var ar07 = am.BeginInvoke(5, "Hetedik példa (asszinkron indítás 07)",Callback, null);
        }
    }
}
