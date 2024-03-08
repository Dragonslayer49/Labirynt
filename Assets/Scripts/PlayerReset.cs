using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerReset : MonoBehaviour
{
    // Sprawdza tag gracza
    public string playerTag = "Player";
    public int Count { get; set; } = 0;
    Counter countingscript;
    void Start()
    {
        countingscript = GameObject.FindGameObjectWithTag("Counter").GetComponent<Counter>();
    }
    void OnCollisionEnter(Collision collision)
    {
        // Sprawdza czy kolizja ma tag gracza
        if (collision.gameObject.CompareTag(playerTag))
        {
            // Resetuje gracza do poczatku labiryntu
            collision.gameObject.transform.position = Vector3.zero;
            countingscript.Counting();

            Debug.Log("Player Reset to (0, 0, 0)");
        }
    }
    
}
