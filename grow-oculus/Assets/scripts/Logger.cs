﻿using System;
using System.Globalization;
using System.IO;
using UnityEngine;

public class Logger : MonoBehaviour
{
    [Space(10)]
    public string filename;
    [Space(10)]
    public GameManager gm;
    [SerializeField] public string path;
    [HideInInspector]
    public StreamWriter writer;

    public void Start()
    {
        DateTime localDate = DateTime.Now;
        writer = new StreamWriter(path + "\\" + gm.playerName + " - " + filename + " - " + localDate.ToString("dd_MM_yyyy-HH_mm_ss") + ".csv", true);
    }

    //log the current condition with all the 
    public void Log(string logentry)
    {
        writer.WriteLine(logentry);
    }

    void OnApplicationQuit()
    {
        writer.Close();
    }

    public void SetPath(string path)
    {
        this.path = path;
    }
}
