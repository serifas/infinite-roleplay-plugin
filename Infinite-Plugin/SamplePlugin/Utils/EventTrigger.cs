using FFXIVClientStructs.FFXIV.Common.Math;
using InfiniteRoleplay.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiniteRoleplay.Utils
{
    internal class EventTrigger
    {
        public static void TriggerEventWindow(Plugin plugin, string title, string descripition, Vector4 titleCol, Vector4 descriptionCol)
        {
            MessageBox.TITLE = title;
            MessageBox.DESCRIPTION = descripition;
            MessageBox.TITLE_COLOR = titleCol;
            MessageBox.DESC_COLOR = descriptionCol;
            plugin.WindowSystem.GetWindow("EVENT").IsOpen = true;
        }
    }
}
