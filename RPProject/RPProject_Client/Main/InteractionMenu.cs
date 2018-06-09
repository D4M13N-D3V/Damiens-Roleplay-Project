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

            var phoneMenu = _interactionMenuPool.AddSubMenu(_interactionMenu, "Phone", "Access your phone.");
            var emergencyButton = new UIMenuItem("(911) Emergency Call");
            var nonEmergencyButton = new UIMenuItem("(311) Non Emergency Call");
            var textButton = new UIMenuItem("Text Somebody", "Use thier server ID or actual phone number");
            var tweetButton = new UIMenuItem("Post to Twitter");
            var torButton = new UIMenuItem("Post to TOR");
            var advertButton = new UIMenuItem("Post to craigslist");
            phoneMenu.AddItem(emergencyButton);
            phoneMenu.AddItem(nonEmergencyButton);
            phoneMenu.AddItem(textButton);
            phoneMenu.OnItemSelect += delegate(UIMenu sender, UIMenuItem item, int index)
            {
                if (item == emergencyButton)
                {
                    _interactionMenuPool.CloseAllMenus();
                    Utility.Instance.KeyboardInput("This is 911, what is your emergency?","",2000, delegate(string s)
                    {
                        TriggerEvent("911CallClient",s);
                    } );
                }
                else if (item == nonEmergencyButton)
                {
                    _interactionMenuPool.CloseAllMenus();
                    Utility.Instance.KeyboardInput("This is 311, what is your emergency?", "", 2000, delegate (string s)
                    {
                        TriggerEvent("311CallClient", s);
                    });
                }
                else if (item == textButton)
                {
                    _interactionMenuPool.CloseAllMenus();
                    Utility.Instance.KeyboardInput("The server id or phone number of the person you are trying to text.","",10,
                        delegate(string s)
                        {
                            Utility.Instance.KeyboardInput("The message you are trying to send to the person.","",2000,
                                delegate(string s1)
                                {
                                    TriggerServerEvent("TextingFromClient",s,s1);
                                });
                        });
                }
            };

            var cardButton = new UIMenuItem("Show Buisness Card", "Show buisness card with name and phone number to everyone nearby.");
            _interactionMenu.AddItem(cardButton);

            var idButton = new UIMenuItem("Show ID", "Show your identification to everyone nearby.");
            _interactionMenu.AddItem(idButton);

            _interactionMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == animationsButton)
                {
                    _interactionMenuPool.CloseAllMenus();
                    TriggerEvent("OpenAnimationsMenu");
                }
                else if (item == idButton)
                {
                    TriggerServerEvent("RequestID", Game.Player.ServerId);
                }
                else if (item == cardButton)
                {
                    TriggerServerEvent("BuisnessCardRequest", Game.Player.ServerId);
                }
            };

            _interactionMenuPool.Add(_interactionMenu);
            _interactionMenuPool.RefreshIndex();
            _interactionMenuPool.CloseAllMenus(); ;

            Tick += new Func<Task>(async delegate
            {
                _interactionMenuPool.ProcessMenus();
                if ( API.IsInputDisabled(2) && API.IsControlJustReleased(0, 288) && !_interactionMenuPool.IsAnyMenuOpen() && !Restraints.Instance.Restrained)
                {
                    _interactionMenu.Visible = true;
                }
            });
        }

    }
}
