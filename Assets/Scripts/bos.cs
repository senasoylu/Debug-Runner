using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bos : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int laneCount = 5;
       // int obstacleAmount = Random.Range(1, laneCount);
        int obstacleAmount =5;
        List<int> selectedLanes= new List<int>();
        for (int i = 0; i < obstacleAmount; i++)
        {
            int selectedLane= Random.Range(0, laneCount);
            selectedLanes.Add(selectedLane);

            Debug.Log( selectedLane );

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
