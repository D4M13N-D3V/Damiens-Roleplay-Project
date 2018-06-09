using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using NativeUI;

namespace roleplay.Main.Clothes
{
    public class HeadOverlayUI
    {
        public UIMenu Menu;
        public UIMenuSliderItem Drawables;
        public UIMenuSliderItem Colors1;
        public UIMenuSliderItem Colors2;
        public UIMenuSliderItem Pallet;

        public HeadOverlayUI(UIMenu menu, string title, int prop, HeadOverlayTypes type)
        {
            Menu = InteractionMenu.Instance._interactionMenuPool.AddSubMenuOffset(menu, title, new PointF(5, Screen.Height / 2));

            Menu.OnMenuClose += sender => { ClothesManager.Instance.SaveHeadOverlays(); };

            var drawables = new List<dynamic>();
            for (int i = 0; i < 11; i++)
            {
                drawables.Add("");
            }
            Drawables = new UIMenuSliderItem("Drawables", drawables, 0);
            Menu.AddItem(Drawables);

            Drawables.OnSliderChanged += (sender, index) =>
            {
                var color1 = new List<dynamic>();
                var color2 = new List<dynamic>();
                if (color2 != null)
                {
                    Menu.RemoveItemAt(2);
                }
                if (Colors1 != null)
                {
                    Menu.RemoveItemAt(1);
                }
                color1.Clear();
                color2.Clear();
                for (int i = 0; i < 63; i++)
                {
                    color1.Add("");
                }
                for (int i = 0; i < 63; i++)
                {
                    color2.Add("");
                }
                Colors1 = new UIMenuSliderItem("Primary Color", color1, 0);
                Menu.AddItem(Colors1);

                Colors2 = new UIMenuSliderItem("Secondary Color", color2, 0);
                Menu.AddItem(Colors2);

                ClothesManager.Instance.SetHeadOverlays(type, index, 0,0);
                Colors1.OnSliderChanged += (textsender, textindex) =>
                {
                    ClothesManager.Instance.SetHeadOverlays(type, index, Colors1.Index,Colors2.Index);
                };
                Colors2.OnSliderChanged += (textsender, textindex) =>
                {
                    ClothesManager.Instance.SetHeadOverlays(type, index, Colors1.Index,Colors2.Index);
                };
                InteractionMenu.Instance._interactionMenuPool.RefreshIndex();
            };
            
        }
    }
}