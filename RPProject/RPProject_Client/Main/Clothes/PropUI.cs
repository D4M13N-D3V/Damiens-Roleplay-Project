using System.Collections.Generic;
using System.Drawing;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using NativeUI;

namespace client.Main.Clothes
{
    public class PropUI
    {
        public UIMenu Menu;
        public UIMenuSliderItem Drawables;
        public UIMenuSliderItem Textures;
        public UIMenuSliderItem Pallet;

        public PropUI(UIMenu menu, string title, int prop, PropTypes type)
        {
            Menu = InteractionMenu.Instance._interactionMenuPool.AddSubMenuOffset(menu, title, new PointF(5, Screen.Height / 2));

            Menu.OnMenuClose += sender => { ClothesManager.Instance.SaveProps(); };

            var drawables = new List<dynamic>();
            drawables.Add("");
            for (int i = 0; i < API.GetNumberOfPedPropDrawableVariations(Game.PlayerPed.Handle, prop); i++)
            {
                drawables.Add("");
            }
            Drawables = new UIMenuSliderItem("Drawables", drawables, 0);
            Drawables.Index = API.GetPedPropIndex(Game.PlayerPed.Handle, prop);
            Menu.AddItem(Drawables);

            Drawables.OnSliderChanged += (sender, index) =>
            {
                var textures = new List<dynamic>();
                if (Textures != null && Menu.MenuItems[1]!=null)
                {
                    Menu.RemoveItemAt(1);
                }
                textures.Clear();
                for (int i = 0; i < API.GetNumberOfPedPropTextureVariations(Game.PlayerPed.Handle, prop, index); i++)
                {
                    textures.Add("");
                }
                Textures = new UIMenuSliderItem("Textures", textures, 0);
                Menu.AddItem(Textures);
                ClothesManager.Instance.SetProp(type, index-1, 0);
                Textures.OnSliderChanged += (textsender, textindex) =>
                {
                    ClothesManager.Instance.SetProp(type, index-1, textindex);
                };
                InteractionMenu.Instance._interactionMenuPool.RefreshIndex();
            };
        }
    }
}