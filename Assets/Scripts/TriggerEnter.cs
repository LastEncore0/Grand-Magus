using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class TriggerEnter : MonoBehaviour
{
    public int typenum;
    public float value;
    public GameObject explosionPrefab;
    public AudioClip sound;
    AudioSource aud;
    public GameObject greatfire;
    public GameObject ice;
    public GameObject steam;
    public float damageRadius = 5.0f; // 伤害范围半径
    public LayerMask enemyLayer;

    private ObjectPool explosionPool;
    private ObjectPool greatfirePool;
    private ObjectPool icePool;
    private ObjectPool steamPool;

    void Start()
    {
        aud = GetComponent<AudioSource>();

        explosionPool = new ObjectPool(explosionPrefab);
        greatfirePool = new ObjectPool(greatfire);
        icePool = new ObjectPool(ice);
        steamPool = new ObjectPool(steam);
    }

    public void ShootProjectile(ObjectPool pool)
    {
        // 确定发射方向和速度
        Vector2 fireDirection = GetComponent<Rigidbody2D>().velocity.normalized;
        float speed = GetComponent<Rigidbody2D>().velocity.magnitude;

        /// 从对象池中获取新的发射物
        GameObject projectile = pool.GetObject();
        projectile.transform.position = transform.position;

        // 获取新发射物的Rigidbody2D组件并设置速度和方向
        Rigidbody2D newProjectileRb = projectile.GetComponent<Rigidbody2D>();
        if (newProjectileRb != null)
        {
            // 将老发射物的速度和方向赋予新的发射物
            newProjectileRb.velocity = fireDirection * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        // 创建一个新的临时GameObject播放音效
        GameObject tempAudioObject = new GameObject("TempAudio");
        AudioSource tempAudioSource = tempAudioObject.AddComponent<AudioSource>();
        tempAudioSource.clip = sound;
        tempAudioSource.Play();
        Destroy(gameObject);
        this.aud.PlayOneShot(this.sound);
        
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);



        // 衝突対象の名前を取得
        string collidedObjectName = other.gameObject.name;
        // タイプに応じた処理を呼び出し
        switch (typenum)
        {
            case 0:
                HandleType0(collidedObjectName, other);
                break;
            case 1:
                HandleType1(collidedObjectName);
                break;
            case 2:
                HandleType2(collidedObjectName, other);
                break;
            case 4:
                HandleType4();
                break;
            case 5:
                HandleType5(other);
                break;
        }
        
    }

    void HandleType0(string collidedObjectName, Collider2D other)
    {
        if (collidedObjectName == "CFXR4 Bouncing Glows Bubble(Clone)")
        {
            Vector3 center = GetComponent<Collider2D>().bounds.center;
            GameObject steamInstance = steamPool.GetObject();
            steamInstance.transform.position = center;
            RaycastHit2D[] hits = Physics2D.RaycastAll(center, Vector2.up);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null && hit.collider.GetComponent<Cloud>() != null)
                {
                    Cloud cloud = hit.collider.GetComponent<Cloud>();
                    cloud.ChangeColor();
                    break;
                }
            }
        }
        else if (collidedObjectName != "Wizard")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.damaged(value);
            }
            else
            {
                Destroy(other.gameObject);
            }
        }
    }

    void HandleType1(string collidedObjectName)
    {
        if (collidedObjectName != "CFXR Fire(Clone)")
        {
            HealthBar healthBar = FindObjectOfType<HealthBar>();
            if (healthBar != null)
            {
                healthBar.SetHealth(value);
            }
        }
    }

    void HandleType2(string collidedObjectName, Collider2D other)
    {
        switch (collidedObjectName)
        {
            case "CFXR Fire(Clone)":
                ShootProjectile(greatfirePool);
                break;

            case "CFXR4 Bouncing Glows Bubble(Clone)":
                ShootProjectile(icePool);
                break;

            default:
                Vector3 pushDirection = (other.transform.position - transform.position).normalized;
                other.transform.position += pushDirection * value;
                break;
        }
    }

    void HandleType4()
    {
        Vector2 center = new Vector2(transform.position.x, transform.position.y);
        Collider2D[] enemies = Physics2D.OverlapCircleAll(center, 5.0f, enemyLayer);
        foreach (Collider2D enemyCollider in enemies)
        {
            Enemy enemy = enemyCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.damaged(value);
            }
        }
    }

    void HandleType5(Collider2D other)
    {
        Enemy enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.Forzen();
            enemy.Forzen_area();
        }
    }
}
