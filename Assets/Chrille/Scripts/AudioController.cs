using UnityEngine;
using System.Collections; // Needed for Coroutine

public class AudioController : MonoBehaviour
{
    [Header("Looping Background Audio")]
    public AudioSource backgroundMusicLoop;

    [Header("Sporadic Audio")]
    public AudioSource sporadicSound;
    public float minTimeBetweenPlays = 25f; // Minimum time in seconds
    public float maxTimeBetweenPlays = 35f; // Maximum time in seconds
    public float fadeInDuration = 1.0f;    // How long it takes for the sound to fade in (seconds)
    public float fadeOutDuration = 2.0f;   // How long it takes for the sound to fade out (seconds)
    public float maxSporadicVolume = 0.8f; // The maximum volume the sporadic sound will reach

    private float timeToNextPlay;
    private Coroutine currentSporadicSoundRoutine; // To keep track of the current sound playing coroutine

    void Start()
    {
        // Ensure both AudioSource references are set
        if (backgroundMusicLoop == null)
        {
            Debug.LogError("BackgroundMusicLoop AudioSource is not assigned in the Inspector! Please drag it in.");
        }
        if (sporadicSound == null)
        {
            Debug.LogError("SporadicSound AudioSource is not assigned in the Inspector! Please drag it in.");
        }
        else
        {
            // Set initial volume of sporadic sound to 0 to prepare for fade-in
            sporadicSound.volume = 0f;
        }


        // Start the coroutine to play the sporadic sound
        currentSporadicSoundRoutine = StartCoroutine(PlaySporadicSoundRoutine());
    }

    IEnumerator PlaySporadicSoundRoutine()
    {
        while (true) // Infinite loop to play repeatedly
        {
            // Calculate a random time within the specified interval
            timeToNextPlay = Random.Range(minTimeBetweenPlays, maxTimeBetweenPlays);
            Debug.Log($"Next sporadic sound will play in: {timeToNextPlay:F2} seconds.");

            // Wait for the calculated time
            yield return new WaitForSeconds(timeToNextPlay);

            // Play the sound if AudioSource is assigned
            if (sporadicSound != null)
            {
                // Stop any previous fade-in/out routine for this sound, if it exists
                if (currentSporadicSoundRoutine != null)
                {
                    StopCoroutine(currentSporadicSoundRoutine); // This might stop the parent routine if not careful,
                                                                 // so better to manage individual sound routines.
                                                                 // Let's refactor slightly to avoid this.
                }

                // Start the fade-in process
                StartCoroutine(FadeInAndOutSporadicSound());
            }
            else
            {
                Debug.LogError("SporadicSound AudioSource is null when trying to play!");
            }
        }
    }

    IEnumerator FadeInAndOutSporadicSound()
    {
        // --- Fade In ---
        sporadicSound.volume = 0f; // Ensure starting from 0
        sporadicSound.Play(); // Start playing the sound

        float timer = 0f;
        while (timer < fadeInDuration)
        {
            sporadicSound.volume = Mathf.Lerp(0f, maxSporadicVolume, timer / fadeInDuration);
            timer += Time.deltaTime;
            yield return null; // Wait for next frame
        }
        sporadicSound.volume = maxSporadicVolume; // Ensure it reaches max volume

        // --- Play for its duration (or a specific time if you prefer) ---
        // We'll let it play for its natural length if not fading out sooner.
        // If the sound clip is shorter than total fade in + fade out, it will finish playing naturally.
        // If it's longer, we'll fade it out after a certain point.
        yield return new WaitForSeconds(sporadicSound.clip.length - fadeInDuration - fadeOutDuration);


        // --- Fade Out ---
        timer = 0f;
        float startVolume = sporadicSound.volume; // Start fading from its current volume
        while (timer < fadeOutDuration)
        {
            sporadicSound.volume = Mathf.Lerp(startVolume, 0f, timer / fadeOutDuration);
            timer += Time.deltaTime;
            yield return null; // Wait for next frame
        }
        sporadicSound.volume = 0f; // Ensure it reaches 0
        sporadicSound.Stop(); // Stop playing the sound
    }
}