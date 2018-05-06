using System;
using System.Drawing;
using System.Dynamic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using CitizenFX.Core.Native;
using Microsoft.Win32;
using NativeUI;

namespace roleplay
{
    public class Login : BaseScript
    {
        private bool FirstSpawn = true;
        private MenuPool _loginMenuPool;
        private UIMenu _loginMenu;

        public Login()
        {
            //UI CODE
            _loginMenuPool = new MenuPool();
            LoginUI();
            _loginMenuPool.RefreshIndex();
            _loginMenu.Visible = true;

           Tick += new Func<Task>(async delegate
            {
                _loginMenuPool.ProcessMenus();
            });


            EventHandlers["playerSpawned"] += new Action<ExpandoObject>(onSpawn);
        }



        private void LoginUI()
        {
            _loginMenu = new UIMenu("Character Selector", "Select An Character");
            _loginMenuPool.Add(_loginMenu);

            //Create New Character Button
            var newCharacterButton = new UIMenuItem("Create a new character");
            _loginMenu.AddItem(newCharacterButton);

            _loginMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item==newCharacterButton)
                {
                    Debug.WriteLine("TEST");
                }
            };

        }



        private void onSpawn(ExpandoObject pos)
        {
            if (FirstSpawn)
            {
                TriggerServerEvent("roleplay:setFirstSpawn");
                TriggerEvent("roleplay:firstSpawn");
                FirstSpawn = false;
                SetupCamera();
            }
        }

        private void SetupCamera()
        {
            var pid = API.PlayerPedId();
            if (API.IsEntityVisible(pid))
            {
                API.SetEntityVisible(pid,false,false);
            }
            API.SetPlayerInvincible(pid,true);
            API.FreezeEntityPosition(pid,true);
            API.SetEntityCoords(pid, -1400.11f, -1530.36f, 81.1f, true,false,false,true);
        }
    }
}
    