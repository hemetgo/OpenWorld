using UnityEngine;
using TMPro;

public class FpsText : MonoBehaviour
{
    private TextMeshProUGUI fpsText;
    private float hudRefreshRate = .25f;

    private float timer;

    private void Start()
    {
        fpsText = GetComponent<TextMeshProUGUI>();

        fpsText.enabled = (PlayerPrefs.GetInt("ShowFPS", 1) == 1);


        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 240;
    }

    private void Update()
    {
        if (Time.unscaledTime > timer)
        {
            int fps = (int)(1f / Time.unscaledDeltaTime);
            fpsText.text = fps.ToString();
            timer = Time.unscaledTime + hudRefreshRate;
        }
    }
}
