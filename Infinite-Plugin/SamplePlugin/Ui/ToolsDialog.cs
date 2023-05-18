using ImGuiNET;
using System.Numerics;

namespace InfiniteRoleplay.Ui {
    public partial class ToolsDialog : GenericDialog {

        public ToolsDialog() : base( "Tools", false, 300, 400 ) {
        }

        public override void DrawBody() {
            if( ImGui.BeginTabBar( "##ToolsTabs", ImGuiTabBarFlags.NoCloseWithMiddleMouseButton ) ) {
                if( ImGui.BeginTabItem( "Resources##ToolsTabs" ) ) {
                    ImGui.EndTabItem();
                }
                if( ImGui.BeginTabItem( "Utilities##ToolsTabs" ) ) {
                    ImGui.EndTabItem();
                }
                ImGui.EndTabBar();
            }
        }
    }
}
