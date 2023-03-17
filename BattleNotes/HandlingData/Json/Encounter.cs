using System.Collections.Generic;

namespace BattleNotes.HandlingData.Json
{
    public class Encounter
    {
        public string name { get; set; } = "No name";
        public Dictionary<string, Dictionary<string, int>> characters { get; set; }
    }
}