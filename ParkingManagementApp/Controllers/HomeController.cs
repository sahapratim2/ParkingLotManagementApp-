using Microsoft.AspNetCore.Mvc;
using ParkingManagementApp.Common.DTOs;
using ParkingManagementApp.Core.Interfaces;

namespace ParkingManagementApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _uow;

        public HomeController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IActionResult> Index()
        {
             var cars = await _uow.ParkingRecordRepository.GetParkingCars();

            ViewBag.TotalSpots = _uow.ParkingRecordRepository.GetTotalParkingSpot();

            ViewBag.HourlyFee = _uow.ParkingRecordRepository.GetHourlyFee();

            ViewBag.SpotsTaken = cars.Count();

            ViewBag.AvailableSpots = _uow.ParkingRecordRepository.GetAvailableParkingSpot(ViewBag.SpotsTaken);

            return View(cars);
        }

        public async Task<IActionResult> ParkingCars()
        {
            return Ok(await _uow.ParkingRecordRepository.GetParkingCars());
        }


        public async Task<IActionResult> GetCar([FromBody] CarTagNumber carTagNumber)
        {
            return Ok(await _uow.ParkingRecordRepository.GetCurrentParkingByTagNumber(carTagNumber.TagNumber));
        }

        [HttpPost]
        public async Task<IActionResult> CheckIn([FromBody] CarTagNumber carTagNumber)
        {

            var car = await _uow.ParkingRecordRepository.GetCurrentParkingByTagNumber(carTagNumber.TagNumber);

            if (car != null)
            {
                return BadRequest(new { Message = "Car already in the parking lot." });
            }
            else if (_uow.ParkingRecordRepository.GetAvailableParkingSpot().Result == 0)
            {
                return BadRequest(new { Message = "There are no spots available." });
            }

            if (await _uow.ParkingRecordRepository.CheckInCar(carTagNumber) == 1)
            {

                return Ok(new { Message = "Successfully reserved a parking lot." });
            }
            else
            {
                return BadRequest(new { Message = "Parking data did not saved." });
            }
        }

        [HttpPut]
        public async Task<IActionResult> CheckOut([FromBody] CarTagNumber carTagNumber)
        {
            var car = await _uow.ParkingRecordRepository.GetCurrentParkingByTagNumber(carTagNumber.TagNumber);

            if (car == null)
            {
                return BadRequest(new { Message = "The car is not registered in the parking lot." });
            }

            decimal totalAmount = 0;

            var carCheckout = await _uow.ParkingRecordRepository.CheckOutCar(car);

            if(carCheckout != null)
            {
                totalAmount = carCheckout.TotalAmount;
            }
           
            return Ok(new { Message = $"Your have to pay: ${totalAmount}" });
        }

        public async Task<IActionResult> ParkingStats()
        {
            return Ok(await _uow.ParkingRecordRepository.GetParkingStats());
        }

    }
}