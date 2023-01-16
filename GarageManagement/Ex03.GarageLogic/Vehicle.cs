using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex03.GarageLogic
{
    internal class Vehicle
    {
        private readonly string r_LicenseNumber;
        private readonly Wheel[] r_Wheels;
        private readonly object r_EngineType;
        private string m_ModelName;
        private float m_RemainingEnergyPercentage;

        protected Vehicle(string i_LicenseNumber, uint i_NumOfWheels, float io_MaxAirPressure, object i_EngineType)
        {
            r_LicenseNumber = i_LicenseNumber;
            r_EngineType = i_EngineType;
            m_ModelName = string.Empty;
            m_RemainingEnergyPercentage = 0;
            r_Wheels = new Wheel[i_NumOfWheels];

            for(int i = 0; i<r_Wheels.Length; i++)
            {
                r_Wheels[i] = new Wheel(io_MaxAirPressure);
            }

        }

        internal Wheel[] Wheels
        {
            get { return r_Wheels; }
        }

        internal object EngineType
        {
            get { return r_EngineType; }
        }

        internal string LicenseNumber
        {
            get { return r_LicenseNumber; }
        }

        internal string ModelName
        {
            get { return m_ModelName; }
            set
            {
                if (value != string.Empty)
                {
                    m_ModelName = value;
                }
                else
                {
                    throw new FormatException("Invalid model name!");
                }
            }
        }

        internal float RemainingEnergyPercentage
        {
            get { return m_RemainingEnergyPercentage; }
            set
            {
                if (value >= 0 && value <= 100)
                {
                    m_RemainingEnergyPercentage = value;
                }
                else
                {
                    Exception ex = new Exception("The remaining energy percentages is invalid!");
                    throw new ValueOutOfRangeException(ex, 100f, 0f);
                }
            }
        }

        public override string ToString()
        {
            StringBuilder vehicleInfo = new StringBuilder();
            int index = 1;

            vehicleInfo.Append(string.Format(
                "License Number is: {0}{1}Model Name is: {2}{1}Wheels info: {1}",
                LicenseNumber,
                Environment.NewLine,
                ModelName));

            foreach (Wheel currentWheel in Wheels)
            {
                vehicleInfo.Append(string.Format("Wheel #{0}. {1}", index++, currentWheel.ToString()));
            }
            vehicleInfo.Append(string.Format("Enregy is ({0})% full. (Energy type: {1})", RemainingEnergyPercentage, r_EngineType.ToString()));

            return vehicleInfo.ToString();
        }
    }
}
