using ParkingManagementApp.Core.Respositories;

namespace ParkingManagementApp.Core.Interfaces
{
    public interface IUnitOfWork
    {
        // Repositories
        IParkingRecordRepository ParkingRecordRepository { get; }

    }

}
