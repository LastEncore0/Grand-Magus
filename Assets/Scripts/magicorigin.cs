using ClearSky;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using Unity.VisualScripting;
using UnityEngine;

public class magicorigin : MonoBehaviour
{
    private Transform player;
    public GameObject projectilePrefab;
    public float speed;
    
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<SimplePlayerController>().transform;
        InvokeRepeating("GenMagic", 3, 3);
    }

    void GenMagic()
    {
        // 实例化发射物
        GameObject projectileInstance = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Debug.Log("player" + player);
        Vector2 direction = player.position - transform.position;
        direction.Normalize(); // 标准化方向向量

        // 获取发射物的Rigidbody2D组件并添加速度
        Rigidbody2D projectileRb = projectileInstance.GetComponent<Rigidbody2D>();
        if (projectileRb != null)
        {
            Debug.Log("projectileRb" + projectileRb);
            projectileRb.velocity = direction * speed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
