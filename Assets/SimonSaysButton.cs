using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class SimonSaysButton : MonoBehaviour
{
    public Image ToBeTinted;
    public GameObject glowEffect;
    private int buttonID;
    private Action<int> callback;
    public void Setup(int id, Action<int> onPressed)
    {
        buttonID = id;
        callback = onPressed;
        // GetComponent<Button>().onClick.AddListener(SimonSays);
        // I AM SO STUPID I HAVE MADE THIS MISTAKE BEFORE. ANIKA STOP ASSIGNING THE BUTTONS WHILE SIMULTANEOUSLY USING ADDLISTENER
    }
    public void SimonSays()
    {
        StartCoroutine(SimonSaysButtonClicked());
    }
    private IEnumerator SimonSaysButtonClicked()
    {
        Debug.Log($"SimonSaysButtonClicked on ID: {buttonID} — object name: {gameObject.name}");
        glowEffect?.SetActive(true);
        // GetComponent<Button>().interactable = false;
        yield return new WaitForSeconds(0.5f);
        glowEffect?.SetActive(false);
        // GetComponent<Button>().interactable = true;
        callback?.Invoke(buttonID);
    }
    public Color GetColor()
    {
        return ToBeTinted.color;
    }
}