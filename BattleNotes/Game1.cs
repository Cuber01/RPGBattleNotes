using System.Collections.Generic;
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
            graphics.PreferredBackBufferWidth = 1000;
            graphics.ApplyChanges();

            guiRenderer = new ImGuiRenderer(this);
            guiRenderer.RebuildFontAtlas();

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
            }
            
            guiRenderer.AfterLayout();
            
            base.Draw(gameTime);
        }
    }
}