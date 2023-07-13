namespace Pathing
{
    using UnityEngine;

    public class StalkPathMaster : PathMaster
    {
        public StalkPathMaster()
        {
            // Why does this throw an error in Unity?
            //path = new StalkPath();
        }

        public void begin(Vector3 position)
        {
            path.reset(position);
            currentState = PathState.SETTING;
        }
        public void end()
        {
            currentState = PathState.SETTING;
        }
    }
}
