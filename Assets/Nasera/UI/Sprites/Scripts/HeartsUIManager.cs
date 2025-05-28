using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartsUIManager : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public GameObject fullHeartPrefab;
    public GameObject halfHeartPrefab;
    public GameObject emptyHeartPrefab;
    public Transform heartsParent;
   

    private List<GameObject> hearts = new();

    void Start()
    {
        UpdateHearts();
    }

    public void UpdateHearts()
    {
        foreach (var heart in hearts)
            Destroy(heart);
        hearts.Clear();

        for (float i = 0; i < playerHealth.maxHealth; i++)
        {
            float heartValue = Mathf.Clamp(playerHealth.currentHealth - i, 0f, 1f);
            GameObject heartPrefab;

            if (heartValue >= 0.9f)
                heartPrefab = fullHeartPrefab;
            else if (heartValue >= 0.25f)
                heartPrefab = halfHeartPrefab;
            else
                heartPrefab = emptyHeartPrefab;

            GameObject newHeart = Instantiate(heartPrefab, heartsParent);
            hearts.Add(newHeart);

            // Blinkar det senaste påverkade hjärtat om det inte är fullt
            if (i == Mathf.FloorToInt(playerHealth.currentHealth) && heartPrefab != fullHeartPrefab)
            {
                StartCoroutine(BlinkHeart(newHeart));
            }
        }
    }


    IEnumerator BlinkHeart(GameObject heart)
    {
        Image img = heart.GetComponent<Image>();
        Color originalColor = img.color;

        for (int i = 0; i < 3; i++)
        {
            img.color = Color.clear;
            yield return new WaitForSeconds(0.1f);
            img.color = originalColor;
            yield return new WaitForSeconds(0.1f);
        }
    }



}

