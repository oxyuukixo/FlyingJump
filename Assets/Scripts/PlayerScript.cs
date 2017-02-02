using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerScript : MonoBehaviour
{
    public float Speed = 3.0f;

    public bool IsFly = true;

    public GameObject MainCamera;

    public float GravityScale;

    public float[] JumpPower;

    public LayerMask GroundLayer;

    public GameObject InputController;

    public GameObject Bullet;

    public float ShotInterval;


    [System.NonSerialized]
    public bool IsDied;


    private Animator Anim;

    private Rigidbody2D RigiBody;

    private bool[] IsJump;

    private bool IsJumping = false;

    private int JumpNum = 0;

    private float ShotElapsedTime;

    // Use this for initialization
    void Start()
    {
        //gameObject.layer = LayerMask.NameToLayer("Player");

        IsJump = new bool[JumpPower.Length];

        Anim = GetComponent<Animator>();

        RigiBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsFly)
        {
            //float AxisX = Input.GetAxis("Horizontal") / 1;
            //float AxisY = Input.GetAxis("Vertical") / 1;

            float AxisX = CrossPlatformInputManager.GetAxis("Horizontal") / 1;
            float AxisY = CrossPlatformInputManager.GetAxis("Vertical") / 1;

            if (AxisX * AxisX + AxisY * AxisY > 0)
            {
                float Dir;

                if (Mathf.Abs(AxisX) >= Mathf.Abs(AxisY))
                {
                    Dir = AxisY / AxisX;
                }
                else
                {
                    Dir = AxisX / AxisY;
                }

                float Rate = 1.0f / Mathf.Sqrt(1.0f + Dir * Dir);

                AxisX *= Rate * Speed;
                AxisY *= Rate * Speed;
            }

            RigiBody.velocity = new Vector2(AxisX + MainCamera.GetComponent<CameraMovement>().Speed, AxisY);
        }
        else
        {
            //if (Input.GetKeyDown(KeyCode.Space) && JumpNum < IsJump.Length)
            if (CrossPlatformInputManager.GetButtonDown("Jump") && JumpNum < IsJump.Length)
            {
                RigiBody.velocity = new Vector2(RigiBody.velocity.x, 0);

                RigiBody.AddForce(Vector2.up * JumpPower[JumpNum], ForceMode2D.Impulse);

                Anim.SetBool("IsJumping", true);

                IsJumping = true;

                JumpNum++;
            }

            Anim.SetFloat("JumpPower", RigiBody.velocity.y);

            float ChaseSpeed = 0;

            RigiBody.velocity = new Vector2(MainCamera.GetComponent<CameraMovement>().Speed + ChaseSpeed, RigiBody.velocity.y);
        }

        if (CrossPlatformInputManager.GetButton("Fire"))
        {
            if (ShotElapsedTime >= ShotInterval)
            {
                GameObject BulletObj = Instantiate(Bullet);

                BulletObj.transform.position = transform.position;

                BulletObj.GetComponent<Rigidbody2D>().velocity = new Vector2(5, 0);

                ShotElapsedTime = 0;
            }
            else
            {
                ShotElapsedTime += Time.deltaTime;
            }
        }
        else
        {
            ShotElapsedTime = ShotInterval;
        }

        //
        //移動方向によって向きを変える
        //if (AxisX < 0 && transform.localScale.x > 0 || AxisX > 0 && transform.localScale.x < 0)
        //{
        //    Vector2 pos = transform.localScale;
        //    pos.x *= -1;
        //    transform.localScale = pos;
        //}
    }

    void FixedUpdate()
    {

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            if (IsFly)
            {
                IsFly = false;

                IsJumping = true;

                Anim.SetTrigger("Run");

                Anim.SetBool("IsJumping", true);

                gameObject.layer = LayerMask.NameToLayer("DamagedPlayer");

                RigiBody.gravityScale = GravityScale;

                MainCamera.GetComponent<Rigidbody2D>().gravityScale = GravityScale;

                MainCamera.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Continuous;

                InputController.GetComponent<InputCtl>().RunButtonSet();
            }
            else
            {

            }
        }

        if (col.gameObject.tag == "Block")
        {
            if (IsJumping)
            {
                Vector2 Start = transform.position;

                Vector2 End = new Vector2(transform.position.x, transform.position.y - GetComponent<SpriteRenderer>().bounds.size.y / 2 - 0.1f);

                if (Physics2D.Linecast(Start, End, GroundLayer))
                {
                    for (int j = 0; j < IsJump.Length; j++)
                    {
                        IsJump[j] = false;
                    }

                    Anim.SetBool("IsJumping", false);

                    IsJumping = false;

                    JumpNum = 0;

                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Block")
        {
            Vector2 Start = transform.position;

            Vector2 End = new Vector2(transform.position.x, transform.position.y - GetComponent<SpriteRenderer>().bounds.size.y / 2 - 0.1f);

            if (!Physics2D.Linecast(Start, End, GroundLayer) && JumpNum == 0)
            {
                Anim.SetBool("IsJumping", true);

                IsJumping = true;

                JumpNum++;
            }
        }
    }
}
