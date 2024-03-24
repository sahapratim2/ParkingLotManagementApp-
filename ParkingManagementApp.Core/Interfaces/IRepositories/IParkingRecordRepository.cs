using ParkingManagementApp.Common.DTOs;
using ParkingManagementApp.Model;

namespace ParkingManagementApp.Core.Respositories
{
    public interface IParkingRecordRepository
    {
        public Task<IEnumerable<ParkingRecord>> GetAllCars();
        public Task<IEnumerable<ParkingLotViewDto>> GetParkingCars();
        public Task<ParkingCarDto?> GetCurrentParkingByTagNumber(string tagNumber);
        public Task<int> GetAvailableParkingSpot();
        public Task<int> CheckInCar(CarTagNumber car);
        public Task<ParkingCarDto> CheckOutCar(ParkingCarDto car);
        public int GetTotalParkingSpot();
        public int GetAvailableParkingSpot(int allocatedSpot);
        public decimal GetHourlyFee();
        public Task<IEnumerable<ParkingStatsDto>> GetParkingStats();

    }
}
