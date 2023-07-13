namespace WaveTrial
{
    using UnityEngine;

    public class SoundManager : MonoBehaviour
    {
        public AudioSource Source;
        public AudioClip tick;
        public AudioClip specialTick;
        public AudioClip whoop;

        public float pitch1 = 0.2f;
        public float pitch2 = 0.7f;
        public float pitch3 = 1.2f;

        public static SoundManager S;

        public void Awake()
        {
            S = this;
        }

        private void setPitch(int i)
        {
            switch (i)
            {
                case 1:
                    Source.pitch = pitch1;
                    break;

                case 2:
                    Source.pitch = pitch2;
                    break;

                default:
                    Source.pitch = pitch3;
                    break;
            }
        }

        public void playTick(int pitch)
        {
            setPitch(pitch);
            Source.PlayOneShot(tick);
        }
        public void playSpecialTick(int pitch)
        {
            setPitch(pitch);
            Source.PlayOneShot(specialTick);
        }
        public void playSeperator(int pitch)
        {
            setPitch(pitch);
            Source.PlayOneShot(whoop);
        }
    }
}