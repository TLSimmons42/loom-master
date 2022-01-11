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


    public void WriteData(string eventString, string participant, string sessionTime)
    {
        DataPoint data = new DataPoint();
        data.timestamp = DateTime.Now.ToString();
        data.participant = "placeholder";
        data.task = "Loom";
        data.sessionTime = sessionTime;
        data.eventName = eventString;
        

        string jsonString = JsonUtility.ToJson(data);
        string csvstring = String.Join(",", GetDataArray(data));
        //File.AppendAllText(filePath, "\n");
        if (!File.Exists(csvPath))
        {
            File.WriteAllText(csvPath, "TimeStamp,participant,Experiment,SessionTime,TrialType,Event");
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
        stringlist.Add(data.sessionTime);
        stringlist.Add(data.eventName);

        return stringlist.ToArray();
    }

    public class DataPoint
    {
        public string timestamp;
        public string participant;
        public string task;
        public string sessionTime;
        public string eventName;

    }
}
