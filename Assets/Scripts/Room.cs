using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Status")]
    public bool isStartRoom;

    [Header("Doors")]
    public GameObject DoorD;
    public GameObject DoorU;
    public GameObject DoorL;
    public GameObject DoorR;

    [Header("Korytarze")]
    public GameObject KorytarzD;
    public GameObject KorytarzU;
    public GameObject KorytarzL;
    public GameObject KorytarzR;

    public GameObject[] FindObjectsWithTag(Transform parent, string tag)
    {
        List<GameObject> taggedGameObjects = new List<GameObject>();

        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.CompareTag(tag))
                taggedGameObjects.Add(child.gameObject);
            if (child.childCount > 0)
                taggedGameObjects.AddRange(FindObjectsWithTag(child, tag));
        }

        return taggedGameObjects.ToArray();
    }
}