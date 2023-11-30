using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace InfiniteRoleplay.Windows.Defines
{
    internal class ProfileDefines
    {

        public enum InputTypes
        {
            multiline = 1,
            single = 2,
        }
        /*
                    ImGui.Text("Name:   ");
                    ImGui.SameLine();
                    ImGui.InputTextWithHint("##playername", $"Character Name (The name or nickname of the character you are currently playing as)", ref characterAddName, 100);
                    //race input
                    ImGui.Text("Race:    ");
                    ImGui.SameLine();
                    ImGui.InputTextWithHint("##race", $"The IC Race of your character", ref characterAddRace, 100);
                    //gender input
                    ImGui.Text("Gender: ");
                    ImGui.SameLine();
                    ImGui.InputTextWithHint("##gender", $"The IC gender of your character", ref characterAddGender, 100);
                    //age input
                    ImGui.Text("Age:    ");
                    ImGui.SameLine();
                    ImGui.InputTextWithHint("##age", $"The IC age of your character if your character is not 18+, do not put nsfw images in your gallery (numbers only)", ref characterAddAge, 100, ImGuiInputTextFlags.CharsHexadecimal);
                    //age input
                    ImGui.Text("Height:");
                    ImGui.SameLine();
                    ImGui.InputTextWithHint("##height", $"Height in Fulms (numbers only)", ref characterAddHeight, 100);
                    //age input
                    ImGui.Text("Weight:");
                    ImGui.SameLine();
                    ImGui.InputTextWithHint("##weight", $"Weight in Ponze (numbers only)", ref characterAddWeight, 100);
                    //at first glance input
                    ImGui.Text("At First Glance:");
                    ImGui.SameLine();
                    ImGui.InputTextMultiline("##afg", ref characterAddAfg, 500, new Vector2(400, 100));
        */

        //specifies type, field string values, size and max characters
        public static List<Tuple<InputTypes, string[], Vector2, int>> ProfileBioFields()
        {
            string name = "", race = "", gender = "", age = "", height = "", weight = "", afg = "";
            List<Tuple<InputTypes, string[], Vector2, int>> result = new List<Tuple<InputTypes, string[], Vector2, int>>();
            string[] nameFields = new string[] { "Name:   ", "##playername", $"Character Name (The name or nickname of the character you are currently playing as)", name};
            string[] raceFields = new string[] { "Race:    ", "##race", $"The IC Race of your character", race };
            string[] genderFields = new string[] { "Gender: ", "##gender", $"The IC gender of your character", gender };
            string[] ageFields = new string[] { "Age:   ", "##age", $"If not 18+, do not put nsfw images in your gallery (numbers only)", age };
            string[] heightFields = new string[] { "Height:", "##height", $"Character Name (The name or nickname of the character you are currently playing as)", height };
            string[] weightFields = new string[] { "Weight:", "##weight", $"Character Name (The name or nickname of the character you are currently playing as)", weight };
            string[] atFirstGlance = new string[] { "At First Glance:", "##afg", $"Character Name (The name or nickname of the character you are currently playing as)", afg };


            Tuple<InputTypes, string[], Vector2, int> BioName = Tuple.Create(InputTypes.single, nameFields, new Vector2(0, 0), 100);
            Tuple<InputTypes, string[], Vector2, int> BioRace = Tuple.Create(InputTypes.single, nameFields, new Vector2(0, 0), 100);
            Tuple<InputTypes, string[], Vector2, int> BioGender = Tuple.Create(InputTypes.single, nameFields, new Vector2(0, 0), 100);
            Tuple<InputTypes, string[], Vector2, int> BioAge = Tuple.Create(InputTypes.single, nameFields, new Vector2(0, 0), 100);
            Tuple<InputTypes, string[], Vector2, int> BioHeight = Tuple.Create(InputTypes.single, nameFields, new Vector2(0, 0), 100);
            Tuple<InputTypes, string[], Vector2, int> BioWeight = Tuple.Create(InputTypes.single, nameFields, new Vector2(0, 0), 100);
            Tuple<InputTypes, string[], Vector2, int> BioAFG = Tuple.Create(InputTypes.multiline, nameFields, new Vector2(400, 100), 500);


            result.Add(BioName);
            result.Add(BioRace);
            result.Add(BioGender);
            result.Add(BioAge);
            result.Add(BioHeight);
            result.Add(BioWeight);
            result.Add(BioAFG);

            return result;
        }
    }
}
