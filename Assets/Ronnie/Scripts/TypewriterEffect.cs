using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // <-- NYTT!

public class TypewriterEffect : MonoBehaviour
{
    public Text uiText;
    [TextArea(3, 10)] public string fullText;
    public float delay = 0.05f;

    public AudioSource audioSource;
    public AudioClip[] typeKeySounds;
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

            if (c == '\n')
            {
                PlaySound(enterSound);
            }
            else if (!char.IsWhiteSpace(c))
            {
                PlaySound(typeKeySounds[Random.Range(0, typeKeySounds.Length)]);
            }

            yield return new WaitForSeconds(delay);
        }

        // Vänta 2 sek efter att texten är klar och byt scen
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("DockThing"); // <- byt till exakt namn på din scen
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource && clip)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
