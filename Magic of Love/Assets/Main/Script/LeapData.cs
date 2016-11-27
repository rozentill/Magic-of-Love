using UnityEngine;
using System.Collections;
using Leap;

namespace Leap.Unity
{
    public class LeapData : MonoBehaviour
    {
        LeapProvider provider;
        // Use this for initialization
        void Start()
        {
            provider = FindObjectOfType<LeapProvider>() as LeapProvider;

        }

        // Update is called once per frame
        void Update()
        {
            Frame frame = provider.CurrentFrame;
            foreach (Hand hand in frame.Hands)
            {
                if (hand.IsRight)
                {
                    foreach (Finger finger in hand.Fingers)
                    {
                        if (finger.Type== Finger.FingerType.TYPE_INDEX)
                        {
                            Global.handPosition = Camera.main.WorldToScreenPoint(finger.TipPosition.ToVector3());

                        }
                    }
                    //transform.position = hand.PalmPosition.ToVector3() +
                    //                     hand.PalmNormal.ToVector3() *
                    //                    (transform.localScale.y * .5f + .02f);
                    //transform.rotation = hand.Basis.Rotation();
                }
            }
        }
    }
}
