using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex03.GarageLogic
{
    public class Garage
    {
        private readonly string r_Name = "MC Garage!";
        private readonly Dictionary<string, GarageCustomer> r_Vehicles = new Dictionary<string, GarageCustomer>();

        public string Name
        {
            get { return r_Name; }
        }

        public bool AddNewVehicle(string io_CustomerName, string io_CustomerPhoneNumber, int io_Choice, string io_LicenseNumber)
        {
            //// Gets new customer (name, phone number and license plate number) and choice out of 
            //// the garage supported vehicles.
            //// Two possible cases:
            //// 1. New license plate number - add it to the garage.
            //// 2. Existing license plate number - change state of car to - "In-Repair"
            //// Return T/F if insertion succeeded.

            bool isAdded = false;

            if (isExist(io_LicenseNumber))
            {
                try
                {
                    r_Vehicles[io_LicenseNumber].VehicleStatus = eServiceStatuses.InRepair;
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("License number {0} is not in the system.", io_LicenseNumber), ex);
                }
            }
            else
            {
                if (Enum.IsDefined(typeof(eSupportedVehicles), io_Choice))
                {
                    Vehicle newVehicle = VehicleCreation.CreateVehicle((eSupportedVehicles)io_Choice, io_LicenseNumber);
                    GarageCustomer newCustomer = new GarageCustomer(io_CustomerName, io_CustomerPhoneNumber, eServiceStatuses.InRepair, newVehicle);
                    r_Vehicles.Add(io_LicenseNumber, newCustomer);
                    isAdded = true;
                }
                else
                {
                    Exception ex = new Exception("Choice is invalid!");
                    throw new ValueOutOfRangeException(ex, (float)AmountOfSupportedVehicles(), 1f);
                }
            }

            return isAdded;
        }

        public string VehicleFilteredByStatus(bool i_ShowInRepair, bool i_ShowFixed, bool i_ShowPaid)
        {
            //// Return all vehicles license number in the garage by filter or unfilter.

            int index = 1;
            StringBuilder vehicleList = new StringBuilder();

            foreach (KeyValuePair<string, GarageCustomer> entry in r_Vehicles)
            {
                if (i_ShowInRepair && entry.Value.VehicleStatus.Equals(eServiceStatuses.InRepair))
                {
                    vehicleList.Append(string.Format("{0}. {1}{2}", index++, entry.Key, Environment.NewLine));
                }

                if (i_ShowFixed && entry.Value.VehicleStatus.Equals(eServiceStatuses.Fixed))
                {
                    vehicleList.Append(string.Format("{0}. {1}{2}", index++, entry.Key, Environment.NewLine));
                }

                if (i_ShowPaid && entry.Value.VehicleStatus.Equals(eServiceStatuses.Paid))
                {
                    vehicleList.Append(string.Format("{0}. {1}{2}", index++, entry.Key, Environment.NewLine));
                }
            }

            if (vehicleList.ToString().Equals(string.Empty))
            {
                vehicleList.Append("No vehicles found!");
            }

            return vehicleList.ToString();
        }

        public bool ChangeServiceStatus(string i_LicenseNumber, int i_NewStatus)
        {
            bool isChanged = false;

            if (isExist(i_LicenseNumber))
            {
                if (Enum.IsDefined(typeof(eServiceStatuses), i_NewStatus))
                {
                    try
                    {
                        r_Vehicles[i_LicenseNumber].VehicleStatus = (eServiceStatuses)i_NewStatus;
                        isChanged = true;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("License Number {0} is not in the system!", i_LicenseNumber), ex);
                    }
                }
                else
                {
                    Exception ex = new Exception("Choice of vehicle status is invalid!");
                    throw new ValueOutOfRangeException(ex, (float)AmountOfServiceStatuses(), 1f);
                }
            }

            return isChanged;
        }

        public bool InflateWheels(string i_LicenseNumber)
        {
            bool isInflated = false;
            float amountToAdd = 0f;

            if (isExist(i_LicenseNumber))
            {
                if (r_Vehicles[i_LicenseNumber].VehicleStatus.Equals(eServiceStatuses.InRepair))
                {
                    try
                    {
                        Wheel[] currentWheels = r_Vehicles[i_LicenseNumber].Vehicle.Wheels;

                        for (int i = 0; i < currentWheels.Length; i++)
                        {
                            amountToAdd = currentWheels[i].MaxAirPressure - currentWheels[i].CurrentAirPressure;
                            currentWheels[i].InflatingWheel(amountToAdd);
                        }

                        isInflated = true;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("License Number {0} is not in the system", i_LicenseNumber), ex);
                    }
                }
                else
                {
                    throw new ArgumentException("The vehicle is not 'In-Repair' status. You can not work on this vehicle.");
                }
            }

            return isInflated;
        }

        public bool FillFuelTank(string i_LicenseNumber, int io_FuelType, float io_AmountToAdd)
        {
            bool isFilled = false;

            if (isExist(i_LicenseNumber))
            {
                if (r_Vehicles[i_LicenseNumber].VehicleStatus.Equals(eServiceStatuses.InRepair))
                {
                    if (Enum.IsDefined(typeof(eFuelTypes), io_FuelType))
                    {
                        if (r_Vehicles[i_LicenseNumber].Vehicle.EngineType is Fuel)
                        {
                            Fuel currrentEngine = r_Vehicles[i_LicenseNumber].Vehicle.EngineType as Fuel;
                            isFilled = currrentEngine.FillTank(io_AmountToAdd, (eFuelTypes)io_FuelType);

                            if (isFilled)
                            {
                                float percent = currrentEngine.CurrentFuelTank / currrentEngine.MaxTank;
                                r_Vehicles[i_LicenseNumber].Vehicle.RemainingEnergyPercentage = percent * 100f;
                            }
                        }
                        else
                        {
                            throw new FormatException("The vehicle engine is not runnig on fuel.");
                        }
                    }
                    else
                    {
                        Exception ex = new Exception("Choice of fuel type is invalid!");
                        throw new ValueOutOfRangeException(ex, (float)AmountOfFuelTypes(), 1f);
                    }
                }
                else
                {
                    throw new ArgumentException("The vehicle is not 'In-Repair' status. You can not work on this vehicle!");
                }
            }

            return isFilled;
        }

        public bool ChargeElectricBattery(string i_LicenseNumber, float io_MinutesToAdd)
        {
            bool isFilled = false;

            if (isExist(i_LicenseNumber))
            {
                if (r_Vehicles[i_LicenseNumber].VehicleStatus.Equals(eServiceStatuses.InRepair))
                {
                    if (r_Vehicles[i_LicenseNumber].Vehicle.EngineType is Electric)
                    {
                        Electric currentEngine = r_Vehicles[i_LicenseNumber].Vehicle.EngineType as Electric;
                        isFilled = currentEngine.ChargeBattery(io_MinutesToAdd / 60f);

                        if (isFilled)
                        {
                            float percent = currentEngine.BatteryRemainingHours / currentEngine.MaxHoursInBattery;
                            r_Vehicles[i_LicenseNumber].Vehicle.RemainingEnergyPercentage = percent * 100f;
                        }
                    }
                    else
                    {
                        throw new FormatException("The vehicle engine is not electric!");
                    }
                }
                else
                {
                    throw new ArgumentException("The vehicle is not 'In-Repair' status. You can not work on this vehicle!");
                }
            }

            return isFilled;
        }

        public bool VehicleInfo(string i_LicenseNumber, out string io_VehicleInfo)
        {
            bool found = false;

            StringBuilder vehicleInfo = new StringBuilder();

            if (isExist(i_LicenseNumber))
            {
                vehicleInfo.Append(r_Vehicles[i_LicenseNumber].ToString());
                found = true;
            }

            io_VehicleInfo = vehicleInfo.ToString();
            return found;
        }

        public Dictionary<eDataInputFields, object> GetExtraInfo(int io_VehicleChoice)
        {
            //// For UI - gets vehicle choice out of supported vehicles.
            //// Return a dictionary where key is Enum of eQusetion and data is object for the UI to insert.
            //// eQusetion helps UI to know what information is needed from user.
            //// Enum of eQusetion can be found in SupportedVehicles Class.

            VehicleCreation.GetInfo(out Dictionary<eDataInputFields, object> DicToFill, io_VehicleChoice);
            return DicToFill;
        }

        public void InsertInfo(Dictionary<eDataInputFields, object> i_FilledDictionary, string io_LicenseNumber)
        {
            //// For UI - gets FILLED dictionary that was given to UI by Garage::GetExtraInfo
            //// and license number of car needed for extra information.
            //// If value is invalid - an exception will be thrown.

            VehicleCreation.FillVehicleInfo(r_Vehicles[io_LicenseNumber].Vehicle, i_FilledDictionary);
        }

        private bool isExist(string io_LicenseNumber)
        {
            return r_Vehicles.ContainsKey(io_LicenseNumber);
        }

        public int AmountOfSupportedVehicles()
        {
            return VehicleCreation.ArrayOfSupportedVehicles.Length;
        }

        public int AmountOfServiceStatuses()
        {
            return Enum.GetValues(typeof(eServiceStatuses)).Length;
        }

        public int AmountOfFuelTypes()
        {
            return Enum.GetValues(typeof(eFuelTypes)).Length;
        }

        public string ShowSupportedVehicles()
        {
            return VehicleCreation.ShowSupportedVehiclesTypes();
        }

        public string ShowFuelTypes()
        {
            return Fuel.ShowFuelTypes();
        }

        public string ShowCarDoors()
        {
            return Car.ShowDoorsOptions();
        }

        public string ShowCarColors()
        {
            return Car.ShowColorsOptions();
        }

        public string ShowMotorcycleLicenseTypes()
        {
            return Motorcycle.ShowLicenseTypes();
        }

        public string ShowOptionsOfServiceStatuses()
        {
            return GarageCustomer.ShowServiceStatuses();
        }
    }
}
