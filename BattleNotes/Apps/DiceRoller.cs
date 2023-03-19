using System;
using System.Collections.Generic;
using System.Numerics;
using ImGuiNET;

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

            public int result;
            public readonly List<int> components = new List<int>();
        }

        private RollData currentRoll = new RollData(1, 10, 0);
        private readonly List<RollData> history = new List<RollData>();

        private readonly Random rnd = new Random();

        private readonly Vector4 highlightColor = new Vector4(1f, 209/255f, 0f, 1f);
        
        private readonly Vector2 itemSpacingInMainUI = new Vector2(4, 2);
        private readonly Vector2 dummySize = new Vector2(0, 10);

        private const int historyLength = 5;

        public override void imGuiUpdate()
        {
            if (!running) return;

            ImGui.Begin("Dice Roller", ref running);
            
            showMainInterface();
            showResult();
            showPresets();
            removeRedundantHistory(historyLength);
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
                    roll(currentRoll.diceNum, currentRoll.maxDiceRange, currentRoll.modifier, true);
                }
            
            ImGui.PopStyleVar();
            ImGui.PopStyleVar();
            ImGui.PopItemWidth();
        }

        private void showPresets()
        {
            ImGui.Dummy(dummySize);
            
            ImGui.Text("Presets:");
            if (ImGui.Button("d20")) rollPreset(1, 20, 0, true);
            ImGui.SameLine();
            if (ImGui.Button("d10")) rollPreset(1, 10, 0, true);
            ImGui.SameLine();
            if (ImGui.Button("5d6")) rollPreset(5, 6, 0, true);
            
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
                else if (r.modifier < 0)
                {
                    diceText += (" - " + Math.Abs(r.modifier));
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
            for (int i = history.Count-1; i >= 0; i--)
            {
                ImGui.PushID(i);

                RollData r = history[i];
                string text = r.diceNum + "d" + r.maxDiceRange;

                if (r.modifier > 0)
                {
                    text += "+" + r.modifier;
                }
                else if (r.modifier < 0)
                {
                    text += "-" + Math.Abs(r.modifier);
                }

                if (ImGui.Button(text))
                {
                    rollPreset(r.diceNum, r.maxDiceRange, r.modifier, false);
                }
                
                ImGui.PopID();
                ImGui.SameLine();
            }
        }

        private void removeRedundantHistory(int amountToSave)
        {
            if (history.Count > amountToSave)
            {
                history.RemoveAt(0);
            }
        }

        private void rollPreset(int diceNum, int maxDiceRange, int modifier, bool save)
        {
            currentRoll.diceNum = diceNum;
            currentRoll.maxDiceRange = maxDiceRange;
            currentRoll.modifier = modifier;
            
            roll(diceNum, maxDiceRange, modifier, save);
        }
        
        private void roll(int diceNum, int maxDiceRange, int modifier, bool save)
        {
            if(save) history.Add(currentRoll);
            currentRoll = new RollData(diceNum, maxDiceRange, modifier);
            
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