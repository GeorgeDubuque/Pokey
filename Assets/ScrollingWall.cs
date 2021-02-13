using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingWall : MonoBehaviour
{
    // Start is called before the first frame update
    Queue<Transform> walls = new Queue<Transform>();
    Vector3 respawnPoint;
    Vector3 despawnPoint;
    public float wallSpeed = 3f;
    float wallWidth = 1.5f;
    private void Awake()
    {
        Vector3 furthestLeft = new Vector3(int.MaxValue, 0 , 0);
        Vector3 furthestRight = new Vector3(-int.MaxValue, 0 , 0);
        for(int i = 0; i < transform.childCount; i++)
        {
            Transform currWall = transform.GetChild(i);
            wallWidth = currWall.localScale.x;
            walls.Enqueue(currWall);
            if(currWall.localPosition.x >= furthestRight.x)
            {
                furthestRight = currWall.localPosition;
            }
            if(currWall.localPosition.x <= furthestLeft.x)
            {
                furthestLeft = currWall.localPosition;
            }
        }

        respawnPoint = furthestRight;
        despawnPoint = furthestLeft;
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Transform wall in walls)
        {
            wall.localPosition += Vector3.right * wallSpeed * Time.deltaTime;
            if(wall.localPosition.x <= despawnPoint.x - wallWidth)
            {
                wall.localPosition = respawnPoint;
            }
            if(wall.localPosition.x >= respawnPoint.x + wallWidth)
            {
                wall.localPosition = despawnPoint;
            }
        }
        
    }
}
