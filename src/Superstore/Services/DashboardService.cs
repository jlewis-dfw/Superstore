using DocumentFormat.OpenXml.Spreadsheet;
using Superstore.Helpers.Contants;
using Superstore.Models;
using Superstore.ViewModels;
using Superstore.ViewModels.Charts;
using Syncfusion.Blazor.Data;
using System.Globalization;

namespace Superstore.Services
{

	public interface IDashboardService
	{
		ViewModels.Dashboard GetDashboardViewModel(StandardLookupFilters filters = null);
		PieChart GetPieChartSalesByRegion(StandardLookupFilters filters = null, IEnumerable<CsvData> data = null);
		PieChart GetPieChartSalesBySeason(StandardLookupFilters filters = null, IEnumerable<CsvData> data = null);
		PieChart GetPieChartSalesByCategory(StandardLookupFilters filters = null, IEnumerable<CsvData> data = null);
		LineChart GetLineSalesByCategoryRegionSeason(StandardLookupFilters filters = null, IEnumerable<CsvData> data = null);
		StoreSummary GetStoreSummary(StandardLookupFilters filters = null, IEnumerable<CsvData> data = null);
		Task<StoreSummary> GetStoreSummaryAsync(StandardLookupFilters filters = null);

	}
	public class DashboardService : IDashboardService
	{
		private readonly ICsvDataService csvDataService;
		private readonly ISuperstoreCache superstoreCache;

		public DashboardService(ICsvDataService csvDataService, ISuperstoreCache superstoreCache)
		{
			this.csvDataService = csvDataService;
			this.superstoreCache = superstoreCache;
		}





		public Dashboard GetDashboardViewModel(StandardLookupFilters filters = null)
		{
			if (filters == null)
			{
				var data = superstoreCache.GetCahceDataByType<Dashboard>(CacheKeys.Dashboard);
				if (data != null)
				{
					if (!data.IsFiltered)
					{
						return data;
					}
				}
			}

			var standardLookupDataSet = csvDataService.GetStandardLookupDataSet();

			var csvData = csvDataService.GetCsvImport(filters);
			var dashboard = new Dashboard()
			{
				StandardLookupDataSet = standardLookupDataSet,
				PieChartSalesByRegion = GetPieChartSalesByRegion(data: csvData),
				PieChartSalesBySeason = GetPieChartSalesBySeason(data: csvData),
				StoreSummary = GetStoreSummary(data: csvData),
				LineChartSalesByCategoryRegionSeason = GetLineSalesByCategoryRegionSeason(data: csvData),
				PieChartSalesByCategory = GetPieChartSalesByCategory(data: csvData),
				
			};


			if(filters == null)
			{
				dashboard.Filters = new StandardLookupFilters()
				{
					CategoriesFilter = standardLookupDataSet.Categories.Select(c => c.Name).ToArray(),
					RegionsFilter = standardLookupDataSet.Regions.Select(c => c.Name).ToArray(),
					SeasonsFilter = standardLookupDataSet.Seasons.Select(c => c.Name).ToArray()
				};
			}
			else
			{
				dashboard.Filters = new StandardLookupFilters()
				{
					CategoriesFilter = filters.CategoriesFilter,
					RegionsFilter = filters.RegionsFilter,
					SeasonsFilter = filters.SeasonsFilter
				};
			}


			superstoreCache.AddObjectToCache(CacheKeys.Dashboard, dashboard, 30);
			return dashboard;
		}



		public LineChart GetLineSalesByCategoryRegionSeason(StandardLookupFilters filters = null, IEnumerable<CsvData> data = null)
		{
			if (data == null)
				data = csvDataService.GetCsvImport();

			var salesByCat = RegionSales(data).OrderBy(x=>x.Season.Id);
			var seriedDataSet = new List<SeriesData>();
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


					if (lineChartData != null && lineChartData.Any())
					{
						seriedDataSet.Add(new SeriesData($"{region}: {category}", StandardLookupFields.Regions[r], lineChartData));
					}
				}
			}
			var lineChart = new LineChart()
			{
				Title = string.Empty,
				TooltipFormat = "\"<b>${series.name} : ${point.y}</b>\"",
				TooltipEnable = false,
				SeriesDataSet = seriedDataSet
			};

			lineChart.SuperstoreDropDown = new ViewModels.DropDowns.SuperstoreDropDownVm()
			{
				BindVales = StandardLookupFields.Regions,
				StandardLookups = StandardLookupFields.Regions.Select(r => new StandardLookup
				{
					Id = r,
					Name = r
				}),

			};
			return lineChart;
		}

		public RegionSalesByCategoryByTimeCategoryViewModel RegionSalesByCategoryByTimeCategoryViewModel(StandardLookupFilters filters = null, IEnumerable<CsvData> data = null)
		{
			if (data == null)
				data = csvDataService.GetCsvImport();
			var regionSalesByCategoryByTimeCategoryViewModel = new RegionSalesByCategoryByTimeCategoryViewModel();
			var salesByCat = RegionSales(data).OrderBy(x => x.Season.Id);
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


		private IEnumerable<RegionSalesByCategoryByTimeCategory> RegionSales(IEnumerable<CsvData> data)
		{

			List<RegionSalesByCategoryByTimeCategory> regionSalesByCategoryByTimeCategories = new List<RegionSalesByCategoryByTimeCategory>();
			var x = data.GroupByMany(d => d.Region, d => d.Season, d => d.Category).ToList();
			var regions = data.DistinctBy(c => c.Region).Select(c => c.Region).ToArray();
			var seasons = Season.GetSeasons().OrderBy(s=>s.Id).ToList();
			var quarter = data.DistinctBy(c => c.YearQuarter).Select(c => c.YearQuarter).ToArray();
			foreach (var region in regions)
			{
				var regionData = data.Where(s => s.Region == region).ToList();
				foreach (var season in seasons)
				{
					var seasonData = regionData.Where(s => s.Season == season.Name).ToList();

					regionSalesByCategoryByTimeCategories.AddRange(seasonData.GroupBy(d => d.Category)
						   .Select(h => new RegionSalesByCategoryByTimeCategory
						   {
							   Category = h.Key,
							   Region = region,
							   Season = season,
							   TimeCategory = season.Name,
							   Total = (double)h.Sum(s => s.Sales)
						   }));
				}
			}

			return regionSalesByCategoryByTimeCategories;
		}



		public PieChart GetPieChartSalesByRegion(StandardLookupFilters filters = null, IEnumerable<CsvData> data = null)
		{
			if (data == null)
				data = csvDataService.GetCsvImport();
			var salesSum = data.Sum(x => x.Sales);
			var x = data.GroupBy(d => d.Region)
			.Select(h => new RegionSalesByCategory
			{
				Region = h.Key,
				Total = h.Sum(s => s.Sales),
				Percent = GetPercent(salesSum, h.Sum(s => s.Sales), 2)
			});
			var chartDataSet = x.Select(h => new ChartData
			{
				Name = h.Region,
				XValue = h.Region,
				YValue = (double)h.Total,
				DataLabelMappingName = $"{h.Region}: {h.Percent}% {h.Total.ToString("C", new CultureInfo("en-US"))}"
			}).ToList();
			var pieChart = new PieChart()
			{
				Title = "Sales By Region",
				TooltipFormat = "<b>${point.x}</b><br>Sales By Region: <b>${point.y}</b>",
				TooltipEnable = false
			};

			pieChart.SeriesDataSet.Add(new SeriesData("DS1", null, chartDataSet));

			return pieChart;
		}



		public PieChart GetPieChartSalesByCategory(StandardLookupFilters filters = null, IEnumerable<CsvData> data = null)
		{

			if (data == null)
				data = csvDataService.GetCsvImport();
			var salesSum = data.Sum(x => x.Sales);
			var x = data.GroupBy(d => d.Category)
			.Select(h => new RegionSalesByCategory
			{
				Category = h.Key,
				Total = h.Sum(s => s.Sales),
				Percent = GetPercent(salesSum, h.Sum(s => s.Sales), 2)
			});

			var chartDataSet = x.Select(h => new ChartData
			{
				Name = h.Season,
				XValue = h.Season,
				YValue = (double)h.Total,
				DataLabelMappingName = $"{h.Category}: {h.Percent}% {h.Total.ToString("C", new CultureInfo("en-US"))}"
			}).ToList();
			var pieChart = new PieChart()
			{
				Title = String.Empty,
				TooltipFormat = "<b>${point.x}</b><br>Sales By Category: <b>${point.y}</b>",
				TooltipEnable = false
			};

			pieChart.SeriesDataSet.Add(new SeriesData("DS1", null, chartDataSet));

			return pieChart;
		}




		public PieChart GetPieChartSalesBySeason(StandardLookupFilters filters = null, IEnumerable<CsvData> data = null)
		{
			if (data == null)
				data = csvDataService.GetCsvImport();
			var salesSum = data.Sum(x => x.Sales);
			var x = data.GroupBy(d => d.Season)
			.Select(h => new RegionSalesByCategory
			{
				Season = h.Key,
				Total = h.Sum(s => s.Sales),
				Percent = GetPercent(salesSum, h.Sum(s => s.Sales), 2)
			});

			var chartDataSet = x.Select(h => new ChartData
			{
				Name = h.Season,
				XValue = h.Season,
				YValue = (double)h.Total,
				DataLabelMappingName = $"{h.Season}: {h.Percent}% {h.Total.ToString("C", new CultureInfo("en-US"))}"
			}).ToList();
			var pieChart = new PieChart()
			{
				Title = string.Empty,
				TooltipFormat = "<b>${point.x}</b><br>Sales By Season: <b>${point.y}</b>",
				TooltipEnable = false
			};

			pieChart.SeriesDataSet.Add(new SeriesData("DS1", null, chartDataSet));

			return pieChart;
		}

		public async Task<StoreSummary> GetStoreSummaryAsync(StandardLookupFilters filters = null)
		{
			var data = await csvDataService.GetCsvImportAsync(new CancellationToken());
			return await Task.Run(() => GetStoreSummary(filters, data));
		}

		public StoreSummary GetStoreSummary(StandardLookupFilters filters = null, IEnumerable<CsvData> data = null)
		{
			if (data == null)
				data = csvDataService.GetCsvImport();
			StoreSummary supersalesSummary = new StoreSummary();
			supersalesSummary.TotalProfit = data.Sum(x => x.Profit).ToString("C", new CultureInfo("en-US"));
			supersalesSummary.TotalSales = data.Sum(x => x.Sales).ToString("C", new CultureInfo("en-US"));
			supersalesSummary.UniqueCustomers = data.DistinctBy(r => r.CustomerID).Count();
			supersalesSummary.FirstOrderDate = data.Min(x => x.OrderDate).ToString("MM/dd/yyyy");
			supersalesSummary.LastOrderDate = data.Max(x => x.OrderDate).ToString("MM/dd/yyyy");
			supersalesSummary.TotalOrders = data.DistinctBy(r => r.OrderDate).Count();
			supersalesSummary.TotalItemsShipped = data.Sum(x => x.Quantity);
			return supersalesSummary;
		}


		private decimal GetPercent(decimal sum, decimal part, int decimalPlaces)
		{
			return Math.Round(((part / sum) * 100), decimalPlaces);
		}

	}
}
