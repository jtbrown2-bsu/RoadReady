using RoadReady.Models;

namespace RoadReady.ViewModels
{
    public class StopViewModel
    {
        public int Order { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
    public class TourViewModel
    {
        public string Name { get; set; }
        public List<StopViewModel> Stops { get; set; }
    }

    public class TourEditViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<StopViewModel> Stops { get; set; }
    }
}
