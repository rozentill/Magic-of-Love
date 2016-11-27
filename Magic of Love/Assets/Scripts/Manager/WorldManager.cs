using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class WorldManager : MonoBehaviour {

    public Transform witch, earth;
    public Image block;

    bool isLeft, isRight, isUp, isDown;

    Vector3 left = new Vector3(-1f, 1f, 1f);
    Vector3 right = Vector3.one;
    Vector3 up = new Vector3(0f, 0f, 50f);
    Vector3 down = new Vector3(0f, 0f, -70f);

    float distance = 0f;

    bool isEnter = false;

    public Vector3[] initAngle = new Vector3[3];


    void Awake()
    {
        CursorManager.Instance.ShowEffect(true);

        isEnter = true;
        block.material.SetFloat("_Scale", 200f);
        block.material.DOFloat(0.1f, "_Scale", 2f).SetEase(Ease.OutQuint).OnComplete(()=> { isEnter = false; });
        earth.eulerAngles = initAngle[Global.currentStage];


        AudioManager.Instance.Play(BGM.BGM_WORLD);
    }

    // Use this for initialization
    void Start () {
        distance = witch.position.y - earth.position.y;

        for (int i = 0; i < earth.childCount; i++)
            earth.GetChild(i).GetComponent<SpriteRenderer>().color = i == Global.currentStage ? Color.white : new Color(0.2f, 0.2f, 0.2f, 1f);
    }

    // Update is called once per frame
    void Update () {

        if (isEnter)
            return;

        Vector2 mouse = Global.GetPosition();
        isLeft = mouse.x < Screen.width / 3f;
        isRight = mouse.x > Screen.width * 2f / 3f;
        isUp = mouse.y > Screen.height * 5f / 6f;
        isDown = mouse.y < Screen.height / 3f;
        //print("L " + isLeft+ " R " + isRight + " U " + isUp + " D " + isDown);

        


        if (isLeft)
        {
            witch.localScale = left;
            earth.eulerAngles += Vector3.back * Time.deltaTime *40f;
        }
        else if (isRight)
        {
            witch.localScale = right;
            earth.eulerAngles += Vector3.forward * Time.deltaTime * 40f;
        }
        else
        {
            //_animator.enabled = false;
        }

        if (isUp)
        {
            if (witch.position.y < 3.1f)
                witch.DORotate(up * witch.localScale.x, 0.7f);
            else 
                witch.DORotate(Vector3.zero, 0.7f);


            if (witch.position.y < 3.2f)
                witch.position += Vector3.up * Time.deltaTime * 2f;            
        }
        else if (isDown)
        {
            if (witch.position.y > -0.4f)
                witch.DORotate(down * witch.localScale.x, 0.7f);
            else 
                witch.DORotate(down / 3f * witch.localScale.x, 0.7f);

            if (witch.position.y > -0.5f)
                witch.position += Vector3.down * Time.deltaTime * 2f;
        }
        else
        {
            witch.DORotate(Vector3.zero, 0.5f);
        }

        earth.localScale = Vector3.one * (1.7f + (2f - witch.position.y) / 7f);
    }

    public void Enter(int index)
    {
        if (!isEnter && index == Global.currentStage)
        {
            isEnter = true;
            Global.currentStage++;
            Global.currentStory++;
            block.transform.localPosition += Vector3.down * 350f;
            Sequence seq = DOTween.Sequence();
            seq.Append(block.material.DOFloat(240f, "_Scale", 2f).SetEase(Ease.InQuint));
            seq.AppendCallback(()=> { block.material = null; });
            seq.AppendInterval(0.2f);
            seq.AppendCallback(() => {  SceneManager.Instance.Load(); });
        }
    }

}
