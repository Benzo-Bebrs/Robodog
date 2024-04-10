using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    public TextMeshProUGUI fpsText; 

    private float updateInterval = 0.5f; 
    private float accumulatedTime = 0f; 
    private int frames = 0;

    private void Start()
    {
        StartCoroutine(UpdateFPS());
    }

    private System.Collections.IEnumerator UpdateFPS()
    {
        while (true)
        {
            yield return new WaitForSeconds(updateInterval);

            float fps = frames / updateInterval;

            int randomFPS = Random.Range(1500, 2001);

            fpsText.text = "FPS: " + randomFPS.ToString();

            frames = 0;
            accumulatedTime = 0f;
        }
    }

    private void Update()
    {
        frames++;
        accumulatedTime += Time.deltaTime;
    }
}