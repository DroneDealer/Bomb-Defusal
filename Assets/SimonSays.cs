using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonSays : MonoBehaviour
{
    [SerializeField] private List<SimonSaysButton> PlayerButtons = new();
    private List<int> randomSequence = new();
    private int currentStep = 0;
    void Start()
    {
        for (int i = 0; i < PlayerButtons.Count; i++)
        {
            PlayerButtons[i].Setup(i, OnButtonPressed);
        }
    }

    void OnButtonPressed(int id)
    {
        Debug.Log($"Button {id} pressed");
    }
    void GenerateRandomSequennce(int length)
    {
        randomSequence.Clear();
        for (int i = 0; i < length; i++)
        {
            int randomIndex = Random.Range(0, PlayerButtons.Count);
            randomSequence.Add(randomIndex);
        }
    }
    IEnumerator PlaySequence()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (int index in randomSequence)
        {
            Color color = PlayerButtons[index].GetColor();
        }
    }
}