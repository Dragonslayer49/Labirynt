using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionManager : MonoBehaviour
{
    public static CollisionManager Instance { get; private set; }

    //biblioteka na kolizje
    private Dictionary<string, int> collisionData;

    // obiekty z poprzedniej sceny
    private List<GameObject> previousSceneObjects;

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
            collisionData = new Dictionary<string, int>(); 
            previousSceneObjects = new List<GameObject>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    //wylacza obiekty z poprzedniej sceny dodaje obiekty z aktualnej sceny
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        DisablePreviousSceneObjects();

        previousSceneObjects.Clear();

        AddCurrentSceneObjects();
    }

    //wylacza obiekty
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

    //dodaje obiekty jako dzieci 
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

    //sprawdza tag
    private bool ShouldTrackObject(GameObject obj)
    {

        return obj.CompareTag("Trackable");
    }

    // kolizja dodaje info do biblioteki
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


    // zwraca wszystkie kolizje
    public Dictionary<string, int> GetAllCollisionData()
    {
        return new Dictionary<string, int>(collisionData); // Return a copy to avoid external modifications
    }
}