using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorial : MonoBehaviour
{
    public TMP_Text Instructions;
    public TMP_Text pressEnter;
    public TMP_Text BadWay;
    public GameObject Panel;
    public GameObject door;
    public GameObject doorGood;
    public Vector3 newPosition;
    private Rigidbody playerRigidbody;
    public string[] newText;
    public int numer = 0;
    public bool routineEnd = true;
    public float duration = 5f;
    public float fadeDuration = 1f;

    void Start()
    {
        Panel.SetActive(false);
        BadWay.alpha = 0;
        pressEnter.alpha = 0;
        StartCoroutine(TestRoutine(duration));

    }

void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && routineEnd && numer<4)
        {
            Vector3 halfScreenPosition = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);

            Vector3 canvasPosition;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(Instructions.rectTransform, halfScreenPosition, Camera.main, out canvasPosition);

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

        if ( numer == 4)
        {
            Instructions.text = "";
            pressEnter.text = "";
            Debug.Log(Instructions.text);
        }
            StartCoroutine(FadeInPressEnter(fadeDuration,pressEnter));
        
        
        
    }

    IEnumerator WrongWay(float duration)
    {
        Debug.Log($"Started at {Time.time}, waiting for {duration} seconds");
        yield return new WaitForSeconds(duration);
        Debug.Log("test way");
    }

    IEnumerator FadeInPressEnter(float duration, TMP_Text fade)
    {
        float elapsedTime = 0f;
        Color originalColor = fade.color;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / duration);
            fade.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }
        routineEnd = true;
    }

    public void AfterBad()
    {
        
        BadWay.alpha = 1;
        Panel.SetActive(true);
        Destroy(doorGood);
        Instructions.text = "Po ka¿dym z³ym przejœciu zostaniesz przeniesiony na pocz¹tek labiryntu";
        StartCoroutine(FadeInPressEnter(0.5f, Instructions));
        StartCoroutine(SequenceAfterBad());
    }

    IEnumerator SequenceAfterBad()
    {
        yield return StartCoroutine(WrongWay(6f));
        Instructions.text = "Spróbuj zapamiêtaæ z³e przejœcia i przejœc labirynt z minimaln¹ iloœci¹ b³êdnych przejœæ";
        yield return StartCoroutine(FadeInPressEnter(1f, Instructions));
        yield return StartCoroutine(WrongWay(6f));
        Panel.SetActive(false);
        Instructions.text = "";
        BadWay.alpha = 0;
    }
}
