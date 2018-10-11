using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace assinkronFile
{
    class Program
    {
        static int pufferMeret = 90000;
        static byte[] puffer = new byte[pufferMeret];

        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            // FileKezelesNemSzepModon();
            // FileKezeles2();
            FileKezeles3();
            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }

        private static void FileKezeles3()
        {
            string filenev = "teszt.txt";
            using (var fscreate = new FileStream(filenev, FileMode.Create))
            {
                fscreate.SetLength(1000000);
            }
            var fs = new FileStream(filenev, FileMode.Open);

            fs.BeginRead(puffer, 0, pufferMeret, CallbackFv, fs);
        }

        private static void CallbackFv(IAsyncResult ar)
        {
            var fs = (FileStream)ar.AsyncState;
            var olvasottByteok = fs.EndRead(ar);
            if (olvasottByteok > 0)
            { // még nem végeztünk
                Console.WriteLine("Beolvasott byte-ok {0}, pozício {1}", olvasottByteok, fs.Position);
                fs.BeginRead(puffer, 0, pufferMeret, CallbackFv, fs);
            }
            else
            {// Végeztünk
                Console.WriteLine("Állomány végére értünk!");
                fs.Dispose();
            }
        }

        private static void FileKezeles2()
        {
            string filenev = "teszt.txt";
            using (var fscreate = new FileStream(filenev, FileMode.Create))
            {
                fscreate.SetLength(1000000);
            }
            var fs = new FileStream(filenev, FileMode.Open);           
            int pufferMeret = 90000;
            byte[] puffer = new byte[pufferMeret];
            AsyncCallback rekurzivcallback = null;
            AsyncCallback callback = ar =>
            {
                var olvasottByteok = fs.EndRead(ar);
                if (olvasottByteok > 0)
                { // még nem végeztünk
                    Console.WriteLine("Beolvasott byte-ok {0}, pozício {1}", olvasottByteok, fs.Position);
                    fs.BeginRead(puffer, 0, pufferMeret, rekurzivcallback, null);
                }
                else
                {// Végeztünk
                    Console.WriteLine("Állomány végére értünk!");
                    fs.Dispose();
                }
            };
            rekurzivcallback = callback;
           
            fs.BeginRead(puffer, 0, pufferMeret, callback, null);            
        }



        private static void FileKezelesNemSzepModon()
        {
            string filenev = "teszt.txt";
            using (var fs = new FileStream(filenev, FileMode.Create))
            {
                fs.SetLength(1000000);
            }

            var mre = new ManualResetEvent(false);

            using (var fs = new FileStream(filenev, FileMode.Open))
            {
                int pufferMeret = 90000;
                byte[] puffer = new byte[pufferMeret];
                AsyncCallback rekurzivcallback = null;
                AsyncCallback callback = ar =>
                {
                    var olvasottByteok = fs.EndRead(ar);
                    if (olvasottByteok > 0)
                    { // még nem végeztünk
                        Console.WriteLine("Beolvasott byte-ok {0}, pozício {1}", olvasottByteok, fs.Position);
                        fs.BeginRead(puffer, 0, pufferMeret, rekurzivcallback, null);
                    }
                    else
                    {// Végeztünk
                        Console.WriteLine("Végeztünk!");
                        mre.Set();
                        Console.WriteLine("Szemafor beállítva");
                    }
                };
                rekurzivcallback = callback;
                fs.BeginRead(puffer, 0, pufferMeret, callback, null);

                Console.WriteLine("Szemaforra várunk");
                mre.WaitOne();
                Console.WriteLine("Szemafor zöld");
            }

        }
    }
}
