using ClearSky;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float healty;
    public float forzen_time;
    private float current_healty;
    private float duration = 1.0f;
    public float movespeed;
    private Animator m_Anim;
    private bool isDying = false;
    private bool forzen = false;
    private bool forzen_area = false;
    public int scoreValue;
    private GameRuler GameRuler;
    private Transform player;
    public float attackRange; // 攻击范围
    private bool canAttack = true;
    public float damage;

    public AudioClip attack_sound;
    AudioSource aud;
    private SpriteRenderer[] spriteRenderers;

    private GameObject ice;
    public Image healthBarImage; // 生命值的Image

    private void Start()
    {
        m_Anim = GetComponent<Animator>();
        current_healty = healty;
        GameRuler = FindObjectOfType<GameRuler>();
        aud = GetComponent<AudioSource>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        ice = transform.Find("ice").gameObject;
        ice.SetActive(false);
        player = FindObjectOfType<SimplePlayerController>().transform;
    }

    private void Update()
    {

        if (current_healty <= 0 && !isDying)
        {
            isDying = true;
            m_Anim.Play("Die");
            GameRuler.AddScore(scoreValue);
            StartCoroutine(WaitForAnimation(duration));
            StartCoroutine(BlinkEffect(duration, 0.1f));
        }

        healthBarImage.fillAmount = current_healty / healty;

        if (transform.position.y < -4.16f)
        {
            transform.position = new Vector3(transform.position.x, -4.16f);
        }

        if (forzen_area)
        {
            Collider2D[] forzen_enemies = Physics2D.OverlapCircleAll(transform.position, 1.0f);
            foreach (Collider2D forzen_enemy in forzen_enemies)
            {
                Enemy Enemy = forzen_enemy.gameObject.GetComponent<Enemy>();
                if (Enemy != null)
                {
                    Enemy.Forzen();
                }
            }
        }

        if (!forzen && !isDying)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer > attackRange)
            {
                m_Anim.Play("Run");
                transform.position += Vector3.left * movespeed * Time.deltaTime;
            }
            else // 如果在攻击范围内，则发动攻击
            {
                if (canAttack)
                {
                    m_Anim.Play("Attack");
                    this.aud.PlayOneShot(this.attack_sound);
                    HealthBar healthBar = FindObjectOfType<HealthBar>();
                    if (healthBar != null)
                    {
                        healthBar.SetHealth(damage);
                    }

                    StartCoroutine(AttackCooldown());
                }
            }
        }
    }
    IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(0.8f);
        canAttack = true;
    }
    private IEnumerator WaitForAnimation(float duration)
    {
        yield return new WaitForSeconds(duration);

        // 動畫播放完畢後，銷毀物體
        Destroy(gameObject);
    }

    private IEnumerator BlinkEffect(float duration, float blinkTime)
    {
        float endTime = Time.time + duration;
        while (Time.time < endTime)
        {
            foreach (var renderer in spriteRenderers)
            {
                renderer.enabled = !renderer.enabled;
            }
            yield return new WaitForSeconds(blinkTime);
        }

        foreach (var renderer in spriteRenderers)
        {
            renderer.enabled = true; // 确保所有的 sprite 在闪烁结束后是可见的
        }
    }

    public void Forzen()
    {
        forzen = true;
        ice.SetActive(true);
        StartCoroutine(forzen_duration());
        m_Anim.enabled = false;
    }

    private IEnumerator forzen_duration()
    {
        yield return new WaitForSeconds(forzen_time);
        m_Anim.enabled = true;
        ice.SetActive(false);
        forzen = false;
    }

    public void Forzen_area()
    {
        forzen_area = true;
        StartCoroutine(forzen_area_duration());
    }

    private IEnumerator forzen_area_duration()
    {
        yield return new WaitForSeconds(forzen_time);
        forzen_area = false;
    }

    public void damaged(float damage)
    {
        current_healty -= damage;
        Debug.Log("current_healty" + current_healty);
    }



}
