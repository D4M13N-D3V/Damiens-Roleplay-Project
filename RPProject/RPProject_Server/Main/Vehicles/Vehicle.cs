namespace roleplay.Main.Vehicles
{
    public enum VehicleStatuses { Stored, Impounded, Missing, Stolen, Destroyed };


    public class NeonsColor
    {
        public int Red = 255;
        public int Green = 255;
        public int Blue = 255;

        public NeonsColor(int r,int g,int b)
        {
            Red = r;
            Green = g;
            Blue = b;
        }
    }

    public class Vehicle
    {

        public Vehicle(string name, string model, VehicleStatuses status, int price, string plate)
        {
            Name = name;
            Model = model;
            Status = status;
            Price = price;
            SellPrice = price / 2;
            InsurancePrice = price / 4;
            Plate = plate;
        }
       

        public string Name = "Vehicle Name";
        public string Model = "vehicleModel";
        public string RegisteredOwner = "None";
        public VehicleStatuses Status = VehicleStatuses.Stored;
        public int Price = 0;
        public int SellPrice = 0;
        public int InsurancePrice = 0;
        public bool Insured = false;
        public string Plate = "0000000";
        public int ColorPrimary = 0;
        public int ColorSecondary = 0;
        public NeonsColor NeonColors = new NeonsColor(255,255,255);
        public bool FrontNeons = false;
        public bool BackNeons = false;
        public bool LeftNeons = false;
        public bool RightNeons = false;
        public int WindowTint = 0;
        public int WheelType = 0;
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
