using UnityEngine;

public class BombCode : MonoBehaviour
{
    public string BombCombo()
    {
        int[] digits = new int[4];
        for (int i = 0; i < digits.Length; i++)
        {
            digits[i] = Random.Range(0, 10);
        }
        return string.Join("", digits);
    }
}
