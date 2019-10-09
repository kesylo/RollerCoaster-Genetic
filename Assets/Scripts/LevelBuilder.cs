using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZenFulcrum.Track;

public class LevelBuilder : MonoBehaviour
{
    public GameObject buttons;
    public Text Track;
    public Text FinalTest;
    int e=0, m=0, h=0, count = 1;
    public Room startRoomPrefab, endRoomPrefab;
    public List<Room> roomPrefabsH = new List<Room>();
    public List<Room> roomPrefabsM = new List<Room>();
    public List<Room> roomPrefabsE = new List<Room>();
    public Vector2 iterationRange = new Vector2(3, 10);
    List<Doorway> availableDoorways = new List<Doorway>();

    int choose;
    StartRoom startRoom;
    EndRoom endRoom;
    List<Room> placeRooms = new List<Room>();

    LayerMask roomLayerMask;

    void Start()
    {
        FinalTest.enabled = false;
        Track.text = "Track: " + count;
        roomLayerMask = LayerMask.GetMask("Room");
        StartCoroutine("GenerateLevel");
    }

    IEnumerator GenerateLevel()
    {
        Track.text = "Track: " + count;
        if (count >= 11)
        {
            if (e > m && e > h)
            {
                choose = 50;
            }
            else if (m > e && m > h)
            {
                choose = 100;
            }
            else if (h > e && h > m)
            {
                choose = 150;
            }
            buttons.SetActive(false);
            Track.enabled = false;
            FinalTest.enabled = true;
            NewTestController.instance.Show_Button();

            // ResetLevelGenerator();

        }
      //  WaitForSeconds startup = new WaitForSeconds(1);
        WaitForFixedUpdate interval = new WaitForFixedUpdate();

      //  yield return startup;

        //Place start room
        PlaceStartRoom();
        yield return interval;

        //Random iterations
        int iterations = Random.Range((int)iterationRange.x, (int)iterationRange.y);
        if(choose == 50 || choose == 100 || choose == 150)
        {
        }
        else if(choose != 50 || choose != 100 || choose != 150)
        {
            choose = Random.Range(1, 30);
        }
        for (int i = 0; i < iterations; i++)
        {
            //Place random room from list
            PlaceRoom();
            yield return interval;
        }

        //Place end room
        PlaceEndRoom();
        yield return interval;
        //Level generation finished
        Debug.Log("Level generation finished");
        //    yield return new WaitForSeconds(3);
        //ResetLevelGenerator();


    }

    /// <summary>
    /// custome
    /// </summary>
    private Track prevTrack = null;
    private Track nextTrack = null;
    void PlaceStartRoom()
    {
        //Instantiate room
        startRoom = Instantiate(startRoomPrefab) as StartRoom;
        startRoom.transform.parent = this.transform;

        //Get doorways from current room and add them randomly to the list of available doorways
        AddDoorwaysToList(startRoom, ref availableDoorways);

        //Position room
        startRoom.transform.position = Vector3.zero;
        startRoom.transform.rotation = Quaternion.identity;

        for(int i = 0; i < startRoom.gameObject.transform.childCount;i++)
        {
            Track track = startRoom.gameObject.transform.GetChild(i).GetComponent<Track>();
            if (prevTrack != null)
            {
                prevTrack.NextTrack = track;
                track.PrevTrack = prevTrack;
            }
            prevTrack = track;
        }
    }

    void AddDoorwaysToList(Room room, ref List<Doorway> list)
    { 
        foreach(Doorway doorway in room.doorways)
        {
            int r = Random.Range(0, list.Count);
            list.Insert(r, doorway);
        }
    }

    void PlaceRoom()
    {
        if (choose > 0 && choose < 11 || choose == 50)
        {
            //PlaceRoomE();
            PlaceRoomNew("E");
        }
        else if (choose > 10 && choose < 21 || choose == 100)
        {
            //PlaceRoomM();
            PlaceRoomNew("M");
        }
        else if (choose > 20 && choose < 31 || choose == 150)
        {
            //PlaceRoomH();
            PlaceRoomNew("H");
        }
    }
    void PlaceRoomE()
    {
        // Instantiate room
        Room currentRoom = Instantiate(roomPrefabsE[Random.Range(0, roomPrefabsE.Count)]) as Room;
        currentRoom.transform.parent = this.transform;

        for (int i = 0; i < currentRoom.gameObject.transform.childCount; i++)
        {
            Track track = currentRoom.gameObject.transform.GetChild(i).GetComponent<Track>();
            if (prevTrack != null)
            {
                prevTrack.NextTrack = track;
                track.PrevTrack = prevTrack;
            }
            prevTrack = track;
        }
        //Create doorway lists to loop over
        List<Doorway> allAvailableDoorways = new List<Doorway>(availableDoorways);
        List<Doorway> currentRoomDoorways = new List<Doorway>();
        AddDoorwaysToList(currentRoom, ref currentRoomDoorways);

        //Get doorways from current room and add then randomly to the list of available doorways
        AddDoorwaysToList(currentRoom, ref availableDoorways);

        bool roomPlaced = false;

        //Try all available doorways
        foreach (Doorway availableDoorway in allAvailableDoorways)
        {
            //Try all available doorways in current room
            foreach (Doorway currentDoorway in currentRoomDoorways)
            {
                //Position room
                PositionRoomAtDoorway(ref currentRoom, currentDoorway, availableDoorway);

                //Check room overlaps
                //if (CheckRoomOverlap(currentRoom))
                //{
                //    continue;

                //}

                roomPlaced = true;

                //Add room to list
                placeRooms.Add(currentRoom);

                //Remove occupied doorways
                currentDoorway.gameObject.SetActive(false);
                availableDoorways.Remove(currentDoorway);

                availableDoorway.gameObject.SetActive(false);
                availableDoorways.Remove(availableDoorway);

                //Exit loop if room has been placed
                break;
            }

            //Exit loop if room has been placed
            if (roomPlaced)
            {
                break;
            }
        }

        //Room couldn't be placed. Restart generator and try again
        if (!roomPlaced)
        {
            Destroy(currentRoom.gameObject);
            ResetLevelGenerator();
        }
    }
    void PlaceRoomM()
    {
        // Instantiate room
        Room currentRoom = Instantiate(roomPrefabsM[Random.Range(0, roomPrefabsM.Count)]) as Room;
        currentRoom.transform.parent = this.transform;

        for (int i = 0; i < currentRoom.gameObject.transform.childCount; i++)
        {
            Track track = currentRoom.gameObject.transform.GetChild(i).GetComponent<Track>();
            if (prevTrack != null)
            {
                prevTrack.NextTrack = track;
                track.PrevTrack = prevTrack;
            }
            prevTrack = track;
        }
        //Create doorway lists to loop over
        List<Doorway> allAvailableDoorways = new List<Doorway>(availableDoorways);
        List<Doorway> currentRoomDoorways = new List<Doorway>();
        AddDoorwaysToList(currentRoom, ref currentRoomDoorways);

        //Get doorways from current room and add then randomly to the list of available doorways
        AddDoorwaysToList(currentRoom, ref availableDoorways);

        bool roomPlaced = false;

        //Try all available doorways
        foreach (Doorway availableDoorway in allAvailableDoorways)
        {
            //Try all available doorways in current room
            foreach (Doorway currentDoorway in currentRoomDoorways)
            {
                //Position room
                PositionRoomAtDoorway(ref currentRoom, currentDoorway, availableDoorway);

                //Check room overlaps
                //if (CheckRoomOverlap(currentRoom))
                //{
                //    continue;

                //}

                roomPlaced = true;

                //Add room to list
                placeRooms.Add(currentRoom);

                //Remove occupied doorways
                currentDoorway.gameObject.SetActive(false);
                availableDoorways.Remove(currentDoorway);

                availableDoorway.gameObject.SetActive(false);
                availableDoorways.Remove(availableDoorway);

                //Exit loop if room has been placed
                break;
            }

            //Exit loop if room has been placed
            if (roomPlaced)
            {
                break;
            }
        }

        //Room couldn't be placed. Restart generator and try again
        if (!roomPlaced)
        {
            Destroy(currentRoom.gameObject);
            ResetLevelGenerator();
        }
    }
    void PlaceRoomH()
    {
        // Instantiate room
        Room currentRoom = Instantiate(roomPrefabsH[Random.Range(0, roomPrefabsH.Count)]) as Room;
        currentRoom.transform.parent = this.transform;

        for (int i = 0; i < currentRoom.gameObject.transform.childCount; i++)
        {
            Track track = currentRoom.gameObject.transform.GetChild(i).GetComponent<Track>();
            if (prevTrack != null)
            {
                prevTrack.NextTrack = track;
                track.PrevTrack = prevTrack;
            }
            prevTrack = track;
        }
        //Create doorway lists to loop over
        List<Doorway> allAvailableDoorways = new List<Doorway>(availableDoorways);
        List<Doorway> currentRoomDoorways = new List<Doorway>();
        AddDoorwaysToList(currentRoom, ref currentRoomDoorways);

        //Get doorways from current room and add then randomly to the list of available doorways
        AddDoorwaysToList(currentRoom, ref availableDoorways);

        bool roomPlaced = false;

        //Try all available doorways
        foreach (Doorway availableDoorway in allAvailableDoorways)
        {
            //Try all available doorways in current room
            foreach (Doorway currentDoorway in currentRoomDoorways)
            {
                //Position room
                PositionRoomAtDoorway(ref currentRoom, currentDoorway, availableDoorway);

                //Check room overlaps
                //if (CheckRoomOverlap(currentRoom))
                //{
                //    continue;

                //}

                roomPlaced = true;

                //Add room to list
                placeRooms.Add(currentRoom);

                //Remove occupied doorways
                currentDoorway.gameObject.SetActive(false);
                availableDoorways.Remove(currentDoorway);

                availableDoorway.gameObject.SetActive(false);
                availableDoorways.Remove(availableDoorway);

                //Exit loop if room has been placed
                break;
            }

            //Exit loop if room has been placed
            if (roomPlaced)
            {
                break;
            }
        }

        //Room couldn't be placed. Restart generator and try again
        if (!roomPlaced)
        {
            Destroy(currentRoom.gameObject);
            ResetLevelGenerator();
        }
    }

    private void PlaceRoomNew(string level)
    {
        Room currentRoom = null;
        if(level == "E")
        {
            currentRoom = Instantiate(roomPrefabsE[Random.Range(0, roomPrefabsE.Count)]) as Room;
        }
        else if(level == "M")
        {
            currentRoom = Instantiate(roomPrefabsM[Random.Range(0, roomPrefabsM.Count)]) as Room;
        }
        else
        {
            currentRoom = Instantiate(roomPrefabsH[Random.Range(0, roomPrefabsH.Count)]) as Room;
        }
        currentRoom.transform.parent = this.transform;

        for (int i = 0; i < currentRoom.gameObject.transform.childCount; i++)
        {
            Track track = currentRoom.gameObject.transform.GetChild(i).GetComponent<Track>();
            if (prevTrack != null)
            {
                prevTrack.NextTrack = track;
                track.PrevTrack = prevTrack;
            }
            prevTrack = track;
        }

        Room prevRoom = null;
        if(placeRooms.Count == 0)
        {
            prevRoom = startRoom;
        }
        else
        {
            prevRoom = placeRooms[placeRooms.Count - 1];
        }

        Doorway enddoorway_prevroom = prevRoom.doorways[prevRoom.doorways.Length - 1];
        Quaternion rotation_end = enddoorway_prevroom.gameObject.transform.rotation;

        currentRoom.transform.rotation = rotation_end;
        currentRoom.transform.position = enddoorway_prevroom.transform.position - new Vector3(0,0.45f,0);

        enddoorway_prevroom.gameObject.SetActive(false);
        foreach(Doorway doorway in currentRoom.doorways)
        {
            doorway.gameObject.SetActive(false);
        }
        placeRooms.Add(currentRoom);
    }

    private void PlaceEndRoomNew()
    {
        endRoom = Instantiate(endRoomPrefab) as EndRoom;
        endRoom.transform.parent = this.transform;

        for (int i = 0; i < endRoom.gameObject.transform.childCount; i++)
        {
            Track track = endRoom.gameObject.transform.GetChild(i).GetComponent<Track>();
            if (prevTrack != null)
            {
                prevTrack.NextTrack = track;
                track.PrevTrack = prevTrack;
            }
            prevTrack = track;
            startRoom.gameObject.transform.GetChild(0).GetComponent<Track>().PrevTrack = track;
        }

        Room prevRoom = null;
        if (placeRooms.Count == 0)
        {
            prevRoom = startRoom;
        }
        else
        {
            prevRoom = placeRooms[placeRooms.Count - 1];
        }

        Doorway enddoorway_prevroom = prevRoom.doorways[prevRoom.doorways.Length - 1];
        Quaternion rotation_end = enddoorway_prevroom.gameObject.transform.rotation;

        endRoom.transform.rotation = rotation_end;
        endRoom.transform.position = enddoorway_prevroom.transform.position - new Vector3(0, 0.45f, 0);

        endRoom.doorways[0].gameObject.SetActive(false);        
    }

    void PositionRoomAtDoorway(ref Room room, Doorway roomDoorway, Doorway targetDoorway)
    {
        //Reset room position and rotation
        room.transform.position = Vector3.zero;
        room.transform.rotation = Quaternion.identity;

        //Rotate room to match previous doorway orientation
        Vector3 targetDoorwayEuler = targetDoorway.transform.eulerAngles;
        Vector3 roomDoorwayEuler = roomDoorway.transform.eulerAngles;
        float deltaAngle = Mathf.DeltaAngle(roomDoorwayEuler.y, targetDoorwayEuler.y);
        Quaternion currentRoomTargetRotation = Quaternion.AngleAxis(deltaAngle, Vector3.up);
        room.transform.rotation = currentRoomTargetRotation * Quaternion.Euler(0, 180f, 0);

        //Position room
        Vector3 roomPositionOffset = roomDoorway.transform.position - room.transform.position;
        room.transform.position = targetDoorway.transform.position - roomPositionOffset;
    }

    //bool CheckRoomOverlap(Room room)
    //{
    //    Bounds bounds = room.RoomBounds;
    //    bounds.Expand(-0.1f);

    //    Collider[] colliders = Physics.OverlapBox(bounds.center, bounds.size / 2, room.transform.rotation, roomLayerMask);
    //    if(colliders.Length > 0)
    //    {
    //        Ignore collisions with current room
    //        foreach(Collider c in colliders)
    //        {
    //            if(c.transform.parent.gameObject.Equals(room.gameObject))
    //            {
    //                continue;
    //            }
    //            else
    //            {
    //                Debug.LogError("Overlap detected");
    //                return true;
    //            }
    //        }
    //    }
    //}

    void PlaceEndRoom()
    {
        PlaceEndRoomNew();
        return;
        //Instantiate room
        endRoom = Instantiate(endRoomPrefab) as EndRoom;
        endRoom.transform.parent = this.transform;

        for (int i = 0; i < endRoom.gameObject.transform.childCount; i++)
        {
            Track track = endRoom.gameObject.transform.GetChild(i).GetComponent<Track>();
            if (prevTrack != null)
            {
                prevTrack.NextTrack = track;
                track.PrevTrack = prevTrack;
            }
            prevTrack = track;
            startRoom.gameObject.transform.GetChild(0).GetComponent<Track>().PrevTrack = track;
        }
        NewTestController.instance.Show_Button();
        //Create doorway lists to loop over
        List<Doorway> allAvailableDoorways = new List<Doorway>(availableDoorways);
        Doorway doorway = endRoom.doorways[0];

        bool roomPlaced = false;

        //Try all available doorways
        foreach (Doorway availableDoorway in allAvailableDoorways)
        {
            //Position room
            Room room = (Room)endRoom;
            PositionRoomAtDoorway(ref room, doorway, availableDoorway);

            //Check room overlaps
            //    if (CheckRoomOverlap(endRoom))
            //{
            //    continue;

            //}

            roomPlaced = true;

                //Remove occupied doorways
                doorway.gameObject.SetActive(false);
                availableDoorways.Remove(doorway);

                availableDoorway.gameObject.SetActive(false);
                availableDoorways.Remove(availableDoorway);

                //Exit loop if room has been placed
                break;
            }

        //Room couldn't be placed. Restart generator and try again
        if (!roomPlaced)
        {
            ResetLevelGenerator();
        }

    }

    void ResetLevelGenerator()
    {
        Debug.LogError("Reset level generator");
        StopCoroutine("GenerateLevel");

        //Delete all rooms
        if (startRoom)
        {
            Destroy(startRoom.gameObject);
        }
        if (endRoom)
        {
            Destroy(endRoom.gameObject);
        }

        foreach (Room room in placeRooms)
        {
            Destroy(room.gameObject);
        }

        //Clear lists
        placeRooms.Clear();
        availableDoorways.Clear();

        //Reset coroutine
        StartCoroutine("GenerateLevel");
    }
    
    public void BtnOne()
    {
        count++;
        if (choose > 0 && choose < 11)
        {
            e += 1;
        }
        else if (choose > 10 && choose < 21)
        {
            m += 1;
        }
        else if (choose > 20 && choose < 31)
        {
            h += 1;
        }
        ResetLevelGenerator();
    }

    public void BtnTwo()
    {
        count++;
        if (choose > 0 && choose < 11)
        {
            e += 2;
        }
        else if (choose > 10 && choose < 21)
        {
            m += 2;
        }
        else if (choose > 20 && choose < 31)
        {
            h += 2;
        }
        ResetLevelGenerator();
    }

    public void BtnThree()
    {
        count++;
        if (choose > 0 && choose < 11)
        {
            e += 3;
        }
        else if (choose > 10 && choose < 21)
        {
            m += 3;
        }
        else if (choose > 20 && choose < 31)
        {
            h += 3;
        }
        ResetLevelGenerator();
    }

    public void BtnFour()
    {
        count++;
        if (choose > 0 && choose < 11)
        {
            e += 4;
        }
        else if (choose > 10 && choose < 21)
        {
            m += 4;
        }
        else if (choose > 20 && choose < 31)
        {
            h += 4;
        }
        ResetLevelGenerator();
    }

    public void BtnFive()
    {
        count++;
        if (choose > 0 && choose < 11)
        {
            e += 5;
        }
        else if (choose > 10 && choose < 21)
        {
            m += 5;
        }
        else if (choose > 20 && choose < 31)
        {
            h += 5;
        }
        ResetLevelGenerator();
    }

    public void BtnSix()
    {
        count++;
        if (choose > 0 && choose < 11)
        {
            e += 6;
        }
        else if (choose > 10 && choose < 21)
        {
            m += 6;
        }
        else if (choose > 20 && choose < 31)
        {
            h += 6;
        }
        ResetLevelGenerator();
    }

    public void BtnSeven()
    {
        count++;
        if (choose > 0 && choose < 11)
        {
            e += 7;
        }
        else if (choose > 10 && choose < 21)
        {
            m += 7;
        }
        else if (choose > 20 && choose < 31)
        {
            h += 7;
        }
        ResetLevelGenerator();
    }

    public void BtnEight()
    {
        count++;
        if (choose > 0 && choose < 11)
        {
            e += 8;
        }
        else if (choose > 10 && choose < 21)
        {
            m += 8;
        }
        else if (choose > 20 && choose < 31)
        {
            h += 8;
        }
        ResetLevelGenerator();
    }

    public void BtnNine()
    {
        count++;
        if (choose > 0 && choose < 11)
        {
            e += 9;
        }
        else if (choose > 10 && choose < 21)
        {
            m += 9;
        }
        else if (choose > 20 && choose < 31)
        {
            h += 9;
        }
        ResetLevelGenerator();
    }

    public void BtnTen()
    {
        count++;
        if (choose > 0 && choose < 11)
        {
            e += 10;
        }
        else if (choose > 10 && choose < 21)
        {
            m += 10;
        }
        else if (choose > 20 && choose < 31)
        {
            h += 10;
        }
        ResetLevelGenerator();
    }
}
