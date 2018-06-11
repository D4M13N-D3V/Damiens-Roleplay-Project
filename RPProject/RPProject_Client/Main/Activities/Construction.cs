using System.Collections.Generic;
using CitizenFX.Core;

namespace client.Main.Activities
{
    public class ConstructionSite
    {
        public Vector3 Location;
        public List<Vector3> HammerSpots;
        public ConstructionSite(Vector3 _location, List<Vector3>hammerSpots)
        {
            Location = _location;
            HammerSpots = hammerSpots;
        }
    }

    public class Construction : BaseScript
    {
        public static Construction Instance;

        private readonly List<ConstructionSite> _sites = new List<ConstructionSite>()
        {
            new ConstructionSite( new Vector3(85.829498291016f,-380.9948425293f,40.890609741211f),
                new List<Vector3>()
                {
                    new Vector3(109.5673828125f,-379.96490478516f,41.805198669434f),
                    new Vector3(92.751502990723f,-364.35079956055f,42.159297943115f),
                    new Vector3(66.896003723145f,-354.74417114258f,42.52156829834f),
                    new Vector3(52.106685638428f,-348.41494750977f,42.524463653564f),
                    new Vector3(64.923469543457f,-334.89672851563f,43.754806518555f),
                    new Vector3(85.367309570313f,-328.50225830078f,44.164611816406f),
                    new Vector3(33.990737915039f,-432.0798034668f,45.557754516602f),
                    new Vector3(40.28197479248f,-390.83187866211f,45.546283721924f),
                    new Vector3(36.43293762207f,-377.45889282227f,45.501010894775f),
                    new Vector3(40.781120300293f,-368.40576171875f,45.500659942627f)
                }),
        };

        public Construction()
        {
            Instance = this;
        }
    }
}
