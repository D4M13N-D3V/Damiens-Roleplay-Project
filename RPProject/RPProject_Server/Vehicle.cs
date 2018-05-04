using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace roleplay
{
    public enum VehicleStatuses { Stored, Impounded, Missing, Stolen, Destroyed };

    /// <summary>
    /// Colors class for the neon lights.
    /// </summary>
    public class NeonsColor
    {
        public int Red = 255;
        public int Green = 255;
        public int Blue = 255;

        /// <summary>
        /// Creates a neon color
        /// </summary>
        /// <param name="r">Red</param>
        /// <param name="g">Green</param>
        /// <param name="b">Blue</param>
        public NeonsColor(int r,int g,int b)
        {
            Red = r;
            Green = g;
            Blue = b;
        }
    }

    public class Vehicle
    {
        /// <summary>
        /// Creates a new vehicle for the users class
        /// </summary>
        /// <param name="name">The vehicle name</param>
        /// <param name="model">The model unhashed</param>
        /// <param name="status">The status of the vehicle</param>
        /// <param name="price">The price of the vehicle</param>
        /// <param name="plate">The plate of the vehicle</param>
        /// <param name="color1">The primary color</param>
        /// <param name="color2">The secondary color</param>
        public Vehicle(string name, string model, VehicleStatuses status, int price, string plate, int color1, int color2 )
        {
            VehicleName = name;
            VehicleModel = model;
            Status = status;
            Price = price;
            SellPrice = price / 2;
            InsurancePrice = price / 4;
            Plate = plate;
            ColorPrimary = color1;
            ColorSecondary = color2;
        }

        /// <summary>
        /// The name of the vehicle.
        /// </summary>
        public string VehicleName = "Vehicle Name";

        /// <summary>
        /// The model of the vehicle
        /// </summary>
        public string VehicleModel = "vehicleModel";

        /// <summary>
        /// The current status of the vehicle.
        /// </summary>
        public VehicleStatuses Status = VehicleStatuses.Stored;

        /// <summary>
        /// The price of the vehicle
        /// </summary>
        public int Price = 0;

        /// <summary>
        /// The sell price of the vehicle ( 1/2 of price)
        /// </summary>
        public int SellPrice = 0;

        /// <summary>
        /// The insurance price of the vehicle. ( 1/4 of the insurance 
        /// </summary>
        public int InsurancePrice = 0;

        /// <summary>
        /// The plate number of the vehicle
        /// </summary>
        public string Plate = "0000000";

        /// <summary>
        /// The primary color of the vehicle
        /// </summary>
        public int ColorPrimary = 0;

        /// <summary>
        /// The secondary color of the vehicle.
        /// </summary>
        public int ColorSecondary = 0;

        /// <summary>
        /// The color of the neon lights on the car
        /// </summary>
        public NeonsColor NeonColors = new NeonsColor(255,255,255);
        
        /// <summary>
        /// If the front neons are enabled.
        /// </summary>
        public bool FrontNeons = false;

        /// <summary>
        /// IF the rear neons are enabled
        /// </summary>
        public bool BackNeons = false;
        
        /// <summary>
        /// If the left neons are enabled.
        /// </summary>
        public bool LeftNeons = false;

        /// <summary>
        /// If the right neons are enabled
        /// </summary>
        public bool RightNeons = false;

        /// <summary>
        /// The level of tint the window has 
        /// </summary>
        public int WindowTint = 0;
        
        /// <summary>
        /// What type of wheels the vehicle has.
        /// </summary>
        public int WheelType = 0;

        /// <summary>
        /// Does it have bullet proof tires.
        /// </summary>
        public float BulletProof = 0;

        public int VehicleMod1 = -1;
        public int VehicleMod2 = -1;
        public int VehicleMod3 = -1;
        public int VehicleMod4 = -1;
        public int VehicleMod5 = -1;
        public int VehicleMod6 = -1;
        public int VehicleMod7 = -1;
        public int VehicleMod8 = -1;
        public int VehicleMod9 = -1;
        public int VehicleMod10 = -1;
        public int VehicleMod11 = -1;
        public int VehicleMod12 = -1;
        public int VehicleMod13 = -1;
        public int VehicleMod14 = -1;
        public int VehicleMod15 = -1;
        public int VehicleMod16 = -1;
        public int VehicleMod17 = -1;
        public int VehicleMod18 = -1;
        public int VehicleMod19 = -1;
        public int VehicleMod20 = -1;
        public int VehicleMod21 = -1;
        public int VehicleMod22 = -1;
        public int VehicleMod23 = -1;
        public int VehicleMod24 = -1;
        public int VehicleMod25 = -1;
        public int VehicleMod26 = -1;
        public int VehicleMod27 = -1;
        public int VehicleMod28 = -1;
        public int VehicleMod29 = -1;
        public int VehicleMod30 = -1;
        public int VehicleMod31 = -1;
        public int VehicleMod32 = -1;
        public int VehicleMod33 = -1;
        public int VehicleMod34 = -1;
        public int VehicleMod35 = -1;
        public int VehicleMod36 = -1;
        public int VehicleMod37 = -1;
        public int VehicleMod38 = -1;
        public int VehicleMod39 = -1;
        public int VehicleMod40 = -1;
        public int VehicleMod41 = -1;
        public int VehicleMod42 = -1;
        public int VehicleMod43 = -1;
        public int VehicleMod44 = -1;
        public int VehicleMod45 = -1;
        public int VehicleMod46 = -1;
        public int VehicleMod47 = -1;
        public int VehicleMod48 = -1;
        public int VehicleMod49 = -1;
        public int VehicleMod50 = -1;
    }
}
