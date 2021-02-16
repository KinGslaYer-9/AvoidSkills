using UnityEngine;
using UnityEngine.UI; 

public class DisplayFPS : MonoBehaviour
{
    [SerializeField]
    private Text text;

    private float _frames = 0f;
    private float _timeElap = 0f;
    private float _frametime = 0f;

    private void Start()
    {
        Application.targetFrameRate = 60;
        
        if (Debug.isDebugBuild)
        {
            text.gameObject.SetActive(true);
        }
        else
        {
            text.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        _frames++;
        _timeElap += Time.unscaledDeltaTime;
        if (_timeElap > 1f)
        {
            _frametime = _timeElap / (float) _frames;
            _timeElap -= 1f;
            UpdateText();
            _frames = 0;
        }
    }

    private void UpdateText()
    {
        text.text = string.Format(
            "FPS : {0}, FrameTime : {1:F2} ms",
            _frames, _frametime * 1000.0f);
    }
}