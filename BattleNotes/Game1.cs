using System.Collections.Generic;
using BattleNotes.Apps;
using BattleNotes.HandlingData;
using BattleNotes.ImGuiTools;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BattleNotes
{
    public class Game1 : Game
    { 
        public static List<GuiApp> windows = new List<GuiApp>();
        
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private ImGuiRenderer guiRenderer;
        private StyleManager styleManager;
        private FontLoader fontLoader;

        private const string mainPopupID = "#main_popup";

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();

            IsMouseVisible = true;
            
            graphics.PreferredBackBufferHeight = 1000;
            graphics.PreferredBackBufferWidth = 1200;
            Window.AllowUserResizing = true;
            graphics.ApplyChanges();

            guiRenderer = new ImGuiRenderer(this);
            styleManager = new StyleManager(Consts.themesLocation, ImGui.GetStyle());
            fontLoader = new FontLoader(Consts.fontsLocation, ImGui.GetIO());
            
            guiRenderer.RebuildFontAtlas();

            // This makes for a theme I like
            styleManager.setTheme("steam");
            styleManager.setTheme("gold");
            
            // Enable keyboard navigation
            ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.NavEnableKeyboard;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }
        
        protected override void Update(GameTime gameTime)
        {
            for (int i = 0; i < windows.Count; i++)
            {
                if (!windows[i].running)
                {
                    windows.Remove(windows[i]);
                    continue;
                }
                
                windows[i].update();
            }

        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Gray);

            guiRenderer.BeforeLayout(gameTime);

            ImGui.BeginPopupContextVoid("Hello");
                updatePopupMenu();
                drawPopupMenu();
            ImGui.End();
            
            foreach (var window in windows)
            {
                window.imGuiUpdate();
                ImGui.ShowDemoWindow();
            }
            
            guiRenderer.AfterLayout();
            
            base.Draw(gameTime);
        }

        private void updatePopupMenu()
        {

            if (ImGui.BeginPopupContextVoid(mainPopupID))
            {
                if (ImGui.Selectable("Enemy Board")) windows.Add(new EnemyBoard());
                
                if (ImGui.Selectable("Dice Roller")) windows.Add(new DiceRoller());
                
                if (ImGui.Selectable("Theme Settings")) windows.Add(new ThemeSettings(styleManager));
            
                ImGui.EndPopup();
            }
            
        }

        private void drawPopupMenu()
        {
            if (ImGui.GetIO().MouseClicked[1])
            {
                ImGui.OpenPopup(mainPopupID);
            }
        }
    }
}