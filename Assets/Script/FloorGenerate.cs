using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorGenerate : MonoBehaviour {
    public GameObject[] simpleFloor;
    public GameObject[] specialFlooe;
    public GameObject[] position;
    public int proportion1;
    public int proportion2;
    public int count;
    public float speed;
	// Use this for initialization
	void Start () {
        InvokeRepeating("generate", 0, speed);
    }

    // Update is called once per frame
    void Update () {

	}

    void generate()
    {
        List<int> indexArray = new List<int>();
        //加入位置index
        for (int i = 0; i < position.Length; i++)
            indexArray.Add(i);
        //生成地板
        for(int i=0;i<count;i++)
        {
            int posIndex = Random.Range(0, indexArray.Count);
            int probability = Random.Range(0, proportion1+proportion2);
            //一般地板
            if(probability/proportion1 < 1)
            {
                int floorIndex = Random.Range(0, simpleFloor.Length);
                Instantiate<GameObject>(simpleFloor[floorIndex], position[indexArray[posIndex]].transform.position, new Quaternion());
            }
            //特殊地板
            else
            {
                int floorIndex = Random.Range(0, specialFlooe.Length);
                Instantiate<GameObject>(specialFlooe[floorIndex], position[indexArray[posIndex]].transform.position, new Quaternion());
            }
            indexArray.RemoveAt(posIndex);
        }

    }
}
