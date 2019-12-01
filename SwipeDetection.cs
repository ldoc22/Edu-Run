using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SwipeDetection : MonoBehaviour
{
    [SerializeField]
    private bool Spoofing;
    private PlayerMovement pm;

    private Vector2 FingerDownPosition;
    private Vector2 FingerUpPosition;

    [SerializeField]
    private bool DetectSwipeOnlyAfterRelease = false;

    [SerializeField]
    private float minDistanceForSwipe = 20f;

    public static event Action<SwipeData> OnSwipe = delegate { };

    private void Start()
    {
        pm = GetComponent<PlayerMovement>();
    }
    // Start is called before the first frame update


    // Update is called once per frame
    private void Update()
    {
        if (Spoofing)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    FingerUpPosition = touch.position;
                    FingerUpPosition = touch.position;
                }
                if (!DetectSwipeOnlyAfterRelease && touch.phase == TouchPhase.Moved)
                {

                    FingerDownPosition = touch.position;
                    DetectSwipe();
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    FingerDownPosition = touch.position;
                    DetectSwipe();
                }

            }
        }
        if (Spoofing)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                pm.SwipeRight();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                pm.SwipeLeft();
            }
        }

    }
    private void DetectSwipe()
    {
        if (SwipeDistanceCheckMet())
        {
            if (IsVerticalSwipe())
            {
                var direction = FingerDownPosition.y - FingerUpPosition.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;
                SendSwipe(direction);
            }
            else
            {
                var direction = FingerDownPosition.x - FingerUpPosition.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
                SendSwipe(direction);
            }
            FingerUpPosition = FingerDownPosition;
        }
    }
    private bool IsVerticalSwipe()
    {
        return VerticalMovementDistance() > HorizontalMovementDistance();
    }
    private bool SwipeDistanceCheckMet()
    {
        return VerticalMovementDistance() > minDistanceForSwipe || HorizontalMovementDistance() > minDistanceForSwipe;
    }
    private float VerticalMovementDistance()
    {
        return Mathf.Abs(FingerDownPosition.y - FingerDownPosition.y);
    }
    private float HorizontalMovementDistance()
    {
        return Mathf.Abs(FingerDownPosition.x - FingerDownPosition.x);
    }

    private void SendSwipe(SwipeDirection direction)
    {
        SwipeData swipeData = new SwipeData()
        {
            Direction = direction,
            StartPosition = FingerDownPosition,
            EndPosition = FingerUpPosition
        };
    OnSwipe(swipeData);
    }
    
}

public struct SwipeData
{
    public Vector2 StartPosition;
    public Vector2 EndPosition;
    public SwipeDirection Direction;
}
public enum SwipeDirection
{
    Up,
    Down,
    Left,
    Right
}

