using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using NativeUI;

namespace roleplay.Main
{
    public class InteractionMenu : BaseScript
    {
        public static InteractionMenu Instance;



        private MenuPool _interactionMenuPool;
        private UIMenu _interactionMenu;
        public InteractionMenu()
        {
            Instance = this;

            _interactionMenuPool = new MenuPool();
            _interactionMenu = new UIMenu("Interaction", "Menu");
            _interactionMenuPool.CloseAllMenus(); ;

            Tick += new Func<Task>(async delegate
            {
                if (API.IsControlJustReleased(0, 288))
                {
                    Debug.WriteLine("ETSTSETSETSETESTS");
                    _interactionMenu.Visible = true;
                }
            });
        }
        
    }
}
