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

    private bool starterRoomPlaced = false; // Flag to ensure the starter room is only placed once

    private void Start()
    {
        Instance = this;

        spawnedRooms = new Room[roomsAmount, roomsAmount];

        // Wyszukuje pierwszy pokoj
        var initialRoom = RoomPrefabs.Find(x => x.isStartRoom == true);

        // Jeœli nie ma pierwszego pokoju, ostrze¿enie
        if (initialRoom != null)
        {
            spawnedRooms[0, 0] = Instantiate(initialRoom);
            starterRoomPlaced = true; // Mark that the starter room has been placed
            initialRoom.transform.position = Vector3.zero;

            for (int i = 1; i < roomsAmount; i++) // Start loop from 1 as the first room is already placed
            {
                PlaceOneRoom();
            }

            // After all rooms are placed, delete unconnected korytarze
            DeleteUnconnectedKorytarze();
        }
        else
        {
            Debug.LogWarning("Nie ma pokoju startowego");
        }
    }

    // Funkcja wstawia pokoj
    private void PlaceOneRoom()
    {
        // Wolne miejsce na pokoje
        HashSet<Vector2Int> vacantPlaces = new HashSet<Vector2Int>();

        // Sprawdza x wszystkich pokoji
        for (int x = 0; x < spawnedRooms.GetLength(0); x++)
        {
            // Sprawdza y wszystkich pokoji
            for (int y = 0; y < spawnedRooms.GetLength(1); y++)
            {
                // Jeœli nie ma w danym miejscu pokoju
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

        // Filtruj prefaby pokojów, aby nie wybraæ pokoju startowego po jego pocz¹tkowym ustawieniu
        List<Room> availableRooms = RoomPrefabs;
        if (starterRoomPlaced)
        {
            availableRooms = RoomPrefabs.Where(x => !x.isStartRoom).ToList();
        }

        if (availableRooms.Count == 0) return;

        // Dodajemy pokoj
        Room newRoom = Instantiate(availableRooms[Random.Range(0, availableRooms.Count)]);

        // Wstawiamy pokoj w wczeœniej dodane wolne miejsce
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

    // Funkcja sprawdza pokoje obok i wy³¹cza drzwi, ¿eby robiæ przejœcia
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

    // Funkcja usuwa korytarze, które nie prowadz¹ do s¹siedniego pokoju lub gdzie drzwi nie s¹ usuniête
    private void DeleteUnconnectedKorytarze()
    {
        int maxX = spawnedRooms.GetLength(0) - 1;
        int maxY = spawnedRooms.GetLength(1) - 1;

        for (int x = 0; x < spawnedRooms.GetLength(0); x++)
        {
            for (int y = 0; y < spawnedRooms.GetLength(1); y++)
            {
                Room room = spawnedRooms[x, y];
                if (room == null) continue;

                // Check each direction and disable Korytarze if they do not connect to another room or if the door is not deleted
                if (room.KorytarzU != null && (y >= maxY || spawnedRooms[x, y + 1] == null || room.DoorU.activeSelf))
                    room.KorytarzU.SetActive(false);

                if (room.KorytarzD != null && (y <= 0 || spawnedRooms[x, y - 1] == null || room.DoorD.activeSelf))
                    room.KorytarzD.SetActive(false);

                if (room.KorytarzR != null && (x >= maxX || spawnedRooms[x + 1, y] == null || room.DoorR.activeSelf))
                    room.KorytarzR.SetActive(false);

                if (room.KorytarzL != null && (x <= 0 || spawnedRooms[x - 1, y] == null || room.DoorL.activeSelf))
                    room.KorytarzL.SetActive(false);
            }
        }
    }
}