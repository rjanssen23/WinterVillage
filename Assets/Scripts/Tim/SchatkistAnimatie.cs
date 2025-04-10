using UnityEngine;
using UnityEngine.UI;

public class SchatkistAnimatieController : MonoBehaviour
{
    public Image[] animatieFrames;
    public float frameDuration = 0.1f; // Duur van elk frame in seconden
    private int currentFrame = 0;
    private float timer = 0f;
    private bool isAnimating = false;

    void Start()
    {
        // Zorg ervoor dat alle frames inactief zijn behalve de eerste
        for (int i = 0; i < animatieFrames.Length; i++)
        {
            animatieFrames[i].gameObject.SetActive(i == 0);
        }
    }

    void Update()
    {
        if (isAnimating)
        {
            // Update de timer
            timer += Time.deltaTime;

            // Controleer of het tijd is om naar het volgende frame te gaan
            if (timer >= frameDuration)
            {
                // Zet het huidige frame inactief
                animatieFrames[currentFrame].gameObject.SetActive(false);

                // Verhoog de frame index
                currentFrame = (currentFrame + 1) % animatieFrames.Length;

                // Zet het volgende frame actief
                animatieFrames[currentFrame].gameObject.SetActive(true);

                // Reset de timer
                timer = 0f;

                // Stop de animatie als we het laatste frame hebben bereikt
                if (currentFrame == animatieFrames.Length - 1)
                {
                    isAnimating = false;
                }
            }
        }
    }

    public void StartAnimatie()
    {
        isAnimating = true;
        currentFrame = 0;
        timer = 0f;

        // Zorg ervoor dat alle frames inactief zijn behalve de eerste
        for (int i = 0; i < animatieFrames.Length; i++)
        {
            animatieFrames[i].gameObject.SetActive(i == 0);
        }
    }
}
