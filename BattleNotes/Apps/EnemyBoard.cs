using System;
using System.Collections.Generic;
using System.IO;
using BattleNotes.HandlingData;
using ImGuiNET;

namespace BattleNotes.Apps
{

    public class EnemyBoard : GuiApp
    {
        private struct Enemy
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
        
        private const ImGuiTableFlags tableFlags = ImGuiTableFlags.BordersOuter | ImGuiTableFlags.Resizable | ImGuiTableFlags.Sortable;
        private const int colCount = 6;
        private int hpToRemove = 0;
        private int shieldToRemove = 0;
        
        public EnemyBoard()
        {
            enemies.Add(new Enemy("Baddie", 10, 10));
            enemies.Add(new Enemy("Baddie", 10, 10));
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
            ImGui.TableNextRow();

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
                    case 5: ImGui.Button("Kill"); break;
                }

            }
            
            ImGui.TableNextRow();

            for (int row = 0; row < enemies.Count; row++)
            {
                for (int col = 0; col < colCount; col++)
                {
                    ImGui.TableSetColumnIndex(col);

                    switch (col)
                    {
                        case 0: ImGui.Text(enemies[row].name);                  break;
                        case 1: ImGui.Text(enemies[row].hp.ToString());      break;
                        case 2: ImGui.Text(enemies[row].shield.ToString());  break;
                        case 3: ImGui.InputInt("", ref hpToRemove); break;
                        case 4: ImGui.InputInt("", ref shieldToRemove); break;
                        case 5: ImGui.Button(AwesomeIcons.Crosshairs); break;
                    }

                }
                
                ImGui.TableNextRow();
            }

        }
    }
}