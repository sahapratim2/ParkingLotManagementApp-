using Microsoft.Extensions.Configuration;
using ParkingManagementApp.Core.Interfaces;
using ParkingManagementApp.Data;

namespace ParkingManagementApp.Core.Respositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDatabaseManager _databaseManager;
        private readonly IConfiguration _configuration;

        public UnitOfWork(IDatabaseManager databaseManager,IConfiguration configuration)
        {
            _databaseManager = databaseManager;
            _configuration = configuration;
        }
        public IParkingRecordRepository ParkingRecordRepository => new ParkingRecordRepository(_databaseManager,_configuration);

    }
}
