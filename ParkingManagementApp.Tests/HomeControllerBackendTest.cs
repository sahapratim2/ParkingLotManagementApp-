using Microsoft.AspNetCore.Mvc;
using Moq;
using ParkingManagementApp.Common.DTOs;
using ParkingManagementApp.Controllers;
using ParkingManagementApp.Core.Interfaces;
using ParkingManagementApp.Core.Respositories;

namespace ParkingManagementApp.Tests
{
    public class HomeControllerBackendTest
    {
        private readonly Mock<IParkingRecordRepository> mockParkingRecordRepository = new Mock<IParkingRecordRepository>();

        private readonly Mock<IUnitOfWork>  mockUnitOfWork = new Mock<IUnitOfWork>();

        private const string tagNumber = "A7";

        private readonly CarTagNumber carTagNumber = new CarTagNumber { TagNumber = tagNumber };

        [Fact]
        public async Task Index_Should_Return_View_With_Model()
        {
    
            var parkingLotViewDtoToReturn = new List<ParkingLotViewDto>
        {
            new ParkingLotViewDto { Id = 1, TagNumber = "A1", CheckInTime = DateTime.Now.AddHours(-2)},
            new ParkingLotViewDto { Id = 2, TagNumber = "A2", CheckInTime = DateTime.Now.AddHours(-1)}
        };

            mockParkingRecordRepository.Setup(repo => repo.GetParkingCars())
                                       .ReturnsAsync(parkingLotViewDtoToReturn);

            var totalSpots = 15;

            var hourlyFee = 15.0M;

            mockParkingRecordRepository.Setup(repo => repo.GetTotalParkingSpot())
                                       .Returns(totalSpots);

            mockParkingRecordRepository.Setup(repo => repo.GetHourlyFee())
                                       .Returns(hourlyFee);

            var spotsTaken = parkingLotViewDtoToReturn.Count;

            var availableSpots = totalSpots - spotsTaken;

            mockParkingRecordRepository.Setup(repo => repo.GetAvailableParkingSpot(spotsTaken))
                                     .Returns(availableSpots);

            var viewBagToReturn = new Dictionary<string, object>
        {
            { "TotalSpots", totalSpots },
            { "HourlyFee", hourlyFee },
            { "SpotsTaken", spotsTaken },
            { "AvailableSpots", availableSpots }
        };

            mockUnitOfWork.Setup(uow => uow.ParkingRecordRepository)
                        .Returns(mockParkingRecordRepository.Object);


            var controller = new HomeController(mockUnitOfWork.Object);

            var result = await controller.Index() as ViewResult;
     
            Assert.NotNull(result);

            Assert.NotNull(result.Model); 

            var model = Assert.IsType<List<ParkingLotViewDto>>(result.Model);

            Assert.Equal(parkingLotViewDtoToReturn.Count, model.Count); 

            Assert.NotNull(controller.ViewBag); 

            var returnedViewBag = (IDictionary<string, object>)controller.ViewData;

            // Compare ViewBag values
            foreach (var viewBag in viewBagToReturn)
            {
                returnedViewBag.TryGetValue(viewBag.Key, out object viewBagValue);

                Assert.Equal(viewBag.Value, viewBagValue);
            }
        }

        [Fact]
        public async Task GetCar_Should_Return_OK_With_TagNumber()
        {

            var parkingCarDtoToReturn = new ParkingCarDto
            {
                Id = 1,
                TagNumber = tagNumber,
                CheckInTime = DateTime.Now
            };

            var carTagNumber = new CarTagNumber { TagNumber = tagNumber };


            mockParkingRecordRepository.Setup(repo => repo.GetCurrentParkingByTagNumber(carTagNumber.TagNumber))
                                       .ReturnsAsync(parkingCarDtoToReturn);


            mockUnitOfWork.Setup(uow => uow.ParkingRecordRepository)
                          .Returns(mockParkingRecordRepository.Object);

            var controller = new HomeController(mockUnitOfWork.Object);

            var result = await controller.GetCar(carTagNumber) as OkObjectResult;

            var returnedParkingCarDto = new ParkingCarDto();

            if (result != null)
            {
                returnedParkingCarDto = result.Value as ParkingCarDto;
            }

            Assert.NotNull(result);

            Assert.Equal(200, result.StatusCode);

            Assert.Equal(parkingCarDtoToReturn.TagNumber, returnedParkingCarDto.TagNumber);
        }

        [Fact]
        public async Task ParkingCars_Should_Return_Ok_With_ParkingCars()
        {
            var parkingLotViewDto = new ParkingLotViewDto
            {
                Id = 1,
                TagNumber = tagNumber,
                CheckInTime = DateTime.Now
            };

            var carTagNumber = new CarTagNumber { TagNumber = tagNumber };

            var parkingLotViewDtoToReturn = new List<ParkingLotViewDto> { parkingLotViewDto };


            mockParkingRecordRepository.Setup(repo => repo.GetParkingCars())
                                    .ReturnsAsync(parkingLotViewDtoToReturn);

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork.Setup(uow => uow.ParkingRecordRepository)
                          .Returns(mockParkingRecordRepository.Object);

            var controller = new HomeController(mockUnitOfWork.Object);

            var result = await controller.ParkingCars() as OkObjectResult;

            var returnedParkingLotViewDto = result.Value as List<ParkingLotViewDto>;

            Assert.NotNull(result);

            Assert.Equal(200, result.StatusCode);

            Assert.IsAssignableFrom<IEnumerable<ParkingLotViewDto>>(result.Value);

            Assert.Equal(parkingLotViewDtoToReturn.Count, returnedParkingLotViewDto.Count);

            Assert.Equal(parkingLotViewDtoToReturn[0].TagNumber, returnedParkingLotViewDto[0].TagNumber);

        }

        [Fact]
        public async Task CheckIn_Should_Return_BadRequest_If_Car_Already_Parked()
        {
            var parkingCarDtoToReturn = new ParkingCarDto
            {
                Id = 1,
                TagNumber = tagNumber,
                CheckInTime = DateTime.Now
            };

            var carTagNumber = new CarTagNumber { TagNumber = tagNumber };


            mockParkingRecordRepository.Setup(repo => repo.GetCurrentParkingByTagNumber(tagNumber))
                                    .ReturnsAsync(parkingCarDtoToReturn);

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork.Setup(uow => uow.ParkingRecordRepository)
                          .Returns(mockParkingRecordRepository.Object);

            var controller = new HomeController(mockUnitOfWork.Object);

            var result = await controller.CheckIn(carTagNumber) as BadRequestObjectResult;

            Assert.NotNull(result);

            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task CheckIn_CarNotInParkingLot_SuccessfullyCheckedIn()
        {
            var carTagNumber = new CarTagNumber { TagNumber = tagNumber };

            mockParkingRecordRepository.Setup(repo => repo.GetCurrentParkingByTagNumber(carTagNumber.TagNumber))
                .ReturnsAsync((ParkingCarDto)null); // Simulate Car Not In Parking Lot

            mockParkingRecordRepository.Setup(repo => repo.GetAvailableParkingSpot())
                .ReturnsAsync(5); // Simulate Available Parking Spot

            mockParkingRecordRepository.Setup(repo => repo.CheckInCar(carTagNumber))
                .ReturnsAsync(1); // Simulate Successful Check-In

            mockUnitOfWork.Setup(uow => uow.ParkingRecordRepository)
                .Returns(mockParkingRecordRepository.Object);

            var controller = new HomeController(mockUnitOfWork.Object);

           var result = await controller.CheckIn(carTagNumber) as OkObjectResult;

  
            Assert.Contains("Successfully reserved a parking lot.", result.Value.ToString());
        }

        [Fact]
        public async Task CheckOut_Should_Return_BadRequest_When_Car_Not_Registered()
        {
            var carTagNumber = new CarTagNumber { TagNumber = tagNumber };

            mockParkingRecordRepository.Setup(repo => repo.GetCurrentParkingByTagNumber(carTagNumber.TagNumber))
                                       .ReturnsAsync((ParkingCarDto)null);

            mockUnitOfWork.Setup(uow => uow.ParkingRecordRepository)
                          .Returns(mockParkingRecordRepository.Object);

            var controller = new HomeController(mockUnitOfWork.Object);

            var result = await controller.CheckOut(carTagNumber) as BadRequestObjectResult;

            Assert.NotNull(result);

            Assert.Equal(400, result.StatusCode);

            Assert.NotNull(result.Value);

            Assert.Contains("The car is not registered in the parking lot.", result.Value.ToString());
        }

        [Fact]
        public async Task CheckOut_Should_Return_Ok_With_TotalAmount()
        {

            var carTagNumber = new CarTagNumber { TagNumber = tagNumber };

            var parkingCarDto = new ParkingCarDto
            {
                Id = 1,
                TagNumber = tagNumber,
                CheckInTime = DateTime.Now.AddMinutes(-50)
            };

            mockParkingRecordRepository.Setup(repo => repo.GetCurrentParkingByTagNumber(carTagNumber.TagNumber))
                                       .ReturnsAsync(parkingCarDto); // Provide test data

            var parkingCarDtoToReturn = new ParkingCarDto
            {
                Id = parkingCarDto.Id,
                TagNumber = parkingCarDto.TagNumber,
                CheckInTime = parkingCarDto.CheckInTime,
                CheckOutTime = DateTime.Now,
                TotalAmount = 45.00M,
            };

            mockParkingRecordRepository.Setup(repo => repo.CheckOutCar(parkingCarDto))
                                       .ReturnsAsync(parkingCarDtoToReturn);

            mockUnitOfWork.Setup(uow => uow.ParkingRecordRepository)
                          .Returns(mockParkingRecordRepository.Object);

            var controller = new HomeController(mockUnitOfWork.Object);

            var result = await controller.CheckOut(carTagNumber) as OkObjectResult;

            Assert.NotNull(result);

            Assert.Equal(200, result.StatusCode);

            Assert.NotNull(result.Value);

            Assert.Contains("Your have to pay", result.Value.ToString());
        }

        [Fact]
        public async Task ParkingStats_Should_Return_Ok_With_Stats()
        {
            var ParkingStatsDtoToreturn = new List<ParkingStatsDto>
        {
            new ParkingStatsDto { Id=1, StatsText = "Number of spots available as of now: ", StatsValue = "8" },
            new ParkingStatsDto { Id=2, StatsText = "Today’s revenue as of now: ", StatsValue = "$405.00" },
            new ParkingStatsDto { Id=3, StatsText = "Average number of cars per day (past 30 days): ", StatsValue = "10" },
            new ParkingStatsDto { Id=4, StatsText = "Average revenue per day (past 30 days): ", StatsValue = "$475.00" }
        };

            mockParkingRecordRepository.Setup(repo => repo.GetParkingStats())
                                       .ReturnsAsync(ParkingStatsDtoToreturn);

            mockUnitOfWork.Setup(uow => uow.ParkingRecordRepository)
                          .Returns(mockParkingRecordRepository.Object);

            var controller = new HomeController(mockUnitOfWork.Object);

            var result = await controller.ParkingStats() as OkObjectResult;

            Assert.NotNull(result);

            Assert.Equal(200, result.StatusCode);

            Assert.NotNull(result.Value);

            var returnedParkingStatsDto = result.Value as List<ParkingStatsDto>;

            Assert.Equal(ParkingStatsDtoToreturn.Count, returnedParkingStatsDto.Count);
        }
    }

}
