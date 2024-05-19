using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using static UnityEngine.Rendering.DebugUI;

namespace ClearSky
{
    public class SimplePlayerController : MonoBehaviour
    {
        public float movePower = 10f;
        public float jumpPower = 15f; //Set Gravity Scale in Rigidbody2D Component to 5

        public float leftlimit = -14.5f;
        public float rightlimit = 14.5f;
        private Rigidbody2D rb;
        private Animator anim;
        private SpriteRenderer spriteRenderer;

        Vector3 movement;
        private int direction = 1;
        bool isJumping = false;
        private bool alive = true;

        public GameObject Fireball;
        public float FireballSpeed;
        public GameObject Waterball;
        public float WaterSpeed;
        public GameObject Wind;
        public float WindSpeed;
        public float upDistance;

        public float fallBackForce = 5f;
        public float fallDuration = 1f;

        public float maxhealty;
        public float healty;

        public float fire_mana;
        public float water_mana;
        public float wind_mana;
        private Vector2 startposition;


        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            startposition = transform.position;
        }

        private void Update()
        {
            Restart();
            if (alive)
            {
                //Hurt();
                //Die();
                Attack();
                Jump();
                Run();
                standonground();

            }
        }
        //private void OnTriggerEnter2D(Collider2D other)
        //{
        //    anim.SetBool("isJump", false);
        //}

        void standonground()
        {
            if (transform.position.y <= startposition.y && anim.GetBool("isJump"))
            {
                isJumping = false;
                anim.SetBool("isJump", false);
            }
        }


        void Run()
        {
            Vector3 moveVelocity = Vector3.zero;
            anim.SetBool("isRun", false);


            if (Input.GetAxisRaw("Horizontal") < 0 && transform.position.x > leftlimit)
            {
                direction = -1;
                moveVelocity = Vector3.left;

                transform.localScale = new Vector3(direction, 1, 1);
                if (!anim.GetBool("isJump"))
                    anim.SetBool("isRun", true);

            }
            if (Input.GetAxisRaw("Horizontal") > 0 && transform.position.x < rightlimit)
            {
                direction = 1;
                moveVelocity = Vector3.right;

                transform.localScale = new Vector3(direction, 1, 1);
                if (!anim.GetBool("isJump"))
                    anim.SetBool("isRun", true);

            }
            transform.position += moveVelocity * movePower * Time.deltaTime;
        }
        void Jump()
        {
            if ((Input.GetKeyDown(KeyCode.Space) ) && !anim.GetBool("isJump"))
            {
                isJumping = true;
                anim.SetBool("isJump", true);
            }
            if (!isJumping)
            {
                return;
            }

            rb.velocity = Vector2.zero;

            Vector2 jumpVelocity = new Vector2(0, jumpPower);
            rb.AddForce(jumpVelocity, ForceMode2D.Impulse);

            isJumping = false;
        }
        void Attack()
        {
            ManaBar ManaBar = FindObjectOfType<ManaBar>();
            if (Input.GetKeyDown(KeyCode.Q) && ManaBar.currentMana > fire_mana)
            {
                ShootProjectile(Fireball, FireballSpeed);
                if (ManaBar != null)
                {
                    ManaBar.SetMana(fire_mana);
                }
            }
            if (Input.GetKeyDown(KeyCode.W) && ManaBar.currentMana > water_mana)
            {
                ShootProjectile(Waterball, WaterSpeed);
                if (ManaBar != null)
                {
                    ManaBar.SetMana(water_mana);
                }
            }
            if (Input.GetKeyDown(KeyCode.E) && ManaBar.currentMana > wind_mana)
            {
                ShootProjectile(Wind, WindSpeed);
                if (ManaBar != null)
                {
                    ManaBar.SetMana(wind_mana);
                }
            }
        }
        void Hurt()
        {
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                anim.SetTrigger("hurt");
                if (direction == 1)
                    rb.AddForce(new Vector2(-5f, 1f), ForceMode2D.Impulse);
                else
                    rb.AddForce(new Vector2(5f, 1f), ForceMode2D.Impulse);
            }
        }
        public void Die()
        {
            anim.SetTrigger("hurt");
            alive = false;
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            StartCoroutine(FallBackWhileHurt());
        }

        private IEnumerator FallBackWhileHurt()
        {
            float elapsedTime = 0f;
            float startAngle = transform.eulerAngles.z;
            float endAngle = transform.localScale.x > 0 ? 90f : -90f; // 根据角色朝向决定最终角度

            while (elapsedTime < fallDuration)
            {
                elapsedTime += Time.deltaTime;
                float zRotation = Mathf.LerpAngle(startAngle, endAngle, elapsedTime / fallDuration);
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, zRotation);
                yield return null;
            }

            // 确保最终角度准确
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, endAngle);
        }

        void Restart()
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                anim.SetTrigger("idle");
                alive = true;
            }
        }

        public void ShootProjectile(GameObject projectilePrefab, float speed)
        {
            anim.SetTrigger("attack");
            // 确定发射方向
            Vector3 fireDirection = transform.localScale.x < 0 ? -transform.right : transform.right;

            // 根据角色朝向确定旋转角度
            Quaternion rotation = transform.localScale.x < 0 ? Quaternion.Euler(0, 0, -90) : Quaternion.Euler(0, 0, 90);

            // 实例化发射物
            GameObject projectileInstance = Instantiate(projectilePrefab, transform.position + fireDirection * 2.5f + Vector3.up * upDistance, rotation);

            // 获取发射物的Rigidbody2D组件并添加速度
            Rigidbody2D projectileRb = projectileInstance.GetComponent<Rigidbody2D>();
            if (projectileRb != null)
            {
                projectileRb.velocity = fireDirection * speed;
            }
        }
    }
}