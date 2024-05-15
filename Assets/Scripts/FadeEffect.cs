using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeEffect : MonoBehaviour
{
    public CanvasGroup uiElement;
    public float fadeInDuration = 1.0f;
    public float fadeOutDuration = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeCanvasGroup(uiElement, uiElement.alpha, 0, fadeOutDuration));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float lerpTime = 1.0f)
    {
        float _timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - _timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        while (true)
        {
            timeSinceStarted = Time.time - _timeStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentageComplete);

            cg.alpha = currentValue;

            if (percentageComplete >= 1) break;

            yield return new WaitForEndOfFrame();
        }
    }
}
