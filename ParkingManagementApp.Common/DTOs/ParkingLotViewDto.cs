namespace ParkingManagementApp.Common.DTOs
{
    public class ParkingLotViewDto
    {
        public long Id { get; set; }
        public string TagNumber { get; set; }
        public DateTime CheckInTime { get; set; }
        public decimal HourlyFee { get; set; }
    }
}
