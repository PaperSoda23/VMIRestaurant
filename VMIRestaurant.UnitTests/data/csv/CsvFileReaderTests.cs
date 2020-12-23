using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using VMIRestaurant.data.csv;
using Xunit;

namespace VMIRestaurant.UnitTests.data.csv
{
    public class CsvFileReaderTests
    {
        private static readonly string TestFileDirectory = GetTestDataDirectory();
        
        private static string GetTestDataDirectory()
        {
            var rootTestProjectDirectory = Path.GetDirectoryName(Assembly
                .GetExecutingAssembly().Location.Replace(@"bin\Debug\netcoreapp3.1", string.Empty));
            
            var dataDirectory = Path.Join(rootTestProjectDirectory, @"\data\csv\data");

            return dataDirectory;
        }
        
        [Fact]
        public async Task errors_when_file_is_not_found()
        {
            var ex = await Assert.ThrowsAsync<ArgumentException>(
                () => CsvFileReader.ReadData(TestFileDirectory, "random.csv")
            );
            
            Assert.Contains("file random.csv does not exist in directory", ex.Message);
        }
        
        [Fact]
        public async Task errors_when_file_is_not_of_type_csv()
        {
            var ex = await Assert.ThrowsAsync<ArgumentException>(
                () => CsvFileReader.ReadData(TestFileDirectory, "file.txt")
            );
            
            Assert.Equal("file file.txt is not a csv file", ex.Message);
        }

        [Fact]
        public async Task reads_csv_file()
        {
            var data = await CsvFileReader.ReadData(TestFileDirectory, "file.csv");
            
            Assert.Equal(2, data.Count);
            Assert.Equal(data[0], new List<string>{"hello", "world"});
            Assert.Equal(data[1], new List<string>{"bye", "world"});
        }
    }
}