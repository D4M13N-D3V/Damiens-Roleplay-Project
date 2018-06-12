using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using NativeUI;
using client.Main;
using client.Main.Clothes;

namespace client.Main.Users.Login
{
    public class Login : BaseScript
    {
        public static Login Instance;
        private bool _hasChosenCharacter = false;
        private bool _firstSpawn = true;
        private readonly ModifiedMenuPool _loginMenuPool;
        private UIMenu _loginMenu;


        private bool _requestpending = false;

        private List<UIMenuItem> _characterMenuItems = new List<UIMenuItem>();

        private UIMenu _creationMenu;
        private bool _isKeyboardUp = false;

        public List<CharacterUIEntry> CharacterUiList = new List<CharacterUIEntry>();

        public Login()
        {
            Instance = this;
            EventHandlers["playerSpawned"] += new Action<ExpandoObject>(onSpawn);
            EventHandlers["refreshCharacters"] += new Action<dynamic,dynamic,dynamic,dynamic>(RefreshCharacters);
            EventHandlers["RequestReset"] += new Action(RequestReset);
            EventHandlers["characterSelected"] += new Action<dynamic, dynamic, dynamic, dynamic>(SelectCharacter);
            //UI CODE
            _loginMenuPool = new ModifiedMenuPool();
            LoginUI();
            _loginMenuPool.RefreshIndex();
            _loginMenu.Visible = true;

           Tick += new Func<Task>(async delegate
            {

                if (API.UpdateOnscreenKeyboard() == 1 && _isKeyboardUp)
                {
                    _creationMenu.MenuItems[_creationMenu.CurrentSelection].Text = new string(API.GetOnscreenKeyboardResult().Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '-' || c == '_').ToArray());
                    API.EnableAllControlActions(0);
                    _isKeyboardUp = false;
                    _creationMenu.Visible = true;
                }
                else if (API.UpdateOnscreenKeyboard() == 2  && _isKeyboardUp)
                {
                    _isKeyboardUp = false;
                    API.EnableAllControlActions(0);
                    _creationMenu.Visible = true;
                }

                if (_loginMenuPool.IsAnyMenuOpen() == false && _hasChosenCharacter==false)
                {
                    _loginMenu.Visible = true;
                }

                _loginMenuPool.ProcessMenus();
            });


        }

        private void RefreshCharacters(dynamic firstNames, dynamic lastNames, dynamic dateOfBirths, dynamic genders)
        {
            _loginMenuPool.CloseAllMenus();
            _loginMenu.Clear();
            CharacterUiList.Clear();;
            SetupCreationMenu();
            if (firstNames.Count == lastNames.Count)
            {
                for (int i = 0; i < firstNames.Count; i++)
                {
                    Debug.Write(Convert.ToString(genders[i]));
                    var uiEntry = new CharacterUIEntry(_loginMenuPool,_loginMenu, firstNames[i], lastNames[i], dateOfBirths[i],genders[i]);
                    CharacterUiList.Add(uiEntry);
                }
            }
            _loginMenuPool.RefreshIndex();
        }

        private void LoginUI()
        {
            _loginMenu = new UIMenu("Character Selector", "Select An Character", new PointF(Screen.Width / 2, Screen.Height / 2));
            _loginMenuPool.Add(_loginMenu);
            SetupCreationMenu();;
        }

        private void SetupCreationMenu()
        {
            //Create New Character Button
            _creationMenu = _loginMenuPool.AddSubMenuOffset(_loginMenu, "~g~Create a new character!", "Start the process of creating your character here.", new PointF(Screen.Width / 2, Screen.Height / 2));
            var firstNameButton = new UIMenuItem("First Name", "The first name of your character.");
            var lastNameButton = new UIMenuItem("Last Name", "The last name of your character.");
            var dateOfBirthButton = new UIMenuItem("Date of birth", "The date of birth of your character.");

            List<dynamic> itemList = new List<dynamic>();
            var maleOption = new UIMenuItem("Male");
            var femaleOption = new UIMenuItem("Female");
            itemList.Add(maleOption);
            itemList.Add(femaleOption);
            var genderOption = new UIMenuSliderItem("Male/Female", itemList, 0, "The gender of your character", true);
            var createButton = new UIMenuItem("Finalize Character", "Finalize the creation of your character!");
            _creationMenu.AddItem(firstNameButton);
            _creationMenu.AddItem(lastNameButton);
            _creationMenu.AddItem(dateOfBirthButton);
            _creationMenu.AddItem(genderOption);
            _creationMenu.AddItem(createButton);
            _creationMenu.OnMenuClose += (sender) =>
            {
                firstNameButton.Text = "First Name";
                lastNameButton.Text = "Last Name";
                dateOfBirthButton.Text = "Date of birth";
            };
            _creationMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == firstNameButton || item == lastNameButton || item == dateOfBirthButton)
                {
                    API.DisplayOnscreenKeyboard(1, "", "First Name Of Your Character", "", "", "", "", 15);
                    API.DisableAllControlActions(0);
                    _creationMenu.Visible = false;
                    _isKeyboardUp = true;
                }
                else if (item == createButton)
                {
                    if (_requestpending) { return; }
                    _requestpending = true;
                    TriggerServerEvent("characterCreationRequest", firstNameButton.Text, lastNameButton.Text, dateOfBirthButton.Text,genderOption.Index);
                }
            };
        }
        public void SelectCharacterRequest(string firstName, string lastName)
        {
            if (_requestpending) { return; }
            _requestpending = true;
            TriggerServerEvent("selectCharacterRequest", firstName,lastName);
        }

        public void DeleteCharacterRequest(string firstName, string lastName)
        {
            if (_requestpending) { return; }
            _requestpending = true;
            TriggerServerEvent("deleteCharacterRequest", firstName, lastName);
        }

        private void onSpawn(ExpandoObject pos)
        {
            if (_firstSpawn)
            {
            API.NetworkSetTalkerProximity(15);
                TriggerServerEvent("roleplay:setFirstSpawn");
                TriggerEvent("roleplay:firstSpawn");
                _firstSpawn = false;
                SetupCamera();
            }
        }

        public void RequestReset()
        {
            _requestpending = false;
        }

        public async void SelectCharacter(dynamic x, dynamic y, dynamic z, dynamic model)
        {
            API.DisablePlayerVehicleRewards(Game.PlayerPed.Handle);
            API.DisablePlayerVehicleRewards(Game.Player.Handle);
            _requestpending = false;
            Utility.Instance.Log(model);
            User.Instance.StartUpdatingPosistion();
            _loginMenuPool.CloseAllMenus();
            _hasChosenCharacter = true;
            var pid = Game.PlayerPed.Handle;
            API.SetEntityCoords(pid, x, y, z,true,false,false,true);
            Utility.Instance.Log("Player Posistion Loaded!");

            var modelHash = (uint)API.GetHashKey(model);
            API.RequestModel(modelHash);
            Utility.Instance.Log("Loading Player Model");
            while (API.HasModelLoaded(modelHash)==false)
            {
                await Delay(0);
            }

            var rdm = new Random();

            API.SetPedComponentVariation(Game.PlayerPed.Handle, 3, rdm.Next(0,30), 0, 0);
            API.SetPedComponentVariation(Game.PlayerPed.Handle, 4, rdm.Next(0, 30), 0, 0);
            API.SetPedComponentVariation(Game.PlayerPed.Handle, 5, rdm.Next(0, 30), 0, 0);
            API.SetPedComponentVariation(Game.PlayerPed.Handle, 6, rdm.Next(0, 30), 0, 0);
            API.SetPedComponentVariation(Game.PlayerPed.Handle, 7, rdm.Next(0, 30), 0, 0);
            API.SetPedComponentVariation(Game.PlayerPed.Handle, 8, rdm.Next(0, 30), 0, 0);
            API.SetPedComponentVariation(Game.PlayerPed.Handle, 11, rdm.Next(0, 30), 0, 0);

            API.SetEntityVisible(pid, true, true);
            API.FreezeEntityPosition(pid, false);
            API.SetPlayerInvincible(pid, false);
            Utility.Instance.Log("Player Model Loaded!");
            API.SetPlayerModel(API.PlayerId(), modelHash);
            Utility.Instance.Log("Player model has been set to the player!");
            await Delay(0);
            ClothesManager.Instance.modelSet = true;
        }

        private void SetupCamera()
        {
            var pid = Game.PlayerPed.Handle;
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
    