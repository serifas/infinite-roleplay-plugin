using ImGuiNET;
using System;
using System.Collections.Generic;

namespace InfiniteRoleplay.Select.PapSelect {
    public class PapSelectDialog : SelectDialog {
        private readonly List<SelectTab> GameTabs;

        public PapSelectDialog(
                string id,
                List<SelectResult> recentList,
                bool showLocal,
                Action<SelectResult> onSelect
            ) : base( id, "pap", recentList, null, showLocal, onSelect ) {

            GameTabs = new List<SelectTab>( new SelectTab[]{              
                
            } );
        }

        protected override List<SelectTab> GetTabs() => GameTabs;
    }
}
