using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigid2D;
    Animator animator;
    //float jumpForce = 680.0f;
    float walkForce = 4.0f;
    float maxWalkSpeed = 4.0f;
    int jumpGauge = 0;
    bool isJumping;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60;
        this.rigid2D = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        

        if (Input.GetKeyDown(KeyCode.Space) && this.rigid2D.linearVelocity.y == 0)
        {
            this.rigid2D.linearVelocity = new Vector2(0, 0);
            
        }
        if (Input.GetKey(KeyCode.Space) && this.rigid2D.linearVelocity.y == 0)
        {
            transform.Translate(0, 0, 0);
            jumpGauge++;
        }
        if (Input.GetKeyUp(KeyCode.Space) && this.rigid2D.linearVelocity.y == 0)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                this.rigid2D.AddForce(transform.up * jumpGauge * 23f);
                this.rigid2D.AddForce(transform.right*150f);
                jumpGauge = 0;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                this.rigid2D.AddForce(transform.up * jumpGauge * 23f);
                this.rigid2D.AddForce(-transform.right*150f);
                jumpGauge = 0;
            }
            else
            {
                this.rigid2D.AddForce(transform.up * jumpGauge * 25f);
                jumpGauge = 0;
            }
            
        }
        int key = 0;
        if (Input.GetKey(KeyCode.LeftArrow)) key = -1;
        if (Input.GetKey(KeyCode.RightArrow)) key = 1;
        float speedx = Mathf.Abs(this.rigid2D.linearVelocity.x);

        if (this.rigid2D.linearVelocity.y == 0 && !Input.GetKey(KeyCode.Space) && !isJumping)
        {
            this.rigid2D.linearVelocity = new Vector2(this.walkForce * key, 0);
        }

        if (key != 0)
        {
            transform.localScale = new Vector3(-key*0.1f, 0.1f, 1.0f);
        }
        this.animator.speed = speedx/2.0f ;
        Debug.Log(isJumping);

    }
    void OnTriggerEnter2D(Collider2D other)
        {
            SceneManager.LoadScene("ClearScene");
        }
    void OnCollisionEnter2D(Collision2D other)
    {
        Vector2 normal = other.contacts[0].normal;

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
