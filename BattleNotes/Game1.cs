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

            windows.Add(new DiceRoller());
            windows.Add(new EnemyBoard());
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
            
            foreach (var window in windows)
            {
                window.imGuiUpdate();
                ImGui.ShowDemoWindow();
            }
            
            guiRenderer.AfterLayout();
            
            base.Draw(gameTime);
        }
    }
}