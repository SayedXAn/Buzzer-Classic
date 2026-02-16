using UnityEngine;
using System.IO.Ports;
using System;

public class SerialRelayController : MonoBehaviour
{
    SerialPort serialPort;

    [Header("Serial Settings")]
    public string portName = "COM6";   
    public int baudRate = 115200;

    void Start()
    {
        try
        {
            serialPort = new SerialPort(portName, baudRate);
            serialPort.ReadTimeout = 50;
            serialPort.Open();
            Debug.Log("Serial Port Opened");
        }
        catch (Exception e)
        {
            Debug.LogError("Serial Connection Failed: " + e.Message);
        }
    }

    void Update()
    {
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
