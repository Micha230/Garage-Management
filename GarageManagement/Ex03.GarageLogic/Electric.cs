using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex03.GarageLogic
{
    internal class Electric
    {
        private readonly float r_MaxBatteryHours;
        private float m_BatteryRemainingHours;

        internal Electric(float i_MaxBatteryHours)
        {
            r_MaxBatteryHours = i_MaxBatteryHours;
            m_BatteryRemainingHours = 0;
        }

        internal float MaxHoursInBattery
        {
            get { return r_MaxBatteryHours; }
        }

        internal float BatteryRemainingHours
        {
            get { return m_BatteryRemainingHours; }
            set
            {
                if (value >= 0 && value <= MaxHoursInBattery)
                {
                    m_BatteryRemainingHours = value;
                }
                else
                {
                    Exception ex = new Exception("Amount of hours is invalid!");
                    throw new ValueOutOfRangeException(ex, MaxHoursInBattery, 0f);
                }
            }
        }

        internal bool ChargeBattery(float io_HoursToAdd)
        {
            bool charged = false;

            if (io_HoursToAdd + BatteryRemainingHours <= MaxHoursInBattery)
            {
                BatteryRemainingHours += io_HoursToAdd;
                charged = true;
            }
            else
            {
                Exception ex = new Exception("Amount of hours to charge is invalid!");
                throw new ValueOutOfRangeException(ex, (MaxHoursInBattery - BatteryRemainingHours) * 60f, 0f);
            }

            return charged;
        }

        public override string ToString()
        {
            string electricInfo = string.Format(
                "The battay has {0} hours left out of {1} hours{2}",
                BatteryRemainingHours,
                MaxHoursInBattery,
                Environment.NewLine);

            return electricInfo;
        }

    }
}
