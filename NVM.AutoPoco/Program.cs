using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;

namespace NVM.AutoPoco
{
    internal class Program
    {
        private static void Main(string[] args)
        {
/*
            var file = Path.GetTempFileName() + ".csv";
            using (var textWriter = File.CreateText(file))
            {
                var csv = new CsvWriter(textWriter);
                csv.WriteRecords(TestData.Items);
            }
*/
            var file = Path.GetTempFileName() + ".txt";
            File.WriteAllText(file, JsonConvert.SerializeObject(TestData.Items, Formatting.Indented));
            Process.Start(file);
        }
    }
}