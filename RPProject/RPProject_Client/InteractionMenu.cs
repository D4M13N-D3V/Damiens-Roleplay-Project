using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using NativeUI;

namespace roleplay
{
    public class InteractionMenu : BaseScript
    {
        public static InteractionMenu Instance;

        public InteractionMenu()
        {
            Instance = this;

            SetupMenu();

            Tick += new Func<Task>(async delegate
            {
                if (API.IsControlJustPressed(0, 288) && !_interactionMenuPool.IsAnyMenuOpen())
                {
                    _interactionMenu.Visible = true;
                }
            });
        }


        private MenuPool _interactionMenuPool;
        private UIMenu _interactionMenu;

        public void SetupMenu()
        {
            _interactionMenuPool = new MenuPool();
            _interactionMenu = new UIMenu("Interaction","Menu");
            _interactionMenuPool.CloseAllMenus();;
        }
    }
}
