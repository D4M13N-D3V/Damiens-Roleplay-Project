using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using Newtonsoft.Json;

namespace roleplay.Main
{
    public enum LEODepartments
    {
        LSPD,
        BCSO,
        LSCSO,
        SASP,
        SAHP,
        SAAO,
        USMS,
        FBI,
        DEA
    }
    public class PoliceRank
    {
        public string Name = "None";
        public int Salary = 0;
        public LEODepartments Department = LEODepartments.LSPD;
        public List<string> WeaponLoadout = new List<string>()
        {

        };
        public List<string> AvailableVehicles = new List<string>()
        {

        };
        public bool CanUseHeli = true;
        public bool CanUseSpikeStrips = true;
        public bool CanUseRadar = true;
        public bool CanUseK9 = true;
        public bool CanUseEMS = true;
        public bool CanPromote = false;
    }

    public class Police : BaseScript
    {
        private Dictionary<string, Dictionary<int, List<int>>> _maleUniforms =
            new Dictionary<string, Dictionary<int, List<int>>>()
            {
                ["LSPD"] =
                {
                    [11] = new List<int>()
                    {
                        55,
                        0,
                        0
                    },
                    [4] = new List<int>()
                    {
                        25,
                        0,
                        0
                    },
                    [3] = new List<int>()
                    {
                        0,
                        0,
                        0
                    },
                    [6] = new List<int>()
                    {
                        25,
                        0,
                        0
                    },
                    [0] = new List<int>()
                    {
                        122,
                        0,
                        0
                    }
                },

                ["BCSO"] =
                {
                    [11] = new List<int>()
                    {
                        26,
                        2,
                        0
                    },
                    [7] = new List<int>()
                    {
                        125,
                        0,
                        0
                    },
                    [4] = new List<int>()
                    {
                        25,
                        0,
                        0
                    },
                    [3] = new List<int>()
                    {
                        11,
                        0,
                        0
                    },
                    [6] = new List<int>()
                    {
                        25,
                        0,
                        0
                    },
                    [0] = new List<int>()
                    {
                        122,
                        0,
                        0
                    }
                }
            };

        private Dictionary<string, Dictionary<int, List<int>>> _femaleUniforms =
            new Dictionary<string, Dictionary<int, List<int>>>()
            {
                ["LSPD"] =
                {
                    [3] = new List<int>()
                    {
                        0,
                        0,
                        0
                    },
                    [11] = new List<int>()
                    {
                        48,
                        0,
                        0
                    },
                    [4] = new List<int>()
                    {
                        34,
                        0,
                        0
                    },
                    [6] = new List<int>()
                    {
                        25,
                        0,
                        0
                    },
                    [0] = new List<int>()
                    {
                        152,
                        0,
                        0
                    },
                },

                ["BCSO"] =
                {
                    [7] = new List<int>()
                    {
                        0,
                        0,
                        0
                    },
                    [3] = new List<int>()
                    {
                        0,
                        0,
                        0
                    },
                    [11] = new List<int>()
                    {
                        48,
                        0,
                        0
                    },
                    [4] = new List<int>()
                    {
                        34,
                        2,
                        0
                    },
                    [6] = new List<int>()
                    {
                        25,
                        0,
                        0
                    },
                    [0] = new List<int>()
                    {
                        152,
                        0,
                        0
                    },
                }
            };

        private PoliceRank myRank;

        public Police()
        {
            EventHandlers["Police:OnDuty"] += new Action<dynamic>(OnDuty);
        } 

        private void OnDuty(dynamic json)
        {
            var jsonString = Convert.ToString(json);
            myRank = JsonConvert.DeserializeObject<PoliceRank>(jsonString);
        }

    }
}
