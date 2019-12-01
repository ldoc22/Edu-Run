using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private float SideMoveDistance;
    private GameManager gm;
    private int rowIndex, minRow, maxRow;

    private Rigidbody rb;

    private bool movingRight, movingLeft;

    public GameObject Player;
    //lerp variables
    public Vector3 startMarker;
    public Vector3 endMarker;
    public Vector3 SM;
    public Vector3 EM;

    public float speed = .01f;

    
    private float startTime;

  
    private float journeyLength;

    public bool lerping;
    private float currentTime;


    private MapCreation MC;
    private AudioManager AM;
  
    // Start is called before the first frame update
    void Start()
    {
        
        gm = GetComponent<GameManager>();
        AM = AudioManager.instance;
        SideMoveDistance = GetComponent<MapCreation>().row.transform.localScale.x;
        
        minRow = 0;
        rowIndex = minRow;

        //hard coded for now
        maxRow = 2;

        startTime = Time.time;
        FindPlayer();

        
        
        startMarker = Player.transform.position;
        endMarker = Player.transform.position;
        // Calculate the journey length.
        //journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
        lerping = false;
        rb = Player.GetComponent<Rigidbody>();
        gm.SetRowIndex(rowIndex);
        MC = GetComponent<MapCreation>();
       
    }

    void FindPlayer()
    {
        Player = GetComponent<MapCreation>().getPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if(Player.transform.position.z >= MC.CheckPoints.Peek())
        {
            gm.NextQuestion();
            MC.CheckPoints.Dequeue();
        }

        if (movingRight)
        {
            if(Player.transform.position.x < (rowIndex * SideMoveDistance))
            {
                rb.velocity = new Vector3(gm.PlayerSpeed, rb.velocity.y, gm.getSpeed());
            }else
            {
                movingRight = false;
            }
        }else if (movingLeft)
        {
            if (Player.transform.position.x > (rowIndex * SideMoveDistance))
            {
                rb.velocity = new Vector3(-gm.PlayerSpeed, rb.velocity.y, gm.getSpeed());
            }
            else
            {
                movingLeft = false;
            }
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, gm.getSpeed());
        }


        /*
        
        if (lerping)
        {
            currentTime += Time.deltaTime;
            float distCovered = (currentTime - startTime) * gm.SideMovementSpeed;

            // Fraction of journey completed = current distance divided by total distance.
            float fracJourney = distCovered / journeyLength;

            // Set our position as a fraction of the distance between the markers.
            Player.transform.position = Vector3.Lerp(startMarker, endMarker, fracJourney);
            print(fracJourney);
            if(fracJourney >= .99f)
            {
                lerping = false;
            }
        }

        EM = endMarker;
        SM = startMarker;
        */

    }
    public void SwipeRight()
    {
        if (rowIndex < maxRow)
        {
            AM.swipe();
            rowIndex++;
            gm.SetRowIndex(rowIndex);
            movingRight = true;
           
        }
    }

    public void SwipeLeft()
    {
        if (rowIndex > minRow) {
            AM.swipe();
            rowIndex--;
            gm.SetRowIndex(rowIndex);
            movingLeft = true;
           
        }
    }

   

   
}
