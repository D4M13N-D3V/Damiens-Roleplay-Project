using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using NativeUI;

namespace roleplay.Main.Clothes
{
    public class PropUI
    {
        public UIMenu Menu;
        public UIMenuSliderItem Drawables;
        public UIMenuSliderItem Textures;
        public UIMenuSliderItem Pallet;

        public PropUI(UIMenu menu, string title, int prop, PropTypes type)
        {
            Menu = InteractionMenu.Instance._interactionMenuPool.AddSubMenu(menu, title);

            Menu.OnMenuClose += sender => { ClothesManager.Instance.SaveProps(); };

            var drawables = new List<dynamic>();
            for (int i = 0; i < API.GetNumberOfPedPropDrawableVariations(API.PlayerPedId(), prop); i++)
            {
                drawables.Add("");
            }
            Drawables = new UIMenuSliderItem("Drawables", drawables, 0);
            Menu.AddItem(Drawables);

            Drawables.OnSliderChanged += (sender, index) =>
            {
                var textures = new List<dynamic>();
                if (Textures != null)
                {
                    Menu.RemoveItemAt(1);
                }
                textures.Clear();
                for (int i = 0; i < API.GetNumberOfPedPropTextureVariations(API.PlayerPedId(), prop, index); i++)
                {
                    textures.Add("");
                }
                Textures = new UIMenuSliderItem("Textures", textures, 0);
                Menu.AddItem(Textures);
                ClothesManager.Instance.SetProp(type, index, 0);
                Textures.OnSliderChanged += (textsender, textindex) =>
                {
                    ClothesManager.Instance.SetProp(type, index, textindex);
                };
                InteractionMenu.Instance._interactionMenuPool.RefreshIndex();
            };
        }
    }
}