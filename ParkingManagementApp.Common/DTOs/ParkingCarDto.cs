using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagementApp.Common.DTOs
{
    public class ParkingCarDto
    {
        public long Id { get; set; }
        public string TagNumber { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public decimal HourlyFee { get; set; }
        public decimal TotalAmount { get; set; } = 0;
    }
}
