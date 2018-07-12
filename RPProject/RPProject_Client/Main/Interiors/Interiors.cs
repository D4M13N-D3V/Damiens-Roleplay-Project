using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace client.Main.Interiors
{
    public class Interiors : BaseScript
    {
        public static Interiors Instance;

        private List<Interior> _interiorList = new List<Interior>() {
            new Interior(new Vector3(442.05142211914f,-997.04083251953f,4.803505897522f), new Vector3( 1446.67474365234f,-986.51947021484f,26.674211502075f), "PD1"),
            new Interior(new Vector3(456.75201416016f,-1000.9271240234f,4.803505897522f), new Vector3( 456.86697387695f,-985.86614990234f,26.674213409424f), "PD2"),
            new Interior(new Vector3(452.13150024414f,-984.66253662109f,26.674213409424f), new Vector3(442.18240356445f,-986.87261962891f,4.803505897522f), "PD3"),
            new Interior(new Vector3( 1395.099f, 1141.742f, 114.8f), new Vector3( 1397.330f, 1142.050f, 114.5f), "Ranch Main"),
            new Interior(new Vector3( -1910.726f, -576.919f, 19.3f), new Vector3( -1910.099f, -574.972f, 19.3f), "Beach Office 1"),
            new Interior(new Vector3( -269.457f, -955.8555f, 31.5f), new Vector3( -269.782f, -941.065f, 92.8f), "Condo Luxury 1"),
            new Interior(new Vector3( -44.646f, -587.163f, 38.3f), new Vector3( -30.817f, -595.315f, 80.3f), "Condo Luxury 2"),
            new Interior(new Vector3( -43.931f, -584.379f, 38.3f), new Vector3( -18.440f, -591.497f, 90.3f), "Condo Luxury 3"),
            new Interior(new Vector3( -480.583f, -688.393f, 33.5f), new Vector3( -467.465f, -708.712f, 77.3f), "Condo Luxury 4"),
            new Interior(new Vector3( -770.619f, 318.839f, 85.8f), new Vector3( -784.695f, 323.346f, 212.2f), "Condo Luxury 5"),
            new Interior(new Vector3( 415.044f, -217.058f, 60.2f), new Vector3( 476.898f, -195.184f, 71.4f), "Terrace Hotel"),
            new Interior(new Vector3( -98.541f, 367.420f, 113.5f), new Vector3( -101.874f, 372.153f, 142.8f), "Terrace C"),
            new Interior(new Vector3( 119.249f, 564.305f, 184.2f), new Vector3( 117.411f, 559.382f, 184.5f), "Premium House 1"),
            new Interior(new Vector3( 373.699f, 427.730f, 145.8f), new Vector3( 373.523f, 423.222f, 146.1f), "Premium House 2"),
            new Interior(new Vector3( -174.951f, 502.270f, 137.6f), new Vector3( -174.111f, 497.260f, 137.8f), "Premium House 3"),
            new Interior(new Vector3( 346.895f, 440.671f, 147.9f), new Vector3( 341.693f, 437.470f, 149.5f), "Premium House 4"),
            new Interior(new Vector3( -635.626f, 44.295f, 42.8f), new Vector3( -603.831f, 58.761f, 98.4f), "Condo Luxury 6"),
            new Interior(new Vector3( -776.924f, 318.661f, 85.8f), new Vector3( -781.812f, 326.140f, 177.0f), "Condo Luxury 7"),
            new Interior(new Vector3( -1581.250f, -558.371f, 35.2f), new Vector3( -1582.834f, -558.808f, 108.7f), "Mayors Office"),
            new Interior(new Vector3( -1447.424f, -537.894f, 34.9f), new Vector3( -1449.844f, -525.866f, 57.2f), "Condo Luxury 10"),
            new Interior(new Vector3( -889.443f, -333.081f, 34.8f), new Vector3( -912.898f, -365.340f, 114.4f), "Condo Luxury"),
            new Interior(new Vector3( -901.707f, -339.162f, 34.8f), new Vector3( -907.138f, -372.565f, 109.6f), "Condo Luxury 11"),
            new Interior(new Vector3( -894.848f, -353.676f, 34.8f), new Vector3( -922.912f, -378.570f, 85.6f), "Condo Luxury 12"),
            new Interior(new Vector3( -844.549f, -391.217f, 31.6f), new Vector3( -907.707f, -453.481f, 126.7f), "Condo Luxury 13"),
            new Interior(new Vector3( -837.556f, -405.570f, 31.6f), new Vector3( -890.781f, -452.866f, 95.6f), "Condo Luxury 14"),
            new Interior(new Vector3( -3093.068f, 349.211f, 7.7f), new Vector3( -3094.154f, 339.901f, 11.1f), "Beach apartment 1"),
            new Interior(new Vector3( -3100.382f, 360.864f, 7.8f), new Vector3( -3094.473f, 340.733f, 14.6f), "Beach apartment 2"),
            new Interior(new Vector3(  -817.214f, 178.084f, 72.4f), new Vector3( -814.913f, 178.959f, 72.4f), "Michaels Mansion"),
            new Interior(new Vector3(  8.692f, 540.461f, 176.3f), new Vector3( 7.493f, 537.588f, 176.3f), "Franklin Mansion"),
            new Interior(new Vector3(  -14.135f, -1442.271f, 31.4f), new Vector3( -14.221f, -1439.709f, 31.4f), "Franklin House"),
            new Interior(new Vector3(  1973.747f, 3815.094f, 33.6f), new Vector3( 1972.815f, 3816.431f, 33.6f), "Trevor Trash Trailor"),
            new Interior(new Vector3(  1274.836f, -1721.286f, 54.8f), new Vector3( 1273.983f, -1719.372f, 54.9f), "Lesters Residence"),
            new Interior(new Vector3(  136.16485595703f, -761.90856933594f, 45.752025604248f), new Vector3( 134.5835f, -749.339f, 258.152f), "Cerberus Tower Floor 60"),
            new Interior(new Vector3(  138.94178771973f, -762.96014404297f, 45.752025604248f), new Vector3( 134.573f, -766.486f, 234.152f), "Cerberus Tower Floor 47"),
            new Interior(new Vector3(  140.91604614258f, -766.28350830078f, 45.752025604248f), new Vector3( 134.635f, -765.831f, 242.152f), "Cerberus Tower Floor 49"),
            new Interior(new Vector3(  136.56838989258f, -769.55541992188f, 45.752025604248f), new Vector3( 150.126f, -754.591f, 262.865f), "Cerberus Tower Roof"),
            new Interior(new Vector3(  -1056.7100830078f, -237.93852233887f, 44.021144866943f), new Vector3( -1058.5439453125f, -236.36503601074f, 44.021186828613f), "Life Invader Lounge"),
            new Interior(new Vector3(  -1047.9486083984f, -238.47183227539f, 44.021060943604f), new Vector3( -1046.4213867188f, -237.32202148438f, 44.021060943604f), "Life Invader Meeting"),
            new Interior(new Vector3(  -1390.6942138672f, -600.11590576172f, 30.319564819336f), new Vector3( -1391.1859130859f, -598.04339599609f, 30.315368652344f), "The Saints"),
            new Interior(new Vector3(126.27487945557f, -1282.6379394531f, 29.278224945068f), new Vector3( 128.93930053711f, -1280.5223388672f, 29.269550323486f), "Behind the bar"),
            new Interior(new Vector3(236.67063903809f, -408.21270751953f, 47.924362182617f), new Vector3( 236.101f, -413.360f, -118.150f), "Court House Reception"),
            new Interior(new Vector3( 225.338f, -419.716f, -118.150f), new Vector3( 238.794f, -334.078f, -118.760f), "Court Room"),
            new Interior(new Vector3( 241.035f, -304.235f, -118.794f), new Vector3( -1003.101f, -477.870f, 50.030f), "Judge Office"),
            new Interior(new Vector3( 246.438f, -337.090f, -118.794f), new Vector3( 248.171f, -337.797f, -118.794f), "Defence Hall"),
            new Interior(new Vector3( 257.839f, -326.372f, -118.799f), new Vector3( 240.234f,  -306.841f, -118.800f), "Judges Stand"),
        };

        private Interior _currentInterior;
        private bool _isNearInterior;
        public bool _nearInteriorIsOutside;

        public Interiors()
        {
            Instance = this;
            Logic();
        }

        private async Task Logic()
        {
            while (true)
            {
                _currentInterior = null;
                foreach (var interior in _interiorList)
                {
                    if (Utility.Instance.GetDistanceBetweenVector3s(
                            new Vector3(interior.Inside.X, interior.Inside.Y, interior.Inside.Z), Game.PlayerPed.Position) < 30)
                    {

                        if (Utility.Instance.GetDistanceBetweenVector3s(
                                new Vector3(interior.Inside.X, interior.Inside.Y, interior.Inside.Z), Game.PlayerPed.Position) < 1)
                        {
                            _currentInterior = interior;
                            _nearInteriorIsOutside = false;
                        }
                       
                        World.DrawMarker(MarkerType.UpsideDownCone, interior.Inside, Vector3.Zero, Vector3.Zero,
                            new Vector3(0.5f, 0.5f, 0.5f), Color.FromArgb(255, 0, 150, 0));
                    }

                    if (Utility.Instance.GetDistanceBetweenVector3s(
                            new Vector3(interior.Outside.X, interior.Outside.Y, interior.Outside.Z), Game.PlayerPed.Position) < 30)
                    {

                        if (Utility.Instance.GetDistanceBetweenVector3s(
                                new Vector3(interior.Outside.X, interior.Outside.Y, interior.Outside.Z), Game.PlayerPed.Position) < 1)
                        {
                            _currentInterior = interior;
                            _nearInteriorIsOutside = true;
                        }
                        
                        World.DrawMarker(MarkerType.UpsideDownCone, interior.Outside, Vector3.Zero, Vector3.Zero,
                            new Vector3(0.5f,0.5f,0.5f), Color.FromArgb(255, 0, 150, 0));
                    }
                }

                if (_currentInterior != null && Game.IsControlJustPressed(0, Control.Context))
                {
                    if (_nearInteriorIsOutside)
                    {
                        Game.PlayerPed.Position = _currentInterior.Inside;
                    }
                    else
                    {
                        Game.PlayerPed.Position = _currentInterior.Outside;
                    }
                }

                await Delay(0);
            }
        }

    }
}
