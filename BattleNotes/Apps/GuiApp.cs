namespace BattleNotes
{
    public class GuiApp
    {
        protected GuiApp()
        {
        }
    
        public bool running = true;

        public virtual void update() {}
        public virtual void imGuiUpdate() {}
    }
}