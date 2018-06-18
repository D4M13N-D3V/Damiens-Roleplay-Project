using System;
using CitizenFX.Core;

namespace server.Main.Items
{
    /// <summary>
    /// SErver side restraints manager
    /// </summary>
    public class Restraints : BaseScript
    {
        /// <summary>
        /// Singleton Instance
        /// </summary>
        public static Restraints Instance;

        public Restraints()
        {
            Instance = this;
            EventHandlers["DragRequest"] += new Action<Player, int>(DragRequest);
            EventHandlers["RestrainRequest"] += new Action<Player, int, int>(RestrainRequest);
            EventHandlers["ForceEnterVehicleRequest"] += new Action<Player, int>(ForceEnterVehicleRequest);
        }

        /// <summary>
        /// Event handler for when somene tryst ob e drestrained
        /// </summary>
        /// <param name="restrainer">person trying to restrain</param>
        /// <param name="target">the target being restraianed</param>
        /// <param name="type">the type of restarints</param>
        private void RestrainRequest([FromSource] Player restrainer, int target, int type)
        {
            var plyList = new PlayerList();
            var targetPlayer = plyList[target];
            TriggerClientEvent(targetPlayer,"Restrained",type);
        }

        /// <summary>
        /// Event handler for attemping to drag a player
        /// </summary>
        /// <param name="dragger">person</param>
        /// <param name="draged"></param>
        private void DragRequest([FromSource] Player dragger, int draged)
        {
            var plyList = new PlayerList();
            var tgtPly = plyList[draged];
            TriggerClientEvent(tgtPly, "Dragged", Convert.ToInt32(dragger.Handle));
        }

        /// <summary>
        /// Event handler for trying to force someone into the vehicle
        /// </summary>
        /// <param name="dragger"></param>
        /// <param name="draged"></param>
        private void ForceEnterVehicleRequest([FromSource] Player dragger, int draged)
        {
            var plyList = new PlayerList();
            var tgtPly = plyList[draged];
            TriggerClientEvent(tgtPly, "Forced");
        }
    }
}
