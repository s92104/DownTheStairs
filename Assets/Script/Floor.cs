using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour {
    public float height = 5f;
    public float speed = 0.1f;
    public enum floorType
    {
        simple,
        fire,
        ice,
        knife,
        sand
    }
    public floorType type;
    public GameObject fireAnimation;
    public GameObject iceAnimation;
    float nowTime;
    float nextTime;
    // Use this for initialization
    void Start()
    {
        //熔岩地板特效
        if(type==floorType.fire)
        {
            InvokeRepeating("fireEffect", 0, 1);
        }
        //冰塊地板特效
        else if(type==floorType.ice)
        {
            InvokeRepeating("iceEffect", 0, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //地板上升
        transform.position += new Vector3(0, speed*Time.deltaTime, 0);
        //自動刪除
        if (transform.position.y > height)
            Destroy(this.gameObject);
        //到頂關掉碰撞
        if (transform.position.y >= 3.2f)
            GetComponent<EdgeCollider2D>().enabled = false;
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        //一般地板:加血
        if(type==floorType.simple)
        {
            if(other.gameObject.tag=="2P")
                other.gameObject.GetComponent<PlayerController2>().Health += 10;
            else
                other.gameObject.GetComponent<PlayerController>().Health += 10;
        }
        //尖刺地板:扣血
        else if(type==floorType.knife)
        {
            if (other.gameObject.tag == "2P")
                other.gameObject.GetComponent<PlayerController2>().Health -= 10;
            else
                other.gameObject.GetComponent<PlayerController>().Health -= 10;
        }
        //熔岩地板:紀錄進入時間，計算執行時間
        else if(type==floorType.fire)
        {
            nowTime = Time.time;
            nextTime = nowTime + 1;
        }
        //沙子地板:關掉碰撞
        else if(type==floorType.sand)
        {
            Invoke("sandEffect", 1);
        }
    }
    void OnCollisionStay2D(Collision2D other)
    {
        //熔岩地板:更新時間，檢查執行時間
        if (type==floorType.fire)
        {
            nowTime += Time.deltaTime;
            if(nowTime>=nextTime)
            {
                if (other.gameObject.tag == "2P")
                    other.gameObject.GetComponent<PlayerController2>().Health -= 2;
                else
                    other.gameObject.GetComponent<PlayerController>().Health -= 2;
                nextTime += 1;
            }
        }
        //冰塊地板:減速
        else if(type==floorType.ice)
        {
            if (other.gameObject.tag == "2P")
                other.gameObject.GetComponent<PlayerController2>().AddSpeed = 8;
            else
                other.gameObject.GetComponent<PlayerController>().AddSpeed = 8;
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        //冰塊地板:回復正常速度
        if (type == floorType.ice)
        {
            if (other.gameObject.tag == "2P")
                other.gameObject.GetComponent<PlayerController2>().AddSpeed = 15;
            else
                other.gameObject.GetComponent<PlayerController>().AddSpeed = 15;
        }
    }
    //地板Function
    void fireEffect()
    {
        //清除上次的
        if(transform.Find("FireEffect(Clone)")!=null)
            Destroy(transform.Find("FireEffect(Clone)").gameObject);
        //0.5 0.6
        int index = Random.Range(0, 3);
        GameObject fire;
        if(index==0)
        {
            fire=Instantiate<GameObject>(fireAnimation, transform.position + new Vector3(-0.5f, 0.6f, 0), new Quaternion());
        }
        else if(index==1)
        {
            fire=Instantiate<GameObject>(fireAnimation, transform.position + new Vector3(0, 0.6f, 0), new Quaternion());
        }
        else
        {
            fire=Instantiate<GameObject>(fireAnimation, transform.position + new Vector3(0.5f, 0.6f, 0), new Quaternion());
        }
        fire.transform.parent = transform;
    }
    void iceEffect()
    {
        //清除上次的
        if (transform.Find("IceEffect(Clone)") != null)
            Destroy(transform.Find("IceEffect(Clone)").gameObject);
        //0.56 0.85
        int index = Random.Range(0, 3);
        GameObject ice;
        if (index == 0)
        {
            ice = Instantiate<GameObject>(iceAnimation, transform.position + new Vector3(-0.56f, 0.85f, 0), new Quaternion());
        }
        else if (index == 1)
        {
            ice = Instantiate<GameObject>(iceAnimation, transform.position + new Vector3(0, 0.85f, 0), new Quaternion());
        }
        else
        {
            ice = Instantiate<GameObject>(iceAnimation, transform.position + new Vector3(0.56f, 0.85f, 0), new Quaternion());
        }
        ice.transform.parent = transform;
    }
    void sandEffect()
    {
        GetComponent<EdgeCollider2D>().enabled = false;
    }

}
