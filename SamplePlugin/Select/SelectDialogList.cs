using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace InfiniteRoleplay {
    public class SelectDialogList {
        private readonly SelectDialog Dialog;
        private readonly List<SelectResult> Items;
        private SelectResult Selected;
        private bool IsSelected = false;
        private readonly string Name;

        public SelectDialogList( SelectDialog dialog, string name, List<SelectResult> items ) {
            Dialog = dialog;
            Name = name;
            Items = items;
        }

        public void Draw( string parentId ) {
            if( Items == null ) return;

            var id = $"{parentId}/{Name}";
            if( !ImGui.BeginTabItem( $"{Name}{id}" ) ) return;

            var footerHeight = ImGui.GetFrameHeightWithSpacing();
            ImGui.BeginChild( id + "/Child", new Vector2( 0, -footerHeight ), true );

            if( ImGui.BeginTable( $"{id}/Table", 2, ImGuiTableFlags.RowBg ) ) {
                ImGui.TableSetupColumn( $"{id}/Col1", ImGuiTableColumnFlags.WidthFixed, 20 );
                ImGui.TableSetupColumn( $"{id}/Col2", ImGuiTableColumnFlags.WidthStretch );

                var idx = 0;
                foreach( var item in Items ) {
                    if( item.Type == SelectResultType.Local && !Dialog.ShowLocal ) continue;

                    ImGui.TableNextRow();
                    ImGui.TableNextColumn();

                    
                    ImGui.TableNextColumn();

                    if( ImGui.Selectable( item.DisplayString + id + idx, Selected.Equals( item ) ) ) {
                        IsSelected = true;
                        Selected = item;
                        break;
                    }
                    if( ImGui.IsMouseDoubleClicked( ImGuiMouseButton.Left ) && ImGui.IsItemHovered() ) {
                        Dialog.Invoke( Selected );
                        break;
                    }
                    idx++;
                }

                ImGui.EndTable();
            }

            ImGui.EndChild();
            // Disable button if nothing selected
            if( !IsSelected ) ImGui.PushStyleVar( ImGuiStyleVar.Alpha, ImGui.GetStyle().Alpha * 0.5f );
            if( ImGui.Button( "SELECT" + id ) && IsSelected ) Dialog.Invoke( Selected );
            if( !IsSelected ) ImGui.PopStyleVar();

            ImGui.SameLine();
            ImGui.TextDisabled( "Double-clicking can also be used to select items" );

            ImGui.EndTabItem();
        }
    }
}
