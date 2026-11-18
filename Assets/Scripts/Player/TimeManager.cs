using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [Range(0f, .99f)]
    public float GamePaused = 0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = 1 - GamePaused;
        Time.fixedDeltaTime = 0.02f * Mathf.Max(1 - GamePaused, 0.01f);  

    }
}
