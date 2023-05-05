using System.Text.Json.Serialization;

namespace RoadReady.Models
{
    public class Stop
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int TourId { get; set; }
        [JsonIgnore]
        public virtual Tour Tour {get; set;}
    }
}
