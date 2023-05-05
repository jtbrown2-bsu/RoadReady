namespace RoadReady.Models
{
    public class Tour
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Stop> Stops { get; set; }
    }
}
