using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class GameRuler : MonoBehaviour
{
    public static class ScoreManager
    {
        public static int score = 0;
    }
    public GameObject scoretext;
    public GameObject Soilder;
    public Transform genpos;
    // Start is called before the first frame update
    void Start()
    {
        
        InvokeRepeating("GenSoilder", 1, 2);
        this.scoretext = GameObject.Find("scoretext");
    }

    void GenSoilder()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "MainScene")
        {
            Instantiate(Soilder, genpos.position, Quaternion.identity);
        }       
    }

    public void AddScore(int value)
    {
        ScoreManager.score += value;
        Debug.Log("Score: " + ScoreManager.score);
    }

    public void RestartGame()
    {
        ScoreManager.score = 0;
        SceneManager.LoadScene("MainScene");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    // Update is called once per frame
    void Update()
    {
        if (this.scoretext != null)
        {
            this.scoretext.GetComponent<TextMeshProUGUI>().text = "Score: " + ScoreManager.score.ToString();
        }
    }
}
