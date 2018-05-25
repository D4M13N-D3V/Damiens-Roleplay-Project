using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using NativeUI;
using roleplay.Main.Clothes;
using roleplay.Users;

namespace roleplay.Main
{
    public class InteractionMenu : BaseScript
    {
        public static InteractionMenu Instance;

        public List<UIMenu> _menus = new List<UIMenu>();

        public readonly MenuPool _interactionMenuPool;
        public readonly UIMenu _interactionMenu;
        public InteractionMenu()
        {
            Instance = this;

            _interactionMenuPool = new MenuPool();
            _interactionMenu = new UIMenu("Interaction Menu","A menu to intereact with the world!");

            var animationsButton = new UIMenuItem("Animations Menu", "Browse all the animations!");

            _interactionMenu.AddItem(animationsButton);

            _interactionMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == animationsButton)
                {
                    _interactionMenuPool.CloseAllMenus();
                    TriggerEvent("OpenAnimationsMenu");
                }
            };

            _interactionMenuPool.Add(_interactionMenu);
            _interactionMenuPool.RefreshIndex();
            _interactionMenuPool.CloseAllMenus(); ;

            Tick += new Func<Task>(async delegate
            {
                _interactionMenuPool.ProcessMenus();
                if ( API.IsControlJustReleased(0, 288) && !_interactionMenuPool.IsAnyMenuOpen())
                {
                    _interactionMenu.Visible = true;
                }
            });
        }

    }
}
