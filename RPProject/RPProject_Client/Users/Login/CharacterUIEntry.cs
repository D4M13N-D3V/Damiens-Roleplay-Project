using System.Drawing;
using CitizenFX.Core.UI;
using NativeUI;
using roleplay.Main;

namespace roleplay.Users.Login
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

        public CharacterUIEntry(ModifiedMenuPool pool, UIMenu mainMenu,string firstName, string lastName, string dateOfBirth, int gender)
        {
            var genderstring = "Male";
            if (gender == 1)
            {
                genderstring = "Female";
            }

            Menu = pool.AddSubMenuOffset(mainMenu, firstName+" "+lastName, firstName+"\n"+lastName+"\n"+dateOfBirth+"\n"+ genderstring,new PointF(Screen.Width/2,Screen.Height/2));
            SelectButton = new UIMenuItem("~b~Play This Character!");
            YesButton = new UIMenuItem("~r~Delete!", "Permantly delete this character!");
            NoButton = new UIMenuItem("~g~No!", "Dont permantly delete this character!");
            DeleteMenu = pool.AddSubMenuOffset(Menu,"~r~Delete Character", new PointF(Screen.Width / 2, Screen.Height / 2));
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
