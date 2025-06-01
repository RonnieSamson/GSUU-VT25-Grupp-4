using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TypewriterEffect : MonoBehaviour
{
    public Text uiText;
    [TextArea(3, 10)] public string fullText;
    public float delay = 0.05f;

    public AudioSource audioSource;
    public AudioClip[] typeKeySounds; // för slumpmässigt val mellan t.ex. TypeKey01/02
    public AudioClip enterSound;

    private void Start()
    {
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        uiText.text = "";

        for (int i = 0; i < fullText.Length; i++)
        {
            char c = fullText[i];
            uiText.text += c;

            // Ljud för radbrytning
            if (c == '\n')
            {
                PlaySound(enterSound);
            }
            // Ljud för vanliga tecken
            else if (!char.IsWhiteSpace(c)) // undvik ljud för mellanslag
            {
                PlaySound(typeKeySounds[Random.Range(0, typeKeySounds.Length)]);
            }

            yield return new WaitForSeconds(delay);
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource && clip)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
