using System.Collections.Generic;
using System.Drawing;
using CitizenFX.Core.UI;
using NativeUI;

namespace client.Main.Clothes
{
    public class TatooUI
    {
        public UIMenu Menu;

        public List<List<string>> Tattoos = new List<List<string>>()
        {
        new List<string>(){"mpbeach_overlays","MP_Bea_M_Head_000"},
        new List<string>(){"mpbeach_overlays","MP_Bea_M_Head_001"},
        new List<string>(){"mpbeach_overlays","MP_Bea_M_Head_002"},
        new List<string>(){"mpbeach_overlays","MP_Bea_F_Neck_000"},
        new List<string>(){"mpbeach_overlays","MP_Bea_M_Neck_000"},
        new List<string>(){"mpbeach_overlays","MP_Bea_M_Neck_001"},
        new List<string>(){"mpbeach_overlays","MP_Bea_F_Back_000"},
        new List<string>(){"mpbeach_overlays","MP_Bea_F_Back_001"},
        new List<string>(){"mpbeach_overlays","MP_Bea_F_Back_002"},
        new List<string>(){"mpbeach_overlays","MP_Bea_M_Back_000"},
        new List<string>(){"mpbeach_overlays","MP_Bea_F_Chest_000"},
        new List<string>(){"mpbeach_overlays","MP_Bea_F_Chest_001"},
        new List<string>(){"mpbeach_overlays","MP_Bea_F_Chest_002"},
        new List<string>(){"mpbeach_overlays","MP_Bea_M_Chest_000"},
        new List<string>(){"mpbeach_overlays","MP_Bea_M_Chest_001"},
        new List<string>(){"mpbeach_overlays","MP_Bea_F_Stom_000"},
        new List<string>(){"mpbeach_overlays","MP_Bea_F_Stom_001"},
        new List<string>(){"mpbeach_overlays","MP_Bea_F_Stom_002"},
        new List<string>(){"mpbeach_overlays","MP_Bea_M_Stom_000"},
        new List<string>(){"mpbeach_overlays","MP_Bea_M_Stom_001"},
        new List<string>(){"mpbeach_overlays","MP_Bea_F_RSide_000"},
        new List<string>(){"mpbeach_overlays","MP_Bea_F_Should_000"},
        new List<string>(){"mpbeach_overlays","MP_Bea_F_Should_001"},
        new List<string>(){"mpbeach_overlays","MP_Bea_F_RArm_001"},
        new List<string>(){"mpbeach_overlays","MP_Bea_M_RArm_001"},
        new List<string>(){"mpbeach_overlays","MP_Bea_M_RArm_000"},
        new List<string>(){"mpbeach_overlays","MP_Bea_F_LArm_000"},
        new List<string>(){"mpbeach_overlays","MP_Bea_F_LArm_001"},
        new List<string>(){"mpbeach_overlays","MP_Bea_M_LArm_000"},
        new List<string>(){"mpbeach_overlays","MP_Bea_M_Lleg_000"},
        new List<string>(){"mpbeach_overlays","MP_Bea_F_RLeg_000"},
        new List<string>(){"mpbusiness_overlays","MP_Buis_M_Neck_000"},
        new List<string>(){"mpbusiness_overlays","MP_Buis_M_Neck_001"},
        new List<string>(){"mpbusiness_overlays","MP_Buis_M_Neck_002"},
        new List<string>(){"mpbusiness_overlays","MP_Buis_M_Neck_003"},
        new List<string>(){"mpbusiness_overlays","MP_Buis_M_LeftArm_000"},
        new List<string>(){"mpbusiness_overlays","MP_Buis_M_LeftArm_001"},
        new List<string>(){"mpbusiness_overlays","MP_Buis_M_RightArm_000"},
        new List<string>(){"mpbusiness_overlays","MP_Buis_M_RightArm_001"},
        new List<string>(){"mpbusiness_overlays","MP_Buis_M_Stomach_000"},
        new List<string>(){"mpbusiness_overlays","MP_Buis_M_Chest_000"},
        new List<string>(){"mpbusiness_overlays","MP_Buis_M_Chest_001"},
        new List<string>(){"mpbusiness_overlays","MP_Buis_M_Back_000"},
        new List<string>(){"mpbusiness_overlays","MP_Buis_F_Chest_000"},
        new List<string>(){"mpbusiness_overlays","MP_Buis_F_Chest_001"},
        new List<string>(){"mpbusiness_overlays","MP_Buis_F_Chest_002"},
        new List<string>(){"mpbusiness_overlays","MP_Buis_F_Stom_000"},
        new List<string>(){"mpbusiness_overlays","MP_Buis_F_Stom_001"},
        new List<string>(){"mpbusiness_overlays","MP_Buis_F_Stom_002"},
        new List<string>(){"mpbusiness_overlays","MP_Buis_F_Back_000"},
        new List<string>(){"mpbusiness_overlays","MP_Buis_F_Back_001"},
        new List<string>(){"mpbusiness_overlays","MP_Buis_F_Neck_000"},
        new List<string>(){"mpbusiness_overlays","MP_Buis_F_Neck_001"},
        new List<string>(){"mpbusiness_overlays","MP_Buis_F_RArm_000"},
        new List<string>(){"mpbusiness_overlays","MP_Buis_F_LArm_000"},
        new List<string>(){"mpbusiness_overlays","MP_Buis_F_LLeg_000"},
        new List<string>(){"mpbusiness_overlays","MP_Buis_F_RLeg_000"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_000"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_001"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_002"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_003"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_004"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_005"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_006"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_007"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_008"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_009"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_010"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_011"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_012"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_013"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_014"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_015"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_016"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_017"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_018"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_019"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_020"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_021"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_022"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_023"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_024"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_025"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_026"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_027"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_028"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_029"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_030"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_031"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_032"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_033"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_034"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_035"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_036"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_037"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_038"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_039"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_040"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_041"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_042"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_043"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_044"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_045"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_046"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_047"},
        new List<string>(){"mphipster_overlays","FM_Hip_M_Tat_048"},
        };

        public TatooUI(UIMenu menu, string title)
        {
            Menu = InteractionMenu.Instance._interactionMenuPool.AddSubMenuOffset(menu, title, new PointF(5, Screen.Height / 2));
            ClothesStoreManager.Instance.Menu = Menu;
            Menu.OnMenuClose += sender => { ClothesManager.Instance.SaveTattoos(); };
            var clearButton = new UIMenuItem("Clear");
            Menu.AddItem(clearButton);
            foreach (List<string> tatoo in Tattoos)
            {
                var button = new UIMenuItem(tatoo[1]);
                Menu.AddItem(button);
            }

            Menu.OnItemSelect += (sender, item, index) =>
            {
                if (item == clearButton)
                {
                    ClothesManager.Instance.ClearTattoos();
                }
                else
                {
                    ClothesManager.Instance.SetTattoo(Tattoos[index][0], Tattoos[index][1]);
                    ClothesManager.Instance.SaveTattoos();
                }
            };
            Menu.OnMenuClose += (sender) =>
            {
                ClothesManager.Instance.SaveTattoos();
            };
        }
    }
}