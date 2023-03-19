using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using BattleNotes.HandlingData;
using BattleNotes.HandlingData.Json;
using ImGuiNET;
using Newtonsoft.Json;

namespace BattleNotes.Apps
{
    public class DiceRoller : GuiApp
    {
        private class RollData
        {
            public RollData(int diceNum = 0, int maxDiceRange = 0, int modifier = 0)
            {
                this.diceNum = diceNum;
                this.maxDiceRange = maxDiceRange;
                this.modifier = modifier;
            }
            
            public int diceNum;
            public int maxDiceRange;
            public int modifier;

            public int result = 0;
            public readonly List<int> components = new List<int>();
        }

        private readonly RollData currentRoll = new RollData(1, 10, 0);
        private readonly List<RollData> history = new List<RollData>();

        private readonly Random rnd = new Random();

        private readonly Vector4 highlightColor = new Vector4(1f, 209/255f, 0f, 1f);
        
        private readonly Vector2 itemSpacingInMainUI = new Vector2(4, 2);
        private readonly Vector2 dummySize = new Vector2(0, 10);

        public override void imGuiUpdate()
        {
            if (!running) return;

            ImGui.Begin("Dice Roller", ref running);
            
            showMainInterface();
            showResult();
            showPresets();
            showRecentRolls();
        
            ImGui.End();
        }

        private void showMainInterface()
        {
            ImGui.PushItemWidth(100); 
            ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, itemSpacingInMainUI);
            
                ImGui.PushID("#a");
                    ImGui.InputInt("", ref currentRoll.diceNum, 1);
                    ImGui.SameLine();
                ImGui.PopID();
                
                ImGui.PushID("#b");
                    ImGui.Text("d");
                    ImGui.SameLine();
                    ImGui.InputInt("", ref currentRoll.maxDiceRange, 1);
                ImGui.PopID();
                
                ImGui.PushID("#c");
                    ImGui.SameLine();
                    ImGui.Text("+");
                    ImGui.SameLine();
                    ImGui.InputInt("", ref currentRoll.modifier, 1);
                ImGui.PopID();
                
                ImGui.SameLine();
                ImGui.PushStyleVar(ImGuiStyleVar.FrameRounding, 0);
                if (ImGui.Button("[Roll]"))
                {
                    roll(currentRoll.diceNum, currentRoll.maxDiceRange, currentRoll.modifier);
                }
            
            ImGui.PopStyleVar();
            ImGui.PopStyleVar();
            ImGui.PopItemWidth();
        }

        private void showPresets()
        {
            ImGui.Dummy(dummySize);
            
            ImGui.Text("Presets:");
            if (ImGui.Button("d20")) rollPreset(1, 20, 0);
            ImGui.SameLine();
            if (ImGui.Button("d10")) rollPreset(1, 10, 0);
            ImGui.SameLine();
            if (ImGui.Button("5d6")) rollPreset(5, 6, 0);
            
        }

        private void showResult()
        {
            ImGui.Dummy(dummySize);
            
            ImGui.Text("Result:");

            RollData r = currentRoll;

            if (r.result == 0)
            {
                ImGui.Text("None: 0");    
            }
            else
            {
                // How many dice were used + modifier
                string diceText = String.Concat(r.diceNum, "d", r.maxDiceRange);

                if (r.modifier > 0)
                {
                    diceText += (" + " + r.modifier);
                }

                diceText += ":";

                // Result of roll
                string resultText = r.result + " (";

                for (int i = 0; i < r.components.Count; i++)
                {
                    resultText += r.components[i];

                    if (i == r.components.Count - 1)
                    {
                        resultText += ")";
                        continue;
                    }

                    resultText += " + ";
                }
                
                ImGui.Text(diceText);
                ImGui.SameLine();
                ImGui.TextColored(highlightColor, resultText);
            }
        }
        
        private void showRecentRolls()
        {
            ImGui.Dummy(dummySize);
            
            ImGui.Text("Recent rolls:");
            for (int i = 0; i < history.Count; i++)
            {
                ImGui.PushID(i);
                
                ImGui.PopID();
                ImGui.SameLine();
            }
        }

        private void rollPreset(int diceNum, int maxDiceRange, int modifier)
        {
            currentRoll.diceNum = diceNum;
            currentRoll.maxDiceRange = maxDiceRange;
            currentRoll.modifier = modifier;
            
            roll(diceNum, maxDiceRange, modifier);
        }
        
        private void roll(int diceNum, int maxDiceRange, int modifier)
        {
            history.Add(currentRoll);
            currentRoll.components.Clear();
            currentRoll.result = 0;
            
            for (int i = 0; i < diceNum; i++)
            {
                int cmp = rnd.Next(1, maxDiceRange+1);
                currentRoll.components.Add(cmp);
                currentRoll.result += cmp;
            }

            currentRoll.result += modifier;
        }

    
    }
}