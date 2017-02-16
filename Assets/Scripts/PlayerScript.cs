using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerScript : MonoBehaviour
{
    public float m_Speed = 3.0f;

    public float m_GravityScale;

    public float[] m_JumpPower;

    public LayerMask m_GroundLayer;

    public GameObject m_InputController;

    public GameObject m_Bullet;

    public float m_ShotInterval;

    public GameObject m_DeathObject;

    [System.NonSerialized]
    public bool m_IsDied;

    private enum State
    {
        Fly,
        Run,
        FlyStart,
        RunStart
    }

    private State m_State;

    private Animator m_Anim;

    private Rigidbody2D m_RigiBody;

    private Vector2 m_NeutralPos;

    private bool[] m_IsJump;

    private bool m_IsJumping = false;

    private int m_JumpNum = 0;

    private float m_ShotElapsedTime;

    // Use this for initialization
    void Start()
    {
        m_IsJump = new bool[m_JumpPower.Length];

        m_Anim = GetComponent<Animator>();

        m_RigiBody = GetComponent<Rigidbody2D>();

        m_NeutralPos = transform.position;

        SoundManager.Instance.LoadSE("jump", "jump_1");
        SoundManager.Instance.LoadSE("enemyjump", "enemyjump");
        SoundManager.Instance.LoadSE("fire", "player_bullet");
    }

    // Update is called once per frame
    void Update()
    {
        switch(m_State)
        {
            case State.Fly:

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

                    AxisX *= Rate * m_Speed;
                    AxisY *= Rate * m_Speed;
                }

                m_RigiBody.velocity = new Vector2(AxisX, AxisY);

                break;

            case State.Run:

                if (CrossPlatformInputManager.GetButtonDown("Jump") && m_JumpNum < m_IsJump.Length)
                {
                    SoundManager.Instance.PlaySE("jump",0);

                    m_RigiBody.velocity = new Vector2(m_RigiBody.velocity.x, 0);

                    m_RigiBody.AddForce(Vector2.up * m_JumpPower[m_JumpNum], ForceMode2D.Impulse);

                    m_Anim.SetBool("IsJumping", true);

                    m_IsJumping = true;

                    m_JumpNum++;
                }

                m_Anim.SetFloat("JumpPower", m_RigiBody.velocity.y);

                if (transform.position.x < m_NeutralPos.x)
                {
                    transform.position += new Vector3(ScrollManager.Instance.m_ScrollSpeed * Time.deltaTime / 5, 0);

                    if (transform.position.x > m_NeutralPos.x)
                    {
                        transform.position = new Vector2(m_NeutralPos.x, transform.position.y);
                    }
                }
                if (transform.position.x > m_NeutralPos.x)
                {
                    transform.position -= new Vector3(ScrollManager.Instance.m_ScrollSpeed * Time.deltaTime, 0);

                    if (transform.position.x < m_NeutralPos.x)
                    {
                        transform.position = new Vector2(m_NeutralPos.x, transform.position.y);
                    }
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    FlyStart();
                }

                break;

            case State.FlyStart:

                if(transform.position.y >= m_NeutralPos.y)
                {
                    transform.position = new Vector2(transform.position.x, m_NeutralPos.y);
                    m_RigiBody.velocity = new Vector2(0, 0);
                    m_RigiBody.gravityScale = 0;

                    m_Anim.SetTrigger("Move");

                    if (!MainCameraManager.Instance.IsChange)
                    {
                        m_State = State.Fly;

                        gameObject.layer = LayerMask.NameToLayer("FlyPlayer");

                        m_InputController.GetComponent<InputCtl>().FlyButtonSet();
                    }
                }
                
                break;

            case State.RunStart:

                Vector2 Start = transform.position;

                Vector2 End = new Vector2(transform.position.x, transform.position.y - GetComponent<SpriteRenderer>().bounds.size.y / 2 - 0.1f);

                LayerMask layer = LayerMask.GetMask(new string[] { "Ground" });

                if (Physics2D.Linecast(Start, End,layer))
                {
                    m_State = State.Run;

                    gameObject.layer = LayerMask.NameToLayer("GroundPlayer");
                }

                break;
        }

        if(m_State == State.Fly || m_State == State.Run)
        {
            if (CrossPlatformInputManager.GetButton("Fire"))
            {
                if (m_ShotElapsedTime >= m_ShotInterval)
                {
                    SoundManager.Instance.PlaySE("fire", 1);

                    GameObject BulletObj = Instantiate(m_Bullet);

                    BulletObj.transform.position = transform.position;

                    BulletObj.GetComponent<Rigidbody2D>().velocity = new Vector2(5, 0);

                    m_ShotElapsedTime = 0;
                }
                else
                {
                    m_ShotElapsedTime += Time.deltaTime;
                }
            }
            else
            {
                m_ShotElapsedTime = m_ShotInterval;
            }
        }

        if(transform.position.x < MainCameraManager.Instance.GetCameraCornerPos(new Vector2(0,0)).x ||
           transform.position.y < MainCameraManager.Instance.GetCameraCornerPos(new Vector2(0,1)).y)
        {
            Death();
        }
    }

    void FlyStart()
    {
        m_State = State.FlyStart;

        m_Anim.SetTrigger("Fly");

        gameObject.layer = LayerMask.NameToLayer("ChangeStatePlayer");

        MainCameraManager.Instance.Change(-m_GravityScale,"UpCameraBlock");

        m_RigiBody.gravityScale = -m_GravityScale;
    }
    void RunStart()
    {
        m_State = State.RunStart;
        m_IsJumping = true;

        m_Anim.SetTrigger("Run");
        m_Anim.SetBool("IsJumping", true);

        gameObject.layer = LayerMask.NameToLayer("ChangeStatePlayer");

        m_RigiBody.velocity = new Vector2(0, 0);

        m_RigiBody.gravityScale = m_GravityScale;

        MainCameraManager.Instance.Change(m_GravityScale,"DownCameraBlock");

        m_InputController.GetComponent<InputCtl>().RunButtonSet();
    }

    void Death()
    {
        GameObject obj = Instantiate(m_DeathObject);

        obj.transform.position = transform.position;

        Destroy(gameObject);

        GameManager.Instance.GameOver();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Block")
        {
            if (m_IsJumping)
            {
                Vector2 Start = transform.position;

                Vector2 End = new Vector2(transform.position.x, transform.position.y - GetComponent<SpriteRenderer>().bounds.size.y / 2 - 0.1f);

                if (Physics2D.Linecast(Start, End, m_GroundLayer))
                {
                    for (int j = 0; j < m_IsJump.Length; j++)
                    {
                        m_IsJump[j] = false;
                    }

                    m_Anim.SetBool("IsJumping", false);

                    m_IsJumping = false;

                    m_JumpNum = 0;
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

            if (!Physics2D.Linecast(Start, End, m_GroundLayer) && m_JumpNum == 0)
            {
                m_Anim.SetBool("IsJumping", true);

                m_IsJumping = true;

                m_JumpNum++;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "FlyEnemy" || col.gameObject.tag == "EnemyBullet")
        {
            switch (m_State)
            {
                case State.Fly:

                    RunStart();

                    break;

                case State.Run:

                    Death();

                    break;
            }
        }
        if (col.gameObject.tag == "GroundEnemy")
        {
            switch (m_State)
            {
                case State.Fly:

                    RunStart();

                    break;

                case State.Run:

                    if (col.gameObject.transform.position.y + col.gameObject.GetComponent<SpriteRenderer>().bounds.size.y / 2 < transform.position.y)
                    {
                        SoundManager.Instance.PlaySE("enemyjump", 0);

                        m_RigiBody.velocity = new Vector2(0, 0);

                        m_RigiBody.AddForce(Vector2.up * m_JumpPower[0], ForceMode2D.Impulse);

                        m_Anim.SetBool("IsJumping", true);

                        m_IsJumping = true;

                        m_JumpNum = 1;

                        col.GetComponent<Enemy>().Death();
                    }
                    else
                    {
                        Death();
                    }

                    break;
            }
        }
    }
}
