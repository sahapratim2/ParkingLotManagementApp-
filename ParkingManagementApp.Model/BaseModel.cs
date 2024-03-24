namespace ParkingManagementApp.Model
{
    public abstract class BaseModel
    {
        public string AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public string AddedIP { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedIP { get; set; }
        public bool Deleted { get; set; }
        public string DeletedBy { get; set; }
        public DateTime DeletedDate { get; set; }
        public string DeletedIP { get; set; }

    }
}
