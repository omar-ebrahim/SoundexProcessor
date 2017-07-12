using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundexProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = new List<Tuple<int, string, string, DateTime>>();
            list.Add(new Tuple<int, string, string, DateTime>(1, "mark", "jacobs", new DateTime(2000, 1, 1)));
            list.Add(new Tuple<int, string, string, DateTime>(3, "david", "hollister", new DateTime(2000, 1, 1)));
            list.Add(new Tuple<int, string, string, DateTime>(2, "marc", "jacobson", new DateTime(2000, 1, 1)));
            list.Add(new Tuple<int, string, string, DateTime>(3, "dave", "hollister", new DateTime(2000, 1, 1)));

            var processor = new Processor();
            var result = processor.Process(list);

            foreach (var item in result)
            {
                Console.WriteLine(item.Key);

                foreach (var person in item.Value)
                {
                    var id = person.Item1;
                    var forenames = person.Item2;
                    var surname = person.Item3;
                    var forenamesSoundex = person.Item4;
                    var surnameSoundex = person.Item5;
                    var dob = person.Item6;
                    Console.WriteLine($"{id}: {forenames} {surname} ({forenamesSoundex} {surnameSoundex}). DOB: {dob.ToString("dd/MM/yyyy")}");
                }
            }

            Console.ReadLine();
        }
    }
}
