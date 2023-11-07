using Dalamud.Interface.Internal;
using FFXIVClientStructs.Havok;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace InfiniteRoleplay.Windows.Defines
{
    internal class ProfileDefines
    {
        public static IDalamudTextureWrap lawfulgood, neutralgood, chaoticgood,
                                   lawfulneutral, trueneutral, chaoticneutral,
                                   lawfulevil, neutralevil, chaoticevil,

                                   //bars

                                   lawfulgoodBar, neutralgoodBar, chaoticgoodBar,
                                   lawfulneutralBar, trueneutralBar, chaoticneutralBar,
                                   lawfulevilBar, neutralevilBar, chaoticevilBar;
        public enum InputTypes
        {
            multiline = 1,
            single = 2,
        }
        public enum AlignmentType
        {
            lawfulgood = 1,
            neutralgood = 2,
            chaoticgood = 3,

            lawfulneutral = 4,
            trueneutral = 5,
            chaoticneutral = 6,

            lawfulevil = 7, 
            neutralevil = 8,
            chaoticevil = 9,
        }
        public enum EditFields
        {
            characterEditName = 1,
            characterEditRace = 2,
            characterEditGender = 3,
            characterEditAge = 4,
            characterEditAfg = 5,
            characterEditHeight = 6,
            characterEditWeight = 7
        }
        public enum AddFields
        {
            characterAddName = 1,
            characterAddRace = 2,
            characterAddGender = 3,
            characterAddAge = 4,
            characterAddAfg = 5,
            characterAddHeight = 6,
            characterAddWeight = 7,
        }
            
            
        /*
         //LAWFUL GOOD
                    ImGui.Image(this.lawfulGood.ImGuiHandle, new Vector2(32, 32));
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("LAWFUL GOOD:\n" +
                                        "    These characters always do the right thing as expected by society.\n" +
                                        "    They always follow the rules, tell the truth and help people out.\n" +
                                        "    They like order, trust and believe in people with social authority, \n" +
                                        "    and they aim to be an upstanding citizen.");
                    }
                    ImGui.SameLine();
                    if (ImGui.Button("+##lawfulgoodplus", new Vector2(20, 20))) { ModAlignment("lawfulgood", true); }
                    ImGui.SameLine();
                    if (ImGui.Button("-##lawfulgoodminus", new Vector2(20, 20))) { ModAlignment("lawfulgood", false); }
                    ImGui.SameLine();
                    ImGui.Image(this.lawfulGoodBar.ImGuiHandle, new Vector2(alignmentWidthVals[0] * 30, 20));
                    ImGui.SameLine();
                    ImGui.TextColored(new Vector4(1, 1, 1, 1), alignmentVals[0].ToString());
         
         */
        public static List<Tuple<InputTypes, string[], Vector2, int>> ProfileBioFields()
        {
            string name = "", race = "", gender = "", age = "", height = "", weight = "", afg = "";
            List<Tuple<InputTypes, string[], Vector2, int>> result = new List<Tuple<InputTypes, string[], Vector2, int>>();
            string[] nameFields = new string[] { "Name:   ", "##playername", $"Character Name (The name or nickname of the character you are currently playing as)", name };
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
        //sets the TextureWrap of the icon and bar, index of alignmentVals, tooltip info and tag for Modding Alignment
        public static List<Tuple<IDalamudTextureWrap, IDalamudTextureWrap, AlignmentType, string, string>> ProfileAlignmentFields(Plugin plugin)
        {
            List<Tuple<IDalamudTextureWrap,IDalamudTextureWrap, AlignmentType, string, string>> result = new List<Tuple<IDalamudTextureWrap,IDalamudTextureWrap, AlignmentType, string, string>>();
            //lawfulgood 
            Tuple<IDalamudTextureWrap, IDalamudTextureWrap, AlignmentType, string, string> LawfulGood = Tuple.Create(LoadAlignmentUIIcons(plugin)[(int)AlignmentType.lawfulgood], LoadAlignmentUIBars(plugin)[(int)AlignmentType.lawfulgood],
                                       AlignmentType.lawfulgood, "lawfulgood", AlignmentTooltips()[(int)AlignmentType.lawfulgood]);

            Tuple<IDalamudTextureWrap, IDalamudTextureWrap, AlignmentType, string, string> NeutralGood = Tuple.Create(LoadAlignmentUIIcons(plugin)[(int)AlignmentType.neutralgood], LoadAlignmentUIBars(plugin)[(int)AlignmentType.neutralgood],
                                       AlignmentType.neutralgood, "neutralgood", AlignmentTooltips()[(int)AlignmentType.neutralgood]);

            Tuple<IDalamudTextureWrap, IDalamudTextureWrap, AlignmentType, string, string> ChaoticGood = Tuple.Create(LoadAlignmentUIIcons(plugin)[(int)AlignmentType.chaoticgood], LoadAlignmentUIBars(plugin)[(int)AlignmentType.chaoticgood],
                                       AlignmentType.chaoticgood, "chaoticgood", AlignmentTooltips()[(int)AlignmentType.chaoticgood]);

            Tuple<IDalamudTextureWrap, IDalamudTextureWrap, AlignmentType, string, string> LawfulNeutral = Tuple.Create(LoadAlignmentUIIcons(plugin)[(int)AlignmentType.lawfulneutral], LoadAlignmentUIBars(plugin)[(int)AlignmentType.lawfulneutral],
                                       AlignmentType.lawfulneutral, "lawfulneutral", AlignmentTooltips()[(int)AlignmentType.lawfulneutral]);

            Tuple<IDalamudTextureWrap, IDalamudTextureWrap, AlignmentType, string, string> TrueNeutral = Tuple.Create(LoadAlignmentUIIcons(plugin)[(int)AlignmentType.trueneutral], LoadAlignmentUIBars(plugin)[(int)AlignmentType.trueneutral],
                                       AlignmentType.trueneutral, "trueneutral", AlignmentTooltips()[(int)AlignmentType.trueneutral]);

            Tuple<IDalamudTextureWrap, IDalamudTextureWrap, AlignmentType, string, string> ChaoticNeutral = Tuple.Create(LoadAlignmentUIIcons(plugin)[(int)AlignmentType.chaoticneutral], LoadAlignmentUIBars(plugin)[(int)AlignmentType.chaoticneutral],
                                       AlignmentType.chaoticneutral, "chaoticneutral", AlignmentTooltips()[(int)AlignmentType.chaoticneutral]);

            Tuple<IDalamudTextureWrap, IDalamudTextureWrap, AlignmentType, string, string> LawfulEvil = Tuple.Create(LoadAlignmentUIIcons(plugin)[(int)AlignmentType.lawfulevil], LoadAlignmentUIBars(plugin)[(int)AlignmentType.lawfulevil],
                                       AlignmentType.lawfulevil, "lawfulevil", AlignmentTooltips()[(int)AlignmentType.lawfulevil]);

            Tuple<IDalamudTextureWrap, IDalamudTextureWrap, AlignmentType, string, string> NeutralEvil = Tuple.Create(LoadAlignmentUIIcons(plugin)[(int)AlignmentType.neutralevil], LoadAlignmentUIBars(plugin)[(int)AlignmentType.neutralevil],
                                       AlignmentType.neutralevil, "neutralevil", AlignmentTooltips()[(int)AlignmentType.neutralevil]);

            Tuple<IDalamudTextureWrap, IDalamudTextureWrap, AlignmentType, string, string> ChaoticEvil = Tuple.Create(LoadAlignmentUIIcons(plugin)[(int)AlignmentType.chaoticevil], LoadAlignmentUIBars(plugin)[(int)AlignmentType.chaoticevil],
                                       AlignmentType.chaoticevil, "chaoticevil", AlignmentTooltips()[(int)AlignmentType.chaoticevil]);


            result.Add(LawfulGood);
            result.Add(NeutralGood);
            result.Add(ChaoticGood);
            result.Add(LawfulNeutral);
            result.Add(TrueNeutral);
            result.Add(ChaoticNeutral);
            result.Add(LawfulEvil);
            result.Add(NeutralEvil);
            result.Add(ChaoticEvil);
            return result;
        }
        public static IDalamudTextureWrap[] LoadAlignmentUIIcons(Plugin plugin)
        {
            Task.Run(async () =>
            {
                lawfulgood = plugin.PluginInterfacePub.UiBuilder.LoadImage(Path.Combine(plugin.PluginInterfacePub.AssemblyLocation.Directory?.FullName!, "UI/alignments/lawful_good.png"));
                neutralgood = plugin.PluginInterfacePub.UiBuilder.LoadImage(Path.Combine(plugin.PluginInterfacePub.AssemblyLocation.Directory?.FullName!, "UI/alignments/neutral_good.png"));
                chaoticgood = plugin.PluginInterfacePub.UiBuilder.LoadImage(Path.Combine(plugin.PluginInterfacePub.AssemblyLocation.Directory?.FullName!, "UI/alignments/chaotic_good.png"));
                lawfulneutral = plugin.PluginInterfacePub.UiBuilder.LoadImage(Path.Combine(plugin.PluginInterfacePub.AssemblyLocation.Directory?.FullName!, "UI/alignments/lawful_neutral.png"));
                trueneutral = plugin.PluginInterfacePub.UiBuilder.LoadImage(Path.Combine(plugin.PluginInterfacePub.AssemblyLocation.Directory?.FullName!, "UI/alignments/true_neutral.png"));
                chaoticneutral = plugin.PluginInterfacePub.UiBuilder.LoadImage(Path.Combine(plugin.PluginInterfacePub.AssemblyLocation.Directory?.FullName!, "UI/alignments/chaotic_neutral.png"));
                lawfulevil = plugin.PluginInterfacePub.UiBuilder.LoadImage(Path.Combine(plugin.PluginInterfacePub.AssemblyLocation.Directory?.FullName!, "UI/alignments/lawful_evil.png"));
                neutralevil = plugin.PluginInterfacePub.UiBuilder.LoadImage(Path.Combine(plugin.PluginInterfacePub.AssemblyLocation.Directory?.FullName!, "UI/alignments/neutral_evil.png"));
                chaoticevil = plugin.PluginInterfacePub.UiBuilder.LoadImage(Path.Combine(plugin.PluginInterfacePub.AssemblyLocation.Directory?.FullName!, "UI/alignments/chaotic_evil.png"));
            
              });
            IDalamudTextureWrap[] result = new IDalamudTextureWrap[9] {
                lawfulgood,
                neutralgood,
                chaoticgood,
                lawfulneutral,
                trueneutral,
                chaoticneutral,
                lawfulevil,
                neutralevil,
                chaoticevil
            };
            return result;
        }
        public static IDalamudTextureWrap[] LoadAlignmentUIBars(Plugin plugin)
        {
            Task.Run(async () =>
            {
                lawfulgoodBar = plugin.PluginInterfacePub.UiBuilder.LoadImage(Path.Combine(plugin.PluginInterfacePub.AssemblyLocation.Directory?.FullName!, "UI/alignments/lawful_good_bar.png"));
                neutralgoodBar = plugin.PluginInterfacePub.UiBuilder.LoadImage(Path.Combine(plugin.PluginInterfacePub.AssemblyLocation.Directory?.FullName!, "UI/alignments/neutral_good_bar.png"));
                chaoticgoodBar = plugin.PluginInterfacePub.UiBuilder.LoadImage(Path.Combine(plugin.PluginInterfacePub.AssemblyLocation.Directory?.FullName!, "UI/alignments/chaotic_good_bar.png"));
                lawfulneutralBar = plugin.PluginInterfacePub.UiBuilder.LoadImage(Path.Combine(plugin.PluginInterfacePub.AssemblyLocation.Directory?.FullName!, "UI/alignments/lawful_neutral_bar.png"));
                trueneutralBar = plugin.PluginInterfacePub.UiBuilder.LoadImage(Path.Combine(plugin.PluginInterfacePub.AssemblyLocation.Directory?.FullName!, "UI/alignments/true_neutral_bar.png"));
                chaoticneutralBar = plugin.PluginInterfacePub.UiBuilder.LoadImage(Path.Combine(plugin.PluginInterfacePub.AssemblyLocation.Directory?.FullName!, "UI/alignments/chaotic_neutral_bar.png"));
                lawfulevilBar = plugin.PluginInterfacePub.UiBuilder.LoadImage(Path.Combine(plugin.PluginInterfacePub.AssemblyLocation.Directory?.FullName!, "UI/alignments/lawful_evil_bar.png"));
                neutralevilBar = plugin.PluginInterfacePub.UiBuilder.LoadImage(Path.Combine(plugin.PluginInterfacePub.AssemblyLocation.Directory?.FullName!, "UI/alignments/neutral_evil_bar.png"));
                chaoticevilBar = plugin.PluginInterfacePub.UiBuilder.LoadImage(Path.Combine(plugin.PluginInterfacePub.AssemblyLocation.Directory?.FullName!, "UI/alignments/chaotic_evil_bar.png"));
            });
            IDalamudTextureWrap[] result = new IDalamudTextureWrap[9] {
                  //bars
                lawfulgoodBar,
                neutralgoodBar,
                chaoticgoodBar,
                lawfulneutralBar,
                trueneutralBar,
                chaoticneutralBar,
                lawfulevilBar,
                neutralevilBar,
                chaoticevilBar
            };
            return result;
        }
        public static string[] AlignmentTooltips()
        {
            string lawfulGoodTooltip = "LAWFUL GOOD:\n" +
                                        "    These characters always do the right thing as expected by society.\n" +
                                        "    They always follow the rules, tell the truth and help people out.\n" +
                                        "    They like order, trust and believe in people with social authority, \n" +
                                        "    and they aim to be an upstanding citizen.";
            string neutralGoodTooltip = "NEUTRAL GOOD:\n" +
                                        "    These characters do their best to help others, \n" +
                                        "    but they do it because they want to, not because they have \n" +
                                        "    been told to by a person in authority or by society’s laws.\n" +
                                        "    A Neutral Good person will break the rules if they are doing it \n" +
                                        "    for good reasons and they will feel confident \n" +
                                        "    and justified in their actions.  ";
            string chaoticGoodTooltip = "CHAOTIC GOOD:\n" +
                                        "    Chaotic Good characters do what their conscience tells \n" +
                                        "    them to for the greater good. They do not care about following society’s rules, \n" +
                                        "    they care about doing what’s right. \n" +
                                        "\n" +
                                        "    A Chaotic Good character will speak up for and help, those who are being needlessly \n" +
                                        "    held back because of arbitrary rules and laws. They do not like seeing people \n" +
                                        "    being told what to do for nonsensical reasons. ";
            string lawfulNeutralTooltip = "LAWFUL NEUTRAL:\n" +
                                        "    A Lawful Neutral character behaves in a way that matches \n" +
                                        "    the organization, authority or tradition they follow. \n" +
                                        "    They live by this code and uphold it above all else, taking actions \n" +
                                        "    that are sometimes considered Good and sometimes considered Evil by others.\n" +
                                        "    The Lawful Neutral character does not care about what others think of \n" +
                                        "    their actions, they only care about their actions being correct according \n" +
                                        "    to their code.But they do not preach their code to others and try to convert them. ";
            string trueNeutralTooltip = "TRUE NEUTRAL:\n" +
                                        "    True Neutral characters don’t like to take sides.\n" +
                                        "    They are pragmatic rather than emotional in their actions, \n" +
                                        "    choosing the response which makes the most sense for them in each situation. " +
                                        "\n" +
                                        "    Neutral characters don’t believe in upholding the rules and laws of society, but nor \n" +
                                        "    do they feel the need to rebel against them. There will be times when a Neutral character \n" +
                                        "    has to make a choice between siding with Good or Evil, perhaps casting the deciding vote \n" +
                                        "    in a party. They will make a choice in these situations, usually siding with whichever causes \n" +
                                        "    them the least hassle, or they stand to gain the most from. ";
            string chaoticNeutralTooltip = "CHAOTIC NEUTRAL:\n" +
                                        "    Chaotic Neutral characters are free spirits. \n" +
                                        "    They do what they want but don’t seek to disrupt the usual norms and laws of society. \n" +
                                        "    These individuals don’t like being told what to do, following traditions, \n" +
                                        "    or being controlled. That said, they will not work to change these restrictions,\n" +
                                        "    instead, they will just try to avoid them in the first place.\n" +
                                        "    Their need to be free is the most important thing. ";
            string lawfulEvilTooltip = "LAWFUL EVIL:\n" +
                                        "    Lawful Evil characters operate within a strict code of laws and traditions.\n" +
                                        "    Upholding these values and living by these is more important than anything, \n" +
                                        "    even the lives of others. They may not consider themselves to be Evil, \n" +
                                        "    they may believe what they are doing is right. \n" +

                                        "    These characters enforce their system of control through force.\n" +
                                        "    Anyone who doesn’t follow their code or acts out of line will face consequences. \n" +
                                        "    Lawful Evil characters feel no guilt or remorse for causing harm to others in this way.";
            string neutralEvilTooltip = "NEUTRAL EVIL:\n" +
                                        "    Neutral Evil characters are selfish. Their actions are driven by their own wants \n" +
                                        "    whether that’s power, greed, attention, or something else. \n" +
                                        "    They will follow laws if they happen to align with their ambitions, but they will not \n" +
                                        "    hesitate to break them if they don’t.They don’t believe that following laws \n" +
                                        "    and traditions makes anyone a better person. \n" +
                                        "    Instead, they use other people’s beliefs in codes and loyalty against them, using it \n" +
                                        "    as a tool to influence their behaviour. ";
            string chaoticEvilTooltip = "CHAOTIC EVIL:\n" +
                                        "    Chaotic Evil characters care only for themselves with a complete disregard \n" +
                                        "    for all law and order and for the welfare and freedom of others. \n" +
                                        "    They harm others out of anger or just for fun.\n" +
                                        "    Characters aligned with Chaotic Evil usually operate alone \n" +
                                        "    because they do not work well with others.";
            return new string[]
            {
                lawfulGoodTooltip,
                neutralGoodTooltip,
                chaoticGoodTooltip,
                lawfulNeutralTooltip,
                trueNeutralTooltip,
                chaoticNeutralTooltip,
                lawfulEvilTooltip,
                neutralEvilTooltip,
                chaoticEvilTooltip,
            };
        }
    }
}
