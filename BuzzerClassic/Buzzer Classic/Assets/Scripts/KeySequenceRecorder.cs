using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class KeySequenceRecorder : MonoBehaviour
{
    // Dynamic list (easier than fixed array)
    private List<int> keySequence = new List<int>();
    public TMP_Text[] textHolders;

    // Public read-only array (if you need it elsewhere)
    public int[] CurrentSequence
    {
        get { return keySequence.ToArray(); }
    }

    void Update()
    {
        for(int i = 0; i<10; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                AddNumber(i);
                textHolders[i].text = keySequence[keySequence.Count - 1].ToString();
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            ClearSequence();
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
        Debug.Log("Added: " + number);
        PrintSequence();
    }

    void ClearSequence()
    {
        keySequence.Clear();
        Debug.Log("Sequence Cleared");
    }

    void PrintSequence()
    {
        string sequenceString = "Sequence: ";

        for (int i = 0; i < keySequence.Count; i++)
        {
            sequenceString += keySequence[i];

            if (i < keySequence.Count - 1)
                sequenceString += ", ";
        }

        Debug.Log(sequenceString);
    }
}
