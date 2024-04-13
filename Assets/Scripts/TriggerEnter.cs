using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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

    void Start()
    {
        aud = GetComponent<AudioSource>();

    }

    public void ShootProjectile(GameObject projectilePrefab)
    {
        // 确定发射方向和速度
        Vector2 fireDirection = GetComponent<Rigidbody2D>().velocity.normalized;
        float speed = GetComponent<Rigidbody2D>().velocity.magnitude;

        // 实例化新的发射物
        projectilePrefab = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // 获取新发射物的Rigidbody2D组件并设置速度和方向
        Rigidbody2D newProjectileRb = projectilePrefab.GetComponent<Rigidbody2D>();
        if (projectilePrefab != null)
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



        // 使用other.gameObject.name获取碰撞对象的名字
        string collidedObjectName = other.gameObject.name;


        if (typenum == 0)
        {
            if (collidedObjectName == "CFXR4 Bouncing Glows Bubble(Clone)")
            {
                // 获取物体的中心点
                Vector3 center = GetComponent<Collider2D>().bounds.center;

                // 在物体的中心实例化特效
                Instantiate(steam, center, Quaternion.identity);

                // 向上发射射线
                
                Debug.DrawRay(center, Vector2.up * 10, Color.red, 2f); // 绘制红色射线，持续2秒，方便可视化
                // 检测射线是否碰撞到有Cloud脚本的对象
                RaycastHit2D[] hits = Physics2D.RaycastAll(center, Vector2.up);
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider != null && hit.collider.GetComponent<Cloud>() != null)
                    {
                        // 获取Cloud脚本并执行操作
                        Cloud cloud = hit.collider.GetComponent<Cloud>();
                        cloud.ChangeColor();
                        break; // 如果你找到了云，可能就不需要检查其他的碰撞了
                    }
                }
            }
            else if (collidedObjectName != "Wizard")
            {
                Enemy Enemy = other.gameObject.GetComponent<Enemy>();
                if (Enemy != null)
                {
                    Enemy.damaged(value);
                }
                else
                {
                    Destroy(other.gameObject);
                }
            }
        }
        else if (typenum == 1)
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
        else if (typenum == 2)
        {
            if (collidedObjectName == "CFXR Fire(Clone)")
            {
                ShootProjectile(greatfire);
            }
            else if (collidedObjectName == "CFXR4 Bouncing Glows Bubble(Clone)")
            {
                ShootProjectile(ice);
            }
            else
            {
                Vector3 pushDirection = (other.transform.position - transform.position).normalized; // 从投射物到目标的方向
                other.transform.position += pushDirection * value;
            }
        }
        else if (typenum == 4)
        {
            Vector2 center = new Vector2(transform.position.x, transform.position.y);
            Collider2D[] enemies = Physics2D.OverlapCircleAll(center, 5.0f, enemyLayer);
            Debug.Log("爆炸 " + center);
            Debug.Log("enemies " + enemies);
            // 遍历所有检测到的碰撞体
            foreach (Collider2D enemyCollider in enemies)
            {
                Debug.Log("Collided name: " + enemyCollider.name);
                // 尝试获取Enemy脚本
                Enemy Enemy = enemyCollider.GetComponent<Enemy>();

                if (Enemy != null)
                {
                    Enemy.damaged(value);
                }

            }
        }
        else if (typenum == 5)
        {
            Enemy Enemy = other.gameObject.GetComponent<Enemy>();
            if (Enemy != null)
            {
                Enemy.Forzen();
                Enemy.Forzen_area();
            }
        }
        
    }
}
