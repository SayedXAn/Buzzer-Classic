using UnityEngine;
using System.IO.Ports;
using System;
using TMPro;

public class SerialRelayController : MonoBehaviour
{
    SerialPort serialPort;

    [Header("UI")]
    public TMP_InputField comPortInput;   // Drag your input field here
    private const string COM_PORT_KEY = "ESP32_COM_PORT";
    public GameObject adminPanel;
    public TMP_Text currCOM;


    [Header("Serial Settings")]
    public string portName = "COM6";   
    public int baudRate = 115200;

    void Start()
    {
        currCOM.text = "No COM Port Set";
        if (PlayerPrefs.HasKey(COM_PORT_KEY))
        {
            portName = PlayerPrefs.GetString(COM_PORT_KEY);
            currCOM.text = "Current COM: " + portName;
        }

        // Update UI field text
        if (comPortInput != null)
            comPortInput.text = portName;

        OpenSerialPort();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            OpenAdminPanel();
        }
        if (serialPort == null || !serialPort.IsOpen)
            return;

        // Numbers 0–9
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

        // A or a
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                SendData("A");
            else
                SendData("a");
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

    public void OpenAdminPanel()
    {
        Debug.Log("akdbkdbk");
        currCOM.text = "Current COM: " + portName;
        adminPanel.SetActive(true);
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
}
