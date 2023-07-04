using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField][Min(10)] private int _frameRateLock = 60;
    [SerializeField][Min(0)] private int _vSyncCount = 0;
     private TMP_Dropdown _dropdown;

    private void Start()
    {
        Application.targetFrameRate = _frameRateLock;
        QualitySettings.vSyncCount = _vSyncCount;

        // Application.targetFrameRate = Screen.currentResolution.refreshRate;
    }

    public void ChangeMaxFps()
    {
        switch (_dropdown.value)
        {
            case 0:
                _frameRateLock = 30;
                break;

            case 1:
                _frameRateLock = 60;
                break;

            case 2:
                _frameRateLock = 90;
                break;

            case 3:
                _frameRateLock = 120;
                break;

            default:
                _frameRateLock = 60;
                break;
        }

        Application.targetFrameRate = _frameRateLock;
    }
}