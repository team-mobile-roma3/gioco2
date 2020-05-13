using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Runtime.CompilerServices;

public class RoomInfo
{
    public string name;
    public int X;
    public int Y;
}

public class RoomController : MonoBehaviour
{

    public static RoomController instance;


    string currentWorldName = "Basement";

    public List<int> nBoss;
    RoomInfo currentLoadRoomData;

    Room currRoom;

    Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>();

    public List<Room> loadedRooms = new List<Room>();

    bool isLoadingRoom = false;
    bool spawnedBossRoom = false;
    bool updatedRooms = false;

    void Awake()
    {
    
            instance = this;
      
    }
        void Start()
    {
    //   LoadRoom("Start2", 10, 0);
        //LoadRoom("Empty", 1, 0);
        //LoadRoom("Empty", -1, 0);
        //LoadRoom("Empty", 0, 1);
        //LoadRoom("Empty", 0, -1);
    }

    void Update()
    {
        UpdateRoomQueue();
    }

    void UpdateRoomQueue()
    {
        if(isLoadingRoom)
        {
            return;
        }

    if(loadRoomQueue.Count == 0 )

        { 
            if(!spawnedBossRoom)
            {
               
                StartCoroutine(SpawnBossRoom());
            } 
            else if(spawnedBossRoom && !updatedRooms)
            {
                foreach(Room room in loadedRooms)
                {
                    room.RemoveUnconnectedDoors();
                }
                UpdateRooms();
                updatedRooms = true;
            }
            return;
        }

        currentLoadRoomData = loadRoomQueue.Dequeue();
        isLoadingRoom = true;

        StartCoroutine(LoadRoomRoutine(currentLoadRoomData));
    }

    IEnumerator SpawnBossRoom()
    {
        spawnedBossRoom = true;
        yield return new WaitForSeconds(0.5f);
        if(loadRoomQueue.Count == 0)
        {
            Debug.Log(nBoss[2] + " "  + nBoss[1]);
            for (int i = 1; i <= 4;  i ++) {
                Room bossRoom = loadedRooms[nBoss[i] - i];
                Room tempRoom = new Room(bossRoom.X, bossRoom.Y);
                Destroy(bossRoom.gameObject);
                var roomToRemove = loadedRooms.Single(r => r.X == tempRoom.X && r.Y == tempRoom.Y);
                loadedRooms.Remove(roomToRemove);
                LoadRoom("End", tempRoom.X, tempRoom.Y);
            }
        }
    }

    public int  LoadRoom( string name, int x, int y)
    {   if ( name == "Start2")
        Debug.Log("sto istanziando " + name);
        if(DoesRoomExist(x, y) )
        {   
                
            return 0 ;
        }

        RoomInfo newRoomData = new RoomInfo();
        newRoomData.name = name;
        newRoomData.X = x;
        newRoomData.Y = y;

        loadRoomQueue.Enqueue(newRoomData);
        return 1; 
    }

    IEnumerator LoadRoomRoutine(RoomInfo info)
    {
        string roomName = currentWorldName + info.name;

        AsyncOperation loadRoom = SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive);

        while(loadRoom.isDone == false)
        {
            yield return null;
        }
    }

    public void RegisterRoom( Room room)
    {
        if (!DoesRoomExist(currentLoadRoomData.X, currentLoadRoomData.Y) || (room.X == 10 && room.Y == 0))
        {
            room.transform.position = new Vector3(
                currentLoadRoomData.X * room.Width,
                currentLoadRoomData.Y * room.Height,
                0
            );

            room.X = currentLoadRoomData.X;
            room.Y = currentLoadRoomData.Y;
            room.name = currentWorldName + "-" + currentLoadRoomData.name + " " + room.X + ", " + room.Y;
            room.transform.parent = transform;

            isLoadingRoom = false;

            if (loadedRooms.Count == 0)
            {
                CameraController.instance.currRoom = room;
            }
            if (currentLoadRoomData.name.Contains("Start"))
                nBoss.Add(loadedRooms.Count);
                loadedRooms.Add(room);
                
        }
        else
        {
            Debug.Log("esisteva " + currentLoadRoomData.name);
            Destroy(room.gameObject);
            isLoadingRoom = false;
        }

    }

    public bool DoesRoomExist( int x, int y)
    {
        return loadedRooms.Find( item => item.X == x && item.Y == y) != null;
    }

    public Room FindRoom( int x, int y)
    {
        return loadedRooms.Find( item => item.X == x && item.Y == y);
    }

    public string GetRandomRoomName()
    {
        string[] possibleRooms = new string[] {
            "Empty",
            "Basic1"
        };

        return possibleRooms[Random.Range(0, possibleRooms.Length)];
    }

    public void OnPlayerEnterRoom(Room room)
    {
        CameraController.instance.currRoom = room;
        currRoom = room;

        StartCoroutine(RoomCoroutine());
    }

    public IEnumerator RoomCoroutine()
    {
        yield return new WaitForSeconds(0.2f);
        UpdateRooms();
    }

    public void UpdateRooms()
    {
        foreach(Room room in loadedRooms)
        {
            if(currRoom != room )
            {
                EnemyController[] enemies = room.GetComponentsInChildren<EnemyController>();
                if(enemies != null)
                {
                    foreach(EnemyController enemy in enemies)
                    {
                        enemy.notInRoom = true;
                        Debug.Log("Not in room");
                    }

                    foreach(Door door in room.GetComponentsInChildren<Door>())
                    {
                        if (!door.getNotConnected())
                            door.doorCollider.SetActive(false);
                    }
                }
                else
                {
                    foreach(Door door in room.GetComponentsInChildren<Door>())
                    {
                        if (!door.getNotConnected())
                            door.doorCollider.SetActive(false);
                    }
                }
            }
            else
            {
              EnemyController[] enemies = room.GetComponentsInChildren<EnemyController>();
                if(enemies.Length > 0)
                {
                    foreach(EnemyController enemy in enemies)
                    {                       
                        enemy.notInRoom = false;
                        Debug.Log("In room");
                    }
                    
                    foreach(Door door in room.GetComponentsInChildren<Door>())
                    {
                        door.doorCollider.SetActive(true);
                    }
                }
                else
                {
                    foreach(Door door in room.GetComponentsInChildren<Door>())
                    {   if(!door.getNotConnected())
                         door.doorCollider.SetActive(false);
                    }
                }  
            }
        }
    }
}
