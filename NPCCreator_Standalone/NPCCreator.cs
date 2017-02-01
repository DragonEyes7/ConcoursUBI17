using System;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class NPCCreator
{
    private JObject _npcCaracteristics;
    public NPCCreator(string jsonNPCCarateristicsFilePath)
    {
        if(!File.Exists(jsonNPCCarateristicsFilePath))return;
        _npcCaracteristics = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(jsonNPCCarateristicsFilePath));
    }

    public NPC[] Create(int count)
    {
        var npcs = new NPC[count];
        var rnd = new Random();
        for (var i = 0; i < count; ++i)
        {
            Hat hat = null;
            Top top = null;
            Pants pants = null;
            Hair hair = null;
            FacialHair facialHair = null;
            Smoker smoker = null;
            foreach (var npcCaracteristic in _npcCaracteristics)
            {
                var key = npcCaracteristic.Key;
                var styles = npcCaracteristic.Value["styles"] as JArray;
                var colors = npcCaracteristic.Value["colors"] as JArray;
                var patterns = npcCaracteristic.Value["patterns"] as JArray;
                var lengths = npcCaracteristic.Value["lengths"] as JArray;
                if (styles == null) break;
                var style = styles[rnd.Next(0, styles.Count)].ToString();
                if (key == "hats" && colors != null)
                {
                    var color = colors[rnd.Next(0, colors.Count)].ToString();
                    hat = new Hat(style, color);
                }
                else if (key == "tops" && patterns != null && lengths != null && colors != null)
                {
                    var pattern = patterns[rnd.Next(0, patterns.Count)].ToString();
                    var length = lengths[rnd.Next(0, lengths.Count)].ToString();
                    var color = colors[rnd.Next(0, colors.Count)].ToString();
                    top = new Top(style, color, pattern, length);
                }
                else if (key == "pants" && colors != null)
                {
                    var color = colors[rnd.Next(0, colors.Count)].ToString();
                    pants = new Pants(style, color);
                }
                else if (key == "hairs" && colors != null)
                {
                    var color = colors[rnd.Next(0, colors.Count)].ToString();
                    hair = new Hair(style, color);
                }
                else if (key == "facialHairs")
                {
                    facialHair = new FacialHair(style);
                }
                else if (key == "smoker")
                {
                    smoker = new Smoker(style);
                }
            }
            if (hat == null || top == null || pants == null || hair == null || facialHair == null || smoker == null) return null;
            npcs[i] = new NPC(hat, top, pants, hair, facialHair, smoker);
        }
        return npcs;
    }

}