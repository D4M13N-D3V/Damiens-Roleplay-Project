using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NativeUI;
using CitizenFX;
namespace roleplay
{
    public class CharacterUIEntry
    {
        public UIMenu Menu;
        public UIMenuItem SelectButton;
        public UIMenuItem DeleteButton;
        public UIMenuItem BackButton;

        public UIMenu DeleteMenu;
        public UIMenuItem YesButton;
        public UIMenuItem NoButton;

        public CharacterUIEntry(MenuPool pool, UIMenu mainMenu,string firstName, string lastName, string dateOfBirth, int gender)
        {
            var genderstring = "Male";
            if (gender == 1)
            {
                genderstring = "Female";
            }

            Menu = pool.AddSubMenu(mainMenu, firstName+" "+lastName, firstName+"\n"+lastName+"\n"+dateOfBirth+"\n"+ genderstring);
            SelectButton = new UIMenuItem("~b~Play This Character!");
            YesButton = new UIMenuItem("~r~Delete!", "Permantly delete this character!");
            NoButton = new UIMenuItem("~g~No!", "Dont permantly delete this character!");
            DeleteMenu = pool.AddSubMenu(Menu,"~r~Delete Character");
            DeleteMenu.AddItem(YesButton);
            DeleteMenu.AddItem(NoButton);
            Menu.AddItem(SelectButton);

            Menu.OnItemSelect += (sender, item, index) =>
            {
                if (item == SelectButton)
                {
                    Login.Instance.SelectCharacterRequest(firstName, lastName);
                }
            };

            DeleteMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == YesButton)
                {
                    Login.Instance.DeleteCharacterRequest(firstName,lastName);
                }
                else if (item == NoButton)
                {
                    DeleteMenu.Visible = false;
                    Menu.Visible = true;
                }
            };
        }
    }
}
