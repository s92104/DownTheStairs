using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private Animator PlayerAnimator;
    public int Health;
    //速度
    public float Speed;
    //最大速度
    public float MaxSpeed;
    //加速度
    public float AddSpeed;
    public float JumpStr;
    //可否雙重跳躍
    public bool DoubleJumpAbility;
    //是否已經雙重跳躍
    public bool DoubleJumpEnaable;
    public bool OnGround;
    private bool FaceRight;
    private Rigidbody2D rig;
	// Use this for initialization
	void Start () {
        Health = 100;
        Speed = 0.0f;
        AddSpeed = 15.0f;
        MaxSpeed = 10.0f;
        JumpStr = 2.0f;
        PlayerAnimator = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        FaceRight = true;
    }
	
	// Update is called once per frame
	void Update () {
        transform.localRotation = new Quaternion(0, 0, 0, 0);
        float move = Input.GetAxis("Horizontal");
        PlayerAnimator.SetFloat("Speed", Mathf.Abs(Speed));
        if (Input.GetKey(KeyCode.D))
        {
            Speed = Mathf.MoveTowards(Speed, MaxSpeed, AddSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Speed = Mathf.MoveTowards(Speed, -MaxSpeed, AddSpeed * Time.deltaTime);
        }
        else
        {
            Speed = Mathf.MoveTowards(Speed, 0.0f, 1.5f*AddSpeed * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.W) && OnGround)
        {
            rig.AddForce(new Vector2(0, JumpStr),ForceMode2D.Impulse);
        }
        if((Speed <0.0f && FaceRight) || (Speed > 0.0f && !FaceRight))
        {
            FaceRight = !FaceRight;
            Vector3 scale = transform.localScale;
            scale.x = -scale.x;
            transform.localScale = scale;
        }


        rig.velocity = new Vector2(Speed, rig.velocity.y);

    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "ground")
        {
            DoubleJumpEnaable = true;
            OnGround = true;
        }
    }
    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "ground")
        {
            OnGround = false;
        }
    }
    void ChangeHealth(int amount)
    {
        Health += amount;
    }
}
