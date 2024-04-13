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
        // 根据触发次数改变颜色
        if (hitCount <= 3)
        {
            float darkness = (4f - hitCount) / 4f; // 计算颜色的深度
            spriteRenderer.color = new Color(darkness, darkness, darkness, 1); // 逐渐变黑
        }
    }

    void DestroyBelowEnemies()
    {
        Vector2 center = new Vector2(transform.position.x, transform.position.y - 10f);
        Collider2D[] enemies = Physics2D.OverlapCircleAll(center, 4.0f, enemyLayer);
        Debug.Log("OverlapCircle returned " + enemies.Length + " colliders.");
        Instantiate(thunder, center, Quaternion.identity);
        if (lighting != null)
        {
            lighting.SetActive(true);
            StartCoroutine(DeactivateLightingAfterDelay(1f));
        }
        GameObject effect = Instantiate(thunder, center, Quaternion.identity);
        Debug.Log(effect != null ? "Effect created successfully" : "Effect creation failed");
        
        // 遍历所有检测到的碰撞体
        foreach (Collider2D enemyCollider in enemies)
        {
            Enemy Enemy = enemyCollider.GetComponent<Enemy>();

            if (Enemy != null)
            {
                Enemy.damaged(damage_value);
            }
        }
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
        Gizmos.DrawWireSphere(center, 6.0f);
    }
    // Update is called once per frame
    void Update()
    {
        Vector2 center = new Vector2(transform.position.x, transform.position.y - 10f);
        Collider2D[] enemies = Physics2D.OverlapCircleAll(center, 3.0f, enemyLayer);
        if (enemies.Length > 1 && hitCount == 3)
        {
            StartCoroutine(Lightingbot(1f));
        }
    }

    IEnumerator Lightingbot(float botdelay)
    {
        if (!aud.isPlaying) // 如果当前没有音频正在播放
        {
            aud.PlayOneShot(sound);
        }

        DestroyBelowEnemies();
        yield return new WaitForSeconds(botdelay); // 添加延迟
        spriteRenderer.color = Color.white; // 再次变白
        hitCount = 0; // 重置计数器
    }
}
