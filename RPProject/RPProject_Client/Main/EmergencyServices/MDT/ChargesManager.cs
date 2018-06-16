using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace client.Main.EmergencyServices.MDT
{
    public class ChargesManager : BaseScript
    {

        public static ChargesManager Instance;

        public ChargesManager()
        {
            Instance = this;
        }

        public List<Charge> Charges = new List<Charge>()
        {
            #region Felonys
            new Charge("Murder of an LEO", ChargeTypes.Felony,  70000, 80),
            new Charge("1st Degree Murder", ChargeTypes.Felony,  50000, 60),
            new Charge("2nd Degree Murder", ChargeTypes.Felony,  40000, 60),
            new Charge("Manslaughter", ChargeTypes.Felony,  20000, 30),
            new Charge("Vehicular Manslaughter", ChargeTypes.Felony,  70000, 30),
            new Charge("Attempted Murder", ChargeTypes.Felony,  70000, 30),
            new Charge("Assault of an LEO", ChargeTypes.Felony,  70000, 15),
            new Charge("Threat of Bodily Harm to an LEO", ChargeTypes.Felony,  70000, 10),
            new Charge("Assault with Deadly Weapon", ChargeTypes.Felony,  70000, 15),
            new Charge("Assault and Battery", ChargeTypes.Felony,  70000, 12),
            new Charge("Threat of Bodily Harm", ChargeTypes.Felony,  70000, 10),
            new Charge("Armed Robbery (Bank)", ChargeTypes.Felony,  70000, 30),
            new Charge("Armed Robbery (Store)", ChargeTypes.Felony,  70000, 30),
            new Charge("Unarmed Robbery", ChargeTypes.Felony,  70000, 20),
            new Charge("Attempted Robbery", ChargeTypes.Felony,  70000, 15),
            new Charge("Grand Theft Auto", ChargeTypes.Felony,  70000, 12),
            new Charge("Poss. of a Stolen Vehicle", ChargeTypes.Felony,  70000, 8),
            new Charge("Felony Evading Law Enforcment", ChargeTypes.Felony,  70000, 15),
            new Charge("Driving Under the Influence", ChargeTypes.Felony,  70000, 10),
            new Charge("Hit and Run", ChargeTypes.Felony,  70000, 10),
            new Charge("Reckless Endangerment", ChargeTypes.Felony,  70000, 8),
            new Charge("Tampering of Evidence", ChargeTypes.Felony,  70000, 7),
            new Charge("Attempted Bribery", ChargeTypes.Felony,  70000, 5),
            new Charge("Public Discharge of a Firearm", ChargeTypes.Felony,  70000, 6),
            new Charge("Damage to State Property", ChargeTypes.Felony,  70000, 15),
            new Charge("Poss. of a Class A Narcotic w/ Intent to Sell", ChargeTypes.Felony,  70000, 5),
            new Charge("Damage to state property", ChargeTypes.Felony,  70000, 7),
            new Charge("Poss. of Controlled Substance", ChargeTypes.Felony,  70000, 20),
            new Charge("Money Laundering", ChargeTypes.Felony,  70000, 12),
            new Charge("Rioting 1st Degree -Inciting", ChargeTypes.Felony,  70000, 9),
            new Charge("Disturbing the Peace", ChargeTypes.Felony,  70000, 3),
            new Charge("Human Trafficking", ChargeTypes.Felony,  70000, 12),
            new Charge("Kidnapping", ChargeTypes.Felony,  70000, 9),
            new Charge("Attempted Kidnapping", ChargeTypes.Felony,  70000, 7),
            new Charge("Obstruction of Justice", ChargeTypes.Felony,  70000, 5),
            new Charge("Offenses Against Public Order", ChargeTypes.Felony,  70000, 10),
            #endregion

            #region Misdemeanors
            new Charge("Domestic Violence", ChargeTypes.Misdemeanors, 6, 5000),
            new Charge("Domestic Disturbance", ChargeTypes.Misdemeanors, 6, 5000),
            new Charge("Attempted Grand Theft Auto", ChargeTypes.Misdemeanors, 6, 5000),
            new Charge("Fleeing/Eluding Law Enforcment", ChargeTypes.Misdemeanors, 6, 5000),
            new Charge("Reckless Driving", ChargeTypes.Misdemeanors, 6, 5000),
            new Charge("Identity Theft", ChargeTypes.Misdemeanors, 6, 5000),
            new Charge("Disorderly Conduct", ChargeTypes.Misdemeanors, 6, 5000),
            new Charge("False Identification", ChargeTypes.Misdemeanors, 6, 5000),
            new Charge("Rioting 2nd Degree", ChargeTypes.Misdemeanors, 6, 5000),
            new Charge("Prostitution", ChargeTypes.Misdemeanors, 6, 5000),
            new Charge("Indecent Exposure", ChargeTypes.Misdemeanors, 6, 5000),
            new Charge("J Walking", ChargeTypes.Misdemeanors, 6, 5000),
            new Charge("Loitering", ChargeTypes.Misdemeanors, 6, 5000),
            new Charge("Illegal Gambling", ChargeTypes.Misdemeanors, 6, 5000),
            new Charge("Trespassing", ChargeTypes.Misdemeanors, 6, 5000),
            new Charge("Criminal Mischief", ChargeTypes.Misdemeanors, 6, 5000),
            new Charge("Violating Mask Law", ChargeTypes.Misdemeanors, 6, 5000),
            #endregion

            #region Traffic
            new Charge("Failure to Stop  ( Red Light )", ChargeTypes.Traffic,0,100),
            new Charge("Failure to Stop  ( Stop Sign )", ChargeTypes.Traffic,0,100),
            new Charge("No Insurance/Expired Registration", ChargeTypes.Traffic,0,500),
            new Charge("Speeding 10-30MPH", ChargeTypes.Traffic,0,500),
            new Charge("Speeding 30MPH+", ChargeTypes.Traffic,0,2500),
            new Charge("Careless Driving", ChargeTypes.Traffic,0,1000),
            new Charge("Illegal U-Turn", ChargeTypes.Traffic,0,250),
            new Charge("Failure to Yield to Emergency Services", ChargeTypes.Traffic,0,300),
            new Charge("Failure to Give Right of Way", ChargeTypes.Traffic,0,300),
            new Charge("Failure to Signal", ChargeTypes.Traffic,0,50),
            new Charge("Broken Tail Light", ChargeTypes.Traffic,0,50),
            new Charge("Broken Head Light", ChargeTypes.Traffic,0,50),
            new Charge("Illegal Parking", ChargeTypes.Traffic,0,100),
            new Charge("Parked Near Fire Hydrant", ChargeTypes.Traffic,0,250),
            new Charge("Failure to yield to a Pedestrian", ChargeTypes.Traffic,0,200),
            new Charge("Failure to Obey LEO Signals", ChargeTypes.Traffic,0,400),
            new Charge("Improper/Unsafe Turn", ChargeTypes.Traffic,0,150),
            new Charge("Improper/Missing Plate", ChargeTypes.Traffic,0,200),
            new Charge("Wrong Way", ChargeTypes.Traffic,0,750),
            new Charge("Improper/Illegal Pass", ChargeTypes.Traffic,0,500),
            #endregion
        };

        private List<Charge> _charges = new List<Charge>();
#pragma warning disable CS0649 // Field 'ChargesManager._chargesInfo' is never assigned to, and will always have its default value null
        private ChargesInfo _chargesInfo;
#pragma warning restore CS0649 // Field 'ChargesManager._chargesInfo' is never assigned to, and will always have its default value null

        public void AddCharge(Charge charge)
        {
            _charges.Add(charge);
            Recalculate();
        }

        public void RemoveCharge(Charge charge)
        {
            _charges.Remove(charge);
            Recalculate();
        }

        public void ClearCharges()
        {
            _charges.Clear();
            Recalculate();
        }

        public ChargesInfo GetCharges()
        {
            return _chargesInfo;
        }

        private void Recalculate()
        {
            _chargesInfo.TotalFine = 0;
            _chargesInfo.TotalTime = 0;
            _chargesInfo.Charges = "";
            foreach (var charge in _charges)
            {
                _chargesInfo.TotalFine += charge.Fine;
                _chargesInfo.TotalTime += charge.Time;
                _chargesInfo.Charges += charge.Title + ",";
            }
        }

    }
}
