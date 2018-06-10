using System.Collections.Generic;
namespace roleplay.Main.Users.Customization
{
    public class CharacterCustomization
    {
        public string Model = "";

        public CustomizationComponent Face = new CustomizationComponent(0, 0,0,0);
        public CustomizationComponent Head = new CustomizationComponent(1, 0, 0, 0);
        public CustomizationComponent Hair = new CustomizationComponent(2, 0, 0, 0);
        public CustomizationComponent Eyes = new CustomizationComponent(7, 0, 0, 0);
        public CustomizationComponent Torso = new CustomizationComponent(3, 1, 0, 0);
        public CustomizationComponent Torso2 = new CustomizationComponent(11, 1, 0, 0);
        public CustomizationComponent Legs = new CustomizationComponent(4, 1, 0, 0);
        public CustomizationComponent Hands = new CustomizationComponent(5, 1, 0, 0);
        public CustomizationComponent Feet = new CustomizationComponent(6, 1, 0, 0);
        public CustomizationComponent Tasks = new CustomizationComponent(9, 0, 0, 0);
        public CustomizationComponent Textures = new CustomizationComponent(10, 0, 0, 0);
        public CustomizationComponent Accessories = new CustomizationComponent(0, 1, 0, 0);

        public CustomizationProp Hats =  new CustomizationProp(0,-1,0,0);
        public CustomizationProp Glasses = new CustomizationProp(0, 0, 0, 0);
        public CustomizationProp Ears = new CustomizationProp(0, 0, 0, 0);
        public CustomizationProp Watches = new CustomizationProp(0, 0, 0, 0);

        public CustomizationHeadOverlay Blemishes = new CustomizationHeadOverlay(0,0,0,0,0,255);
        public CustomizationHeadOverlay FacialHair = new CustomizationHeadOverlay(0, 0, 0, 0, 1, 255);
        public CustomizationHeadOverlay Eyebrows = new CustomizationHeadOverlay(0, 0, 0, 0, 1, 255);
        public CustomizationHeadOverlay Ageing = new CustomizationHeadOverlay(0, 0, 0, 0, 0, 255);
        public CustomizationHeadOverlay Makeup = new CustomizationHeadOverlay(0, 0, 0, 0, 2, 255);
        public CustomizationHeadOverlay Blush = new CustomizationHeadOverlay(0, 0, 0, 0, 2, 255);
        public CustomizationHeadOverlay Complexion = new CustomizationHeadOverlay(0, 0, 0, 0, 0, 255);
        public CustomizationHeadOverlay SunDamage = new CustomizationHeadOverlay(0, 0, 0, 0, 0, 255);
        public CustomizationHeadOverlay Lipstick = new CustomizationHeadOverlay(0, 0, 0, 0, 0, 255);
        public CustomizationHeadOverlay Moles = new CustomizationHeadOverlay(0, 0, 0, 0, 0, 255);
        public CustomizationHeadOverlay ChestHair = new CustomizationHeadOverlay(0, 0, 0, 0, 1, 255);
        public CustomizationHeadOverlay BodyBlemishes = new CustomizationHeadOverlay(0, 0, 0, 0, 0, 255);

        public List<CustomizationDecoration> Tattoos = new List<CustomizationDecoration>();
    }
}
