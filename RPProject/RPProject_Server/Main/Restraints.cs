using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace roleplay.Main
{
    public class Restraints : BaseScript
    {
        public static Restraints Instance;

        public Restraints()
        {
            Instance = this;
            EventHandlers["DragRequest"] += new Action<Player, int>(DragRequest);
            EventHandlers["RestrainRequest"] += new Action<Player,int,int>(RestrainRequest);
        }

        private void RestrainRequest([FromSource] Player restrainer, int target, int type)
        {
            var plyList = new PlayerList();
            var tgtPly = plyList[target];
            TriggerClientEvent(restrainer,"Restrain", Convert.ToInt32(tgtPly.Handle));
            TriggerClientEvent(tgtPly,"Restrained", type, Convert.ToInt32(restrainer.Handle));
        }

        private void DragRequest([FromSource] Player dragger, int draged)
        {
            var plyList = new PlayerList();
            var tgtPly = plyList[draged];
            TriggerClientEvent(tgtPly, "Dragged", dragger.Handle);
            TriggerClientEvent(dragger, "Dragging");
        }
    }
}
