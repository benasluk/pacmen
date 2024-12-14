namespace Server.Classes.Services.State
{
    public class StateHandler
    {
        // 0 - Not started
        // 1 - Playing
        // 2 - Paused
        // 3 - Finished
        private int state;
        public StateHandler(int state)
        {
            this.state = state;
        }

        public StateHandler()
        {
            state = 0;
        }

        public void SetState(int state)
        {
            if (state > 4 || state < 0) return;
            this.state = state;
        }

        public int GetState()
        {
            return state;
        }
    }
}
