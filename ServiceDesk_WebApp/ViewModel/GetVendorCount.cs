namespace ServiceDesk_WebApp.ViewModel
{
    public class GetVendorCount
    {
        public int TodayCount { get; set; }
        public int YesterDayCount { get; set; }
        public int Last7DaysCount { get; set; }
        public int Last30DaysCount { get; set; }
        public int Last90DaysCount { get; set; }
    }
}
