using UnityEngine;
using System.Collections;
using Leap;
namespace Leap.Unity
{
    public class Stick : MonoBehaviour
    {
        private LeapProvider provider;
        public GameObject stick;
        private bool flag_stick = false;
        // Use this for initialization
        void Start()
        {
            provider = FindObjectOfType<LeapProvider>() as LeapProvider;
            stick = GameObject.Find("Stick");
        }

        // Update is called once per frame
        void Update()
        {
            Frame frame = provider.CurrentFrame;
            foreach (Hand hand in frame.Hands)
            {
                
                if (hand.IsRight)
                {
                    stick.SetActive(true);

                    foreach (Finger finger in hand.Fingers)
                    {
                        if (finger.Type == Finger.FingerType.TYPE_INDEX)
                        {
                            Vector3 fingerPos = finger.TipPosition.ToVector3();
                            stick.transform.position = new Vector3(fingerPos.x-1f, fingerPos.y - 0.2f, fingerPos.z + 5);
                        }
                    }
                    //transform.position = hand.PalmPosition.ToVector3() +
                    //                     hand.PalmNormal.ToVector3() *
                    //                    (transform.localScale.y * .5f + .02f);
                    //transform.rotation = hand.Basis.Rotation();
                }
                else if (frame.Hands.Count != 2)//left hand or no hand
                {
                    flag_stick = false;
                }
            }

            if (frame.Hands.Count == 0)
            {
                stick.SetActive(false);
            }
            else
            {
                stick.SetActive(true);

            }
        }
    }
}
