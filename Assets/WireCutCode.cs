using System.Collections;
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
    public float lightDuration = 0.6f; // how long a wire highlights during playback
    public float pauseDuration = 0.3f; // pause between highlights
    private bool playerTurn = false;
    private int sequenceLength = 5; // starting sequence length
    void Start()
    {
        //Figure out how to trigger this properly
        for (int i = 0; i < wireButtons.Length; i++)
        {
            int index = i;
            wireButtons[i].onClick.AddListener(() => CutWire(index));
            // PLEASE REMEMBER THIS TIME: ADDLISTENER ENSURES YOU DO NOT HAVE TO ASSIGN THE BUTTONS IN THE INSPECTOR
        }
        StartCoroutine(StartNewRound());
    }
    IEnumerator StartNewRound()
    {
        playerTurn = false;
        feedbackText.text = $"LINE OF DEFENSE: {sequenceLength} WIRES";
        GenerateSimonSaysSequence(sequenceLength);
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(PlaySequence());
        feedbackText.text = "Cut the wires in the correct order";
        playerTurn = true;
        currentStep = 0;
        cutOrder.Clear();
    }
    void GenerateSimonSaysSequence(int length)
    {
        simonSequence.Clear();
        for (int i = 0; i < length; i++)
        {
            simonSequence.Add(Random.Range(0, wireButtons.Length));
        }
    }
    IEnumerator PlaySequence()
    {
        for (int i = 0; i < simonSequence.Count; i++)
        {
            int wireIndex = simonSequence[i];
            HighlightWire(wireIndex);
            yield return new WaitForSeconds(lightDuration);
            UnhighlightWire(wireIndex);
            yield return new WaitForSeconds(pauseDuration);
        }
    }
    void HighlightWire(int index)
    {
        // Add something to color the button
    }
    void UnhighlightWire(int index)
    {
        // Add something to reset the button color
    }
    void CutWire(int wireIndex)
    {
        if (!playerTurn)
        {
            feedbackText.text = "ERROR: DISARMING SEQUENCE IN PROGRESS";
            return;
        }
        if (cutOrder.Contains(wireIndex))
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
                playerTurn = false;
                sequenceLength++;
                StartCoroutine(StartNewRound());
            }
        }
        else
        {
            feedbackText.text = $"Wrong wire!";
            playerTurn = false;
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