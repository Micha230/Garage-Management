using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex03.GarageLogic
{
    internal class Wheel
    {
        private readonly float r_MaxAirPressure;
        private string m_ManufacturerName;
        private float m_CurrentAirPressure;

        internal Wheel(float i_MaxAirPressure)
        {
            r_MaxAirPressure = i_MaxAirPressure;
        }

        internal float MaxAirPressure
        {
            get { return r_MaxAirPressure; }
        }

        internal string ManufacturerName
        {
            get { return m_ManufacturerName; }
            set
            {
                if (value != string.Empty)
                {
                    m_ManufacturerName = value;
                }
                else
                {
                    throw new FormatException("Manufacturer's name is invalid!");
                }
            }
        }

        internal float CurrentAirPressure
        {
            get { return m_CurrentAirPressure; }
            set
            {
                if (value >= 0 && value <= MaxAirPressure)
                {
                    m_CurrentAirPressure = value;
                }
                else
                {
                    Exception ex = new Exception("Amount of wheel's air pressure is invalid!");
                    throw new ValueOutOfRangeException(ex, MaxAirPressure, 0f);
                }
            }
        }

        internal void InflatingWheel(float io_AddedPressure)
        {
            if (io_AddedPressure + CurrentAirPressure <= MaxAirPressure)
            {
                CurrentAirPressure += io_AddedPressure;
            }
            else
            {
                Exception ex = new Exception("Amount of air pressure is above the maximum!");
                throw new ValueOutOfRangeException(ex, MaxAirPressure - CurrentAirPressure, 0f);
            }
        }

        public override string ToString()
        {
            string wheelInfo = string.Format(
                "Name of manufacturer is {0}, current air pressure is {1} out of {2}.{3}",
                ManufacturerName,
                CurrentAirPressure,
                MaxAirPressure,
                Environment.NewLine);

            return wheelInfo;
        }
    }
}
