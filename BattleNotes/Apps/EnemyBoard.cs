using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Authentication.ExtendedProtection;
using BattleNotes.HandlingData;
using ImGuiNET;

namespace BattleNotes.Apps
{

    public class EnemyBoard : GuiApp
    {
        private class Enemy
        {
            public string name = "";
            public int hp = 0;
            public int shield = 0;
            public int maxShield = 0;
        }
        
        private List<Enemy> enemies = new List<Enemy>();
        
        private const ImGuiTableFlags tableFlags = ImGuiTableFlags.BordersV | ImGuiTableFlags.BordersH | ImGuiTableFlags.NoBordersInBody | 
                                                   ImGuiTableFlags.RowBg | ImGuiTableFlags.Reorderable;
        private const ImGuiInputTextFlags inputTextFlags = ImGuiInputTextFlags.EnterReturnsTrue;

        
        private const int colCount = 8;

        private string assembleID(int row, int col) => (enemies[row].name + row + '#' + col);

        public override void imGuiUpdate()
        {
            if (!running) return;

            ImGui.Begin("Enemy Board", ref running);

            if (ImGui.Button("Add character"))
            {
                enemies.Add(new Enemy());
            }
            
            if (ImGui.BeginTable("#main", colCount, tableFlags))
            {
                showEnemies();

                ImGui.EndTable();
            }

            ImGui.End();

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