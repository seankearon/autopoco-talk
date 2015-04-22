using System.Diagnostics;
using System.IO;
using CsvHelper;

namespace NVM.AutoPoco
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var file = Path.GetTempFileName() + ".csv";
            using (var textWriter = File.CreateText(file))
            {
                var csv = new CsvWriter(textWriter);
                csv.WriteRecords(TestData.Items);
            }
            Process.Start(file);
        }
    }
}