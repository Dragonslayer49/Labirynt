using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCounter : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> objectsToTrack;

    private Dictionary<GameObject, int> collisionCounts;

    private void Start()
    {
        collisionCounts = new Dictionary<GameObject, int>();

        foreach (GameObject obj in objectsToTrack)
        {
            if (obj != null)
            {
                // Initialize collision count for each object in the list
                collisionCounts[obj] = 0;


            }
        }
    }

    // This method will be called by each TrackCollision script when the object collides with the Player
    public void OnObjectCollided(GameObject collidedObject)
    {
        if (collisionCounts.ContainsKey(collidedObject))
        {
            collisionCounts[collidedObject]++;
            Debug.Log($"{collidedObject.name} has collided {collisionCounts[collidedObject]} times with Player.");
        }
    }

    // Public method to get the collision count for a specific object
    public int GetCollisionCount(GameObject obj)
    {
        if (collisionCounts.ContainsKey(obj))
        {
            return collisionCounts[obj];
        }

        return 0;
    }
}