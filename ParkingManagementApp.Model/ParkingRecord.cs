namespace ParkingManagementApp.Model
{
    public class ParkingRecord : BaseModel
    {
        public long Id { get; set; }
        public string TagNumber { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public decimal HourlyFee { get; set; }
        public decimal? TotalAmount { get; set; }
    }
}
