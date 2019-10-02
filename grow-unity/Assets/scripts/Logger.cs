using System;
using System.Globalization;
using System.IO;
using UnityEngine;

public class Logger : MonoBehaviour
{
    public GameManager gm;
    [SerializeField] public string path;
    private readonly string filename = "Log";
    public StreamWriter writer;

    public void Start()
    {
        DateTime localDate = DateTime.Now;
        writer = new StreamWriter(path + "\\" + filename + "-" + localDate.ToString("dd_MM_yyyy-HH_mm_ss") + ".csv", true);
        //@TODO: Setup Header for log to see States like: handedness, random Order etc.
        writer.WriteLine("This log is generated on " + localDate.ToString("dd.MM.yyyy HH:mm:ss"));
    }


    void Update()
    {

    }

    void OnApplicationQuit()
    {
        writer.Close();
    }


    public void Log(string logEntry)
    {
        writer.WriteLine(gm.GetTimeStamp() + ";" + logEntry);
    }

    public void SetPath(string path)
    {
        this.path = path;
    }
}
