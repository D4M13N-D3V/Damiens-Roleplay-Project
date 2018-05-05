namespace roleplay.Main.Users.Customization
{
    public class CustomizationHeadOverlay
    {
        public int Id = 0;
        public int Index = 0;
        public int Opacity = 255;
        public int PrimaryColor = 0;
        public int SecondaryColor = 0;
        public int ColorType = 0;

        public CustomizationHeadOverlay(int overlayID, int overlayIndex, int colorPrimary, int colorSecondary, int colorType, int opacity)
        {
            Id = overlayID;
            Index = overlayIndex;
            PrimaryColor = colorPrimary;
            SecondaryColor = colorSecondary;
            ColorType = colorType;
            Opacity = opacity;
        }

    }
}
