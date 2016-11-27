using UnityEngine;
using System.Collections;
using DG.Tweening;

public enum TweenType
{
    TWEEN_ROTATE,
    TWEEN_SCALE
}

public class MyTween : MonoBehaviour
{

    public TweenType type;
    public Vector3 delta;
    public float speed = 1f;

    private float time = 0f;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (type == TweenType.TWEEN_ROTATE)
            transform.Rotate(delta * Time.fixedDeltaTime * speed);

        if (type == TweenType.TWEEN_SCALE)
        {
            transform.localScale = Vector3.Lerp(Vector3.one, delta, 1f - Mathf.Abs(time * 2 - 1f));
        }

        time = (time + Time.fixedDeltaTime * speed) % 1f;
    }

    public void OnDisable()
    {
        if (type == TweenType.TWEEN_SCALE)
        {
            transform.localScale = Vector3.one;
            time = 0f;
        }
    }
}
