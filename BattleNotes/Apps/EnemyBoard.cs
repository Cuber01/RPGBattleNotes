using System.Collections.Generic;
using System.IO;
using BattleNotes.HandlingData;
using ImGuiNET;

namespace BattleNotes.Apps
{
    public struct Enemy
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
    
    public class EnemyBoard : GuiApp
    {
        private List<Enemy> enemies = new List<Enemy>();
        
        private const ImGuiTableFlags tableFlags = ImGuiTableFlags.BordersOuter | ImGuiTableFlags.Resizable | ImGuiTableFlags.Sortable;
        private const int colCount = 6;
        
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
            ImGui.PushID(0);

            // Text and Tree nodes are less high than framed widgets, using AlignTextToFramePadding()
            // we add vertical spacing to make the tree lines equally high.
            ImGui.TableNextRow();

            for (int row = 0; row < enemies.Count; row++)
            {
                for (int col = 0; col < colCount; col++)
                {
                    ImGui.TableSetColumnIndex(col);
                    ImGui.Text("Baddie");
                }
                
                ImGui.TableNextRow();
            }


            ImGui.PopID();
        }
    }
}