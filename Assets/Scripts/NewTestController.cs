using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZenFulcrum.Track;

public class NewTestController : MonoBehaviour
{
    public GameObject MainCamera;
    public GameObject StreamCamera;

    public GameObject VolteButtons;
    public GameObject StartButton;
    // Start is called before the first frame update
    public static NewTestController instance;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Track track = StreamCamera.GetComponent<TrackCart>().CurrentTrack;
        if(track == null)
        {
            return;
        }
        if (track.gameObject.name.Contains("End"))
        {
            StartButton.SetActive(true);
            StreamCamera.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    public void Reset()
    {
        MainCamera.SetActive(true);
        StreamCamera.SetActive(false);
        StartButton.SetActive(false);
        StreamCamera.transform.position = new Vector3(0, 1f, 1f);
    }

    public void Show_Button()
    {
        StartButton.SetActive(true);
        VolteButtons.SetActive(false);
        GameObject startTrack = GameObject.FindGameObjectWithTag("StartTrack");
        StreamCamera.GetComponent<TrackCart>().CurrentTrack = startTrack.GetComponent<Track>();
    }

    public void Click_Start()
    {
        StreamCamera.transform.position = new Vector3(0, 1f, 1f);
        GameObject startTrack = GameObject.FindGameObjectWithTag("StartTrack");
        StreamCamera.GetComponent<TrackCart>().CurrentTrack = startTrack.GetComponent<Track>();
        MainCamera.SetActive(false);
        StreamCamera.SetActive(true);
        StartButton.SetActive(false);
    }

    public void Start_Vote()
    {
        GameObject startTrack = GameObject.FindGameObjectWithTag("StartTrack");
        StreamCamera.GetComponent<TrackCart>().CurrentTrack = startTrack.GetComponent<Track>();
        MainCamera.SetActive(false);
        StreamCamera.SetActive(true);
        StartButton.SetActive(false);
    }
}
