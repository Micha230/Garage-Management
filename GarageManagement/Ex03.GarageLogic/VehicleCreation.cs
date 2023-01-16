using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex03.GarageLogic
{
    internal enum eSupportedVehicles
    {
        RegularMotorcycle = 1,
        ElectricMotorcycle,
        RegularCar,
        ElectricCar,
        Truck
    }

    public enum eDataInputFields
    {
        Color,
        Doors,
        ModelName,
        CurrentFuel,
        CurrentHours,
        HazardousMaterials,
        CargoVolume,
        WheelManufacturerName,
        CurentWheelAirPressure,
        LicenseType,
        EngineCC
    }

    public class VehicleCreation
    {
        private static readonly string[] sr_ArrayOfSupportedVehicles = new string[] { "Regular Motorycle", "Electric Motorcycle", "Regular Car", "Electric Car", "Truck" };

        internal static string[] ArrayOfSupportedVehicles
        {
            get { return sr_ArrayOfSupportedVehicles; }
        }

        internal static Vehicle CreateVehicle(eSupportedVehicles i_VehiclesChoice, string io_LicenseNumber)
        {
            Vehicle newVehicle = null;

            switch (i_VehiclesChoice)
            {
                case eSupportedVehicles.ElectricMotorcycle:
                    {
                        newVehicle = createElectricMotorcycle(io_LicenseNumber);
                        break;
                    }

                case eSupportedVehicles.ElectricCar:
                    {
                        newVehicle = createElectricCar(io_LicenseNumber);
                        break;
                    }

                case eSupportedVehicles.Truck:
                    {
                        newVehicle = createTruck(io_LicenseNumber);
                        break;
                    }

                case eSupportedVehicles.RegularMotorcycle:
                    {
                        newVehicle = createRegularMotorcycle(io_LicenseNumber);
                        break;
                    }

                case eSupportedVehicles.RegularCar:
                    {
                        newVehicle = createRegularCar(io_LicenseNumber);
                        break;
                    }
            }

            return newVehicle;
        }

        private static Vehicle createElectricMotorcycle(string io_LicenseNumber)
        {
            Electric engine = new Electric(1.6f);
            return new Motorcycle(io_LicenseNumber, 2, 28, engine);
        }

        private static Vehicle createRegularMotorcycle(string io_LicenseNumber)
        {
            Fuel engine = new Fuel(eFuelTypes.Octan98, 6f);
            return new Motorcycle(io_LicenseNumber, 2, 28, engine);
        }

        private static Vehicle createElectricCar(string io_LicenseNumber)
        {
            Electric engine = new Electric(4.7f);
            return new Car(io_LicenseNumber, 5, 32, engine);
        }

        private static Vehicle createRegularCar(string io_LicenseNumber)
        {
            Fuel engine = new Fuel(eFuelTypes.Octan95, 50f);
            return new Car(io_LicenseNumber, 5, 32, engine);
        }

        private static Vehicle createTruck(string io_LicenseNumber)
        {
            Fuel engine = new Fuel(eFuelTypes.Soler, 120f);
            return new Truck(io_LicenseNumber, 14, 34, engine);
        }

        internal static void GetInfo(out Dictionary<eDataInputFields, object> io_DictionaryToFill, int i_Choice)
        {
            io_DictionaryToFill = new Dictionary<eDataInputFields, object>();

            if (i_Choice > 0 && i_Choice <= ArrayOfSupportedVehicles.Length)
            {
                eSupportedVehicles currentVehicle = (eSupportedVehicles)i_Choice;

                io_DictionaryToFill.Add(eDataInputFields.WheelManufacturerName, string.Empty);
                io_DictionaryToFill.Add(eDataInputFields.CurentWheelAirPressure, string.Empty);
                io_DictionaryToFill.Add(eDataInputFields.ModelName, string.Empty);

                if (currentVehicle.Equals(eSupportedVehicles.ElectricMotorcycle) || currentVehicle.Equals(eSupportedVehicles.ElectricCar))
                { // Hours left in elecrtic engine
                    io_DictionaryToFill.Add(eDataInputFields.CurrentHours, string.Empty);
                }
                else
                { // Fuel left
                    io_DictionaryToFill.Add(eDataInputFields.CurrentFuel, string.Empty);
                }

                if (currentVehicle.Equals(eSupportedVehicles.RegularMotorcycle) || currentVehicle.Equals(eSupportedVehicles.ElectricMotorcycle))
                { // Case of motorcycle
                    io_DictionaryToFill.Add(eDataInputFields.LicenseType, string.Empty);
                    io_DictionaryToFill.Add(eDataInputFields.EngineCC, string.Empty);
                }

                if (currentVehicle.Equals(eSupportedVehicles.ElectricCar) || currentVehicle.Equals(eSupportedVehicles.RegularCar))
                { // Case of car  
                    io_DictionaryToFill.Add(eDataInputFields.Color, string.Empty);
                    io_DictionaryToFill.Add(eDataInputFields.Doors, string.Empty);
                }

                if (currentVehicle.Equals(eSupportedVehicles.Truck))
                { // Case of truck  
                    io_DictionaryToFill.Add(eDataInputFields.HazardousMaterials, false);
                    io_DictionaryToFill.Add(eDataInputFields.CargoVolume, string.Empty);
                }
            }
        }

        internal static void FillVehicleInfo(Vehicle io_Vehicle, Dictionary<eDataInputFields, object> i_FilledDictionary)
        {
            //// Gets a vehicle X and a filled dictionary.
            //// Fill the blanks in Vehicle X info by the information filled in dictionary.
            //// Throw exceptions with information about each mismatch between information needed and information given.

            if (i_FilledDictionary.ContainsKey(eDataInputFields.ModelName))
            {
                io_Vehicle.ModelName = i_FilledDictionary[eDataInputFields.ModelName].ToString();
            }

            if (i_FilledDictionary.ContainsKey(eDataInputFields.CurrentFuel))
            {
                if (float.TryParse(i_FilledDictionary[eDataInputFields.CurrentFuel].ToString(), out float currentFuel))
                {
                    Fuel engine = io_Vehicle.EngineType as Fuel;
                    engine.CurrentFuelTank = currentFuel;
                    float percent = engine.CurrentFuelTank / engine.MaxTank;
                    io_Vehicle.RemainingEnergyPercentage = percent * 100f;
                }
                else
                {
                    throw new FormatException("Amount of fuel is invalid!");
                }
            }

            if (i_FilledDictionary.ContainsKey(eDataInputFields.CurrentHours))
            {
                if (float.TryParse(i_FilledDictionary[eDataInputFields.CurrentHours].ToString(), out float currentMinutes))
                {
                    Electric engine = io_Vehicle.EngineType as Electric;
                    engine.BatteryRemainingHours = currentMinutes / 60f;
                    float percent = engine.BatteryRemainingHours / engine.MaxHoursInBattery;
                    io_Vehicle.RemainingEnergyPercentage = percent * 100f;
                }
                else
                {
                    throw new FormatException("Amount of hours is invalid!");
                }
            }

            if (i_FilledDictionary.ContainsKey(eDataInputFields.CurentWheelAirPressure))
            {
                if (float.TryParse(i_FilledDictionary[eDataInputFields.CurentWheelAirPressure].ToString(), out float currentAirPressure))
                {
                    for (int i = 0; i < io_Vehicle.Wheels.Length; i++)
                    {
                        io_Vehicle.Wheels[i].CurrentAirPressure = currentAirPressure;
                    }
                }
                else
                {
                    throw new FormatException("Amount of wheel air pressure is invalid!");
                }
            }

            if (i_FilledDictionary.ContainsKey(eDataInputFields.WheelManufacturerName))
            {
                for (int i = 0; i < io_Vehicle.Wheels.Length; i++)
                {
                    io_Vehicle.Wheels[i].ManufacturerName = i_FilledDictionary[eDataInputFields.WheelManufacturerName].ToString();
                }
            }

            if (i_FilledDictionary.ContainsKey(eDataInputFields.Doors))
            {
                if (int.TryParse(i_FilledDictionary[eDataInputFields.Doors].ToString(), out int doors))
                {
                    (io_Vehicle as Car).Doors = (eDoors)doors;
                }
                else
                {
                    throw new FormatException("Number of doors choie is invalid!");
                }
            }

            if (i_FilledDictionary.ContainsKey(eDataInputFields.LicenseType))
            {
                if (int.TryParse(i_FilledDictionary[eDataInputFields.LicenseType].ToString(), out int licenseType))
                {
                    (io_Vehicle as Motorcycle).LicenseType = (eLicenseTypes)licenseType;
                }
                else
                {
                    throw new FormatException("Choice of license type is invalid!");
                }
            }

            if (i_FilledDictionary.ContainsKey(eDataInputFields.Color))
            {
                if (int.TryParse(i_FilledDictionary[eDataInputFields.Color].ToString(), out int color))
                {
                    (io_Vehicle as Car).Color = (eColors)color;
                }
                else
                {
                    throw new FormatException("Choice of color is invalid!");
                }
            }

            if (i_FilledDictionary.ContainsKey(eDataInputFields.HazardousMaterials))
            {
                (io_Vehicle as Truck).IsTransportHazardousMaterials = (bool)i_FilledDictionary[eDataInputFields.HazardousMaterials];
            }

            if (i_FilledDictionary.ContainsKey(eDataInputFields.CargoVolume))
            {
                if (float.TryParse(i_FilledDictionary[eDataInputFields.CargoVolume].ToString(), out float cargo))
                {
                    (io_Vehicle as Truck).CargoVolume = cargo;
                }
                else
                {
                    throw new FormatException("Cargo volume is invalid!");
                }
            }

            if (i_FilledDictionary.ContainsKey(eDataInputFields.EngineCC))
            {
                if (int.TryParse(i_FilledDictionary[eDataInputFields.EngineCC].ToString(), out int engineCC))
                {
                    (io_Vehicle as Motorcycle).EngineVolumeInCC = engineCC;
                }
                else
                {
                    throw new FormatException("Engine volume is invalid!");
                }
            }
        }

        internal static string ShowSupportedVehiclesTypes()
        {
            int index = 1;
            StringBuilder supportedVehicles = new StringBuilder();

            foreach (string currentVehicle in ArrayOfSupportedVehicles)
            {
                supportedVehicles.Append(string.Format("{0}. {1}{2}", index++, currentVehicle, Environment.NewLine));
            }

            return supportedVehicles.ToString();
        }
    }
}
