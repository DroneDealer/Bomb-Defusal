using System.Collections.Generic;
using UnityEngine;

public class SimonSays : MonoBehaviour
{
    [SerializeField] private List<SimonSaysButton> PlayerButtons = new();

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
}