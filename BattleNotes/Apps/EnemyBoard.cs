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
            public Enemy(string name, int hp, int shield)
            {
                this.name = name;
                this.hp = hp;
                this.shield = shield;
            }

            public string name;
            public int hp;
            public int shield;
        }
        
        private List<Enemy> enemies = new List<Enemy>();
        
        private const ImGuiTableFlags tableFlags = ImGuiTableFlags.BordersV | ImGuiTableFlags.BordersH | ImGuiTableFlags.NoBordersInBody | 
                                                   ImGuiTableFlags.RowBg | ImGuiTableFlags.Reorderable;
        private const ImGuiInputTextFlags inputTextFlags = ImGuiInputTextFlags.EnterReturnsTrue;

        
        private const int colCount = 6;

        private string assembleID(int row, int col) => (enemies[row].name + row + '#' + col);
        
        public EnemyBoard()
        {
            enemies.Add(new Enemy("Baddie", 10, 10));
            enemies.Add(new Enemy("Normie", 10, 10));
        }

        public override void imGuiUpdate()
        {
            if (!running) return;

            ImGui.Begin("Enemy Board", ref running);

            if (ImGui.BeginTable("#main", 6, tableFlags))
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

                switch (col)
                {
                    case 0: ImGui.Text("Name");                  break;
                    case 1: ImGui.Text(AwesomeIcons.Heart);      break;
                    case 2: ImGui.Text(AwesomeIcons.Shield);  break;
                    case 3: ImGui.Text("Subtract " + AwesomeIcons.Heart); break;
                    case 4: ImGui.Text("Subtract " + AwesomeIcons.Shield); break;
                    case 5: ImGui.Text("Kill"); break;
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
                            
                            string inputText = enemies[row].name;
                            ImGui.InputText("", ref inputText, byte.MaxValue, inputTextFlags);
                            
                            if (ImGui.IsItemDeactivatedAfterEdit())
                            {
                                enemies[row].name = inputText;
                            }

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
                        // Subtract HP
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
                        // Subtract Shield
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
                        // Kill
                        case 5:
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