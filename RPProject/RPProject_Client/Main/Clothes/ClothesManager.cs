using System;
using System.Collections.Generic;
using CitizenFX.Core;
using NativeUI;
using CitizenFX.Core.Native;
using roleplay.Users.Login;

namespace roleplay.Main.Clothes
{
    public enum ComponentTypes
    {
        Face,
        Head,
        Hair,
        Eyes,
        Torso,
        Torso2,
        Legs,
        Hands,
        Feet,
        Tasks,
        Textures,
        Acessories
    };

    public enum PropTypes
    {
        Hats,
        Glasses,
        Ears,
        Watches
    };

    public enum HeadOverlayTypes
    {
        Blemishes,
        Beards,
        Eyebrows,
        Aging,
        Makeup,
        Blush,
        Complexion,
        Sundamage,
        Lipstick,
        Moles,
        Chesthair,
        BodyBlemishes
    }

    public class ClothesManager : BaseScript
    {
        public bool modelSet = false;
        public static ClothesManager Instance;

        private List<dynamic> _face;
        private List<dynamic> _head;
        private List<dynamic> _hair;
        private List<dynamic> _eyes;
        private List<dynamic> _torso;
        private List<dynamic> _torso2;
        private List<dynamic> _legs;
        private List<dynamic> _hands;
        private List<dynamic> _feet;
        private List<dynamic> _tasks;
        private List<dynamic> _textures;
        private List<dynamic> _accessories;
        
        private List<dynamic> _hats;
        private List<dynamic> _glasses;
        private List<dynamic> _ears;
        private List<dynamic> _watches;

        private List<dynamic> _blemishes;
        private List<dynamic> _beards;
        private List<dynamic> _eyebrows;
        private List<dynamic> _aging;
        private List<dynamic> _makeup;
        private List<dynamic> _blush;
        private List<dynamic> _complexion;
        private List<dynamic> _sundamage;
        private List<dynamic> _lipstick;
        private List<dynamic> _moles;
        private List<dynamic> _chesthair;
        private List<dynamic> _bodyblemishes;

        private List<dynamic> _tattoosCollections;
        private List<dynamic> _tattoosOverlays;

        public ClothesManager()
        {
            Instance = this;
            ClothesMenu();
            EventHandlers["loadComponents"] += new Action<List<dynamic>, List<dynamic>, List<dynamic>, List<dynamic>, List<dynamic>, List<dynamic>, List<dynamic>, List<dynamic>, List<dynamic>, List<dynamic>, List<dynamic>, List<dynamic>>(LoadComponents);
            EventHandlers["loadProps"] += new Action<List<dynamic>, List<dynamic>, List<dynamic>, List<dynamic>>(LoadProps);
            EventHandlers["loadHeadOverlays"] += new Action<List<dynamic>, List<dynamic>, List<dynamic>, List<dynamic>, List<dynamic>, List<dynamic>, List<dynamic>, List<dynamic>, List<dynamic>, List<dynamic>, List<dynamic>, List<dynamic>>(LoadHeadOverlays);
            EventHandlers["loadTattoos"] += new Action<List<dynamic>, List<dynamic>>(LoadTattoos);
        }

        public async void LoadComponents(List<dynamic> face, List<dynamic> head, List<dynamic> hair, List<dynamic> eyes, List<dynamic> torso,
            List<dynamic> torso2, List<dynamic> legs, List<dynamic> hands, List<dynamic> feet, List<dynamic> tasks, List<dynamic> textures, List<dynamic> accessories)
        {
            _face = face;
            _head = head;
            _hair = hair;
            _eyes = eyes;
            _torso = torso;
            _torso2 = torso2;
            _legs = legs;
            _hands = hands;
            _feet = feet;
            _tasks = tasks;
            _textures = textures;
            _accessories = accessories;
            while (!modelSet)
            {
                await Delay(250);
            }
            Utility.Instance.Log("Components have been loaded!");
            API.SetPedComponentVariation(API.PlayerPedId(), 0, face[0], face[1], face[2]);
            API.SetPedComponentVariation(API.PlayerPedId(), 1, head[0], head[1], head[2]);
            API.SetPedComponentVariation(API.PlayerPedId(), 2, hair[0], hair[1], hair[2]);
            API.SetPedComponentVariation(API.PlayerPedId(), 7, eyes[0], eyes[1], eyes[2]);
            API.SetPedComponentVariation(API.PlayerPedId(), 3, torso[0], torso[1], torso[2]);
            API.SetPedComponentVariation(API.PlayerPedId(), 11, torso2[0], torso2[1], torso2[2]);
            API.SetPedComponentVariation(API.PlayerPedId(), 4, legs[0], legs[1], legs[2]);
            API.SetPedComponentVariation(API.PlayerPedId(), 5, hands[0], hands[1], hands[2]);
            API.SetPedComponentVariation(API.PlayerPedId(), 6, feet[0], feet[1], feet[2]);
            API.SetPedComponentVariation(API.PlayerPedId(), 9, tasks[0], tasks[1], tasks[2]);
            API.SetPedComponentVariation(API.PlayerPedId(), 10, textures[0], textures[1], textures[2]);
            API.SetPedComponentVariation(API.PlayerPedId(), 8, _accessories[0], _accessories[1], _accessories[2]);

        }

        public async void LoadProps(List<dynamic> hats, List<dynamic> glasses, List<dynamic> ears, List<dynamic> watches)
        {
            _hats = hats;
            _glasses = glasses;
            _ears = ears;
            _watches = watches;
            while (!modelSet)
            {
                await Delay(250);
            }
            Utility.Instance.Log("Props have been loaded!");
            API.SetPedPropIndex(API.PlayerPedId(), 0, hats[0], hats[1], true);
            API.SetPedPropIndex(API.PlayerPedId(), 1, glasses[0], glasses[1], true);
            API.SetPedPropIndex(API.PlayerPedId(), 2, ears[0], ears[1], true);
            API.SetPedPropIndex(API.PlayerPedId(), 3, watches[0], watches[1], true);
        }

        public async void LoadHeadOverlays(List<dynamic> blemishes, List<dynamic> beards, List<dynamic> eyebrows, List<dynamic> aging, List<dynamic> makeup,
            List<dynamic> blush, List<dynamic> complexion, List<dynamic> sundamage, List<dynamic> lipstick, List<dynamic> moles, List<dynamic> chesthair,
            List<dynamic> bodyblemishes)
        {
            _blemishes = blemishes;
            _beards = beards;
            _eyebrows = eyebrows;
            _aging = aging;
            _makeup = makeup;
            _blush = blush;
            _complexion = complexion;
            _sundamage = sundamage;
            _lipstick = lipstick;
            _moles = moles;
            _chesthair = chesthair;
            _bodyblemishes = bodyblemishes;
            while (!modelSet)
            {
                await Delay(250);
            }
            Utility.Instance.Log("Headoverlays have been loaded!");

            API.SetPedHeadOverlay(API.PlayerPedId(), blemishes[0], blemishes[1], blemishes[5]);
            API.SetPedHeadOverlayColor(API.PlayerPedId(), blemishes[0], blemishes[2], blemishes[3], blemishes[4]);

            API.SetPedHeadOverlay(API.PlayerPedId(), beards[0], beards[1], beards[5]);
            API.SetPedHeadOverlayColor(API.PlayerPedId(), beards[0], beards[2], beards[3], beards[4]);

            API.SetPedHeadOverlay(API.PlayerPedId(), eyebrows[0], eyebrows[1], eyebrows[5]);
            API.SetPedHeadOverlayColor(API.PlayerPedId(), eyebrows[0], eyebrows[2], eyebrows[3], eyebrows[4]);

            API.SetPedHeadOverlay(API.PlayerPedId(), aging[0], aging[1], aging[5]);
            API.SetPedHeadOverlayColor(API.PlayerPedId(), aging[0], aging[2], aging[3], aging[4]);

            API.SetPedHeadOverlay(API.PlayerPedId(), makeup[0], makeup[1], makeup[5]);
            API.SetPedHeadOverlayColor(API.PlayerPedId(), makeup[0], makeup[2], makeup[3], makeup[4]);

            API.SetPedHeadOverlay(API.PlayerPedId(), blush[0], blush[1], blush[5]);
            API.SetPedHeadOverlayColor(API.PlayerPedId(), blush[0], blush[2], blush[3], blush[4]);

            API.SetPedHeadOverlay(API.PlayerPedId(), complexion[0], complexion[1], complexion[5]);
            API.SetPedHeadOverlayColor(API.PlayerPedId(), complexion[0], complexion[2], complexion[3], complexion[4]);

            API.SetPedHeadOverlay(API.PlayerPedId(), sundamage[0], sundamage[1], sundamage[5]);
            API.SetPedHeadOverlayColor(API.PlayerPedId(), sundamage[0], sundamage[2], sundamage[3], sundamage[4]);

            API.SetPedHeadOverlay(API.PlayerPedId(), lipstick[0], lipstick[1], lipstick[5]);
            API.SetPedHeadOverlayColor(API.PlayerPedId(), lipstick[0], lipstick[2], lipstick[3], lipstick[4]);

            API.SetPedHeadOverlay(API.PlayerPedId(), moles[0], moles[1], moles[5]);
            API.SetPedHeadOverlayColor(API.PlayerPedId(), moles[0], moles[2], moles[3], moles[4]);

            API.SetPedHeadOverlay(API.PlayerPedId(), chesthair[0], chesthair[1], chesthair[5]);
            API.SetPedHeadOverlayColor(API.PlayerPedId(), chesthair[0], chesthair[2], chesthair[3], chesthair[4]);

            API.SetPedHeadOverlay(API.PlayerPedId(), bodyblemishes[0], bodyblemishes[1], bodyblemishes[5]);
            API.SetPedHeadOverlayColor(API.PlayerPedId(), bodyblemishes[0], bodyblemishes[2], bodyblemishes[3], bodyblemishes[4]);
        }

        public async void LoadTattoos(List<dynamic> collections, List<dynamic> overlays)
        {
            _tattoosCollections = collections;
            _tattoosOverlays = overlays;
            while (!modelSet)
            {
                await Delay(250);
            }
            for (int i = 0; i < collections.Count; i++)
            {
                Debug.WriteLine(collections[i]+" "+overlays[i]);
                API.ApplyPedOverlay(API.PlayerPedId(), (uint)API.GetHashKey(collections[i]), (uint)API.GetHashKey(overlays[i]));
            }
        }

        public void SaveModel(string model)
        {
            TriggerServerEvent("saveModel",model);
        }

        public void SetProp(PropTypes type, int drawable, int texture)
        {
            switch (type)
            {
                case PropTypes.Hats:
                    _hats[0] = drawable;
                    _hats[1] = texture;
                    API.SetPedPropIndex(API.PlayerPedId(), 0, drawable, texture, true);
                    break;
                case PropTypes.Glasses:
                    _glasses[0] = drawable;
                    _glasses[1] = texture;
                    API.SetPedPropIndex(API.PlayerPedId(), 1, drawable, texture, true);
                    break;
                case PropTypes.Ears:
                    _ears[0] = drawable;
                    _ears[1] = texture;
                    API.SetPedPropIndex(API.PlayerPedId(), 2, drawable, texture, true);
                    break;
                case PropTypes.Watches:
                    _watches[0] = drawable;
                    _watches[1] = texture;
                    API.SetPedPropIndex(API.PlayerPedId(), 3, drawable, texture, true);
                    break;
            }
        }

        public void SetComponents(ComponentTypes type, int drawable, int texture, int pallet)
        {
            Debug.Write(type+","+drawable+","+texture+","+pallet);
            switch (type)
            {
                case ComponentTypes.Face:
                    _face[0] = drawable;
                    _face[1] = texture;
                    _face[2] = pallet;
                    API.SetPedComponentVariation(API.PlayerPedId(), 0, drawable, texture, pallet);
                    break;
                case ComponentTypes.Head:
                    _head[0] = drawable;
                    _head[1] = texture;
                    _head[2] = pallet;
                    API.SetPedComponentVariation(API.PlayerPedId(), 1, drawable, texture, pallet);
                    break;
                case ComponentTypes.Hair:
                    _hair[0] = drawable;
                    _hair[1] = texture;
                    _hair[2] = pallet;
                    API.SetPedComponentVariation(API.PlayerPedId(), 2, drawable, texture, pallet);
                    break;
                case ComponentTypes.Eyes:
                    _eyes[0] = drawable;
                    _eyes[1] = texture;
                    _eyes[2] = pallet;
                    API.SetPedComponentVariation(API.PlayerPedId(), 7, drawable, texture, pallet);
                    break;
                case ComponentTypes.Torso:
                    _torso[0] = drawable;
                    _torso[1] = texture;
                    _torso[2] = pallet;
                    API.SetPedComponentVariation(API.PlayerPedId(), 3, drawable, texture, pallet);
                    break;
                case ComponentTypes.Torso2:
                    _torso2[0] = drawable;
                    _torso2[1] = texture;
                    _torso2[2] = pallet;
                    API.SetPedComponentVariation(API.PlayerPedId(), 11, drawable, texture, pallet);
                    break;
                case ComponentTypes.Legs:
                    _legs[0] = drawable;
                    _legs[1] = texture;
                    _legs[2] = pallet;
                    API.SetPedComponentVariation(API.PlayerPedId(), 4, drawable, texture, pallet);
                    break;
                case ComponentTypes.Hands:
                    _hands[0] = drawable;
                    _hands[1] = texture;
                    _hands[2] = pallet;
                    API.SetPedComponentVariation(API.PlayerPedId(), 5, drawable, texture, pallet);
                    break;
                case ComponentTypes.Feet:
                    _feet[0] = drawable;
                    _feet[1] = texture;
                    _feet[2] = pallet;
                    API.SetPedComponentVariation(API.PlayerPedId(), 6, drawable, texture, pallet);
                    break;
                case ComponentTypes.Tasks:
                    _tasks[0] = drawable;
                    _tasks[1] = texture;
                    _tasks[2] = pallet;
                    API.SetPedComponentVariation(API.PlayerPedId(), 9, drawable, texture, pallet);
                    break;
                case ComponentTypes.Textures:
                    _textures[0] = drawable;
                    _textures[1] = texture;
                    _textures[2] = pallet;
                    API.SetPedComponentVariation(API.PlayerPedId(), 10, drawable, texture, pallet);
                    break;
                case ComponentTypes.Acessories:
                    _accessories[0] = drawable;
                    _accessories[1] = texture;
                    _accessories[2] = pallet;
                    API.SetPedComponentVariation(API.PlayerPedId(), 8, drawable, texture, pallet);
                    break; 
            }
        }

        public void SetHeadOverlays(HeadOverlayTypes type, int index, int primarycolor, int secondarycolor)
        {
            switch (type)
            {
                case HeadOverlayTypes.Blemishes:
                    _blemishes[1] = index;
                    _blemishes[2] = primarycolor;
                    _blemishes[3] = secondarycolor;
                    _blemishes[4] = 0;
                    _blemishes[5] = 255;
                    API.SetPedHeadOverlay(API.PlayerPedId(), _blemishes[0], index, 255);
                    API.SetPedHeadOverlayColor(API.PlayerPedId(), index, _blemishes[4], primarycolor, secondarycolor);
                    break;
                case HeadOverlayTypes.Beards:
                    _beards[1] = index;
                    _beards[2] = primarycolor;
                    _beards[3] = secondarycolor;
                    _beards[4] = 1;
                    _beards[5] = 255;
                    API.SetPedHeadOverlay(API.PlayerPedId(), _beards[0], index, 255);
                    API.SetPedHeadOverlayColor(API.PlayerPedId(), index, _beards[4], primarycolor, secondarycolor);
                    break;
                case HeadOverlayTypes.Eyebrows:
                    _eyebrows[1] = index;
                    _eyebrows[2] = primarycolor;
                    _eyebrows[3] = secondarycolor;
                    _eyebrows[4] = 1;
                    _eyebrows[5] = 255;
                    API.SetPedHeadOverlay(API.PlayerPedId(), _eyebrows[0], index, 255);
                    API.SetPedHeadOverlayColor(API.PlayerPedId(), index, _eyebrows[4], primarycolor, secondarycolor);
                    break;
                case HeadOverlayTypes.Aging:
                    _aging[1] = index;
                    _aging[2] = primarycolor;
                    _aging[3] = secondarycolor;
                    _aging[4] = 0;
                    _aging[5] = 255;
                    API.SetPedHeadOverlay(API.PlayerPedId(), _aging[0], index, 255);
                    API.SetPedHeadOverlayColor(API.PlayerPedId(), index, _aging[4], primarycolor, secondarycolor);
                    break;
                case HeadOverlayTypes.Makeup:
                    _makeup[1] = index;
                    _makeup[2] = primarycolor;
                    _makeup[3] = secondarycolor;
                    _makeup[4] = 0;
                    _makeup[5] = 255;
                    API.SetPedHeadOverlay(API.PlayerPedId(), _makeup[0], index, 255);
                    API.SetPedHeadOverlayColor(API.PlayerPedId(), index, _makeup[4], primarycolor, secondarycolor);
                    break;
                case HeadOverlayTypes.Blush:
                    _blush[1] = index;
                    _blush[2] = primarycolor;
                    _blush[3] = secondarycolor;
                    _blush[4] = 2;
                    _blush[5] = 255;
                    API.SetPedHeadOverlay(API.PlayerPedId(), _blush[0], index, 255);
                    API.SetPedHeadOverlayColor(API.PlayerPedId(), index, _blush[4], primarycolor, secondarycolor);
                    break;
                case HeadOverlayTypes.Complexion:
                    _complexion[1] = index;
                    _complexion[2] = primarycolor;
                    _complexion[3] = secondarycolor;
                    _complexion[4] = 0;
                    _complexion[5] = 255;
                    API.SetPedHeadOverlay(API.PlayerPedId(), _complexion[0], index, 255);
                    API.SetPedHeadOverlayColor(API.PlayerPedId(), index, _complexion[4], primarycolor, secondarycolor);
                    break;
                case HeadOverlayTypes.Sundamage:
                    _sundamage[1] = index;
                    _sundamage[2] = primarycolor;
                    _sundamage[3] = secondarycolor;
                    _sundamage[4] = 0;
                    _sundamage[5] = 255;
                    API.SetPedHeadOverlay(API.PlayerPedId(), _sundamage[0], index, 255);
                    API.SetPedHeadOverlayColor(API.PlayerPedId(), index, _sundamage[4], primarycolor, secondarycolor);
                    break;
                case HeadOverlayTypes.Lipstick:
                    _lipstick[1] = index;
                    _lipstick[2] = primarycolor;
                    _lipstick[3] = secondarycolor;
                    _lipstick[4] = 2;
                    _lipstick[5] = 255;
                    API.SetPedHeadOverlay(API.PlayerPedId(), _lipstick[0], index, 255);
                    API.SetPedHeadOverlayColor(API.PlayerPedId(), index, _lipstick[4], primarycolor, secondarycolor);
                    break;
                case HeadOverlayTypes.Moles:
                    _moles[1] = index;
                    _moles[2] = primarycolor;
                    _moles[3] = secondarycolor;
                    _moles[4] = 0;
                    _moles[5] = 255;
                    API.SetPedHeadOverlay(API.PlayerPedId(), _moles[0], index, 255);
                    API.SetPedHeadOverlayColor(API.PlayerPedId(), index, _moles[4], primarycolor, secondarycolor);
                    break;
                case HeadOverlayTypes.Chesthair:
                    _chesthair[1] = index;
                    _chesthair[2] = primarycolor;
                    _chesthair[3] = secondarycolor;
                    _chesthair[4] = 1;
                    _chesthair[5] = 255;
                    API.SetPedHeadOverlay(API.PlayerPedId(), _chesthair[0], index, 255);
                    API.SetPedHeadOverlayColor(API.PlayerPedId(), index, _chesthair[4], primarycolor, secondarycolor);
                    break;
                case HeadOverlayTypes.BodyBlemishes:
                    _bodyblemishes[1] = index;
                    _bodyblemishes[2] = primarycolor;
                    _bodyblemishes[3] = secondarycolor;
                    _bodyblemishes[4] = 0;
                    _bodyblemishes[5] = 255;
                    API.SetPedHeadOverlay(API.PlayerPedId(), _bodyblemishes[0], index, 255);
                    API.SetPedHeadOverlayColor(API.PlayerPedId(), index, _bodyblemishes[4], primarycolor, secondarycolor);
                    break;
            }
        }

            public void ClearTattoos()
            {
                API.ClearPedDecorations(API.PlayerPedId());
                _tattoosCollections.Clear();
                _tattoosOverlays.Clear();
                SaveTattoos();
            }

        public void SetTattoo(string collection, string overlay)
        {
            _tattoosCollections.Add(collection);
            _tattoosOverlays.Add(overlay);
            API.ApplyPedOverlay(API.PlayerPedId(), (uint)API.GetHashKey(collection), (uint)API.GetHashKey(overlay));
        }

        public async void SetModel(string model)
        {
            var modelHash = (uint)API.GetHashKey(model);
            API.RequestModel(modelHash);
            Utility.Instance.Log("Loading Player Model");
            while (API.HasModelLoaded(modelHash) == false)
            {
                await Delay(0);
            }
            Utility.Instance.Log("Player Model Loaded!");
            API.SetPlayerModel(API.PlayerId(), modelHash);
            TriggerServerEvent("saveModel", model);
        }

        public void SaveComponents()
        {
            TriggerServerEvent("saveComponents", _face, _head, _hair, _eyes, _torso, _torso2, _legs, _hands, _feet, _tasks, _textures, _accessories);
        }

        public void SaveProps()
        {
            TriggerServerEvent("saveProps", _hats, _glasses, _ears, _watches);
        }

        public void SaveHeadOverlays()
        {
            TriggerServerEvent("saveHeadOverlays", _blemishes, _beards, _eyebrows, _aging, _makeup, _blush, _complexion, _sundamage,
                _lipstick, _moles, _chesthair, _bodyblemishes);
        }

        public void SaveTattoos()
        {
            Utility.Instance.Log(" Saving Tattoos");
            TriggerServerEvent("saveTattoos", _tattoosCollections,_tattoosOverlays);
        }

        public void ClothesMenu()
        {
        }
    
    }
}