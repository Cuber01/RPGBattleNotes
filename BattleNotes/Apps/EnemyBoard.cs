using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Security.Authentication.ExtendedProtection;
using BattleNotes.GeneralTools;
using BattleNotes.HandlingData;
using BattleNotes.HandlingData.Json;
using ImGuiNET;
using Newtonsoft.Json;

namespace BattleNotes.Apps
{

    public class EnemyBoard : GuiApp
    {
        private class Enemy
        {
            public Enemy(string name, int hp, int maxShield)
            {
                this.name = name;
                this.hp = hp;
                this.shield = maxShield;
                this.maxShield = maxShield;
            }
            
            public string name;
            public int hp;
            public int shield;
            public int maxShield;
        }
        
        private List<Enemy> enemies = new List<Enemy>();
        private List<Encounter> encounters = new List<Encounter>();

        private const ImGuiTableFlags tableFlags = ImGuiTableFlags.BordersV | ImGuiTableFlags.BordersH | ImGuiTableFlags.NoBordersInBody | 
                                                   ImGuiTableFlags.RowBg | ImGuiTableFlags.Reorderable;

        private const ImGuiInputTextFlags inputTextFlags = ImGuiInputTextFlags.EnterReturnsTrue;

        private const int colCount = 8;
        private int currentEncounter = 0;
        
        private string scenarioName = "";
        private string lastSaveState = "";

        public EnemyBoard()
        {
            string[] encounterFiles = Directory.GetFiles(Consts.encountersLocation);

            foreach (var file in encounterFiles)
            {
                if(Path.GetExtension(file) != ".json") continue;
                
                encounters.Add(JsonConvert.DeserializeObject<Encounter>(FileReader.getFileString(file)));
            }
        }

        private string assembleID(int row, int col) => (enemies[row].name + row + '#' + col);

        public override void imGuiUpdate()
        {
            if (!running) return;

            ImGui.Begin("Enemy Board", ref running);

            showLoadBattle();
            
            if (ImGui.Button("Add character"))
            {
                enemies.Add(new Enemy("", 0, 0));
            }

            if (ImGui.BeginTable("#main", colCount, tableFlags))
            {
                showEnemies();

                ImGui.EndTable();
            }

            ImGui.Spacing();
            showSaveBattle();
            
            ImGui.End();

        }

        private void showSaveBattle()
        {
        
            ImGui.Dummy(new Vector2(0.0f, 80.0f));

            ImGui.InputText("", ref scenarioName, byte.MaxValue);
            
            ImGui.SameLine();
            
            if (ImGui.Button("Save as"))
            {
                lastSaveState = handleSave(scenarioName);    
            }
            
            ImGui.SameLine();
            
            ImGui.Text(lastSaveState);
        }

        private string handleSave(string name)
        {
            if (name == "") return "(E: No name)";
            if (name.Contains('/') || name.Contains('\\')) return "(E: Invalid name)";

            save(name);
            return "(Success!)";
        }

        private void save(string name)
        {
            Dictionary<string, Dictionary<string, int>> characters = new Dictionary<string, Dictionary<string, int>>();

            foreach (var enemy in enemies)
            {
                characters.Add(enemy.name, new Dictionary<string, int>
                {
                    { "hp", enemy.hp            },
                    { "shield", enemy.maxShield }
                });
            }

            Encounter encounter = new Encounter();
            encounter.name = name;
            encounter.characters = characters;
            
            encounters.Add(encounter);
            
            File.WriteAllText(
                String.Format(Consts.encountersLocation + "{0}" + name + ".json", Path.DirectorySeparatorChar),
                JsonConvert.SerializeObject(encounter));
        }

        private void showLoadBattle()
        {
            if (ImGui.BeginCombo("Load battle",  encounters.Count >= 1 ? encounters[currentEncounter].name : null))
            {
                for (int n = 0; n < encounters.Count; n++)
                {
                    bool is_selected = (currentEncounter == n);
                    if (ImGui.Selectable(encounters[n].name, is_selected))
                    {
                        enemies.Clear();
                        
                        foreach (var enemy in encounters[n].characters)
                        {
                            enemies.Add(new Enemy(enemy.Key, enemy.Value["hp"], enemy.Value["shield"]));
                            currentEncounter = n;
                        }
                    }

                    // Set the initial focus when opening the combo (scrolling + keyboard navigation focus)
                    if (is_selected)
                    {
                        ImGui.SetItemDefaultFocus();
                    }
                }

                ImGui.EndCombo();
            }
        }

        private void showEnemies()
        {
            // Headers
            ImGui.TableHeadersRow();
            for (int col = 0; col < colCount; col++)
            {
                ImGui.TableSetColumnIndex(col);
                ImGui.AlignTextToFramePadding();
                
                switch (col)
                {
                    case 0: ImGui.Text("Name");                  break;
                    case 1: ImGui.Text(AwesomeIcons.Heart);      break;
                    case 2: ImGui.Text(AwesomeIcons.Shield);  break;
                    case 3: ImGui.Text("Remove " + AwesomeIcons.Heart); break;
                    case 4: ImGui.Text("Remove " + AwesomeIcons.Shield); break;
                    case 5: ImGui.Text("Max " + AwesomeIcons.Shield); break;
                    case 6: ImGui.Text("Regen " + AwesomeIcons.Shield); break;   
                    case 7: ImGui.Text("Kill"); break;   
                }

            }
            
            // Content
            for (int row = 0; row < enemies.Count; row++)
            {
                ImGui.TableNextRow();
                
                for (int col = 0; col < colCount; col++)
                {
                    ImGui.TableSetColumnIndex(col);

                    switch (col)
                    {
                        // Name
                        case 0:
                        {
                            ImGui.PushID(assembleID(row, col));

                            ImGui.PushItemWidth(115);
                            
                            string inputText = enemies[row].name;
                            ImGui.InputText("", ref inputText, byte.MaxValue, inputTextFlags);
                            
                            if (ImGui.IsItemDeactivatedAfterEdit())
                            {
                                enemies[row].name = inputText;
                            }

                            ImGui.PopItemWidth();
                            ImGui.PopID();
                            
                            break;
                        }
                        // Set HP
                        case 1:
                        {
                            ImGui.PushID(assembleID(row, col));
                            
                            int hp = enemies[row].hp;
                            if(ImGui.InputInt("", ref hp, 0))
                            {
                                enemies[row].hp = hp;
                            }
                            
                            ImGui.PopID();
                            
                            break;
                        }
                        // Set Shield
                        case 2:
                        {
                            ImGui.PushID(assembleID(row, col));
                            
                            int shield = enemies[row].shield;
                            if(ImGui.InputInt("", ref shield, 0))
                            {
                                enemies[row].shield = shield;
                            }
                            
                            ImGui.PopID();
                            
                            break;
                        }
                        // Remove HP
                        case 3:
                        {
                            ImGui.PushID(assembleID(row, col));
                            
                            int hpToRemove = 0;
                            ImGui.InputInt("", ref hpToRemove, 0);

                            if (ImGui.IsItemDeactivatedAfterEdit())
                            {
                                enemies[row].hp -= hpToRemove;
                            }

                            ImGui.PopID();

                            break;
                        }
                        // Remove Shield
                        case 4:               
                        {
                            ImGui.PushID(assembleID(row, col));
                            
                            int shieldToRemove = 0;
                            ImGui.InputInt("", ref shieldToRemove, 0);
                            
                            if (ImGui.IsItemDeactivatedAfterEdit())
                            {
                                enemies[row].shield -= shieldToRemove;
                            }
                            
                            ImGui.PopID();
                            
                            break;
                        }
                        // Set Max Shield
                        case 5:
                        {
                            ImGui.PushID(assembleID(row, col));
                            
                            int maxShield = enemies[row].maxShield;
                            if(ImGui.InputInt("", ref maxShield, 0))
                            {
                                enemies[row].maxShield = maxShield;
                            }
                            
                            ImGui.PopID();
                            
                            break;
                        }
                        // Regen shield
                        case 6:
                        {
                            ImGui.PushID(assembleID(row, col));
                            
                            if(ImGui.Button(AwesomeIcons.Plus))
                            {
                                enemies[row].shield = enemies[row].maxShield;
                            }
                            
                            ImGui.PopID();
                            
                            break;
                        }
                        // Kill
                        case 7:
                        {
                            ImGui.PushID(assembleID(row, col));
                            
                            if (ImGui.Button(AwesomeIcons.Times))
                            {
                                enemies.RemoveAt(row);
                            }
                                
                            ImGui.PopID();
                            
                            break;
                        }
                    }

                }
            }

        }
    }
}