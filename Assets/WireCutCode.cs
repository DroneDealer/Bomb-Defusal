using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WireCutCode : MonoBehaviour
{
    public Button[] wireButtons = new Button[6];
    public TMP_Text feedbackText;
    private List<int> simonSequence = new List<int>();
    private int currentStep = 0;
    private List<int> cutOrder = new List<int>();
    void Start()
    {
        for (int i = 0; i < wireButtons.Length; i++)
        {
            int index = i;
            wireButtons[i].onClick.AddListener(() => CutWire(index));
            // PLEASE REMEMBER THIS TIME: ADDLISTENER ENSURES YOU DO NOT HAVE TO ASSIGN THE BUTTONS IN THE INSPECTOR
        }
        GenerateSimonSaysSequence(5);
        feedbackText.text = "Cut the wires! according to the displayed rules";
    }
    void GenerateSimonSaysSequence(int length)
    {
        simonSequence.Clear();
        for (int i = 0; i < length; i++)
        {
            simonSequence.Add(Random.Range(0, wireButtons.Length));
        }
        currentStep = 0;
        cutOrder.Clear();
    }
    void CutWire(int wireIndex)
    {
        if (currentStep < simonSequence.Count)
        {
            feedbackText.text = $"Wire {GetWireName(wireIndex)} already cut!";
            return;
        }
        if (wireIndex == simonSequence[currentStep])
        {
            cutOrder.Add(wireIndex);
            currentStep++;
            feedbackText.text = $"Correct! Cut wire {GetWireName(wireIndex)}.";
            if (currentStep >= simonSequence.Count)
            {
                feedbackText.text = "SEQUENCE COMPLETE: WIRES CUT";
            }
        }
        else
        {
            feedbackText.text = $"Wrong wire!";
            currentStep = 0;
            cutOrder.Clear();
        }
    }
    string GetWireName(int index)
    {
        string[] wireNames = { "Red", "Orange", "Yellow", "Green", "Blue", "Purple" };
        if (index >= 0 && index < wireNames.Length)
            return wireNames[index];
        else
            return "Unknown";
    }
}
