using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace roleplay
{
    public class ClothesManager:BaseScript
    {
        public bool modelSet = false;
        public static ClothesManager Instance;

        public ClothesManager()
        {
            Instance = this;
            EventHandlers["loadComponents"] += new Action<dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic>(LoadComponents);
            EventHandlers["loadProps"] += new Action<dynamic, dynamic, dynamic, dynamic>(LoadProps);
            EventHandlers["loadHeadOverlays"] += new Action<dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic>(LoadHeadOverlays);
        }

        public async void LoadComponents(dynamic face, dynamic head, dynamic hair, dynamic eyes, dynamic torso, dynamic torso2, dynamic legs, dynamic hands, dynamic feet, dynamic tasks, dynamic textures)
        {
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

        }

        public async void LoadProps(dynamic hats, dynamic glasses, dynamic ears, dynamic watches)
        {
            while (!modelSet)
            {
                await Delay(250);
            }
            Utility.Instance.Log("Props have been loaded!");
            API.SetPedPropIndex(API.PlayerPedId(), 0, hats[0], hats[1], hats[2]);
            API.SetPedPropIndex(API.PlayerPedId(), 0, glasses[0], glasses[1], glasses[2]);
            API.SetPedPropIndex(API.PlayerPedId(), 0, ears[0], ears[1], ears[2]);
            API.SetPedPropIndex(API.PlayerPedId(), 0, watches[0], watches[1], watches[2]);
        }

        public async void LoadHeadOverlays(dynamic blemishes, dynamic beards, dynamic eyebrows, dynamic aging, dynamic makeup,
            dynamic blush, dynamic complexion, dynamic sundamage, dynamic lipstick, dynamic moles, dynamic chesthair,
            dynamic bodyblemishes)
        {
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

    }
}