using ConsoleApp.States;
using Newtonsoft.Json;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            #region deserialize if exist or create default if no
            Gardener gardener;
            if (File.Exists("garden.txt"))
            {
                string json = File.ReadAllText("garden.txt");
                gardener = JsonConvert.DeserializeObject<Gardener>(json);
            }
            else
            {
                gardener = Gardener.Instance;
                gardener.GardenerName = "Default";
                gardener.Garden = new Garden();
                gardener.Garden.GardenName = "Default Garden";
                gardener.Garden.IsWatered = false;
                gardener.Garden.IsWeeded = false;
                gardener.Garden.WaterCount = 0;
            }
            #endregion

            // set the default state as Not planted
            GardenState state = new NotPlantedState();

            // handler for messages
            GardenHandler handler = (g, e) => Console.WriteLine(e.Message);

            while (true)
            {
                #region menu "buttons"
                Console.WriteLine("What do you want to do with the garden?");
                Console.WriteLine("0. Plow the garden");
                Console.WriteLine("1. Plant the garden");
                Console.WriteLine("2. Water the garden");
                Console.WriteLine("3. Remove the weeds");
                Console.WriteLine("4. Harvest the garden");
                Console.WriteLine("5. Check the state of the garden");
                Console.WriteLine("6. Tip");
                Console.WriteLine("8. Change name of the garden and gardener");
                Console.WriteLine("9. Save and exit");
                #endregion

                // user input for navigation
                string choice = Console.ReadLine();

                #region checking if user wrote a number
                if (int.TryParse(choice, out int resChoice))
                {
                    #region navigation
                    switch (resChoice)
                    {
                        #region case 0
                        case 0:
                            if (state.State == "Harvested" || state.State == "Not planted" || state.State == "Destroyed")
                            {
                                gardener.Plow();
                                state = new PlowedState();
                                GardenEventArgs arguments = new GardenEventArgs { Message = "The land is plowed!\n" };
                                handler(gardener, arguments);
                            }
                            else if (state.State == "Planted" || state.State == "Plowed")
                            {
                                Console.Clear();
                                Console.WriteLine("\t==================================================================================");
                                Console.WriteLine("\t\tThe land is already plowed or the garden is not harvested yet!");
                                Console.WriteLine("\t==================================================================================");
                            }
                            else if (state.State == "Ready to harvest")
                            {
                                Console.Clear();
                                Console.WriteLine("\t====================================================================");
                                Console.WriteLine("\t\tYou need to harvest the garden before plowering it!");
                                Console.WriteLine("\t====================================================================");
                            }
                            break;
                        #endregion
                        #region case 1
                        case 1:
                            if (state.State == "Plowed")
                            {
                                gardener.Plant();
                                state = new PlantedState();
                                GardenEventArgs arguments = new GardenEventArgs { Message = "The garden is planted!\n" };
                                handler(gardener, arguments);
                            }
                            else if (state.State == "Planted")
                            {
                                Console.Clear();
                                Console.WriteLine("\t\t\t\t=========================================");
                                Console.WriteLine("\t\t\t\t\tThe garden is already planted!");
                                Console.WriteLine("\t\t\t\t=========================================");
                            }
                            else if (state.State == "Destroyed")
                            {
                                Console.Clear();
                                Console.WriteLine("\t\t=========================================================");
                                Console.WriteLine("\t\t\tThe garden is destroyed! You need to plow it.");
                                Console.WriteLine("\t\t=========================================================");
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("\t==================================================================================");
                                Console.WriteLine("\t\tThe land is not plowed yet or the garden is not harvested yet!");
                                Console.WriteLine("\t==================================================================================");
                            }
                            break;
                        #endregion
                        #region case 2
                        case 2:
                            if (state.State == "Planted")
                            {
                                gardener.Water();
                                ((PlantedState)state).IsWatered = true;
                                ((PlantedState)state).WaterCount++;

                                GardenEventArgs arguments = new GardenEventArgs { Message = "You watered the garden!\n" };
                                handler(gardener, arguments);

                                if (((PlantedState)state).WaterCount > 3)
                                {
                                    Console.Clear();
                                    Console.WriteLine("\t\t=======================================================================");
                                    Console.WriteLine("\t\t\tThe garden is destroyed, you should plow the land again");
                                    Console.WriteLine("\t\t=======================================================================");
                                    state = new DestroyedState();
                                    GardenEventArgs argumentsBad = new GardenEventArgs { Message = "The garden is destroyed, you should plow the land again\n" };
                                    handler(gardener, argumentsBad);
                                }
                            }
                            else if (state.State == "Ready to harvest")
                            {
                                Console.Clear();
                                Console.WriteLine("\t\t\t=====================================================");
                                Console.WriteLine("\t\t\t\tThe garden is already ready to harvest!");
                                Console.WriteLine("\t\t\t=====================================================");
                            }
                            else if (state.State == "Destroyed")
                            {
                                Console.Clear();
                                Console.WriteLine("\t\t\t===========================================================");
                                Console.WriteLine("\t\t\t\tThe garden is destroyed! You need to plow it.");
                                Console.WriteLine("\t\t\t===========================================================");
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("\t\t\t============================================");
                                Console.WriteLine("\t\t\t\tThe garden is not planted yet!");
                                Console.WriteLine("\t\t\t============================================");
                            }
                            break;
                        #endregion
                        #region case 3
                        case 3:
                            if (state.State == "Planted" && ((PlantedState)state).IsWatered && ((PlantedState)state).WaterCount == 3)
                            {
                                gardener.RemoveWeeds();
                                ((PlantedState)state).IsWeeded = true;

                                GardenEventArgs arguments = new GardenEventArgs { Message = $"You removed weeds from the garden named {gardener.Garden.GardenName}!\n" };
                                handler(gardener, arguments);

                                if (((PlantedState)state).WaterCount == 3)
                                {
                                    state = new ReadyToHarvestState();
                                    GardenEventArgs argumentsGood = new GardenEventArgs
                                    {
                                        Message = $"The garden named {gardener.Garden.GardenName} is ready to harvest!\n"
                                    };
                                    handler(gardener, argumentsGood);
                                }
                            }
                            else if (state.State == "Planted" && ((PlantedState)state).WaterCount != 3)
                            {
                                Console.Clear();
                                Console.WriteLine("\t\t\t=================================================================");
                                Console.WriteLine("\t\t\t\tThere're no weeds in the garden. You need to water it!");
                                Console.WriteLine("\t\t\t=================================================================");
                            }
                            else if (state.State == "Ready to harvest")
                            {
                                Console.Clear();
                                Console.WriteLine("\t\t\t=====================================================");
                                Console.WriteLine("\t\t\t\tThe garden is already ready to harvest!");
                                Console.WriteLine("\t\t\t=====================================================");
                            }
                            else if (state.State == "Destroyed")
                            {
                                Console.Clear();
                                Console.WriteLine("\t\t\t===========================================================");
                                Console.WriteLine("\t\t\t\tThe garden is destroyed! You need to plow it.");
                                Console.WriteLine("\t\t\t===========================================================");
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("\t\t=======================================================================");
                                Console.WriteLine("\t\t\tThe garden is not ready to be weeded or not watered yet!");
                                Console.WriteLine("\t\t=======================================================================");
                            }
                            break;
                        #endregion
                        #region case 4
                        case 4:
                            if (state.State == "Ready to harvest")
                            {
                                gardener.Harvest();
                                state = new HarvestedState();
                                GardenEventArgs arguments = new GardenEventArgs { Message = "The garden is harvested!\n" };
                                handler(gardener, arguments);
                            }
                            else if (state.State == "Harvested")
                            {
                                Console.Clear();
                                Console.WriteLine("\t\t\t\t============================================");
                                Console.WriteLine("\t\t\t\t\tThe garden is already harvested!");
                                Console.WriteLine("\t\t\t\t============================================");
                            }
                            else if (state.State == "Destroyed")
                            {
                                Console.Clear();
                                Console.WriteLine("\t\t\t===========================================================");
                                Console.WriteLine("\t\t\t\tThe garden is destroyed! You need to plow it.");
                                Console.WriteLine("\t\t\t===========================================================");
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("\t\t\t===========================================================");
                                Console.WriteLine("\t\t\t\tThe garden is not ready to be harvested yet!");
                                Console.WriteLine("\t\t\t===========================================================");
                            }
                            break;
                        #endregion
                        #region case 5
                        case 5:
                            Console.Clear();
                            Console.WriteLine("\t\t\t\t=========================================");
                            Console.WriteLine("\t\t\t\t\tGardener name: " + gardener.GardenerName);
                            Console.WriteLine("\t\t\t\t\tGarden name: " + gardener.Garden.GardenName);
                            Console.WriteLine("\t\t\t\t\tGarden state: " + state.State);
                            if (state.State == "Planted")
                            {
                                Console.WriteLine("\t\t\t\t\tYou watered your garden: " + ((PlantedState)state).WaterCount);

                                if (((PlantedState)state).IsWeeded == true)
                                    Console.WriteLine("\t\t\t\t\tCongratulation! There're no weeds on your garden! ");
                            }
                            Console.WriteLine("\t\t\t\t=========================================");
                            break;
                        #endregion
                        #region case 6
                        case 6:
                            Console.Clear();
                            Console.WriteLine("\t\t===================================" +
                                "===========================================");
                            Console.WriteLine("\t\t\t\t\t\tTip:\n");
                            Console.WriteLine("\t\t\tRemember to water your garden... But don't make a lake from it");
                            Console.WriteLine("\t\t===================================" +
                                "===========================================");
                            break;
                        #endregion
                        #region case 8
                        case 8:
                            Console.Clear();
                            Console.WriteLine("\t\t\t\t=========================================");
                            Console.WriteLine("\t\t\t\t\tEnter new gardener name: ");
                            Console.WriteLine("\t\t\t\t=========================================");
                            gardener.RenameGardener(Console.ReadLine());

                            Console.WriteLine("\t\t\t\t=========================================");
                            Console.WriteLine("\t\t\t\t\tEnter new garden name: ");
                            Console.WriteLine("\t\t\t\t=========================================");

                            gardener.Garden.RenameGarden(Console.ReadLine());
                            Console.Clear();

                            Console.WriteLine("\t\t\t===========================");
                            Console.WriteLine("\t\t\t\tChanges saved!");
                            Console.WriteLine("\t\t\t===========================");

                            break;
                        #endregion
                        #region case 9
                        case 9:
                            Console.WriteLine("\t\t\t\t\t\tSaving...\n\n\t\t\t\t\t=====================\n\t\t\t\t\t=Exiting the" +
                                " program=\n\t\t\t\t\t=====================\n");

                            #region serialization
                            // save the garden to the file
                            // ConsoleApp\bin\Debug\net6.0\garden.txt
                            string gardenState = JsonConvert.SerializeObject(gardener);
                            File.WriteAllText("garden.txt", gardenState);
                            #endregion
                            return;
                        default:
                            Console.Clear();
                            Console.WriteLine("\t\t\t\t\t===========================\n\t\t\t\t\t=Invalid" +
                                " choice. Try again=\n\t\t\t\t\t===========================\n");
                            break;
                        #endregion
                    }
                    #endregion
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("\t\t\t\t\t==========================\n\t\t\t\t\t=Input should" +
                        " be a number=\n\t\t\t\t\t==========================\n");
                }
                #endregion
            }
        }
    }
}