using UnityEngine;
using TMPro;

public class RescueManager : MonoBehaviour
{
    private int rescuedHostages = 0;
    private int totalHostages = 0;

    public TextMeshProUGUI uiText;

    void Start()
    {
        totalHostages = GameObject.FindGameObjectsWithTag("Hostage").Length;
        UpdateUIText();
    }

    public void HostageRescued()
    {
        rescuedHostages++;
        UpdateUIText();

        if (rescuedHostages >= totalHostages)
        {
            Debug.Log("Alla hostages är räddade!");
            // Du kan visa en win-meny eller liknande här
        }
    }

    private void UpdateUIText()
    {
        uiText.text = $"Hostages Rescued: {rescuedHostages} / {totalHostages}";
    }
}
