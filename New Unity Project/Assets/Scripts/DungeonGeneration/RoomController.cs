using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Runtime.CompilerServices;
using System;

public class RoomInfo
{
    public string name;
    public int X;
    public int Y;
}

public class RoomController : MonoBehaviour
{
    public DungeonGenerationData dungeonGenerationData;
    public static RoomController instance;
   


    string currentWorldName = "Basement";

    public List<int> nBoss;
    RoomInfo currentLoadRoomData;

      public Room currRoom;

    Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>();

    public List<Room> loadedRooms = new List<Room>();

    bool isLoadingRoom = false;
    bool spawnedBossRoom = false;
    bool updatedRooms = false;
    public Boolean dungeonLoaded = false;

    void Awake()
    {
    
            instance = this;
      
    }
    void Start()
    {
        GameObject.Find("Canvas").transform.GetChild(0).gameObject.SetActive(true);
        GameObject.Find("Canvas").transform.GetChild(1).gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.GetChild(2).gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.GetChild(3).gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.GetChild(4).gameObject.SetActive(false);

        StartCoroutine(Wait());
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
 //           Debug.Log(nBoss[0] + " "  + nBoss[1] + nBoss[2] + " " + nBoss[3]+ " " + nBoss[4]);
            for (int i = 1; i <= dungeonGenerationData.livelli-1;  i ++) {  // for per i livelli-1 per spwnare i boss
                Room bossRoom = loadedRooms[nBoss[i] -i];
                Room tempRoom = new Room(bossRoom.X, bossRoom.Y);
                Destroy(bossRoom.gameObject);
                var roomToRemove = loadedRooms.Single(r => r.X == tempRoom.X && r.Y == tempRoom.Y);
                loadedRooms.Remove(roomToRemove);
                LoadRoom("End"+i, tempRoom.X, tempRoom.Y);
            }

            // ora spawno ultimo boss, non posso farlo prima perche non ho l'ultimo start
            Room lastbossRoom = loadedRooms[loadedRooms.Count - 1];
            Room lasttempRoom = new Room(lastbossRoom.X, lastbossRoom.Y);
            Destroy(lastbossRoom.gameObject);
            var lastroomToRemove = loadedRooms.Single(r => r.X == lastbossRoom.X && r.Y == lastbossRoom.Y);
            loadedRooms.Remove(lastroomToRemove);
            LoadRoom("End" + dungeonGenerationData.livelli, lastbossRoom.X, lastbossRoom.Y);
        }
        LoadRoom("Win", 50, 0);
        LoadRoom("Blacksmith", 0, 20);
        dungeonLoaded = true;
    }

    public int  LoadRoom( string name, int x, int y)
    { 
        if(DoesRoomExist(x, y))       
            return 0 ;

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

    public string GetRandomRoomName(int level )
    {
        string[] possibleRooms1 = new string[] {
            "Empty",
            "Basic1",
            "Basic1-2"
        };

        string[] possibleRooms2 = new string[] {
            "Empty",
            "Basic1",
            "Basic1-2",
             "Basic2",
             "Basic2-2"
        };
        string[] possibleRooms3 = new string[] {
              "Empty",
            "Basic1",
            "Basic1-2",
             "Basic2",
             "Basic2-2",
             "Basic3"
           
        };
        string[] possibleRooms4 = new string[] {
               "Empty",
            "Basic1",
            "Basic1-2",
             "Basic2",
             "Basic2-2",
             "Basic3",
             "Basic4"
             
        };
        string[] possibleRooms5 = new string[] {
            "Empty",
            "Basic1",
            "Basic1-2",
             "Basic2",
             "Basic2-2",
             "Basic3",
             "Basic4",
             "Basic5"
        };
        if(level == 1)
        return possibleRooms1[UnityEngine.Random.Range(0, possibleRooms1.Length)];

        if (level == 2)
            return possibleRooms2[UnityEngine.Random.Range(0, possibleRooms2.Length)];
        if (level == 3)
            return possibleRooms3[UnityEngine.Random.Range(0, possibleRooms3.Length)];
        if (level == 4)
            return possibleRooms4[UnityEngine.Random.Range(0, possibleRooms4.Length)];
        if (level == 5)
            return possibleRooms5[UnityEngine.Random.Range(0, possibleRooms5.Length)];

        return "";
        
    }

    public void OnPlayerEnterRoom(Room room)
    {
        CameraController.instance.currRoom = room;
        currRoom = room;
        StartCoroutine(Wait1());
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
    //                    Debug.Log("Not in room");
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
 //                       Debug.Log("In room");
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

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(7f);
        GameObject.Find("Canvas").transform.GetChild(0).gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.GetChild(1).gameObject.SetActive(true);
        GameObject.Find("Canvas").transform.GetChild(2).gameObject.SetActive(true);
        GameObject.Find("Canvas").transform.GetChild(3).gameObject.SetActive(true);
        GameObject.Find("Canvas").transform.GetChild(4).gameObject.SetActive(true);
    }
    IEnumerator Wait1()
    {
        Debug.Log("inv");
        GameObject.Find("Player").GetComponent<PlayerController>().MakeFlash();
        GameController.Invulerability = true;
        yield return new WaitForSeconds(1f);
        GameController.Invulerability = false;

    }
}
