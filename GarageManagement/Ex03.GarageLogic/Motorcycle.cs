using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex03.GarageLogic
{
    public enum eLicenseTypes
    {
        A = 1,
        A1,
        AA,
        B
    }

    internal class Motorcycle : Vehicle
    {
        private eLicenseTypes m_LicenseType;
        private int m_EngineVolumeInCC;

        internal Motorcycle(string i_LicenseNumber, uint i_NumOfWheels, float io_MaxAirPressure, object i_EngineType) :
        base(i_LicenseNumber, i_NumOfWheels, io_MaxAirPressure, i_EngineType)
        {
        }

        internal eLicenseTypes LicenseType
        {
            get { return m_LicenseType; }
            set
            {
                if (Enum.IsDefined(typeof(eLicenseTypes), value))
                {
                    m_LicenseType = value;
                }
                else
                {
                    Exception ex = new Exception("License type is invalid!");
                    throw new ValueOutOfRangeException(ex, (float)Enum.GetValues(typeof(eLicenseTypes)).Length, 1f);
                }
            }
        }

        internal int EngineVolumeInCC
        {
            get { return m_EngineVolumeInCC; }
            set
            {
                if (value >= 0)
                {
                    m_EngineVolumeInCC = value;
                }
                else
                {
                    throw new ArgumentException("Engine capacity is invalid!");
                }
            }
        }

        internal static string ShowLicenseTypes()
        {
            StringBuilder licenseTypes = new StringBuilder();

            foreach (eLicenseTypes licenseType in Enum.GetValues(typeof(eLicenseTypes)))
            {
                licenseTypes.Append(string.Format("{0}. {1}{2}", (int)licenseType, licenseType.ToString(), Environment.NewLine));
            }

            return licenseTypes.ToString();
        }

        public override string ToString()
        {
            string motorcycleInfo = string.Format(
                "{0}The motorcycle license type is {1} and the engine volume is {2} CC{3}",
                base.ToString(),
                LicenseType.ToString(),
                EngineVolumeInCC,
                Environment.NewLine);

            return motorcycleInfo;
        }
    }
}
