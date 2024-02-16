using DocumentFormat.OpenXml.Wordprocessing;
using Superstore.ViewModels.DropDowns;
using Syncfusion.Blazor.Charts;


namespace Superstore.ViewModels.Charts
{

    public class ChartMetadata
    {
        public string Height { get; set; } = "100%";
		public string Width { get; set; } = "100%";
		public string ComponentHeader { get; set; } = "Title";
        public string Title { get; set; } = "Title";
        public List<SeriesData> SeriesDataSet { get; set; } = new List<SeriesData>();
        public bool TooltipEnable { get; set; } = true;
		public string TooltipHeader { get; set; } = "TooltipHeader";
		public bool ChartLegendVisible { get; set; } = true;
      
        

        private SeriesData _primarySeriesData;

        public SeriesData PrimarySeriesData
        {
            get
            {
                if (_primarySeriesData == null)
                {
                    if (SeriesDataSet.Any())
                    {
                        _primarySeriesData = SeriesDataSet.First();
                    }
                    else
                    {
                        _primarySeriesData = new SeriesData();
                    }
                }
                return _primarySeriesData;
            }
            set { _primarySeriesData = value; }
        }


        public SeriesData SetPrimaryChartDataByName(string name)
        {
            var da = SeriesDataSet.Where(d => d.Name == name).FirstOrDefault();
            if (da != null)
            {
                _primarySeriesData = da;
                return _primarySeriesData;
            }
            throw new Exception($"Could not find SeriesData: {name}");

        }
    }


    public class PieChart : ChartMetadata
    {

        public double ExplodeIndex { get; set; } = 1;
        public bool Explode { get; set; } = true;
        public string ExplodeRadius = "10%";
        public string ExplodeOffset = "10%";
        public string ExplodeInnerRadius = "0%";
        public string Radius { get; set; } = "60%";
        public string ConnectorLength { get; set; } = "20px";
        public int StartAngle = 30;
        public string Size { get; set; } = "12px";
        public bool EnableBorderOnMouseMove { get; set; } = false;
        public bool EnableAnimation { get; set; } = true;
        public string AccumulationChartConnector { get; set; } = "20px";
        public string AccumulationChartDataLabelFontSize { get; set; } = "12px";
        public string AccumulationChartDataLabelFontWeight { get; set; } = "600";
        public bool EnableSmartLabels { get; set; } = false;
        public string TooltipFormat { get; set; } = "<b>${point.x}</b><br>Data: <b>${point.y}%</b>";


    }

    public class LineChart : ChartMetadata
    {

        public double ExplodeIndex { get; set; } = 1;
        public bool Explode { get; set; } = true;
        public string ExplodeRadius = "10%";
        public string ExplodeOffset = "10%";
        public string ExplodeInnerRadius = "0%";
        public string Radius { get; set; } = "60%";
        public string ConnectorLength { get; set; } = "20px";
        public int StartAngle = 30;
        public string Size { get; set; } = "12px";
        public bool EnableBorderOnMouseMove { get; set; } = false;
        public bool EnableAnimation { get; set; } = true;
        public string AccumulationChartConnector { get; set; } = "20px";
        public string AccumulationChartDataLabelFontSize { get; set; } = "12px";
        public string AccumulationChartDataLabelFontWeight { get; set; } = "600";
        public string TooltipFormat { get; set; } = "<b>${series.name} : ${point.y}</b>";

		public SuperstoreDropDownVm SuperstoreDropDown { get; set; }


	}


    /// <summary>
    /// Viewmodel for charts
    /// Note region is typically note used in the chart but used as a filter
    /// </summary>
    public class SeriesData
    {
        public SeriesData()
        {
                
        }
        public SeriesData(string name, string region, List<ChartData> data)
        {
            Name = name;
            Data = data;
            Region = region;
        }
        public string Name { get; set; } = String.Empty;
        public string XName { get; set; } = "XValue";
        public string YName { get; set; } = "YValue";

        /// <summary>
        /// Used as a filter
        /// </summary>
        public string Region { get; set; } = default!;
        public List<ChartData> Data { get; set; } = new List<ChartData>();

        public bool ChartMarkerVisible { get; set; } = true;
        public int ChartMarkerHeight { get; set; } = 10;
        public int ChartMarkerWidth { get; set; } = 10;
        //private ChartData _primaryChartData;

        //public ChartData PrimaryChartData
        //{
        //    get 
        //    { 
        //        if(_primaryChartData == null)
        //        {
        //            if(Data.Any())
        //            {
        //                _primaryChartData = Data.First();
        //            }
        //            _primaryChartData = new ChartData();
        //        }
        //        return _primaryChartData; 
        //    }
        //    set { _primaryChartData = value; }
        //}

        public string RegionName
        {
            get { return $"{Region}:{Name}"; }
          
        }

    }

    public class ChartData
    {
        public ChartData()
        {
            
        }
        public ChartData(string xValue, double yValue, string name = null, string nameAppend = null)
        {
            XValue = xValue;
            YValue = yValue;
            Name = name;

            if(name != null) 
            {
                Name = $"{xValue}: {yValue}";
                if(nameAppend != null) Name = $"{Name}{nameAppend}"; 
            }
        }
        public string Name { get; set; } = String.Empty;
        public string XValue { get; set; } = String.Empty;
        public double YValue { get; set; }
        public string DataLabelMappingName { get; set; }
    }

}
