namespace ConsoleApp.States
{
    public class PlantedState : GardenState
    {
        public PlantedState()
        {
            State = "Planted";
            IsWeeded = false;
            IsWatered = false;
            WaterCount = 0;
        }

        public int WaterCount { get; set; }

        public bool IsWeeded { get; set; }

        public bool IsWatered { get; set; }
    }
}
