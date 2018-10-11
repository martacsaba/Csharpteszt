using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SerializeDeserialize
{
    class Program
    {
        static void StreamTszt()
        {
            var adat = new AdatosztalySerialize()
            {
                Egesz = int.MaxValue,
                TizedesTort = 12.72m,
                Datum = DateTime.MaxValue,
                DatumMin = DateTime.MinValue,
                Szoveg = "Árvíztűűrőtkörfúrógép"
            };

            string fileName = "teszt.txt";
            var serializer = new XmlSerializer(typeof(AdatosztalySerialize));
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                serializer.Serialize(fs, adat);
            }

            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                var beolvasott = serializer.Deserialize(fs);
                Console.WriteLine(JsonConvert.SerializeObject(beolvasott));
            }

        } 
        static void Main(string[] args)
        {
            //StreamTszt();
            LambdaKifjezesek();
            Console.ReadKey();
        }

        public delegate int NegyzetreEmel(int x);

        private static void LambdaKifjezesek()
        {
            NegyzetreEmel negyzet = (x) => { return x * x; };
            negyzet = z => z * z;
            Console.WriteLine(negyzet(2));
            // Action és func delegat-ek
            // Action<> visszatérés nélküli (void) delegate definicióra szolgál
            // Func<> visszatérési értékkel rendelkező delegate létrehozása
            Func<int, int> negyzetreemel2 = x => x * x;
            Console.WriteLine(negyzetreemel2(3));
            Func<int, int, string> szorzat = (x, y) => (x * y).ToString();
        }
    }

    [Serializable]
    public class AdatosztalySerialize
    {
        public int Egesz { get; set; }
        public decimal TizedesTort { get; set; }
        public DateTime Datum { get; set; }
        public DateTime DatumMin { get; set; }
        public string Szoveg { get; set; }
    }
}
