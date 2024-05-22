using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{

    public Image ManaBarImage; // 魔法值的Image
    public float maxMana = 100f; // 最大魔法值
    public float currentMana; // 当前魔法值
    public float manarege;
    public bool gameruning = true;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetMana(float mana)
    {
        //Debug.Log("currentMana" + currentMana);
        currentMana -= mana;
        ManaBarImage.fillAmount = currentMana / maxMana;
    }

    public void StopMana()
    {
        gameruning = false;
    }

    // Update is called once per frame
    void Update()
    {
        ManaBarImage.fillAmount = currentMana / maxMana;
        if (currentMana < maxMana && gameruning)
        {
            currentMana += manarege;
        }
    }
}
