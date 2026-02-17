using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class KeySequenceRecorder : MonoBehaviour
{
    // Dynamic list (easier than fixed array)
    private List<int> keySequence = new List<int>();
    public TMP_Text[] textHolders;
    int currText = -1;
    private bool isGameOn = false;
    // Public read-only array (if you need it elsewhere)
    public int[] CurrentSequence
    {
        get { return keySequence.ToArray(); }
    }
    private void Start()
    {
        foreach (var text in textHolders)
        {
            text.text = "";
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ClearSequence();
        }

        if(!isGameOn)
        {
            return;
        }
        for (int i = 0; i<10; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                AddNumber(i);
            }
        }
        
    }

    void AddNumber(int number)
    {
        if(keySequence.Contains(number))
        {
            Debug.Log("Number " + number + " already in sequence. Ignoring.");
            return;
        }
        keySequence.Add(number);
        currText++;
        
        if(number == 0)
        {
            textHolders[currText].text = "10";
        }
        else
        {
            textHolders[currText].text = number.ToString();
        }
        Debug.Log("Added: " + number);

    }

    public void ClearSequence()
    {
        keySequence.Clear();
        currText = -1;
        foreach (var text in textHolders)
        {
            text.text = "";
        }
        Debug.Log("Sequence Cleared");
    }

    public void SetGameOn(bool value)
    {
        isGameOn = value;
    }

}
