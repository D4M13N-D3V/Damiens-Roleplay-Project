using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using NativeUI;
using roleplay.Users.Login;

namespace roleplay.Users.Login
{
    public class Login : BaseScript
    {
        public static Login Instance;
        private bool _hasChosenCharacter = false;
        private bool FirstSpawn = true;
        private MenuPool _loginMenuPool;
        private UIMenu _loginMenu;

        private List<UIMenuItem> _characterMenuItems = new List<UIMenuItem>();

        private UIMenu _creationMenu;
        private bool isKeyboardUp = false;

        public List<CharacterUIEntry> CharacterUIList = new List<CharacterUIEntry>();

        public Login()
        {
            Instance = this;
            EventHandlers["playerSpawned"] += new Action<ExpandoObject>(onSpawn);
            EventHandlers["refreshCharacters"] += new Action<dynamic,dynamic,dynamic,dynamic>(RefreshCharacters);
            EventHandlers["characterSelected"] += new Action<dynamic, dynamic, dynamic, dynamic>(SelectCharacter);
            //UI CODE
            _loginMenuPool = new MenuPool();
            LoginUI();
            _loginMenuPool.RefreshIndex();
            _loginMenu.Visible = true;

           Tick += new Func<Task>(async delegate
            {

                if (API.UpdateOnscreenKeyboard() == 1 && isKeyboardUp)
                {
                    _creationMenu.MenuItems[_creationMenu.CurrentSelection].Text = API.GetOnscreenKeyboardResult();
                    API.EnableAllControlActions(0);
                    isKeyboardUp = false;
                    _creationMenu.Visible = true;
                }
                else if (API.UpdateOnscreenKeyboard() == 2  && isKeyboardUp)
                {
                    isKeyboardUp = false;
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
            CharacterUIList.Clear();;
            SetupCreationMenu();
            if (firstNames.Count == lastNames.Count)
            {
                for (int i = 0; i < firstNames.Count; i++)
                {
                    Debug.Write(Convert.ToString(genders[i]));
                    var uiEntry = new CharacterUIEntry(_loginMenuPool,_loginMenu, firstNames[i], lastNames[i], dateOfBirths[i],genders[i]);
                    CharacterUIList.Add(uiEntry);
                }
            }
        }

        private void LoginUI()
        {
            _loginMenu = new UIMenu("Character Selector", "Select An Character");
            _loginMenuPool.Add(_loginMenu);
            SetupCreationMenu();;
        }

        private void SetupCreationMenu()
        {
            //Create New Character Button
            _creationMenu = _loginMenuPool.AddSubMenu(_loginMenu, "~g~Create a new character!", "Start the process of creating your character here.");
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
                    isKeyboardUp = true;
                }
                else if (item == createButton)
                {
                    TriggerServerEvent("characterCreationRequest", firstNameButton.Text, lastNameButton.Text, dateOfBirthButton.Text,genderOption.Index);
                }
            };
        }

        public void SelectCharacterRequest(string firstName, string lastName)
        {
            TriggerServerEvent("selectCharacterRequest", firstName,lastName);
        }

        public void DeleteCharacterRequest(string firstName, string lastName)
        {
            TriggerServerEvent("deleteCharacterRequest", firstName, lastName);
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

        public async void SelectCharacter(dynamic x, dynamic y, dynamic z, dynamic model)
        {
            Utility.Instance.Log(model);
            User.Instance.StartUpdatingPosistion();
            _loginMenuPool.CloseAllMenus();
            _hasChosenCharacter = true;
            var pid = API.PlayerPedId();
            API.SetEntityVisible(pid, true, true);
            API.FreezeEntityPosition(pid,false);
            API.SetPlayerInvincible(pid, false);
            API.SetEntityCoords(pid, x, y, z,true,false,false,true);
            Utility.Instance.Log("Player Posistion Loaded!");

            var modelHash = (uint)API.GetHashKey(model);
            API.RequestModel(modelHash);
            Utility.Instance.Log("Loading Player Model");
            while (API.HasModelLoaded(modelHash)==false)
            {
                await Delay(0);
            }
            Utility.Instance.Log("Player Model Loaded!");
            API.SetPlayerModel(API.PlayerId(), modelHash);
            Utility.Instance.Log("Player model has been set to the player!");
            ClothesManager.Instance.modelSet = true;
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
    