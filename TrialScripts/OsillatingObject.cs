using UnityEngine;

public class OsillatingObject : MonoBehaviour
{
    public float amplitude = 1f;  // The maximum distance of oscillation
    public float speed = 2f;     // The speed of oscillation
    public bool oscillateX = true;
    public bool oscillateY = true;
    public bool oscillateZ = true;

    private void Update()
    {
        // Calculate the new position based on time and oscillation parameters
        float oscillationX = oscillateX ? Mathf.Sin(TimeKeeper.getOverworld().getTime() * speed) * amplitude : 0f;
        float oscillationY = oscillateY ? Mathf.Sin(TimeKeeper.getOverworld().getTime() * speed) * amplitude : 0f;
        float oscillationZ = oscillateZ ? Mathf.Sin(TimeKeeper.getOverworld().getTime() * speed) * amplitude : 0f;

        Vector3 newPosition = new Vector3(oscillationX, oscillationY, oscillationZ);

        // Update the position of the GameObject
        transform.localPosition = newPosition;
    }
}
// By ChatGPT from the following prompt: Give me another simple script that oscillates the game object left and right, please.