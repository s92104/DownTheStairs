using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.UI;

public class Record
{
    public bool single;
    public string name;
    public int score;
}
public class RecordList
{
    public List<Record> records;
}

public class UI : MonoBehaviour {
    public bool single;
    public bool play;
    public GameObject mainMenu;
    public GameObject recordMenu;
    public GameObject pauseMenu;
    public GameObject deadMenu;
    public GameObject selectMenu;
    //紀錄選單
    public Text[] recordText;
    //死亡選單
    public InputField nameInput;
    public GameObject confirm;
    public Text deadScoreText;
    public Text bestText;
    //血條
    public Slider[] healthSlider;
    //玩家
    public GameObject[] player;
    //難度
    public GameObject floorGenerate;

    GameObject nowMenu;
    bool pause;
    string jsonString;
    RecordList jsonRecords;
    List<Record> singleRecords;
    List<Record> mutiRecords;
    //分數
    public Text scoreText;
    int floor=0;
    bool[] poison;
    // Use this for initialization
    void Start () {
        if(play)
            Time.timeScale = 0;
        poison = new bool[2];
        poison[0]=false;
        poison[1]=false;
        nowMenu = mainMenu;
        pause = false;
        //讀檔
        jsonString = readJson();
        jsonRecords = JsonConvert.DeserializeObject<RecordList>(jsonString);
        singleRecords = new List<Record>();
        mutiRecords = new List<Record>();
        if (jsonRecords==null)
        {
            jsonRecords = new RecordList();
            jsonRecords.records = new List<Record>();
        }
        else
        {
            foreach (Record record in jsonRecords.records)
            {
                if (record.single == true)
                    singleRecords.Add(record);
                else
                    mutiRecords.Add(record);
            }
        }
        InvokeRepeating("plusFloor", 1, 1);
    }
	
	// Update is called once per frame
	void Update () {
        //暫停
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pause)
                goPauseMenu();
            else
                resumeGame(); 
        }
        //血量
            //1P
        if (player[0].transform.position.y <= -5)
            player[0].GetComponent<PlayerController>().Health = 0;
        if (player[0].transform.position.y > 2 && !poison[0])
        {
            poison[0] = true;
            player[0].GetComponent<PlayerController>().Health -= 20;
        }
        if (player[0].transform.position.y <= 2)
            poison[0] = false;
        player[0].GetComponent<PlayerController>().Health = Mathf.Clamp(player[0].GetComponent<PlayerController>().Health, 0, 100);
        healthSlider[0].value = player[0].GetComponent<PlayerController>().Health;
            //2P
        if (player[1].transform.position.y <= -5)
            player[1].GetComponent<PlayerController2>().Health = 0;
        if (player[1].transform.position.y > 2 && !poison[1])
        {
            poison[1] = true;
            player[1].GetComponent<PlayerController2>().Health -= 20;
        }
        if (player[1].transform.position.y <= 2)
            poison[1] = false;
        player[1].GetComponent<PlayerController2>().Health = Mathf.Clamp(player[1].GetComponent<PlayerController2>().Health, 0, 100);
        healthSlider[1].value = player[1].GetComponent<PlayerController2>().Health;
        //判斷結束
        if (single)
        {
            if (healthSlider[0].value <= 0)
                goDeadMenu();
        }
        else
        {
            if (healthSlider[0].value <= 0 && healthSlider[1].value <= 0)
                goDeadMenu();
        }
	}
    void plusFloor()
    {
        floor++;
        scoreText.text = "第" + floor + "層";
    }

    public void goSingleScene()
    {
        SceneManager.LoadScene(1);
    }
    public void goMutiScene()
    {
        SceneManager.LoadScene(2);
    }
    public void goRecordMenu()
    {
        nowMenu.SetActive(false);
        recordMenu.SetActive(true);
        if (single)
            showSingleRecord();
        else
            showMutiRecord();
        nowMenu = recordMenu;
    }
    public void backMainMenu()
    {
        nowMenu.SetActive(false);
        mainMenu.SetActive(true);
        nowMenu = mainMenu;
        pause = false;
        Time.timeScale = 1;
        player[0].SetActive(false);
        player[1].SetActive(false);
    }
    string readJson()
    {
        //檢查檔案
        StreamWriter fileCheck = new StreamWriter(Application.dataPath + "/Record.json", true);
        fileCheck.Close();
        //讀取檔案
        StreamReader file = new StreamReader(Application.dataPath + "/Record.json");
        string tmpString = file.ReadToEnd();
        file.Close();
        return tmpString;
    }
    public void showSingleRecord()
    {
        foreach (Text text in recordText)
            text.text = "";
        //輸出
        if (singleRecords.Count!=0)                              
            for(int i=0;i<singleRecords.Count;i++)
                recordText[i].text = singleRecords[i].name + ":" + singleRecords[i].score + "層\n";
    }
    public void showMutiRecord()
    {
        foreach (Text text in recordText)
            text.text = "";
        //輸出
        if (mutiRecords.Count!=0)
            if (mutiRecords.Count != 0)
                for (int i = 0; i < mutiRecords.Count; i++)
                    recordText[i].text = mutiRecords[i].name + ":" + mutiRecords[i].score + "層\n";
    }
    void goPauseMenu()
    {
        pauseMenu.SetActive(true);
        pause = true;
        Time.timeScale = 0;
        nowMenu = pauseMenu;
    }
    public void resumeGame()
    {
        pauseMenu.SetActive(false);
        pause = false;
        Time.timeScale = 1;
    }
    void goDeadMenu()
    {
        Time.timeScale = 0;
        player[0].GetComponent<PlayerController>().Health = 100;
        player[1].GetComponent<PlayerController2>().Health = 100;
        if (single)
            player[0].transform.position = new Vector3(0, 1.3f, 0);
        else
        {
            player[0].transform.position = new Vector3(-3, 1.3f, 0);
            player[1].transform.position = new Vector3(3, 1.3f, 0);
        }
        deadMenu.SetActive(true);
        nowMenu = deadMenu;
        //顯示這局分數
        deadScoreText.text = floor + "層";
       
        if (single)
        {
            //顯示最高分數
            if (singleRecords.Count != 0)
                bestText.text = "最高紀錄:" + singleRecords[0].score + "層";        
            else
                bestText.text = "最高紀錄:無";
            //判斷是否有超越紀錄
            if (singleRecords.Count < 10)
            {
                nameInput.gameObject.SetActive(true);
                confirm.SetActive(true);
            }
            else if (singleRecords.Count == 10 && floor > singleRecords[singleRecords.Count - 1].score)
            {
                nameInput.gameObject.SetActive(true);
                confirm.SetActive(true);
            }
            else
            {
                nameInput.gameObject.SetActive(false);
                confirm.SetActive(false);
            }
        }
        else
        {
            if (mutiRecords.Count != 0)
                bestText.text = "最高紀錄:" + mutiRecords[0].score + "層";
            else
                bestText.text = "最高紀錄:無";
            //判斷是否有超越紀錄
            if (mutiRecords.Count < 10)
            {
                nameInput.gameObject.SetActive(true);
                confirm.SetActive(true);
            }
            else if (mutiRecords.Count == 10 && floor > mutiRecords[mutiRecords.Count - 1].score)
            {
                nameInput.gameObject.SetActive(true);
                confirm.SetActive(true);
            }
            else
            {
                nameInput.gameObject.SetActive(false);
                confirm.SetActive(false);
            }
        }
    }
    public void recordScore()
    {
        Record newRecord = new Record();
        newRecord.name = nameInput.text;
        newRecord.score = floor;
        newRecord.single = single;
        if(single)
        {
            singleRecords.Add(newRecord);
            //排序
            for (int i = 0; i < singleRecords.Count; i++)
            {
                for (int j = i; j < singleRecords.Count; j++)
                {
                    if (singleRecords[j].score > singleRecords[i].score)
                    {
                        Record tmp = singleRecords[j];
                        singleRecords[j] = singleRecords[i];
                        singleRecords[i] = tmp;
                    }
                }
            }
            if (singleRecords.Count > 10)
                singleRecords.RemoveAt(10);
        }
        else
        {
           mutiRecords.Add(newRecord);
            //排序
            for (int i = 0; i <mutiRecords.Count; i++)
            {
                for (int j = i; j <mutiRecords.Count; j++)
                {
                    if (mutiRecords[j].score >mutiRecords[i].score)
                    {
                        Record tmp =mutiRecords[j];
                       mutiRecords[j] =mutiRecords[i];
                       mutiRecords[i] = tmp;
                    }
                }
            }
            if (mutiRecords.Count > 10)
               mutiRecords.RemoveAt(10);
        }
        StreamWriter file = new StreamWriter(Application.dataPath + "/Record.json", false);
        RecordList tmpRec = new RecordList();
        tmpRec.records = new List<Record>();
        tmpRec.records.AddRange(singleRecords);
        tmpRec.records.AddRange(mutiRecords);
        file.WriteLine(JsonConvert.SerializeObject(tmpRec));
        file.Close();
        //跳記錄選單
        goRecordMenu();
    }
    public void selectSimple()
    {
        Time.timeScale = 1;
        floorGenerate.GetComponent<FloorGenerate>().proportion1 = 2;
        floorGenerate.GetComponent<FloorGenerate>().proportion2 = 1;
        selectMenu.SetActive(false);
    }
    public void selectDifficult()
    {
        Time.timeScale = 1;
        floorGenerate.GetComponent<FloorGenerate>().proportion1 = 1;
        floorGenerate.GetComponent<FloorGenerate>().proportion2 = 2;
        selectMenu.SetActive(false);
    }
}
