using System.IO;
using BattleNotes.HandlingData;
using ImGuiNET;

namespace BattleNotes.Apps
{

    public class ThemeSettings : GuiApp
    {
        private readonly string[] themeFiles = Directory.GetFiles(Consts.themesLocation);
        private readonly string[] fontFiles = Directory.GetFiles(Consts.fontsLocation);

        private readonly StyleManager styleManager;

        public ThemeSettings(StyleManager styleManager)
        {
            this.styleManager = styleManager;
        }

        private int currentTheme = 0;
        private int currentFont = 0;

        public override void imGuiUpdate()
        {
            if (!running) return;

            ImGui.Begin("Style Settings", ref running);

            if (ImGui.BeginCombo("Current Theme", Path.GetFileNameWithoutExtension(themeFiles[currentTheme])))
            {
                for (int n = 0; n < themeFiles.Length; n++)
                {
                    bool is_selected = (currentTheme == n);
                    if (ImGui.Selectable(Path.GetFileNameWithoutExtension(themeFiles[n]), is_selected))
                    {
                        styleManager.setTheme(Path.GetFileNameWithoutExtension(themeFiles[n]));
                        currentTheme = n;
                    }

                    // Set the initial focus when opening the combo (scrolling + keyboard navigation focus)
                    if (is_selected)
                    {
                        ImGui.SetItemDefaultFocus();
                    }
                }

                ImGui.EndCombo();
            }

            if (ImGui.BeginCombo("Current Font", Path.GetFileNameWithoutExtension(fontFiles[currentFont])))
            {
                for (int n = 0; n < fontFiles.Length; n++)
                {
                    bool is_selected = (currentFont == n);
                    if (ImGui.Selectable(Path.GetFileNameWithoutExtension(fontFiles[n]), is_selected))
                    {
                        ImFontPtr a = ImGui.GetIO().Fonts.AddFontFromFileTTF(fontFiles[n], Consts.defaultFontSize);
                        ImGui.PushFont(a);
                        currentFont = n;
                        ImGui.PopFont();
                    }

                    // Set the initial focus when opening the combo (scrolling + keyboard navigation focus)
                    if (is_selected)
                    {
                        ImGui.SetItemDefaultFocus();
                    }
                }

                ImGui.EndCombo();
            }
            
            ImGui.End();

        }
    }

}