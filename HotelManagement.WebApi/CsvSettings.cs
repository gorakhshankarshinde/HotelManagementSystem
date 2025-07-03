namespace HotelManagement.WebApi
{
    public class CsvSettings
    {
        public bool RunAsLocally { get; set; }

        public string LocalMenuItemCsvPath { get; set; }
        public string ProdMenuItemCsvPath { get; set; }

        public string LocalOrderCsvPath { get; set; }
        public string ProdOrderCsvPath { get; set; }
    }

}
