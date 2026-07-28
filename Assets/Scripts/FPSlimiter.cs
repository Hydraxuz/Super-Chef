using UnityEngine;

public class FPSlimiter : MonoBehaviour
{
    [SerializeField] private int targetFPS = 60;

    void Awake()
    {
        Application.targetFrameRate = targetFPS;
        QualitySettings.vSyncCount = 0; // Disable VSync to let Application.targetFrameRate work
    }

    void OnValidate()
    {
        // Update frame rate in the editor when value changes
        Application.targetFrameRate = targetFPS;
    }
}
