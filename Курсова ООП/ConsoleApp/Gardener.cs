namespace ConsoleApp
{
    // Singleton pattern
    public class Gardener
    {
        // ctor
        private Gardener() { }

        private static Gardener instance;
        private Garden garden;

        public static Gardener Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Gardener();
                }
                return instance;
            }
        }

        public string GardenerName { get; set; }

        public Garden Garden
        {
            get { return garden; }
            set { garden = value; }
        }

        public void RenameGardener(string name)
        {
            GardenerName = name;
        }

        public void Plow()
        {
            Console.Clear();
            Console.WriteLine("\t\t\t\t=================");
            Console.WriteLine("\t\t\t\t\tDone!");
            Console.WriteLine("\t\t\t\t=================");
        }

        public void Plant()
        {
            Console.Clear();
            Console.WriteLine("\t\t\t\t=================");
            Console.WriteLine("\t\t\t\t\tDone!");
            Console.WriteLine("\t\t\t\t=================");
        }

        public void Water()
        {
            Console.Clear();
            Console.WriteLine("\t\t\t\t=================");
            Console.WriteLine("\t\t\t\t\tDone!");
            Console.WriteLine("\t\t\t\t=================");
        }

        public void RemoveWeeds()
        {
            Console.Clear();
            Console.WriteLine("\t\t\t\t=================");
            Console.WriteLine("\t\t\t\t\tDone!");
            Console.WriteLine("\t\t\t\t=================");
        }

        public void Harvest()
        {
            Console.Clear();
            Console.WriteLine("\t\t\t\t=================");
            Console.WriteLine("\t\t\t\t\tDone!");
            Console.WriteLine("\t\t\t\t=================");
        }
    }
}
