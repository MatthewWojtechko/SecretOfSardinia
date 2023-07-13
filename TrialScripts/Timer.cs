namespace WaveTrial
{
    using UnityEngine;

    public class Timer : MonoBehaviour
    {
        public float counter = 0;
        public int measures;
        public int beatsPerMeasure;
        public float beatSeconds;

        public int groups = 3;

        public int currentMeasure = 1000;
        public int currentBeat = 0;
        public int currentGroup = 1;

        public void Awake()
        {
            currentGroup = 1000;
        }

        public void BeginImmediately()
        {
            currentMeasure = 0;
            currentBeat = 0;
            counter = beatSeconds;
            currentGroup = 1;

            //SoundManager.S.setPitch(1);
        }
        //public void BeginAnother(int pitch)
        //{
        //    currentMeasure = 0;
        //    currentBeat = 0;
        //    counter = 0;

        //    SoundManager.S.setPitch(pitch);
        //}

        public void End()
        {
            currentGroup = 1000;
        }

        public void Update()
        {
            if (currentGroup > groups)
                return;
            if (hasElapsed())  // Time for the next grouping, reset
            {
                currentGroup++;
                Debug.Log("New group---");//
                currentMeasure = 0;
                currentBeat = 0;
                counter = 0;
                if (currentGroup > groups)
                    return;
            }

            counter += TimeKeeper.deltaPlayTime();

            if (counter < beatSeconds)   // Waiting for beat to pass
                return;

            counter = 0;

            if (currentBeat == 0)
            {
                Debug.Log("BEAT"); 
                SoundManager.S.playSpecialTick(currentGroup);
                currentBeat++;
            }
            else if (currentBeat < beatsPerMeasure - 1)
            {
                Debug.Log("Beat"); // Play tick
                SoundManager.S.playTick(currentGroup);
                currentBeat++;
            }
            else  // Last beat in measure
            {
                Debug.Log("End Beat");// End of measure (same tick)
                SoundManager.S.playTick(currentGroup);

                if (currentMeasure == measures - 1)
                {
                    SoundManager.S.playSeperator(currentGroup);
                    Debug.Log("END OF GROUPING");
                }

                currentMeasure++;
                currentBeat = 0;
            }
        }

        private bool hasElapsed()
        {
            return currentMeasure >= measures;
        }

        public bool isComplete()
        {
            return currentGroup > groups;
        }
    }
}
