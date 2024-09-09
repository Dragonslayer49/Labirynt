using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangePositionAndText : MonoBehaviour
{
    public TMP_Text Instructions;
    public TMP_Text pressEnter;
    public GameObject Panel;
    public GameObject door;
    public GameObject doorGood;
    public Vector3 newPosition;    
    public string[] newText;
    public int numer = 0;
    public bool routineEnd = true;
    public float duration = 5f;
    public float fadeDuration = 1f; // Duration for the fade-in effect

    void Start()
    {
        Panel.SetActive(false);
            pressEnter.alpha = 0; // Ensure "Press Enter" is invisible at the start
        StartCoroutine(TestRoutine(duration));

    }

void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && routineEnd && numer<4)
        {
            Instructions.transform.position = newPosition;
            Instructions.text = newText[numer];
            Debug.Log(newText[numer]);
            if (numer == 3)
            {
                Destroy(door);
            }
            StartCoroutine(TestRoutine(duration));
            numer++;
        }
    }

    IEnumerator TestRoutine(float duration)
    {
        pressEnter.alpha = 0;
        routineEnd = false;
        Debug.Log($"Started at {Time.time}, waiting for {duration} seconds");
        yield return new WaitForSeconds(duration);
        Debug.Log($"Ended at {Time.time}");

        if (numer == 6|| numer == 4)
        {
            Instructions.text = "";
            pressEnter.text = "";
            Debug.Log(Instructions.text);
        }
            StartCoroutine(FadeInPressEnter(fadeDuration));
        
        
        
    }

    IEnumerator FadeInPressEnter(float duration)
    {
        float elapsedTime = 0f;
        Color originalColor = pressEnter.color;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / duration);
            pressEnter.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }
        routineEnd = true;
    }
    public void AfterBad()
    {
        Destroy(doorGood);
        StartCoroutine(TestRoutine(duration));
        Debug.Log("ye");
        Instructions.text = "Wrong way, after every wrong way you will be teleported to beggining location. Try to remember and avoid which paths are wrong";
        StartCoroutine(TestRoutine(duration));
    }
}