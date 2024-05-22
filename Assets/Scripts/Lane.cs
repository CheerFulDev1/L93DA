using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class Lane : MonoBehaviour
{

    private SerialPort serialPort;
    public SpawnMode spawnMode = SpawnMode.HOMING;

    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
    public KeyCode input;
    public GameObject notePrefab;
    List<Note> notes = new List<Note>();
    public List<double> timeStamps = new List<double>();
    public String tag;
    public string stampNumber;

    int spawnIndex = 0;
    int inputIndex = 0;
    private string data;

    // Start is called before the first frame update
    void Start()
    {
            serialPort = new SerialPort("COM3", 9600);
            serialPort.Open();

    }
    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var note in array)
        {
            if (note.NoteName == noteRestriction)
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());
                timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
        if (spawnIndex < timeStamps.Count)
        {
            if (SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - SongManager.Instance.noteTime)
            {
                //var note = Instantiate(notePrefab, transform);
                var note = ObjectPooler.Instance.SpawnFromPool(tag, transform.position, transform.rotation);
                notes.Add(note.GetComponent<Note>());
                note.GetComponent<Note>().assignedTime = (float)timeStamps[spawnIndex];
                spawnIndex++;
            }
        }

        data = serialPort.ReadLine();
        print(data);
        // this is very complicated when notes could be handled with a trigger zone 
        if (inputIndex < timeStamps.Count)
        {
            double timeStamp = timeStamps[inputIndex];
            double marginOfError = SongManager.Instance.marginOfError;
            double audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelayInMilliseconds / 1000.0);

            if (data == "1" || data == "2")
            {
                print("data received");
                if (Math.Abs(audioTime - timeStamp) < marginOfError)
                {
                    Hit();
                    print($"Hit on {inputIndex} note");
                    notes[inputIndex].gameObject.SetActive(false);
                    //Destroy(notes[inputIndex].gameObject);
                    inputIndex++;
                }
                else
                {
                    print($"Hit inaccurate on {inputIndex} note with {Math.Abs(audioTime - timeStamp)} delay");
                }
            }
            if (timeStamp + marginOfError <= audioTime)
            {
                Miss();
                print($"Missed {inputIndex} note");
                inputIndex++;
            }
        }       
    
    }
    private void Hit()
    {
        ScoreManager.Hit();
    }
    private void Miss()
    {
        ScoreManager.Miss();
    }

    void OnMusicEnd()
    {
        // Trigger scene quit logic (consider using Application.Quit() or a custom quit function)
        Application.Quit(); // Quits the entire application
        // OR
        // YourCustomQuitFunction(); // Define your custom quit logic (e.g., save data, display message)
    }

}
