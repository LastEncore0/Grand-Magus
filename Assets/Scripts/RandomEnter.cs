using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class RandomEnter : MonoBehaviour
{
    public float heal;
    public float mana;
    private float chance;
    public GameObject explosionPrefab;
    public AudioClip sound;
    AudioSource aud;
    // Start is called before the first frame update
    void Start()
    {
        aud = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("other.name " + other.name);
        // 创建一个新的临时GameObject播放音效
        GameObject tempAudioObject = new GameObject("TempAudio");
        AudioSource tempAudioSource = tempAudioObject.AddComponent<AudioSource>();
        tempAudioSource.clip = sound;
        tempAudioSource.Play();

        this.aud.PlayOneShot(this.sound);

        explosion();

        //隨機
        chance = Random.Range(0.0f, 4.0f);
        HealthBar healthBar = FindObjectOfType<HealthBar>();
        ManaBar manaBar = FindObjectOfType<ManaBar>();
        Debug.Log("chance"+ chance);
        if (chance < 1.0f)
        {
            if (healthBar != null)
            {
                healthBar.SetHealth(heal);
            }
        }
        else if (chance >= 1.0f && chance < 2.0f)
        {
            if (manaBar != null)
            {
                manaBar.SetMana(mana);
            }
        }
        else if (chance >= 2.0f && chance < 3.0f)
        {
            if (healthBar != null)
            {
                healthBar.SetHealth(-heal);
            }
        }
        else if (chance >= 3.0f )
        {
            if (manaBar != null)
            {
                manaBar.SetMana(-mana);
            }
        }
    }

    void explosion()
    {
        
        Destroy(gameObject);

        // 爆炸特效
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= -5f)
        {
            explosion();
        }
        
    }
}
