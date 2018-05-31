using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

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

    public class PoliceUniformComponent
    {
        public int Component;
        public int Drawable;
        public int Texture;
        public int Pallet;
        public PoliceUniformComponent(int comp, int draw, int text, int pallet)
        {
            Component = comp;
            Drawable = draw;
            Texture = text;
            Pallet = pallet;
        }
    }

    public class Police : BaseScript
    {

        private readonly Dictionary<string, List<PoliceUniformComponent>> _maleUniforms =
            new Dictionary<string, List<PoliceUniformComponent>>()
            {
                ["LSPD"] = new List<PoliceUniformComponent>()
                {
                    new PoliceUniformComponent(3,0,0,0),
                    new PoliceUniformComponent(4,35,0,0),
                    new PoliceUniformComponent(5,0,0,0),
                    new PoliceUniformComponent(6,24,0,0),
                    new PoliceUniformComponent(7,0,0,0),
                    new PoliceUniformComponent(8,122,0,0),
                    new PoliceUniformComponent(11,55,0,0),
                },
                ["BCSO"] = new List<PoliceUniformComponent>()
                {
                    new PoliceUniformComponent(3,11,0,0),
                    new PoliceUniformComponent(4,10,0,0),
                    new PoliceUniformComponent(5,0,0,0),
                    new PoliceUniformComponent(6,24,0,0),
                    new PoliceUniformComponent(7,125,0,0),
                    new PoliceUniformComponent(8,122,0,0),
                    new PoliceUniformComponent(11,26,2,0),
                }
            };

        private readonly Dictionary<string, List<PoliceUniformComponent>> _femaleUniforms =
            new Dictionary<string, List<PoliceUniformComponent>>()
            {
                ["LSPD"] = new List<PoliceUniformComponent>()
                {
                    new PoliceUniformComponent(3,14,0,0),
                    new PoliceUniformComponent(4,34,0,0),
                    new PoliceUniformComponent(5,0,0,0),
                    new PoliceUniformComponent(6,25,0,0),
                    new PoliceUniformComponent(7,0,0,0),
                    new PoliceUniformComponent(8,152,0,0),
                    new PoliceUniformComponent(11,48,0,0),
                },
                ["BCSO"] = new List<PoliceUniformComponent>()
                {
                    new PoliceUniformComponent(3,0,0,0),
                    new PoliceUniformComponent(4,64,1,0),
                    new PoliceUniformComponent(5,0,0,0),
                    new PoliceUniformComponent(6,52,0,0),
                    new PoliceUniformComponent(7,95,0,0),
                    new PoliceUniformComponent(8,152,0,0),
                    new PoliceUniformComponent(11,27,2,0),
                }
            };

        public int CopCount = 0;
        private bool _onDuty = false;
        private string _rankName = "";
        private string _department = "";

        public Police()
        {
            EventHandlers["Police:SetOnDuty"] += new Action<dynamic>(OnDuty);
            EventHandlers["Police:SetOffDuty"] += new Action(OffDuty);
            EventHandlers["Police:RefreshOnDutyOfficers"] += new Action<dynamic>(RefreshCops);
        }
        private void RefreshCops(dynamic copCount)
        {
            CopCount = copCount;
        }

        private void OnDuty(dynamic data)
        {
            var department = Convert.ToString(data);
            _department = department;
            Utility.Instance.SendChatMessage("[POLICE]","You have gone on duty.",0,0,255);
            _onDuty = true;
            GiveUniform();
        }

        private void OffDuty()
        {
            Utility.Instance.SendChatMessage("[POLICE]", "You have gone off duty.", 0, 0, 255);
            _rankName = "";
            _onDuty = false;
            _department = "";
            TakeUniform();
        }

        private void GiveUniform()
        {
            if (_department=="USMS" || _department=="") { return; }
            if (API.GetEntityModel(API.PlayerPedId()) == API.GetHashKey("mp_m_freemode_01"))
            {
                foreach (var uniformParts in _maleUniforms[_department])
                {
                    API.SetPedComponentVariation(API.PlayerPedId(), uniformParts.Component, uniformParts.Drawable, uniformParts.Texture, uniformParts.Pallet);
                }
            }
            else if (API.GetEntityModel(API.PlayerPedId()) == API.GetHashKey("mp_f_freemode_01"))
            {
                foreach (var uniformParts in _femaleUniforms[_department])
                {
                    API.SetPedComponentVariation(API.PlayerPedId(), uniformParts.Component, uniformParts.Drawable, uniformParts.Texture, uniformParts.Pallet);
                }
            }
        }

        private void TakeUniform()
        {
            TriggerServerEvent("RefreshClothes");
        }

    }
}
