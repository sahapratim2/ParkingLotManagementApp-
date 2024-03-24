using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ParkingManagementApp.Common.DataConstants;
using ParkingManagementApp.Common.DTOs;
using ParkingManagementApp.Common.Utilities;
using ParkingManagementApp.Data;
using ParkingManagementApp.Model;

namespace ParkingManagementApp.Core.Respositories
{
    public class ParkingRecordRepository : IParkingRecordRepository
    {
        private readonly IDatabaseManager _databaseManager;

        private readonly IConfiguration _configuration;

        public ParkingRecordRepository(IDatabaseManager databaseManager, IConfiguration configuration)
        {
            _databaseManager = databaseManager;

            _configuration = configuration;

        }

        public async Task<IEnumerable<ParkingRecord>> GetAllCars()
        {
            var parkingRecords = new List<ParkingRecord>();

            using (SqlDataReader reader = await _databaseManager.ExecuteReaderAsync("GetParkingRecord", null))
            {
                parkingRecords = Utilities.DataReaderMapToList<ParkingRecord>(reader);

                reader.Close();

            }
            return parkingRecords;
        }

        public async Task<IEnumerable<ParkingLotViewDto>> GetParkingCars()
        {
            var parkingCars = new List<ParkingLotViewDto>();

            using (SqlDataReader reader = await _databaseManager.ExecuteReaderAsync("GetCurrentParking", null))
            {
                parkingCars = Utilities.DataReaderMapToList<ParkingLotViewDto>(reader);

                reader.Close();
            }
            return parkingCars;
        }

        public async Task<ParkingCarDto?> GetCurrentParkingByTagNumber(string tagNumber)
        {
            var parkingCars = new List<ParkingLotViewDto>();


            var paramList = new List<SqlParameter>
            { 
                 new SqlParameter("TagNumber", tagNumber)
            };

            using (SqlDataReader reader = await _databaseManager.ExecuteReaderAsync("GetCurrentParkingByTagNumber", paramList))
            {
                parkingCars = Utilities.DataReaderMapToList<ParkingLotViewDto>(reader);

                reader.Close();
            }
            if (parkingCars.Count == 0)
            {
                return null;
            }
            var parkingCar = parkingCars.First();

            return new ParkingCarDto
            {
                Id = parkingCar.Id,
                TagNumber = parkingCar.TagNumber,
                CheckInTime = parkingCar.CheckInTime,
                HourlyFee = parkingCar.HourlyFee
            };
        }

        public async Task<int> GetAvailableParkingSpot()
        {
            var currentReserve = await _databaseManager.ExecuteScalarAsync<int>("select count(*) CurrentReserve  from ParkingRecord Where CheckOutTime is  NULL", null, CommandType.Text);

            return Convert.ToInt32(_configuration[AppSettingConstants.DOMAIN_CONSTANTS.TOTAL_PARKING_SPOTS]) - currentReserve;

        }

        public async Task<int> CheckInCar(CarTagNumber car)
        {

            var paramList = new List<SqlParameter>
            {
                new SqlParameter("TagNumber", car.TagNumber),
                new SqlParameter("CheckInTime", DateTime.UtcNow),
                new SqlParameter("HourlyFee", Convert.ToDecimal(_configuration[AppSettingConstants.DOMAIN_CONSTANTS.HOURLY_FEE])),
                new SqlParameter("AddedBy", CommonConstants.DAFULT_CONSTANTS.USER),// Have to change it. Now it is using for testing purpose
                new SqlParameter("AddedDate", DateTime.UtcNow),
                new SqlParameter("AddedIP", CommonConstants.DAFULT_CONSTANTS.USERIP)// Have to change it. Now it is using for testing purpose
            };
     
            return await _databaseManager.ExecuteNonQueryAsync("InsertParkingRecord", paramList);
        }

        public async Task<ParkingCarDto> CheckOutCar(ParkingCarDto car)
        {
            car.CheckOutTime = DateTime.UtcNow;

            TimeSpan difference = (TimeSpan)(car.CheckOutTime- car.CheckInTime);

            int elapsedMinute = (int)difference.TotalMinutes;

            car.TotalAmount = GetTotalAmount(elapsedMinute, car.HourlyFee);

            var paramList = new List<SqlParameter>
            {
                new SqlParameter("TagNumber", car.TagNumber),
                new SqlParameter("CheckOutTime", car.CheckOutTime),
                new SqlParameter("TotalAmount", car.TotalAmount),
                new SqlParameter("UpdatedBy", CommonConstants.DAFULT_CONSTANTS.USER),// Have to change it. Now it is using for testing purpose
                new SqlParameter("UpdatedDate", DateTime.UtcNow),
                new SqlParameter("UpdatedIP", CommonConstants.DAFULT_CONSTANTS.USERIP)// Have to change it. Now it is using for testing purpose
            };

            await _databaseManager.ExecuteNonQueryAsync("UpdateParkingRecord", paramList);

            return car;
        }

        private static decimal GetTotalAmount(int elapsedTime, decimal hourlyFee)
        {
            if (elapsedTime >= 2 && elapsedTime <= 60)
            {

                return hourlyFee;
            }
            else
            {
                decimal totalTimeInHours = (decimal)elapsedTime / 60;

                return hourlyFee * Math.Ceiling(totalTimeInHours);
            }
        }

        public int GetTotalParkingSpot()
        {
            return Convert.ToInt32(_configuration[AppSettingConstants.DOMAIN_CONSTANTS.TOTAL_PARKING_SPOTS]);
        }

        public int GetAvailableParkingSpot(int allocatedSpot)
        {
            return GetTotalParkingSpot() - allocatedSpot;
        }

        public decimal GetHourlyFee()
        {
            return Convert.ToDecimal(_configuration[AppSettingConstants.DOMAIN_CONSTANTS.HOURLY_FEE]);
        }

        public async Task<IEnumerable<ParkingStatsDto>> GetParkingStats()
        {
            var parkingStats = new List<ParkingStatsDto>();

            var paramList = new List<SqlParameter>
            {
                 new SqlParameter("TotalSpots", GetTotalParkingSpot())
            };

            using (SqlDataReader reader = await _databaseManager.ExecuteReaderAsync("GetParkingStats", paramList))
            {
                parkingStats = Utilities.DataReaderMapToList<ParkingStatsDto>(reader);

                reader.Close();
            }

            return parkingStats;
        }
    }
}
