using Dalamud.Interface.Colors;
using Dalamud.Interface.GameFonts;
using Dalamud.Interface.Internal;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using ImGuiNET;
using Newtonsoft.Json.Linq;
using OtterGui;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace InfiniteRoleplay.Helpers
{
    public class Misc
    {
        public static float _modVersionWidth;
        public static GameFontHandle _nameFont;
        public static int loaderIndex = 0;
        public static IDalamudTextureWrap loaderAnimInd;
        public static Plugin pg;
        public static void SetTitle(Plugin plugin, string title)
        {
            _nameFont = plugin.PluginInterfacePub.UiBuilder.GetGameFontHandle(new GameFontStyle(GameFontFamilyAndSize.Jupiter23));
            int NameWidth = title.Length * 10;
            var decidingWidth = Math.Max(500, ImGui.GetWindowWidth());
            var offsetWidth = (decidingWidth - NameWidth) / 2;
            var offsetVersion = title.Length > 0
                ? _modVersionWidth + ImGui.GetStyle().ItemSpacing.X + ImGui.GetStyle().WindowPadding.X
                : 0;
            var offset = Math.Max(offsetWidth, offsetVersion);
            if (offset > 0)
            {
                ImGui.SetCursorPosX(offset);
            }


            using var col = ImRaii.PushColor(ImGuiCol.Border, ImGuiColors.DalamudViolet);
            using var style = ImRaii.PushStyle(ImGuiStyleVar.FrameBorderSize, 2 * ImGuiHelpers.GlobalScale);
            using var font = ImRaii.PushFont(_nameFont.ImFont, _nameFont.Available);
            ImGuiUtil.DrawTextButton(title, Vector2.Zero, 0);

            using var defInfFontDen = ImRaii.DefaultFont();
            using var defCol = ImRaii.DefaultColors();
            using var defStyle = ImRaii.DefaultStyle();
        }

        public void OnEventExecution(System.Object? sender, ElapsedEventArgs eventArgs)
        {
            loaderIndex++;
            if (loaderIndex >= 59)
            {
                loaderIndex = 1;
            }
            loaderAnimInd = pg.PluginInterfacePub.UiBuilder.LoadImage(Path.Combine(pg.PluginInterfacePub.AssemblyLocation.Directory?.FullName!, "UI/common/loader/loader (" + loaderIndex + ").gif"));
        }
        public static void RemoveLoader(System.Timers.Timer timer)
        {
            timer.Stop();
        }
        public void AddLoader(System.Timers.Timer timer)
        {           
            StartLoader(timer);
        }
        public static void StartLoader(System.Timers.Timer timer)
        {
            timer.Start();
            int LoaderWidth = 360;
            var decidingWidth = Math.Max(500, ImGui.GetWindowWidth());
            var offsetWidth = (decidingWidth - LoaderWidth) / 2;
            var offsetVersion = LoaderWidth > 0
                ? _modVersionWidth + ImGui.GetStyle().ItemSpacing.X + ImGui.GetStyle().WindowPadding.X
                : 0;
            var offset = Math.Max(offsetWidth, offsetVersion);
            if (offset > 0)
            {
                ImGui.SetCursorPosX(offset);
            }
            ImGui.Image(loaderAnimInd.ImGuiHandle, new Vector2(340, 180));
        }
        public static byte[] RemoveBytes(byte[] input, byte[] pattern)
        {
            if (pattern.Length == 0) return input;
            var result = new List<byte>();
            for (int i = 0; i < input.Length; i++)
            {
                var patternLeft = i <= input.Length - pattern.Length;
                if (patternLeft && (!pattern.Where((t, j) => input[i + j] != t).Any()))
                {
                    i += pattern.Length - 1;
                }
                else
                {
                    result.Add(input[i]);
                }
            }
            return result.ToArray();
        }
        public static string GetBetween(string content, string startString, string endString)
        {
            int Start = 0, End = 0;
            if (content.Contains(startString) && content.Contains(endString))
            {
                Start = content.IndexOf(startString, 0) + startString.Length;
                End = content.IndexOf(endString, Start);
                return content.Substring(Start, End - Start);
            }
            else
                return string.Empty;
        }
        public static void RemoveAt<T>(ref T[] arr, int index)
        {
            for (int a = index; a < arr.Length - 1; a++)
            {
                // moving elements downwards, to fill the gap at [index]
                arr[a] = arr[a + 1];
            }
            // finally, let's decrement Array's size by one
            Array.Resize(ref arr, arr.Length - 1);
        }
        

    }
}
