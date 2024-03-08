using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Counter : MonoBehaviour
{
    public int count;
    public TextMeshProUGUI counterText;
    public void Counting()
    {
        count += 1;
        if (counterText != null)
        {
            counterText.text = count.ToString();
        }
        else
        {
            Debug.LogError("nie ma counterText");
        }
    }

}
