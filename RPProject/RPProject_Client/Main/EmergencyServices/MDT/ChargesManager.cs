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
            new Charge("Vehicular Manslaughter", ChargeTypes.Felony,  15000, 30),
            new Charge("Attempted Murder", ChargeTypes.Felony,  15000, 30),
            new Charge("Assault of an LEO", ChargeTypes.Felony,  4500, 15),
            new Charge("Threat of Bodily Harm to an LEO", ChargeTypes.Felony,  4000, 10),
            new Charge("Assault with Deadly Weapon", ChargeTypes.Felony,  4000, 15),
            new Charge("Assault and Battery", ChargeTypes.Felony,  3500, 12),
            new Charge("Threat of Bodily Harm", ChargeTypes.Felony,  1500, 10),
            new Charge("Armed Robbery (Bank)", ChargeTypes.Felony,  25000, 30),
            new Charge("Armed Robbery (Store)", ChargeTypes.Felony,  15000, 30),
            new Charge("Unarmed Robbery", ChargeTypes.Felony,  10000, 20),
            new Charge("Attempted Robbery", ChargeTypes.Felony,  5000, 15),
            new Charge("Grand Theft Auto", ChargeTypes.Felony,  5000, 12),
            new Charge("Poss. of a Stolen Vehicle", ChargeTypes.Felony,  2500, 8),
            new Charge("Felony Evading Law Enforcment", ChargeTypes.Felony,  5000, 15),
            new Charge("Driving Under the Influence", ChargeTypes.Felony,  2500, 10),
            new Charge("Hit and Run", ChargeTypes.Felony,  7500, 10),
            new Charge("Reckless Endangerment", ChargeTypes.Felony,  3000, 8),
            new Charge("Tampering of Evidence", ChargeTypes.Felony,  4000, 7),
            new Charge("Attempted Bribery", ChargeTypes.Felony,  5000, 5),
            new Charge("Public Discharge of a Firearm", ChargeTypes.Felony,  5000, 6),
            new Charge("Poss. of an Illegal Firearm/Weapon", ChargeTypes.Felony,  15000, 15),
            new Charge("Damage to State Property", ChargeTypes.Felony,  5000, 7),
            new Charge("Poss. of a Class A Narcotic w/ Intent to Sell", ChargeTypes.Felony,  30000, 5),
            new Charge("Poss. of Controlled Substance", ChargeTypes.Felony,  15000, 20),
            new Charge("Money Laundering", ChargeTypes.Felony,  20000, 12),
            new Charge("Rioting 1st Degree -Inciting", ChargeTypes.Felony,  5000, 9),
            new Charge("Disturbing the Peace", ChargeTypes.Felony,  1000, 3),
            new Charge("Human Trafficking", ChargeTypes.Felony,  10000, 12),
            new Charge("Kidnapping", ChargeTypes.Felony,  7500, 9),
            new Charge("Attempted Kidnapping", ChargeTypes.Felony,  2500, 7),
            new Charge("Obstruction of Justice", ChargeTypes.Felony,  1000, 5),
            new Charge("Offenses Against Public Order", ChargeTypes.Felony,  3000, 10),
            #endregion

            #region Misdemeanors
            new Charge("Domestic Violence", ChargeTypes.Misdemeanors, 5000, 6),
            new Charge("Domestic Disturbance", ChargeTypes.Misdemeanors, 5000, 6),
            new Charge("Attempted Grand Theft Auto", ChargeTypes.Misdemeanors, 5000, 6),
            new Charge("Fleeing/Eluding Law Enforcment", ChargeTypes.Misdemeanors, 5000, 6),
            new Charge("Reckless Driving", ChargeTypes.Misdemeanors, 5000, 6),
            new Charge("Identity Theft", ChargeTypes.Misdemeanors, 5000, 6),
            new Charge("Disorderly Conduct", ChargeTypes.Misdemeanors, 5000, 6),
            new Charge("False Identification", ChargeTypes.Misdemeanors, 5000, 6),
            new Charge("Rioting 2nd Degree", ChargeTypes.Misdemeanors, 5000, 6),
            new Charge("Prostitution", ChargeTypes.Misdemeanors, 5000, 6),
            new Charge("Indecent Exposure", ChargeTypes.Misdemeanors, 5000, 6),
            new Charge("J Walking", ChargeTypes.Misdemeanors, 5000, 6),
            new Charge("Loitering", ChargeTypes.Misdemeanors, 5000, 6),
            new Charge("Illegal Gambling", ChargeTypes.Misdemeanors, 5000, 6),
            new Charge("Trespassing", ChargeTypes.Misdemeanors, 5000, 6),
            new Charge("Criminal Mischief", ChargeTypes.Misdemeanors, 5000, 6),
            new Charge("Violating Mask Law", ChargeTypes.Misdemeanors, 5000, 6),
            #endregion

            #region Traffic
            new Charge("Failure to Stop  ( Red Light )", ChargeTypes.Traffic,100,0),
            new Charge("Failure to Stop  ( Stop Sign )", ChargeTypes.Traffic,100,0),
            new Charge("No Insurance/Expired Registration", ChargeTypes.Traffic,500,0),
            new Charge("Speeding 10-30MPH", ChargeTypes.Traffic,500,0),
            new Charge("Speeding 30MPH+", ChargeTypes.Traffic,2500,0),
            new Charge("Careless Driving", ChargeTypes.Traffic,1000,0),
            new Charge("Illegal U-Turn", ChargeTypes.Traffic,250,0),
            new Charge("Failure to Yield to Emergency Services", ChargeTypes.Traffic,300,0),
            new Charge("Failure to Give Right of Way", ChargeTypes.Traffic,300,0),
            new Charge("Failure to Signal", ChargeTypes.Traffic,50,0),
            new Charge("Broken Tail Light", ChargeTypes.Traffic,50,0),
            new Charge("Broken Head Light", ChargeTypes.Traffic,50,0),
            new Charge("Illegal Parking", ChargeTypes.Traffic,100,0),
            new Charge("Parked Near Fire Hydrant", ChargeTypes.Traffic,250,0),
            new Charge("Failure to yield to a Pedestrian", ChargeTypes.Traffic,200,0),
            new Charge("Failure to Obey LEO Signals", ChargeTypes.Traffic,400,0),
            new Charge("Improper/Unsafe Turn", ChargeTypes.Traffic,150,0),
            new Charge("Improper/Missing Plate", ChargeTypes.Traffic,200,0),
            new Charge("Wrong Way", ChargeTypes.Traffic,750,0),
            new Charge("Improper/Illegal Pass", ChargeTypes.Traffic,500,0),
            #endregion
        };

        private List<Charge> _charges = new List<Charge>();
#pragma warning disable CS0649 // Field 'ChargesManager._chargesInfo' is never assigned to, and will always have its default value null
        private ChargesInfo _chargesInfo = new ChargesInfo(string.Empty,0,0);
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
            _chargesInfo.Charges = string.Empty;
            foreach (var charge in _charges)
            {
                _chargesInfo.TotalFine += charge.Fine;
                _chargesInfo.TotalTime += charge.Time;
                _chargesInfo.Charges += charge.Title + ",";
            }
        }

    }
}
