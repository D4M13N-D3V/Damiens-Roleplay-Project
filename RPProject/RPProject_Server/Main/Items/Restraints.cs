using System;
using CitizenFX.Core;

namespace server.Main.Items
{
    public class Restraints : BaseScript
    {
        public static Restraints Instance;

        public Restraints()
        {
            Instance = this;
            EventHandlers["DragRequest"] += new Action<Player, int>(DragRequest);
            EventHandlers["RestrainRequest"] += new Action<Player, int, int>(RestrainRequest);
            EventHandlers["ForceEnterVehicleRequest"] += new Action<Player, int>(ForceEnterVehicleRequest);
        }

        private void RestrainRequest([FromSource] Player restrainer, int target, int type)
        {
            var plyList = new PlayerList();
            var targetPlayer = plyList[target];
            TriggerClientEvent(targetPlayer,"Restrained",type);
        }

        private void DragRequest([FromSource] Player dragger, int draged)
        {
            var plyList = new PlayerList();
            var tgtPly = plyList[draged];
            TriggerClientEvent(tgtPly, "Dragged", Convert.ToInt32(dragger.Handle));
        }

        private void ForceEnterVehicleRequest([FromSource] Player dragger, int draged)
        {
            var plyList = new PlayerList();
            var tgtPly = plyList[draged];
            TriggerClientEvent(tgtPly, "Forced");
        }
    }
}
