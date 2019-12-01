using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

 namespace Swipper{
    public class SwipeLogger : MonoBehaviour
    {
        private PlayerMovement PM;

        public Text direction;
        private void Awake()
        {
            PM = GetComponent<PlayerMovement>();
            SwipeDetection.OnSwipe += SwipeDetection_OnSwipe;
        }
        private void SwipeDetection_OnSwipe(SwipeData data)
        {
            if (data.Direction == SwipeDirection.Right)
            {
                
                PM.SwipeRight();

            } else if (data.Direction == SwipeDirection.Left)
            {
                PM.SwipeLeft();
            }
            direction.text = data.Direction.ToString();
            Debug.Log("Swipe in the Direction: " + data.Direction);
        }
    }
}
