using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;


public class Analytics : Singleton<Analytics>
{
    string savePath;
    string filePath;
    string csvPath;
    string filePath2;
    string csvPath2;

    public InputField partID;
    public InputField partAge;
    public InputField partGender;
    public InputField condition;
    public InputField trial;



    // Start is called before the first frame update
    void Start()
    {

        

        savePath = Application.dataPath + "/Analytics";
        filePath = Application.dataPath + "/Analytics/analytics.json";
        csvPath = Application.dataPath + "/Analytics/analytics.csv";
        filePath2 = Application.dataPath + "/Analytics/analytics2.json";
        csvPath2 = Application.dataPath + "/Analytics/analytics2.csv";

        if (!File.Exists(savePath))
        {
            Debug.Log("making new analytics forlder");
            Directory.CreateDirectory(savePath);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void WriteData(string eventString, string participant, string sessionTime, string testX, string testY, string testZ)
    {
        DataPoint data = new DataPoint();
        data.timestamp = DateTime.Now.Ticks.ToString();
        data.participant = PlayerPrefs.GetString("ParticipantID");
        data.task = PlayerPrefs.GetString("ParticipantCondition");
        data.trial = PlayerPrefs.GetString("trial");
        data.age = PlayerPrefs.GetInt("ParticipantAge").ToString();
        data.gender = PlayerPrefs.GetString("ParticipantGender");
        data.sessionTime = sessionTime;
        data.eventName = eventString;
        data.testX = testX;
        data.testY = testY;
        data.testZ = testZ;



        string jsonString = JsonUtility.ToJson(data);
        string csvstring = String.Join(",", GetDataArray(data));
        //File.AppendAllText(filePath, "\n");
        if (!File.Exists(csvPath))
        {
            File.WriteAllText(csvPath, "TimeStamp,participant,Experiment,SessionTime,Event, xPos, yPos, zPos");
        }
        File.AppendAllText(csvPath, "\n");
        File.AppendAllText(filePath, jsonString);
        File.AppendAllText(csvPath, csvstring);
    }
    public void WriteData2(string eventString, string participant, string sessionTime, string testX, string testY, string testZ)
    {
        DataPoint data = new DataPoint();
        data.timestamp = DateTime.Now.Ticks.ToString();
        data.participant = PlayerPrefs.GetString("ParticipantID");
        data.task = PlayerPrefs.GetString("ParticipantCondition");
        data.trial = PlayerPrefs.GetString("trial");
        data.age = PlayerPrefs.GetInt("ParticipantAge").ToString();
        data.gender = PlayerPrefs.GetString("ParticipantGender");
        data.sessionTime = sessionTime;
        data.eventName = eventString;
        data.testX = testX;
        data.testY = testY;
        data.testZ = testZ;



        string jsonString = JsonUtility.ToJson(data);
        string csvstring = String.Join(",", GetDataArray(data));
        //File.AppendAllText(filePath, "\n");
        if (!File.Exists(csvPath2))
        {
            File.WriteAllText(csvPath2, "TimeStamp,participant,Experiment,SessionTime,Event, xPos, yPos, zPos");
        }
        File.AppendAllText(csvPath2, "\n");
        File.AppendAllText(filePath2, jsonString);
        File.AppendAllText(csvPath2, csvstring);
    }

    string[] GetDataArray(DataPoint data)
    {
        List<string> stringlist = new List<string>();
        stringlist.Add(data.timestamp);
        stringlist.Add(data.participant);
        stringlist.Add(data.task);
        stringlist.Add(data.trial);
        stringlist.Add(data.age);
        stringlist.Add(data.gender);
        stringlist.Add(data.sessionTime);
        stringlist.Add(data.eventName);
        stringlist.Add(data.testX);
        stringlist.Add(data.testY);
        stringlist.Add(data.testZ);


        return stringlist.ToArray();
    }

    public void SetParticipantID(string id)
    {
        PlayerPrefs.SetString("ParticipantID", id);
    }
    public void SetParticipantAge(string age)
    {
        PlayerPrefs.SetString("ParticipantAge", age);
    }
    public void SetParticipantCondition(string con)
    {
        PlayerPrefs.SetString("ParticipantCondition", con);
    }
    public void Settrial(string trial)
    {
        PlayerPrefs.SetString("trial", trial);
    }
    public void SetParticipantGender(string gen)
    {
        PlayerPrefs.SetString("ParticipantGender", gen);
    }
    public void SetPartVariables()
    {
        SetParticipantID(partID.text);
        SetParticipantAge(partAge.text);
        SetParticipantCondition(condition.text);
        SetParticipantGender(partGender.text);
        Settrial(trial.text);
    }


    public class DataPoint
    {
        public string timestamp;
        public string participant;
        public string task;
        public string trial;
        public string age;
        public string gender;
        public string sessionTime;
        public string eventName;
        public string testX;
        public string testY;
        public string testZ;


    }
}
