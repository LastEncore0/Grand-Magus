using ClearSky;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBarImage; // 生命值的Image
    public float maxHealth = 100f; // 最大生命值
    public float currentHealth; // 当前生命值

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SetHealth(float health)
    {
        currentHealth += health;
        healthBarImage.fillAmount = currentHealth / maxHealth;
    }
    // Update is called once per frame
    void Update()
    {
        healthBarImage.fillAmount = currentHealth / maxHealth;
        SimplePlayerController SimplePlayerController = FindObjectOfType<SimplePlayerController>();
        GameOver GameOver = FindObjectOfType<GameOver>();
        ManaBar manaBar = FindObjectOfType<ManaBar>();
        if (currentHealth <= 0)
        {
            if (SimplePlayerController != null)
            {
                SimplePlayerController.Die();
            }
            GameOver.StartFadeIn();
            manaBar.StopMana();
            StartCoroutine(PlayerDie());
        }
    }
    IEnumerator PlayerDie()
    {
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("EndScene");
    }
}
