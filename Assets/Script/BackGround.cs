using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour {
    public float speed = 1.5f;
    public Vector3 repeatPos = new Vector3(0, 10, 0);
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        transform.position += new Vector3(0, speed * Time.deltaTime, 0);
        if (transform.position.y >= repeatPos.y)
            transform.position = Vector3.zero;
	}
}
