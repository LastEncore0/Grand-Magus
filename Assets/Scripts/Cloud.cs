using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Cloud : MonoBehaviour
{
    private int hitCount = 0; // 触发计数器
    private SpriteRenderer spriteRenderer;
    private float damage_value = 200.0f;
    public LayerMask enemyLayer;
    public GameObject thunder;
    public AudioClip sound;
    AudioSource aud;
    private GameObject lighting;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        aud = GetComponent<AudioSource>();
        lighting = transform.Find("lighting").gameObject;
        lighting.SetActive(false);
    }
    public void ChangeColor()
    {
        hitCount++;
        Debug.Log("ChangeColor");
        // ヒット回数に応じて色を変更
        if (hitCount <= 3)
        {
            float darkness = (4f - hitCount) / 4f; // 色の暗さを計算
            spriteRenderer.color = new Color(darkness, darkness, darkness, 1); // 徐々に黒くなる
        }
    }

    IEnumerator DestroyBelowEnemies(float damagedelay)
    {
        yield return new WaitForSeconds(damagedelay); // 遅延を追加
        Vector2 center = new Vector2(transform.position.x, transform.position.y - 10f);
        if (!aud.isPlaying) // 音声を再生
        {
            aud.PlayOneShot(sound);
        }
        // 一定範囲内の敵を検出
        Collider2D[] enemies = Physics2D.OverlapCircleAll(center, 3.0f, enemyLayer);
        Debug.Log("OverlapCircle returned " + enemies.Length + " colliders.");
        Instantiate(thunder, center, Quaternion.identity);
        if (lighting != null)
        {
            lighting.SetActive(true);
            StartCoroutine(DeactivateLightingAfterDelay(1f));
        }
        // 雷エフェクトを指定の位置に生成
        GameObject effect = Instantiate(thunder, center, Quaternion.identity);
        Debug.Log(effect != null ? "Effect created successfully" : "Effect creation failed");

        // 範囲内の敵を殺す
        foreach (Collider2D enemyCollider in enemies)
        {
            Enemy Enemy = enemyCollider.GetComponent<Enemy>();

            if (Enemy != null)
            {
                Enemy.damaged(damage_value);
            }
        }
        spriteRenderer.color = Color.white; // 曇をリセット
        hitCount = 0; // カウンターをリセット

    }



    IEnumerator DeactivateLightingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        lighting.SetActive(false);
    }
    void OnDrawGizmosSelected()
    {
        Vector2 center = new Vector2(transform.position.x, transform.position.y - 10f);
        Gizmos.color = Color.red; // 设置Gizmos的颜色
        Gizmos.DrawWireSphere(center, 3.0f);
    }
    // Update is called once per frame
    void Update()
    {
        Vector2 center = new Vector2(transform.position.x, transform.position.y - 10f);
        Collider2D[] enemies = Physics2D.OverlapCircleAll(center, 1f, enemyLayer);
        if (enemies.Length >= 1 && hitCount >= 3)
        {
            Debug.Log("Bot returned " + enemies.Length + " colliders.");
            StartCoroutine(DestroyBelowEnemies(2f));

        }
    }

    //IEnumerator Lightingbot(float botdelay)
    //{
        
    //    yield return new WaitForSeconds(botdelay); // 添加延迟
        
        
    //}
}
