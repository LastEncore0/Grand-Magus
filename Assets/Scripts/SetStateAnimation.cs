using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetStateAnimation : MonoBehaviour
{
  // Start is called before the first frame update
  public GameObject cow1;
  public Button btn1;
    public float healty;
    private float current_healty;


    public void setIdle()
  {
    
    cow1.GetComponent<Animator>().SetInteger("State", 0);

  }
  public void setWalk()
  {
   
    cow1.GetComponent<Animator>().SetInteger("State", 1);

  }

  public void setRun()
  {
   
    cow1.GetComponent<Animator>().SetInteger("State", 2);

  }

 public void  damaged(float damage)
    {
        current_healty -= damage;
        Debug.Log("current_healty" + current_healty);
    }


  void Start()
  {
        current_healty = healty;
  }

  // Update is called once per frame
  void Update()
  {
        if (current_healty <= 0) {
            Destroy(cow1);
        }
  }
}
