namespace roleplay.Main.Users.Customization
{
    public class CustomizationComponent
    {
        public int Component = 0;
        public int Drawable = 0;
        public int Texture = 0;
        public int Pallet = 0;

        public CustomizationComponent(int component, int drawable, int texture, int pallet)
        {
            Component = component;
            Drawable = drawable;
            Texture = texture;
            Pallet = pallet;
        }
    }
}
