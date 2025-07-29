using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class SimonSaysButton : MonoBehaviour
{
    public GameObject glowEffect;
    private int buttonID;
    private Action<int> callback;
    public void Setup(int id, Action<int> onPressed)
    {
        buttonID = id;
        callback = onPressed;
        GetComponent<Button>().onClick.AddListener(SimonSays);
    }
    public void SimonSays()
    {
        StartCoroutine(SimonSaysButtonClicked());
    }
    private IEnumerator SimonSaysButtonClicked()
    {
        glowEffect?.SetActive(true);
        GetComponent<Button>().interactable = false;
        yield return new WaitForSeconds(0.5f);
        glowEffect?.SetActive(false);
        callback?.Invoke(buttonID); // reports the button press
    }
}