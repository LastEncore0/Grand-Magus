using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public float fadeInDuration = 1.0f;
    public float fadeOutDuration = 1.0f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    // 公共方法用于触发渐现
    public void StartFadeIn()
    {
        StartCoroutine(Fade(0, 1, fadeInDuration));
    }

    // 公共方法用于触发渐隐
    public void StartFadeOut()
    {
        StartCoroutine(Fade(1, 0, fadeOutDuration));
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, time / duration);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
            time += Time.deltaTime;
            yield return null;
        }
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, endAlpha);
    }
}
