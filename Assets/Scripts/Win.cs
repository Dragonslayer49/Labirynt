using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Win : MonoBehaviour
{
    int Counter = 1;
    public string playerTag = "Player";
    [SerializeField] GameObject winScreen;
    [SerializeField] private TextMeshProUGUI winText;
    //public TextMeshProUGUI counterText;
    void OnCollisionEnter(Collision collision)
    {
        // Sprawdza czy kolizja ma tag gracza
        if (collision.gameObject.CompareTag(playerTag))
        {
            win();

            Debug.Log("Player won");
        }
    }
    private void win()
    {
        winScreen.SetActive(true);
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        //counterText.text = "udalo ci sie przejsc labirynt w " + Counter.ToString();
        // Display collision data on win screen
        UpdateWinText();
    }

    private void UpdateWinText()
    {
        if (winText != null)
        {
            // Get collision data from CollisionManager
            var collisionData = CollisionManager.Instance.GetAllCollisionData();

            winText.text = "Collisions:\n";

            foreach (var entry in collisionData)
            {
                winText.text += $"{entry.Key}: {entry.Value}\n";
            }
        }
    }
}