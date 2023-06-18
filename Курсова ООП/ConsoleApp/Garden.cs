namespace ConsoleApp
{
    public class Garden
    {
        private bool isWatered;
        private bool isWeeded;

        public string GardenName { get; set; }

        public int WaterCount { get; set; }

        public bool IsWatered
        {
            get { return isWatered; }
            set { isWatered = value; }
        }

        public bool IsWeeded
        {
            get { return isWeeded; }
            set { isWeeded = value; }
        }

        public void RenameGarden(string name)
        {
            GardenName = name;
        }
    }
}
