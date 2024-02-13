using DocumentFormat.OpenXml.Wordprocessing;

namespace Superstore.ViewModels.Charts
{
    /// <summary>
    /// Viewmodel for charts
    /// Note region is typically note used in the chart but used as a filter
    /// </summary>
    public class SeriesData
    {
        public SeriesData()
        {
                
        }
        public SeriesData(string name, string region, List<LineChartData> data)
        {
            Name = name;
            Data = data;
            Region = region;
        }
        public string Name { get; set; } = "";
        public string XName { get; set; } = "XValue";
        public string YName { get; set; } = "YValue";

        /// <summary>
        /// Used as a filter
        /// </summary>
        public string Region { get; set; } = default!;
        public List<LineChartData> Data { get; set; } = new List<LineChartData>();

      

        public string RegionName
        {
            get { return $"{Region}:{Name}"; }
          
        }


    }
    public class LineChartData
    {
        public LineChartData()
        {
            
        }
        public LineChartData(string xValue, double yValue)
        {
            XValue = xValue;
            YValue = yValue;
        }

        public string XValue { get; set; } = default!;

        public double YValue { get; set; }
    }

}
