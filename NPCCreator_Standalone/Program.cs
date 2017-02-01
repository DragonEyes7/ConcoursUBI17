using System;
using System.Collections.Generic;
using System.IO;

namespace NPCCreator_Standalone
{
    internal class Program
    {

        public static void Main(string[] args)
        {
            var path = args[0];
            var npcCreator = new NPCCreator($"{path}npcCaracteristics.json");
            var npcs = npcCreator.Create(25);
            PrintNpcs(path, npcs);
        }

        private static void PrintNpcs(string path, IEnumerable<NPC> npcs)
        {
            var npcsString = "";
            foreach (var npc in npcs)
            {
                var hat = npc.Hat.Style == "aucun" ? "Chapeau : aucun\n" : $"Chapeau : {npc.Hat.Style}, {npc.Hat.Color} \n";
                var hair = npc.Hair.Style == "aucun" ? "Cheveux : aucun\n" : $"Cheveux : {npc.Hair.Style}, {npc.Hair.Color} \n";
                var facialHair = $"Pilosité faciale : {npc.FacialHair.Style} \n";
                var smoker = $"Fume : {npc.Smoker.Style} \n";
                var top = npc.Top.Style == "aucun" ? "Top: aucun\n" : $"Top : {npc.Top.Length}, {npc.Top.Style}, {npc.Top.Color}, {npc.Top.Pattern} \n";
                var pants = npc.Pants.Style == "aucun" ? "Culottes : aucun\n" : $"Culottes : {npc.Pants.Style}, {npc.Pants.Color} \n";
                npcsString += "---------------------------------------\n"+
                              $"{hat}"+
                              $"{hair}"+
                              $"{facialHair}"+
                              $"{smoker}"+
                              $"{top}"+
                              $"{pants}";
            }
            File.WriteAllText($"{path}generatedNPCS.txt", npcsString);

        }
    }
}