using CsvHelper;
using Superstore.Helpers.Contants;
using Superstore.Mappers;
using Superstore.Models;
using System.Globalization;

namespace Superstore.Services
{
    public interface ICsvDataService
    {
        Task<IEnumerable<CsvData>> GetCsvImportAsync(CancellationToken cancellationToken);
    }

    public class CsvDataService : ICsvDataService
    {
        private readonly ISuperstoreCache superstoreCache;
    
        public CsvDataService(ISuperstoreCache superstoreCache)
        {
            this.superstoreCache = superstoreCache;
        }
        public async Task<IEnumerable<CsvData>> GetCsvImportAsync(CancellationToken cancellationToken)
        {
            var data = superstoreCache.GetCahceDataByType<List<CsvData>>(CacheKeys.CsvData);
            if (data != null)
            {
                return data;
            }
            else
            {
                List<CsvData> csvDataSet = new List<CsvData>();
                var path = Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles\\Superstore.csv");
                using (var reader = new StreamReader(path))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<CsvDataMap>();
                    csvDataSet = await csv.GetRecordsAsync<CsvData>().ToListAsync();
                }
                csvDataSet.Where(c => c.OrderDate.Month == 12 || c.OrderDate.Month == 1 || c.OrderDate.Month == 2).SetValue(c => c.Season = "Winter").ToList();
                csvDataSet.Where(c => c.OrderDate.Month == 3 || c.OrderDate.Month == 4 || c.OrderDate.Month == 5).SetValue(c => c.Season = "Spring").ToList();
                csvDataSet.Where(c => c.OrderDate.Month == 6 || c.OrderDate.Month == 7 || c.OrderDate.Month == 8).SetValue(c => c.Season = "Summer").ToList();
                csvDataSet.Where(c => c.OrderDate.Month == 9 || c.OrderDate.Month == 10 || c.OrderDate.Month == 11).SetValue(c => c.Season = "Autumn").ToList();
                csvDataSet.SetValue(c => c.YearQuarter = $"{c.OrderDate.Year}Q{Math.Abs(c.OrderDate.Month / 3) + 1}").ToList();

                superstoreCache.AddObjectToCache(CacheKeys.CsvData, csvDataSet, 30);
                return csvDataSet;
            }

        }

    }

}




