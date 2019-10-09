using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class restartScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
        //Time.timeScale = Mathf.Approximately(Time.timeScale, 1.0f) ? 0.0f : 1.0f;
        //AudioListener.pause = false;
    }
}
