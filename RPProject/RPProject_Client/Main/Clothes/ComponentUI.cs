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
    public class ComponentUI
    {
        public UIMenu Menu;
        public UIMenuSliderItem Drawables;
        public UIMenuSliderItem Textures;
        public UIMenuSliderItem Pallet;

        public ComponentUI( UIMenu menu, string title,int component, ComponentTypes type)
        {
            Menu = InteractionMenu.Instance._interactionMenuPool.AddSubMenuOffset(menu, title, new PointF(5, Screen.Height / 2));

            Menu.OnMenuClose += sender => { ClothesManager.Instance.SaveComponents();  };

            var drawables = new List<dynamic>();
            for (int i = 0; i < API.GetNumberOfPedDrawableVariations(Game.PlayerPed.Handle, component); i++)
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
                for (int i = 0; i < API.GetNumberOfPedTextureVariations(Game.PlayerPed.Handle, component,index); i++)
                {
                    textures.Add("");
                }
                Textures = new UIMenuSliderItem("Textures",textures,0);
                Menu.AddItem(Textures);
                ClothesManager.Instance.SetComponents(type, index, 0, 0);
                Textures.OnSliderChanged += (textsender, textindex) =>
                {
                    ClothesManager.Instance.SetComponents(type, index, textindex, 0);
                };
                InteractionMenu.Instance._interactionMenuPool.RefreshIndex();
            };
        }
    }
}
