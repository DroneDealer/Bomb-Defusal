using UnityEngine;

public class BombCode : MonoBehaviour
{
    private string bombCode;
    private void Awake()
    {
        bombCode = BombCombo();
        Debug.Log("Bomb code generated: " + bombCode);
    }
    public string BombCombo()
    {
        int[] digits = new int[4];
        for (int i = 0; i < digits.Length; i++)
        {
            digits[i] = Random.Range(0, 10);
        }
        return string.Join("", digits);
    }
    public string GetCode() 
    //Need this so that I can access the bomb code from BombCodeInput. I could have made BombCombo public, but this is cleaner + safer
    {
        return bombCode;
    }
}