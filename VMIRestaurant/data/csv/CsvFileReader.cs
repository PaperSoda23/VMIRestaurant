using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace VMIRestaurant.data.csv
{
    public static class CsvFileReader
    {
        public static async Task<List<List<string>>> ReadData(string directory, string fileName)
        {
            var fullPath = Path.Combine(directory, fileName);

            if (!File.Exists(fullPath))
                throw new ArgumentException($"file {fileName} does not exist in directory {directory}");
            
            if (!fileName.EndsWith(".csv"))
                throw new ArgumentException($"file {fileName} is not a csv file");
            
            using var reader = new StreamReader(fullPath);
            var lines = new List<List<string>>();
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                var values = line?.Split(',').ToList();
                lines.Add(values);
            }

            return lines;
        }
    }
}