using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour {
    
    float delta = 0f;
    Vector2 lastMouse;

	// Use this for initialization
	void Start ()
    {
        AudioManager.Instance.Play(BGM.BGM_TITLE);
        CursorManager.Instance.ShowEffect(true);

        lastMouse = Global.GetPosition();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!Global.enableInput)
            return;

        delta += Global.Distance(Global.GetPosition(), lastMouse);

        if (delta > Screen.width * 2f)
            FindObjectOfType<Canvas>().GetComponent<Animator>().speed = 1.5f;

        if (delta > Screen.width * 5f)
            FindObjectOfType<Canvas>().GetComponent<Animator>().speed = 2f;

        if (delta > Screen.width * 8f)
        {
            FindObjectOfType<Canvas>().GetComponent<Animator>().speed = 1f;
            FindObjectOfType<Canvas>().GetComponent<Animator>().SetBool("Enter", true);
        }


        lastMouse = Global.GetPosition();
	}
}
