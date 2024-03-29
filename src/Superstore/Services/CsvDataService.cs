﻿using CsvHelper;
using DocumentFormat.OpenXml.Spreadsheet;
using Superstore.Helpers.Contants;
using Superstore.Mappers;
using Superstore.Models;
using Superstore.ViewModels;
using Superstore.ViewModels.Charts;
using Syncfusion.Blazor.Data;
using System.Collections.Generic;
using System.Globalization;

namespace Superstore.Services
{
    public interface ICsvDataService
    {

        /// <summary>
        /// Async: Reads CSV file and stores to cache.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<CsvData>> GetCsvImportAsync(CancellationToken cancellationToken, StandardLookupFilters filters = null);


        /// <summary>
        ///  Reads CSV file and stores to cache.
        /// </summary>
        /// <returns></returns>
        IEnumerable<CsvData> GetCsvImport(StandardLookupFilters filters = null);

		


		/// <summary>
		/// Returns sum of sales by region
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <param name="seasonsFilter">Optional filter: Season</param>
		/// <returns></returns>
		Task<IEnumerable<RegionSalesByCategory>> RegionSalesByCategories(CancellationToken cancellationToken, string[]? seasonsFilter = null);

        /// <summary>
        /// Returns distinct vales of Region, Category, and Season.  
        /// Can use const strings from Helpers.Contants.StandardLookupFields
        /// </summary>
        /// <param name="field"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>StandardLookups based on field param</returns>
        Task<List<StandardLookup>> GetStandardLookupsAsync(string field, CancellationToken cancellationToken);


		List<StandardLookup> GetStandardLookups(string field);

        StandardLookupDataSet GetStandardLookupDataSet();

		Task<IEnumerable<RegionSalesByCategoryByTimeCategory>> RegionSales(CancellationToken cancellationToken);

        /// <summary>
        /// View model for SalesLineChart. Containining ChartSeries and RegionSales
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<RegionSalesByCategoryByTimeCategoryViewModel> RegionSalesByCategoryByTimeCategoryViewModel(CancellationToken cancellationToken);
    }

    public class CsvDataService : ICsvDataService
    {
        private readonly ISuperstoreCache superstoreCache;

        public CsvDataService(ISuperstoreCache superstoreCache)
        {
            this.superstoreCache = superstoreCache;
        }





        public async Task<IEnumerable<CsvData>> GetCsvImportAsync(CancellationToken cancellationToken, StandardLookupFilters filters = null)
        {

            return await Task.Run(() => GetCsvImport());
        }

		private  IEnumerable<CsvData> GetFiltedCsvData(IEnumerable<CsvData> data, StandardLookupFilters filters)
		{
            if(data == null) data = GetCsvImport();

			var filtereddata = data;
            var standardLookupDataSet = GetStandardLookupDataSet();
			if (filters.SeasonsFilter != null && filters.SeasonsFilter.Length != standardLookupDataSet.Seasons.Count())
            {
                filtereddata = filtereddata.Where(s => filters.SeasonsFilter.Contains(s.Season));
			}
			if (filters.RegionsFilter != null && filters.RegionsFilter.Length != standardLookupDataSet.Regions.Count())
			{
				filtereddata = filtereddata.Where(s => filters.RegionsFilter.Contains(s.Region));
			}
			if (filters.CategoriesFilter != null && filters.CategoriesFilter.Length != standardLookupDataSet.Categories.Count())
			{
				filtereddata = filtereddata.Where(s => filters.CategoriesFilter.Contains(s.Category));
			}
            return filtereddata.ToList();
		}


		public IEnumerable<CsvData> GetCsvImport(StandardLookupFilters filters = null)
        {
            var data = superstoreCache.GetCahceDataByType<List<CsvData>>(CacheKeys.CsvData);
            if (data != null && filters == null)
            {
                return data;
            }
            else if(data != null && filters != null)
            {
                return GetFiltedCsvData(data, filters);
			}
            else
            {
                List<CsvData> csvDataSet = new List<CsvData>();
                var path = Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles\\Superstore.csv");
                using (var reader = new StreamReader(path))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<CsvDataMap>();
                    csvDataSet = csv.GetRecords<CsvData>().ToList();
                }
                csvDataSet.Where(c => c.OrderDate.Month == 12 || c.OrderDate.Month == 1 || c.OrderDate.Month == 2).SetValue(c => c.Season = "Winter").ToList();
                csvDataSet.Where(c => c.OrderDate.Month == 3 || c.OrderDate.Month == 4 || c.OrderDate.Month == 5).SetValue(c => c.Season = "Spring").ToList();
                csvDataSet.Where(c => c.OrderDate.Month == 6 || c.OrderDate.Month == 7 || c.OrderDate.Month == 8).SetValue(c => c.Season = "Summer").ToList();
                csvDataSet.Where(c => c.OrderDate.Month == 9 || c.OrderDate.Month == 10 || c.OrderDate.Month == 11).SetValue(c => c.Season = "Autumn").ToList();
                csvDataSet.SetValue(c => c.YearQuarter = $"{c.OrderDate.Year}Q{Math.Abs(c.OrderDate.Month / 3) + 1}").ToList();

                superstoreCache.AddObjectToCache(CacheKeys.CsvData, csvDataSet, 30);

                if(filters != null)
                {
					return GetFiltedCsvData(data, filters);
				}
                return csvDataSet;
            }
        }
        

        public StandardLookupDataSet GetStandardLookupDataSet()
        {
			var data = superstoreCache.GetCahceDataByType<StandardLookupDataSet>(CacheKeys.StandardLookupDataSet);
			if (data != null)
			{
				return data;
			}


			var dataSet = new StandardLookupDataSet()
            {

                Categories = GetStandardLookups(Helpers.Contants.StandardLookupFields.Category),
                Regions = GetStandardLookups(Helpers.Contants.StandardLookupFields.Region),
                Seasons = GetStandardLookups(Helpers.Contants.StandardLookupFields.Season)
            };
			superstoreCache.AddObjectToCache(CacheKeys.StandardLookupDataSet, dataSet, 30);
			return dataSet;
        }
            
		public List<StandardLookup> GetStandardLookups(string field)
		{
			var data =  GetCsvImport();
			switch (field)
			{
				case StandardLookupFields.Season:

					return data.DistinctBy(c => c.Season).Select(c => new StandardLookup
					{
						Id = c.Season,
						Name = c.Season
					}).ToList();
				case StandardLookupFields.Region:
					return data.DistinctBy(c => c.Region).Select(c => new StandardLookup
					{
						Id = c.Region,
						Name = c.Region
					}).ToList();
				case StandardLookupFields.Category:
					return data.DistinctBy(c => c.Category).Select(c => new StandardLookup
					{
						Id = c.Category,
						Name = c.Category
					}).ToList();
				default:
					return new List<StandardLookup>();
			}
		}


		public async Task<List<StandardLookup>> GetStandardLookupsAsync(string field, CancellationToken cancellationToken)
        {
            var data = await GetCsvImportAsync(cancellationToken);
            switch (field)
            {
                case StandardLookupFields.Season:

                    return data.DistinctBy(c => c.Season).Select(c => new StandardLookup
                    {
                        Id = c.Season,
                        Name = c.Season
                    }).ToList();
                case StandardLookupFields.Region:
                    return data.DistinctBy(c => c.Region).Select(c => new StandardLookup
                    {
                        Id = c.Region,
                        Name = c.Region
                    }).ToList();
				case StandardLookupFields.Category:
					return data.DistinctBy(c => c.Category).Select(c => new StandardLookup
					{
						Id = c.Category,
						Name = c.Category
					}).ToList();
				default:
                    return new List<StandardLookup>();
            }
        }

        public async Task<RegionSalesByCategoryByTimeCategoryViewModel> RegionSalesByCategoryByTimeCategoryViewModel(CancellationToken cancellationToken)
        {

            var regionSalesByCategoryByTimeCategoryViewModel = new RegionSalesByCategoryByTimeCategoryViewModel();
            var salesByCat = await RegionSales(cancellationToken);
            regionSalesByCategoryByTimeCategoryViewModel.RegionSalesByCategoryByTimeCategories = salesByCat.ToList();
            for (int r = 0; r < StandardLookupFields.Regions.Length; r++)
            {
                var region = StandardLookupFields.Regions[r];
                for (int c = 0; c < StandardLookupFields.Categories.Length; c++)
                {
                    var category = StandardLookupFields.Categories[c];
                    var lineChartData = salesByCat.Where(s => s.Region == region && s.Category == category)
                    .Select(r => new ChartData
                    {
                        XValue = r.TimeCategory,
                        YValue = r.Total,
                    }).ToList();
                    regionSalesByCategoryByTimeCategoryViewModel.SeriesCollection.Add(new SeriesData(StandardLookupFields.Categories[c], StandardLookupFields.Regions[r], lineChartData));

                }
            }
            return regionSalesByCategoryByTimeCategoryViewModel;
        }


        public async Task<IEnumerable<RegionSalesByCategory>> RegionSalesByCategories(CancellationToken cancellationToken, string[]? seasonsFilter = null)
        {
            var data = await GetCsvImportAsync(cancellationToken);

            if (seasonsFilter != null && seasonsFilter.Length > 0)
            {
                data = data.Where(x => seasonsFilter.Contains(x.Season)).ToList();
            }

            return
            data.GroupBy(d => d.Region)
            .Select(h => new RegionSalesByCategory
            {
                Region = h.Key,
                Total = h.Sum(s => s.Sales),
                Furniture = h.Where(x => x.Category == "Furniture").Sum(s => s.Sales),
                OfficeSupplies = h.Where(x => x.Category == "Office Supplies").Sum(s => s.Sales),
                Technology = h.Where(x => x.Category == "Technology").Sum(s => s.Sales)
            });
        }
        public async Task<IEnumerable<RegionSalesByCategoryByTimeCategory>> RegionSales(CancellationToken cancellationToken)
        {
            var data = await GetCsvImportAsync(cancellationToken);
            return await Task.Run(() => RegionSales(data, cancellationToken));
        }


        private IEnumerable<RegionSalesByCategoryByTimeCategory> RegionSales(IEnumerable<CsvData> data, CancellationToken cancellationToken)
        {

            List<RegionSalesByCategoryByTimeCategory> regionSalesByCategoryByTimeCategories = new List<RegionSalesByCategoryByTimeCategory>();
            var x = data.GroupByMany(d => d.Region, d => d.Season, d => d.Category).ToList();
            var regions = data.DistinctBy(c => c.Region).Select(c => c.Region).ToArray();
            var seasons = data.DistinctBy(c => c.Season).Select(c => c.Season).ToArray();
            var quarter = data.DistinctBy(c => c.YearQuarter).Select(c => c.YearQuarter).ToArray();
            foreach (var region in regions)
            {
                var regionData = data.Where(s => s.Region == region).ToList();
                foreach (var season in seasons)
                {
                    var seasonData = regionData.Where(s => s.Season == season).ToList();

                    regionSalesByCategoryByTimeCategories.AddRange(seasonData.GroupBy(d => d.Category)
                           .Select(h => new RegionSalesByCategoryByTimeCategory
                           {
                               Category = h.Key,
                               Region = region,
                               TimeCategory = season,
                               Total = (double)h.Sum(s => s.Sales)
                           }));
                }
            }

            return regionSalesByCategoryByTimeCategories;
        }
    }
}







