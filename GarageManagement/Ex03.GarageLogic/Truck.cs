using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex03.GarageLogic
{
    internal class Truck : Vehicle
    {
        private bool m_IsTransportHazardousMaterials;
        private float m_CargoVolume;

        internal Truck(string i_LicenseNumber, uint i_NumOfWheels, float io_MaxAirPressure, object i_EngineType) :
            base(i_LicenseNumber, i_NumOfWheels, io_MaxAirPressure, i_EngineType)
        {
        }

        internal bool IsTransportHazardousMaterials
        {
            get { return m_IsTransportHazardousMaterials; }
            set { m_IsTransportHazardousMaterials = value; }
        }

        internal float CargoVolume
        {
            get { return m_CargoVolume; }
            set
            {
                if (value >= 0)
                {
                    m_CargoVolume = value;
                }
                else
                {
                    throw new ArgumentException("Cargo volume is invalid!");
                }
            }
        }

        public override string ToString()
        {
            string transportHazardousMessage = string.Empty;

            if (IsTransportHazardousMaterials)
            {
                transportHazardousMessage = "does";
            }
            else
            {
                transportHazardousMessage = "does not";
            }

            string truck = string.Format(
                "{0}The truck {1} contains hazardous materials and the cargo volume is {2}{3}",
                base.ToString(),
                transportHazardousMessage,
                CargoVolume,
                Environment.NewLine);

            return truck;
        }
    }
}
