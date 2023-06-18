namespace ConsoleApp
{
    // Observer pattern
    public delegate void GardenHandler(Gardener gardener, GardenEventArgs args);

    public class GardenEventArgs : EventArgs
    {
        public string Message { get; set; }
    }
}
