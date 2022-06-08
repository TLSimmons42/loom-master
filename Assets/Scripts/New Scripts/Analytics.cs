using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;


public class Analytics : Singleton<Analytics>
{
    string savePath;
    string filePath;
    string csvPath;
    // Start is called before the first frame update
    void Start()
    {
        savePath = Application.dataPath + "/Analytics";
        filePath = Application.dataPath + "/Analytics/analytics.json";
        csvPath = Application.dataPath + "/Analytics/analytics.csv";

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
        data.participant = "P1";
        data.task = "Loom";
        data.age = "20";
        data.gender = "f";
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

    string[] GetDataArray(DataPoint data)
    {
        List<string> stringlist = new List<string>();
        stringlist.Add(data.timestamp);
        stringlist.Add(data.participant);
        stringlist.Add(data.task);
        stringlist.Add(data.age);
        stringlist.Add(data.gender);
        stringlist.Add(data.sessionTime);
        stringlist.Add(data.eventName);
        stringlist.Add(data.testX);
        stringlist.Add(data.testY);
        stringlist.Add(data.testZ);


        return stringlist.ToArray();
    }

    public class DataPoint
    {
        public string timestamp;
        public string participant;
        public string task;
        public string age;
        public string gender;
        public string sessionTime;
        public string eventName;
        public string testX;
        public string testY;
        public string testZ;


    }
}
