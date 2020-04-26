using UnityEngine;
using System.Collections;

public class EventCaller : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) 
            new OnGameEnded(true);
        else if (Input.GetKeyDown(KeyCode.L))
            new OnGameEnded(false);
    }
}
