using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO.Ports;

public class PlayGame : MonoBehaviour
{

    private SerialPort serialPort;
    // Start is called before the first frame update
    void Start()
    {
        serialPort = new SerialPort("COM3", 9600);
        serialPort.Open();
    }

    // Update is called once per frame
    void Update()
    {
        string data = serialPort.ReadLine();
        if (data == "1" || data == "2")
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}
