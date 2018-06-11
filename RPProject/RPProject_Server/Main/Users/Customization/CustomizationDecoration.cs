namespace server.Main.Users.Customization
{
    public class CustomizationDecoration
    {
        public CustomizationDecoration(string collection, string overlay)
        {
            Collection = collection;
            Overlay = overlay;
        }

        public string Collection;
        public string Overlay;
    }
}
