using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReset : MonoBehaviour
{
    // Sprawdza tag gracza
    public string playerTag = "Player";
    public Vector3 resetPos;
    public ChangePositionAndText changePositionAndTextScript;
    public CollisionManager collisionManager;

    void Start()
    {
        // Ensure the ChangePositionAndText script is set
        if (changePositionAndTextScript == null)
        {
            changePositionAndTextScript = FindObjectOfType<ChangePositionAndText>();
        }

        // Ensure the CollisionManager is set
        collisionManager = CollisionManager.Instance;
        if (collisionManager == null)
        {
            Debug.LogError("CollisionManager instance not found! Ensure CollisionManager is present in the scene.");
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        // Sprawdza czy kolizja ma tag gracza
        if (collision.gameObject.CompareTag(playerTag))
        {
            // Resetuje gracza do poczatku labiryntu
            collision.gameObject.transform.position = resetPos;

            // Register the collision with the CollisionManager
            if (collisionManager != null)
            {
                collisionManager.RegisterCollision(gameObject.name);
            }
            else
            {
                Debug.LogWarning("CollisionManager instance is null when trying to register collision.");
            }

            // Trigger the ChangePositionAndText script
            if (changePositionAndTextScript != null)
            {
                changePositionAndTextScript.AfterBad();
                Debug.Log("Numer increased to " + changePositionAndTextScript.numer);
            }

            Debug.Log("Player Reset to " + resetPos);
        }
    }
}