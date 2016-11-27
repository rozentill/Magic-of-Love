using UnityEngine;
using System.Collections;

public class CursorManager : BehaviorSingleton<CursorManager> {

    Transform cursorEffect;

    Vector2 lastMouse;
    bool showEffect = true;

    // Use this for initialization
    void Start ()
    {
        cursorEffect = Resources.Load<Transform>("CursorEffect");
        lastMouse = Global.GetPosition();
    }

    // Update is called once per frame
    void Update () {
        Vector2 mouse = Global.GetPosition();

        if (mouse != lastMouse && mouse.x != 0f && mouse.y != 0f && showEffect)
        {
            Transform effect = (Transform)Instantiate(cursorEffect, Camera.main.ScreenToWorldPoint(mouse), Quaternion.identity);
            effect.Rotate(mouse - lastMouse);
        }

        lastMouse = mouse;

    }

    public void ShowEffect(bool p)
    {
        showEffect = p;
    }
}
