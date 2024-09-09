using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionManager : MonoBehaviour
{
    // Singleton instance
    public static CollisionManager Instance { get; private set; }

    // Dictionary to store object names and their collision counts
    private Dictionary<string, int> collisionData;

    // List to keep track of objects from the previous scenes
    private List<GameObject> previousSceneObjects;

    private void Awake()
    {
        // Ensure only one instance exists across all scenes (Singleton pattern)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist the manager across all scenes
            collisionData = new Dictionary<string, int>(); // Initialize the dictionary
            previousSceneObjects = new List<GameObject>(); // Initialize the list for previous scene objects
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates if they exist
        }
    }

    private void OnEnable()
    {
        // Register callback for scene loading
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unregister callback for scene loading
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Handle objects from the previous scene
        DisablePreviousSceneObjects();

        // Clear the list of previous scene objects
        previousSceneObjects.Clear();

        // Add new objects from the current scene to the CollisionManager
        AddCurrentSceneObjects();
    }

    // Disable all objects from the previous scene
    private void DisablePreviousSceneObjects()
    {
        for (int i = previousSceneObjects.Count - 1; i >= 0; i--)
        {
            if (previousSceneObjects[i] != null)
            {
                previousSceneObjects[i].SetActive(false);
            }
            else
            {
                previousSceneObjects.RemoveAt(i);
            }
        }
    }

    // Add all objects in the current scene as children of the CollisionManager
    private void AddCurrentSceneObjects()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Trackable"))
        {
            obj.SetActive(true);
            if (!previousSceneObjects.Contains(obj))
            {
                previousSceneObjects.Add(obj);
            }
        }
    }

    // Determine if an object should be tracked
    private bool ShouldTrackObject(GameObject obj)
    {
        // Example logic: track objects with a specific tag or component
        // Modify this condition based on your needs
        return obj.CompareTag("Trackable");
    }

    // Call this method to register a collision with a specific object
    public void RegisterCollision(string objectName)
    {
        if (collisionData.ContainsKey(objectName))
        {
            collisionData[objectName]++;
        }
        else
        {
            collisionData[objectName] = 1;
        }
        Debug.Log($"{objectName} has collided {collisionData[objectName]} times with Player.");
    }

    // Get collision count for a specific object by its name
    public int GetCollisionCount(string objectName)
    {
        if (collisionData.ContainsKey(objectName))
        {
            return collisionData[objectName];
        }
        return 0;
    }

    // Get all collision data as a dictionary (for displaying later)
    public Dictionary<string, int> GetAllCollisionData()
    {
        return new Dictionary<string, int>(collisionData); // Return a copy to avoid external modifications
    }
}