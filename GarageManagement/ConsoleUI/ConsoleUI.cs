using System;
using System.Collections.Generic;
using Ex03.GarageLogic;

namespace ConsoleUI
{
    public class ConsoleUI
    {
        private const int k_NumberOfOptions = 8;

        private readonly string r_ErrorMsg = "Invalid input! Please try again.";
        private readonly Garage r_MyGarage = new Garage();

        internal ConsoleUI()
        {
            int userChoice = 0;

            printOptions(out userChoice);

            while (userChoice != k_NumberOfOptions)
            {
                try
                {
                    switch (userChoice)
                    {
                        case 1:
                            addNewVehicleInput();
                            break;
                        case 2:
                            printVehicleByFilter();
                            break;
                        case 3:
                            changeVehicleStatus();
                            break;
                        case 4:
                            inflateWheelsToMax();
                            break;
                        case 5:
                            fillFuel();
                            break;
                        case 6:
                            chargeBattery();
                            break;
                        case 7:
                            printDataByLicense();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    if (ex is ValueOutOfRangeException)
                    {
                        Console.WriteLine((ex as ValueOutOfRangeException).InnerException.Message);
                    }

                    Console.WriteLine(ex.Message);
                }

                System.Threading.Thread.Sleep(2000);
                Console.Clear();
                printOptions(out userChoice);
            }
        }

        private void printOptions(out int io_Choice)
        {
            io_Choice = 0;
            string error = string.Format("{0}{1}The input must be an option between 1-8.{1}", r_ErrorMsg, Environment.NewLine);
            string menu = string.Format(
                @"Welcome to {0}
Here are your options. Please choose the service you need :) (input a number between 1-8)
1. Enter new vehicle to the garage.
2. Show license number of vehicles filtered by status.
3. Change status of vehicle.
4. Inflating air in wheels.
5. Fill fuel tank.
6. Charge battery.
7. Show vehicle details.
8. Exit {0}",
r_MyGarage.Name);

            Console.WriteLine(menu);

            while (!int.TryParse(Console.ReadLine(), out io_Choice) || io_Choice < 1 || io_Choice > k_NumberOfOptions)
            {
                Console.WriteLine(error);
            }
        }

        private void addNewVehicleInput()
        {
            getNameAndPhone(out string name, out string phoneNumber);
            getVehicle(out int vehicleChoice);
            getLicenseNumber(out string licenseNumber);

            if (r_MyGarage.AddNewVehicle(name, phoneNumber, vehicleChoice, licenseNumber))
            {
                getExtraInfo(vehicleChoice, licenseNumber);
                Console.WriteLine("This vehicle added successfuly to {0}", r_MyGarage.Name);
            }
            else
            {
                Console.WriteLine("This vehicle is already in {0}", r_MyGarage.Name);
            }
        }

        private void printVehicleByFilter()
        {
            bool seeInReapir, seePaid, seeFixed;

            Console.WriteLine("Do you want to see all vehicles (without filter) (Y/N)?");
            getYesOrNO(out bool seeAll);

            if (!seeAll)
            {
                Console.WriteLine("Do you want to see 'in-repair' vehicles? (Y/N)");
                getYesOrNO(out seeInReapir);

                Console.WriteLine("Do you want to see 'fixed' vehicles? (Y/N)");
                getYesOrNO(out seeFixed);

                Console.WriteLine("Do you want to see 'paid' vehicles? (Y/N)");
                getYesOrNO(out seePaid);
            }
            else
            {
                seeInReapir = true;
                seeFixed = true;
                seePaid = true;
            }

            Console.Write(r_MyGarage.VehicleFilteredByStatus(seeInReapir, seeFixed, seePaid));
        }

        private void getYesOrNO(out bool io_Choice)
        {
            string input = Console.ReadLine();

            while (input != "Y" && input != "N")
            {
                Console.WriteLine("{0}{1}Please enter Y/N", r_ErrorMsg, Environment.NewLine);
                input = Console.ReadLine();
            }

            if (input == "Y")
            {
                io_Choice = true;
            }
            else
            {
                io_Choice = false;
            }
        }

        private void changeVehicleStatus()
        {
            int choice = 0;
            getLicenseNumber(out string licenseInput);

            Console.Write("Choose new status:{0}{1}", Environment.NewLine, r_MyGarage.ShowOptionsOfServiceStatuses());

            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > r_MyGarage.AmountOfServiceStatuses())
            {
                Console.WriteLine(r_ErrorMsg);
            }

            if (r_MyGarage.ChangeServiceStatus(licenseInput, choice))
            {
                Console.WriteLine("Status Updated!");
            }
            else
            {
                Console.WriteLine("License does not exists in {0}", r_MyGarage.Name);
            }
        }

        private void inflateWheelsToMax()
        {
            getLicenseNumber(out string licenseNumber);

            if (r_MyGarage.InflateWheels(licenseNumber))
            {
                Console.WriteLine("The wheels are now full");
            }
            else
            {
                Console.WriteLine("License does not exists in {0}", r_MyGarage.Name);
            }
        }

        private void fillFuel()
        {
            int fuelType = -1;
            float amountToadd = -1f;

            getLicenseNumber(out string licenseNumber);

            Console.WriteLine("Which type of fuel do you want to add?");
            Console.Write(r_MyGarage.ShowFuelTypes());

            while (!int.TryParse(Console.ReadLine(), out fuelType) || fuelType < 1 || fuelType > r_MyGarage.AmountOfFuelTypes())
            {
                Console.WriteLine(r_ErrorMsg);
            }

            Console.WriteLine("How much fuel do you want to add?");

            while (!float.TryParse(Console.ReadLine(), out amountToadd) || amountToadd < 0f)
            {
                Console.WriteLine("{0}{1}Please enter positive amount of fuel", r_ErrorMsg, Environment.NewLine);
            }

            if (r_MyGarage.FillFuelTank(licenseNumber, fuelType, amountToadd))
            {
                Console.WriteLine("Tank Filled");
            }
            else
            {
                Console.WriteLine("Vehicle not found!");
            }
        }

        private void chargeBattery()
        {
            string licenseNumber;
            float amountToAdd = -1f;

            getLicenseNumber(out licenseNumber);

            Console.WriteLine("How much minutes do you want to add?");
            float.TryParse(Console.ReadLine(), out amountToAdd);

            while (amountToAdd <= 0)
            {
                Console.WriteLine("{0}{1}Please enter postive number.", r_ErrorMsg, Environment.NewLine);
                float.TryParse(Console.ReadLine(), out amountToAdd);
            }

            if (r_MyGarage.ChargeElectricBattery(licenseNumber, amountToAdd))
            {
                Console.WriteLine("Battery charged.");
            }
            else
            {
                Console.WriteLine("Vehicle not found!");
            }
        }

        private void printDataByLicense()
        {
            getLicenseNumber(out string licenseNumber);

            if (r_MyGarage.VehicleInfo(licenseNumber, out string msg))
            {
                Console.WriteLine("{0}{1}{0}Press any key to go back to menu.", Environment.NewLine, msg);
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Vehicle not found!");
            }
        }

        private void getLicenseNumber(out string io_LicenseNumber)
        {
            Console.WriteLine("Please enter your license number:");
            io_LicenseNumber = Console.ReadLine();
        }

        private void getNameAndPhone(out string io_Name, out string io_Phone)
        {
            Console.WriteLine("Please enter the garage customer's name:");
            io_Name = Console.ReadLine();

            while (io_Name.Length < 2)
            {
                Console.WriteLine("{0}{1}Name should be at least 2 letters.", r_ErrorMsg, Environment.NewLine);
                io_Name = Console.ReadLine();
            }

            Console.WriteLine("Please enter the garage customer's phone number:(05xxxxxxxx)");
            io_Phone = Console.ReadLine();

            while (!int.TryParse(io_Phone, out int check) || io_Phone.Length != 10 || io_Phone[0] != '0' || io_Phone[1] != '5')
            {
                Console.WriteLine("{0}{1}Phone number should be 10 digit's in the form 05xxxxxxxx.", r_ErrorMsg, Environment.NewLine);
                io_Phone = Console.ReadLine();
            }
        }

        private void getVehicle(out int vehicleChoice)
        {
            int maxChoice = r_MyGarage.AmountOfSupportedVehicles();
            vehicleChoice = 0;

            Console.Write("Choose one of our supported vehicles:{0}{1}", Environment.NewLine, r_MyGarage.ShowSupportedVehicles());
            int.TryParse(Console.ReadLine(), out vehicleChoice);

            while (vehicleChoice < 1 || vehicleChoice > maxChoice)
            {
                Console.WriteLine("{0}{1}Must be one of these options (1-{2}).", r_ErrorMsg, Environment.NewLine, maxChoice);
                int.TryParse(Console.ReadLine(), out vehicleChoice);
            }
        }

        private void getExtraInfo(int io_Choice, string io_LicenseNumber)
        {
            Dictionary<eDataInputFields, object> infoDictionaryToFill = r_MyGarage.GetExtraInfo(io_Choice);
            bool invalidInput = true;

            while (invalidInput)
            {
                if (infoDictionaryToFill.ContainsKey(eDataInputFields.ModelName))
                {
                    Console.WriteLine("Please enter Model Name:");
                    infoDictionaryToFill[eDataInputFields.ModelName] = Console.ReadLine();
                }

                if (infoDictionaryToFill.ContainsKey(eDataInputFields.CurrentFuel))
                {
                    Console.WriteLine("Please enter Current Fuel Amount:");
                    infoDictionaryToFill[eDataInputFields.CurrentFuel] = Console.ReadLine();
                }

                if (infoDictionaryToFill.ContainsKey(eDataInputFields.CurrentHours))
                {
                    Console.WriteLine("Please enter Current Minutes in Battery:");
                    infoDictionaryToFill[eDataInputFields.CurrentHours] = Console.ReadLine();
                }

                if (infoDictionaryToFill.ContainsKey(eDataInputFields.CurentWheelAirPressure))
                {
                    Console.WriteLine("Please enter Current Wheels Air Pressure:");
                    infoDictionaryToFill[eDataInputFields.CurentWheelAirPressure] = Console.ReadLine();
                }

                if (infoDictionaryToFill.ContainsKey(eDataInputFields.WheelManufacturerName))
                {
                    Console.WriteLine("Please enter Wheels Manufacturer:");
                    infoDictionaryToFill[eDataInputFields.WheelManufacturerName] = Console.ReadLine();
                }

                if (infoDictionaryToFill.ContainsKey(eDataInputFields.Doors))
                {
                    string question = string.Format("Please choose Number of Doors:{0}{1}", Environment.NewLine, r_MyGarage.ShowCarDoors());
                    Console.Write(question);
                    infoDictionaryToFill[eDataInputFields.Doors] = Console.ReadLine();
                }

                if (infoDictionaryToFill.ContainsKey(eDataInputFields.LicenseType))
                {
                    string question = string.Format("Please choose License Type of your Motorcycle:{0}{1}", Environment.NewLine, r_MyGarage.ShowMotorcycleLicenseTypes());
                    Console.Write(question);
                    infoDictionaryToFill[eDataInputFields.LicenseType] = Console.ReadLine();
                }

                if (infoDictionaryToFill.ContainsKey(eDataInputFields.Color))
                {
                    string question = string.Format("Please choose the car's Color:{0}{1}", Environment.NewLine, r_MyGarage.ShowCarColors());
                    Console.Write(question);
                    infoDictionaryToFill[eDataInputFields.Color] = Console.ReadLine();
                }

                if (infoDictionaryToFill.ContainsKey(eDataInputFields.HazardousMaterials))
                {
                    Console.WriteLine("Does the Truck Contains Hazardous Materials (Y/N)");
                    getYesOrNO(out bool answer);
                    infoDictionaryToFill[eDataInputFields.HazardousMaterials] = answer;
                }

                if (infoDictionaryToFill.ContainsKey(eDataInputFields.CargoVolume))
                {
                    Console.WriteLine("What is the truck cargo volume?");
                    infoDictionaryToFill[eDataInputFields.CargoVolume] = Console.ReadLine();
                }

                if (infoDictionaryToFill.ContainsKey(eDataInputFields.EngineCC))
                {
                    Console.WriteLine("Please enter engine volume in CC:");
                    infoDictionaryToFill[eDataInputFields.EngineCC] = Console.ReadLine();
                }

                try
                {
                    r_MyGarage.InsertInfo(infoDictionaryToFill, io_LicenseNumber);
                    invalidInput = false;
                }
                catch (Exception ex)
                {
                    if (ex is ValueOutOfRangeException)
                    {
                        Console.WriteLine((ex as ValueOutOfRangeException).InnerException.Message);
                    }

                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Please enter extra info again:");
                }
            }
        }
    }
}
