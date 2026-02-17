using UnityEngine;
using System.IO.Ports;
using System;
using TMPro;
using System.Collections;

public class SerialRelayController : MonoBehaviour
{
    SerialPort serialPort;
    public KeySequenceRecorder ksr;
    [Header("UI")]
    public TMP_InputField comPortInput;   // Drag your input field here
    private const string COM_PORT_KEY = "ESP32_COM_PORT";
    public GameObject adminPanel;
    public TMP_Text currCOM;


    [Header("Serial Settings")]
    public string portName = "COM6";
    public int baudRate = 115200;

    [Header("Time Settings")]
    private int timeToCount = 0;
    private int timeCounter = 0;
    public bool isGameOn = false;
    public TMP_Text timeText;
    public TMP_InputField timeIF;
    public AudioSource clockSFX;


    void Start()
    {
        currCOM.text = "No COM Port Set";
        if (PlayerPrefs.HasKey(COM_PORT_KEY))
        {
            portName = PlayerPrefs.GetString(COM_PORT_KEY);
            currCOM.text = "Current COM: " + portName;
        }

        if (comPortInput != null)
            comPortInput.text = portName;

        OpenSerialPort();
        if (timeToCount == 0)
        {
            isGameOn = true;
            timeText.text = "";
            ksr.SetGameOn(true);
        }
        else
        {
            timeText.text = "Time Left: " + timeToCount.ToString() + "s";
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            OpenAdminPanel();
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            StopCountDown();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCountDown();
        }
        if (serialPort == null || !serialPort.IsOpen)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                SendData("A");
            else
                SendData("a");
            if(timeToCount > 0 && isGameOn)
            {
                timeText.text = "Time Left: " + timeToCount.ToString() + "s\nPress space to start";
                isGameOn = false;
                ksr.SetGameOn(false);
            }
        }

        if (isGameOn)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0)) SendData("0");
            if (Input.GetKeyDown(KeyCode.Alpha1)) SendData("1");
            if (Input.GetKeyDown(KeyCode.Alpha2)) SendData("2");
            if (Input.GetKeyDown(KeyCode.Alpha3)) SendData("3");
            if (Input.GetKeyDown(KeyCode.Alpha4)) SendData("4");
            if (Input.GetKeyDown(KeyCode.Alpha5)) SendData("5");
            if (Input.GetKeyDown(KeyCode.Alpha6)) SendData("6");
            if (Input.GetKeyDown(KeyCode.Alpha7)) SendData("7");
            if (Input.GetKeyDown(KeyCode.Alpha8)) SendData("8");
            if (Input.GetKeyDown(KeyCode.Alpha9)) SendData("9");
        }
        
    }

    void OpenSerialPort()
    {
        try
        {
            serialPort = new SerialPort(portName, baudRate);
            serialPort.ReadTimeout = 50;
            serialPort.Open();
            Debug.Log("Serial Port Opened: " + portName);
        }
        catch (Exception e)
        {
            Debug.LogError("Serial Connection Failed: " + e.Message);
        }
    }

    public void UpdateComPortFromInput()
    {
        if (comPortInput == null)
            return;

        string newPort = comPortInput.text.Trim();
        newPort = newPort.ToUpper();
        if (newPort.Length > 5)
        {
            comPortInput.placeholder.GetComponent<TMP_Text>().text = "Invalid COM Format!\nExample: COM3";
            Debug.LogWarning("Invalid COM port format. Please enter something like 'COM3'.");
            return;
        }

        if (string.IsNullOrEmpty(newPort))
        {
            Debug.LogWarning("COM port cannot be empty.");
            return;
        }

        // Close existing port if open
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
            Debug.Log("Old Serial Port Closed");
        }

        portName = newPort;

        // Save to PlayerPrefs
        PlayerPrefs.SetString(COM_PORT_KEY, portName);
        PlayerPrefs.Save();

        Debug.Log("Saved COM Port: " + portName);

        // Re-open with new port
        OpenSerialPort();
        adminPanel.SetActive(false);
        Application.Quit();
    }

    public void SetTime()
    {
        if (timeIF.text == "" || timeIF.text == "0")
        {
            timeIF.placeholder.GetComponent<TMP_Text>().text = "Please enter a valid number!";
            return;
        }
        else
        {
            timeToCount = int.Parse(timeIF.text);
            timeText.text = "Time Left: " + timeToCount.ToString() + "s\nPress space to start";
            isGameOn = false;
            ksr.SetGameOn(false);
        }        
    }

    public void OpenAdminPanel()
    {
        currCOM.text = "Current COM: " + portName;
        adminPanel.SetActive(true);
        isGameOn = false;
        ksr.SetGameOn(false);
    }

    public void CloseAdminPanel()
    {
        adminPanel.SetActive(false);
        if(timeToCount == 0)
        {
            isGameOn = true;
            ksr.SetGameOn(true);
        }
    }


    void SendData(string data)
    {
        try
        {
            serialPort.Write(data);
            Debug.Log("Sent: " + data);
        }
        catch (Exception e)
        {
            Debug.LogError("Send Failed: " + e.Message);
        }
    }

    void OnApplicationQuit()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
            Debug.Log("Serial Port Closed");
        }
    }
    public void StartCountDown()
    {
        if (!isGameOn)
        {
            timeCounter = timeToCount;
            clockSFX.Play();
            StartCoroutine(TimeCountDown());
        }
    }

    IEnumerator TimeCountDown()
    {
        yield return new WaitForSeconds(1f);
        if(timeCounter > 0)
        {
            timeCounter--;
            timeText.text = "Time Left: " + timeCounter.ToString() + "s";
            StartCoroutine(TimeCountDown());
        }
        else
        {
            timeText.text = "";
            isGameOn = true;
            ksr.SetGameOn(true);
            clockSFX.Stop();
        }
    }

    public void StopCountDown()
    {
        StopAllCoroutines();
        clockSFX.Stop();
        isGameOn = false;
        ksr.ClearSequence();
        ksr.SetGameOn(false);
        timeCounter = 0;
        timeText.text = "Time Left: " + timeToCount.ToString() + "s\nPress space to start";
    }
}
