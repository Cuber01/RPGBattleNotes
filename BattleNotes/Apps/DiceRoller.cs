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
            public RollData(int diceNum, int maxDiceRange, int modifier)
            {
                this.diceNum = diceNum;
                this.maxDiceRange = maxDiceRange;
                this.modifier = modifier;
            }
            
            public int diceNum;
            public int maxDiceRange;
            public int modifier;

            public int result = 0;
            public List<int> components = new List<int>();
        }

        private readonly RollData currentRoll = new RollData(1, 10, 0);
        private readonly Random rnd = new Random();

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
            
                ImGui.PushID(0);
                    ImGui.InputInt("", ref currentRoll.diceNum, 1);
                    ImGui.SameLine();
                ImGui.PopID();
                
                ImGui.PushID(1);
                    ImGui.Text("d");
                    ImGui.SameLine();
                    ImGui.InputInt("", ref currentRoll.maxDiceRange, 1);
                ImGui.PopID();
                
                ImGui.PushID(2);
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
            if (ImGui.Button("d20")) roll(1, 20, 0);
            ImGui.SameLine();
            if (ImGui.Button("d10")) roll(1, 10, 0);
            ImGui.SameLine();
            if (ImGui.Button("5d6")) roll(5, 6, 0);
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
                string resultText = String.Concat(r.diceNum, "d", r.maxDiceRange, ": ", r.result, " (");

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
                
                ImGui.Text(resultText);
            }
        }
        
        private void showRecentRolls()
        {
            ImGui.Dummy(dummySize);
            
            ImGui.Text("Recent rolls:");
        }

        private void roll(int diceNum, int maxDiceRange, int modifier)
        {
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