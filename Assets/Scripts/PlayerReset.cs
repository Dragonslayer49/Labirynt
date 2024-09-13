using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReset : MonoBehaviour
{
    public string playerTag = "Player";
    public GameObject resetPanel;
    public Vector3 resetPos;
    public Tutorial Tutorial;
    public CollisionManager collisionManager;
    private Rigidbody playerRigidbody;
    void Start()
    {
        if (Tutorial == null)
        {
            Tutorial = FindObjectOfType<Tutorial>();
        }

        collisionManager = CollisionManager.Instance;
        if (collisionManager == null)
        {
            Debug.LogError("Brak collisionManagera");
        }
        if (resetPanel != null)
        {
           resetPanel.SetActive(false);
        }
        else
        {
          Debug.LogWarning("ResetPanel nie ustawiony");
        }
    }


    public void OnCollisionEnter(Collision collision)
    {
        // Sprawdza czy kolizja ma tag gracza
        if (collision.gameObject.CompareTag(playerTag))
        {
            // Resett gracza
            playerRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            collision.gameObject.transform.position = resetPos;

            StartCoroutine(FreezePlayerForReset(collision.gameObject));

            //Przesyla informacje do collisionManagera
            if (collisionManager != null)
            {
                collisionManager.RegisterCollision(gameObject.name);
            }
            else
            {
                Debug.LogWarning("CollisionManager brak");
            }

            //Tutorial
            if (Tutorial != null)
            {
                Tutorial.AfterBad();
                Debug.Log("Numer to " + Tutorial.numer);
            }

            Debug.Log("Resetuje gracza do " + resetPos);
        }
    }
    //zamraza gracza oraz wlacza mu ekran z zlym przejsciem
    private IEnumerator FreezePlayerForReset(GameObject player)
    {
        // Freeze player movement by disabling the Rigidbody
        if (playerRigidbody != null)
        {
            playerRigidbody.isKinematic = true;  // Disables physics for 3 seconds
        }

        // Show the resetPanel
        if (resetPanel != null)
        {
            resetPanel.SetActive(true);
        }

        // Wait for 3 seconds
        yield return new WaitForSeconds(3f);

        // Unfreeze player movement by enabling the Rigidbody again
        if (playerRigidbody != null)
        {
            playerRigidbody.isKinematic = false;  // Re-enables physics
        }

        // Hide the resetPanel
        if (resetPanel != null)
        {
            resetPanel.SetActive(false);
        }
    }
}