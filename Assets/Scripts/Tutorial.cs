using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangePositionAndText : MonoBehaviour
{
    public TMP_Text Instructions;
    public TMP_Text pressEnter; 
    public GameObject door;
    public GameObject doorBad;
    public GameObject doorGood;
    public Vector3 newPosition;    
    public string[] newText;
    public int numer = 0;
    public bool routineEnd = true;
    public float duration = 5f;
    public float fadeDuration = 1f; // Duration for the fade-in effect

    void Start()
    { 

            pressEnter.alpha = 0; // Ensure "Press Enter" is invisible at the start
        

        }

void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && routineEnd)
        {
            Instructions.transform.position = newPosition;
            Instructions.text = newText[numer];
            Debug.Log(newText[numer]);
            numer++;
            switch (numer)
            {
                case 4:
                    Destroy(door);
                    Destroy(doorBad);
                    StartCoroutine(TestRoutine(duration));
                    break;
                case 5:
                    Destroy(doorGood);
                    StartCoroutine(TestRoutine(duration));
                    //to bylo zle przejscie z kazdym zlym przejsciem naliczane sa punkty probuj nie wchodzic w to samo zle przejscie dwa razy
                    //robi enter przenosi go na koniec i pisze z kazdym zlym przejsciem zostaniesz przeniesiony na poczatek labiryntu zeby sprobowac ponowanie
                    //teraz sprobuj przejsc labirynt bez wchodzenia w zle przejscia
                    break;
            }

            StartCoroutine(TestRoutine(duration));
        }
    }

    IEnumerator TestRoutine(float duration)
    {
        pressEnter.alpha = 0;
        routineEnd = false;
        Debug.Log($"Started at {Time.time}, waiting for {duration} seconds");
        yield return new WaitForSeconds(duration);
        Debug.Log($"Ended at {Time.time}");

        if (numer == 4)
        {
            Instructions.text = "";
            Debug.Log(Instructions.text);
        }

        routineEnd = true;
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
    }
}