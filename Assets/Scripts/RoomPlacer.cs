using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomsPlacer : MonoBehaviour
{
    public static RoomsPlacer Instance;

    [Header("Prefabs")]
    public List<Room> RoomPrefabs;

    [Header("Size")]
    [Tooltip("Ilosc pokoji")]
    public int roomsAmount;
    [Tooltip("Rozmiar:")]
    public int roomSize = 12;

    public Room[,] spawnedRooms;

    private void Start()
    {
        Instance = this;

        spawnedRooms = new Room[roomsAmount, roomsAmount];

        // Wyszukuje pierwszy pokoj
        var initialRoom = RoomPrefabs.Find(x => x.isStartRoom == true);
        //jesli nie ma pierwszego pokoju wstawia pokoje
        if (initialRoom != null)
        {
            spawnedRooms[0, 0] = Instantiate(initialRoom);


            for (int i = 0; i < roomsAmount; i++)
                PlaceOneRoom();

            Room[] startRooms = FindObjectsOfType<Room>().Where(x => x.isStartRoom == true).ToArray();
        }
        else
        {
            Debug.LogWarning("Nie ma pokoju");
        }
    }

    //Funkcja wstawia pokoj
    private void PlaceOneRoom()
    {
        //Wolne miejsce na pokoje
        HashSet<Vector2Int> vacantPlaces = new HashSet<Vector2Int>();
        //Sprawdza x wszystkich pokoji
        for (int x = 0; x < spawnedRooms.GetLength(0); x++)
        {
            //sprawdza y wszystkich pokoji
            for (int y = 0; y < spawnedRooms.GetLength(1); y++)
            {
                //jesli nie ma w danym miejscu pokoju
                if (spawnedRooms[x, y] == null) continue;

                int maxX = spawnedRooms.GetLength(0) - 1;
                int maxY = spawnedRooms.GetLength(1) - 1;

                if (x > 0 && spawnedRooms[x - 1, y] == null)
                    vacantPlaces.Add(new Vector2Int(x - 1, y));
                if (y > 0 && spawnedRooms[x, y - 1] == null)
                    vacantPlaces.Add(new Vector2Int(x, y - 1));
                if (x < maxX && spawnedRooms[x + 1, y] == null)
                    vacantPlaces.Add(new Vector2Int(x + 1, y));
                if (y < maxY && spawnedRooms[x, y + 1] == null)
                    vacantPlaces.Add(new Vector2Int(x, y + 1));
            }
        }
        //Dodajemy pokoj
        Room newRoom;
        newRoom = Instantiate(RoomPrefabs[Random.Range(0, RoomPrefabs.Count)]);
        
        //wstawiamy pokoj w wczesniej dodane wolne miejsce
        int limit = 500;
        while (limit-- > 0)
        {
            Vector2Int position = vacantPlaces.ElementAt(Random.Range(0, vacantPlaces.Count));

            if (ConnectToSomething(newRoom, position))
            {
                newRoom.transform.position = new Vector3(position.x, 0, position.y) * roomSize;
                spawnedRooms[position.x, position.y] = newRoom;

                return;
            }
        }

        Destroy(newRoom.gameObject);
    }

    //Funkcja sprawdza pokoje obok i wylacza drzwi zeby robic przejscia
    private bool ConnectToSomething(Room room, Vector2Int p)
    {
        int maxX = spawnedRooms.GetLength(0) - 1;
        int maxY = spawnedRooms.GetLength(1) - 1;

        List<Vector2Int> neighbours = new List<Vector2Int>();

        if (room.DoorU != null && p.y < maxY && spawnedRooms[p.x, p.y + 1]?.DoorD != null)
            neighbours.Add(Vector2Int.up);
        if (room.DoorD != null && p.y > 0 && spawnedRooms[p.x, p.y - 1]?.DoorU != null)
            neighbours.Add(Vector2Int.down);
        if (room.DoorR != null && p.x < maxX && spawnedRooms[p.x + 1, p.y]?.DoorL != null)
            neighbours.Add(Vector2Int.right);
        if (room.DoorL != null && p.x > 0 && spawnedRooms[p.x - 1, p.y]?.DoorR != null)
            neighbours.Add(Vector2Int.left);

        if (neighbours.Count == 0)
            return false;

        Vector2Int selectedDirection = neighbours[Random.Range(0, neighbours.Count)];
        Room selectedRoom = spawnedRooms[p.x + selectedDirection.x, p.y + selectedDirection.y];

        if (selectedDirection == Vector2Int.up)
        {
            room.DoorU.SetActive(false);
            selectedRoom.DoorD.SetActive(false);
        }
        else if (selectedDirection == Vector2Int.down)
        {
            room.DoorD.SetActive(false);
            selectedRoom.DoorU.SetActive(false);
        }
        else if (selectedDirection == Vector2Int.right)
        {
            room.DoorR.SetActive(false);
            selectedRoom.DoorL.SetActive(false);
        }
        else if (selectedDirection == Vector2Int.left)
        {
            room.DoorL.SetActive(false);
            selectedRoom.DoorR.SetActive(false);
        }

        if (room.DoorU != null && selectedDirection != Vector2Int.up)
            room.DoorU.SetActive(true);

        if (room.DoorD != null && selectedDirection != Vector2Int.down)
            room.DoorD.SetActive(true);

        if (room.DoorR != null && selectedDirection != Vector2Int.right)
            room.DoorR.SetActive(true);

        if (room.DoorL != null && selectedDirection != Vector2Int.left)
            room.DoorL.SetActive(true);

        return true;
    }
}
