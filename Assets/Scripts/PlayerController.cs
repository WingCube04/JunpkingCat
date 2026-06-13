using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigid2D;
    Animator animator;
    private AudioSource audioSource;
    public AudioClip jumpSound;
    public AudioClip landSound;
    public AudioClip walkSound;
    public float footstepDelay = 0.4f; // 발소리 사이의 시간 간격 (초 단위)
    private float nextFootstepTime = 0f; // 다음 발소리가 날 수 있는 최소 시간 시간



    float walkForce = 4.0f;
    int jumpGauge = 0;
    int maxJumpGauge = 45;
    bool isJumping;

    GameObject director;
    
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60;
        this.rigid2D = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        director = GameObject.Find("GameDirector");

    }

    // Update is called once per frame
    void Update()
    {
        

        if (Input.GetKeyDown(KeyCode.Space) && Mathf.Abs(rigid2D.linearVelocity.y) < 0.01f)
        {
            //스페이스바 누르면 그 자리에 멈춤춤
            this.rigid2D.linearVelocity = new Vector2(0, 0);
            this.animator.SetBool("isReady", true);
        }
        if (Input.GetKey(KeyCode.Space) && Mathf.Abs(rigid2D.linearVelocity.y) < 0.01f && !isJumping)
        {
            //착지 전부터 스페이스바 누르면 밀리는 현상 있어서 그 자리 고정시킴킴
            this.rigid2D.linearVelocity = new Vector2(0, 0);
            jumpGauge++;
            this.animator.SetBool("isReady", true);
            if (jumpGauge >= maxJumpGauge)  //일정 게이지 이상으로 가면 자동으로 점프뛰도록 함.(점프킹처럼)
            {
                isJumping = true;
                jumpGauge = maxJumpGauge;
                JumpUp(400);
            }
            
        }
        if (Input.GetKeyUp(KeyCode.Space) && Mathf.Abs(rigid2D.linearVelocity.y) < 0.01f)
        {
            JumpUp(150);
        }
        int key = 0;
        if (Input.GetKey(KeyCode.LeftArrow)) key = -1;
        if (Input.GetKey(KeyCode.RightArrow)) key = 1;
        float speedx = Mathf.Abs(this.rigid2D.linearVelocity.x);

        //걷기
        if (Mathf.Abs(rigid2D.linearVelocity.y) < 0.01f && !Input.GetKey(KeyCode.Space) && !isJumping)
        {
            Walk(key);
        }

        if (key != 0)
        {
            transform.localScale = new Vector3(key*1f, 1f, 1.0f);
        }

        //추락 속도 너무 빠르면 땅을 뚫어버려서 제한 걸어둠.
        if (this.rigid2D.linearVelocity.y < -20)
        {
            this.rigid2D.linearVelocity = new Vector2(this.rigid2D.linearVelocity.x, -20);
        }

        this.animator.SetBool("isJumping", this.isJumping); //애니메이션 isJumping 파라미터에 isJumping Bool값 계속 넣어주기

        // 추락 애니메이션 판정
        if (this.rigid2D.linearVelocity.y < 0f)
        {
            this.animator.SetBool("isFalling", true);
        }
        else
        {
            this.animator.SetBool("isFalling", false);
        }
    }


 
    void Walk(int key)
    {
        this.rigid2D.linearVelocity = new Vector2(this.walkForce * key, 0);
            if(this.rigid2D.linearVelocity.x==0)
            {
                this.animator.SetBool("isWalking", false);
            }
            else{
                this.animator.SetBool("isWalking", true);
                if(Time.time>nextFootstepTime)
                {
                   audioSource.PlayOneShot(walkSound, 0.7f);
                   nextFootstepTime = Time.time + footstepDelay;
                }

            }
    }

    void JumpUp(int jumpConstant)
    {
        //y축 힘
        this.rigid2D.AddForce(transform.up * jumpGauge * 23f);
        //x축 힘힘
        float direction = 0f;
        if(Input.GetKey(KeyCode.RightArrow)) direction = 1f;
        else if(Input.GetKey(KeyCode.LeftArrow)) direction = -1f;
        this.rigid2D.AddForce(transform.right * direction * jumpConstant);

        audioSource.PlayOneShot(jumpSound);
        this.animator.SetBool("isReady", false);
    
        jumpGauge = 0;
    }


    void OnTriggerEnter2D(Collider2D other)
        {
            director.GetComponent<GameDirector>().GameClear();
        }

    void OnCollisionEnter2D(Collision2D other)
    {
        Vector2 normal = other.contacts[0].normal;
        audioSource.PlayOneShot(landSound);

        if(Mathf.Abs(normal.x)>0.5f)
        {
            this.rigid2D.linearVelocity = new Vector2(0, this.rigid2D.linearVelocity.y);
            this.rigid2D.AddForce(new Vector2(normal.x * 120f, 0f));
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        Vector2 normal = other.contacts[0].normal;
        if(normal.y>0.5f)
        {
            isJumping = false;
        }
        else
        {
            // 만약 벽 옆면에 비벼지고 있는 중이라면, Stay 상태에서도 계속 점프 중인 것으로 유지합니다.
            isJumping = true; 
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        isJumping = true;
    }
}
