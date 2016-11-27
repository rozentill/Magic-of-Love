using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class UIMagic : MonoBehaviour {

    public CanvasGroup magicPanel, learn, use, bar, effect;
    public Image time, learnStick;
    public Text count, learnWord;

    public Image heart, circle1, circle2, effectWord, gesture;

    int requireMagic = 0;
    float duration = 1f;

    bool isLearn = false;

    float delta = 0f;
    Vector2 lastMouse;

    int counter = 0;

    // Use this for initialization
    void Start () {
        lastMouse = Global.GetPosition();
    }
	
	// Update is called once per frame
	void Update ()
    {
        delta += Global.Distance(Global.GetPosition(), lastMouse);
            
        if (isLearn && delta > 2f * Screen.height)
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(learn.DOFade(0f, duration / 2f));
            seq.Append(use.DOFade(1f, duration / 2f));
            seq.AppendCallback(Notice);
            delta = 0f;
            isLearn = false;
        }

        lastMouse = Global.GetPosition();

    }



    public void StartMagic(string content)
    {

        requireMagic = content[content.Length - 1] - 48;
        Global.result = -1;

        magicPanel.GetComponent<Image>().sprite = Resources.Load<Sprite>("Magic/B" + requireMagic);
        time.sprite = Resources.Load<Sprite>("Magic/T" + requireMagic);
        gesture.sprite = Resources.Load<Sprite>("Magic/G" + requireMagic);
        gesture.DOFade(0f, 0.01f);

        Sequence seq = DOTween.Sequence();
        seq.Append(magicPanel.DOFade(1f, duration / 2f));

        if (content.Contains("Learn"))
        {
            seq.Append(learn.DOFade(1f, duration / 2f));
            isLearn = true;
            delta = 0f;
            seq.AppendCallback(Learning);
        }
        else
        {
            seq.Append(use.DOFade(1f, duration / 2f));
            seq.OnComplete(Notice);
        }
    }

    void Learning()
    {
        if (isLearn)
        {
            learnStick.transform.localPosition = new Vector3(645f, 160f, 0f);
            learnWord.text = "";
            Sequence seq = DOTween.Sequence();
            seq.Append(learnStick.transform.DOLocalMoveY(-45f, 0.5f));
            seq.Append(learnStick.transform.DOLocalMoveX(800f, 0.5f));
            seq.Insert(0f, learnWord.DOText("Listen", 1f));
            seq.AppendInterval(0.2f);
            seq.OnComplete(Learning);
        }

    }

    void Notice()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(magicPanel.DOFade(1f, duration / 2f));
        seq.AppendCallback(() => { count.fontSize = 96; count.text = "是使用魔法的时候了"; });
        seq.AppendInterval(duration * 1.5f);
        seq.AppendCallback(() => { count.text = "准备好"; });
        seq.OnComplete(CountDown);
    }

    void CountDown()
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(duration);
        seq.AppendCallback(() => { count.fontSize = 180; count.text = "3"; AudioManager.Instance.Play(SE.SE_MENU_SELECT); });
        seq.AppendInterval(duration);
        seq.AppendCallback(() => { count.text = "2"; AudioManager.Instance.Play(SE.SE_MENU_SELECT); });
        seq.AppendInterval(duration);
        seq.AppendCallback(() => { count.text = "1"; AudioManager.Instance.Play(SE.SE_MENU_SELECT); });
        seq.AppendInterval(duration);
        seq.AppendCallback(() => { count.fontSize = 96; count.text = "开始"; AudioManager.Instance.Play(SE.SE_MENU_SELECT); });
        seq.Append(bar.DOFade(1f, duration));
        seq.AppendCallback(() => { count.text = ""; });
        seq.Append(gesture.DOFade(1f, duration));
        seq.AppendCallback(() => { Global.isDetect = true; });
        //seq.Append(time.DOFillAmount(1f, 4.5f * duration)).SetEase(Ease.Linear);
        seq.Append(time.DOFillAmount(1f, 4.8f * duration)).SetEase(Ease.Linear);
        seq.AppendCallback(() => { Global.isDetect = false; });
        seq.Append(bar.DOFade(0f, duration));
        seq.AppendCallback(() => { time.fillAmount = 0f; });

        seq.AppendCallback(() =>
        {
            int result = Global.result;
            if (result < 0)
            {
                AudioManager.Instance.Play(SE.SE_MAGIC_FAIL);
                counter++;
                count.text = "失败了，再来一次吧";
                CountDown();
            }
            else
                UseMagic(result);
        });
    }

    void UseMagic(int magic)
    {
        Sequence seq = DOTween.Sequence();
        if (magic == requireMagic || counter > 1)
        {
            counter = 0;
            heart.sprite = Resources.Load<Sprite>("Magic/H" + requireMagic);
            circle1.sprite = Resources.Load<Sprite>("Magic/C" + requireMagic);
            circle2.sprite = Resources.Load<Sprite>("Magic/C" + requireMagic);
            effectWord.sprite = Resources.Load<Sprite>("Magic/W" + requireMagic);

            AudioManager.Instance.Play(SE.SE_MAGIC_SUCCESS);        
            seq.Append(use.DOFade(0f, duration));
            seq.AppendCallback(() => { effect.GetComponent<Animator>().Play("MagicEffect"); });
            seq.AppendInterval(duration * 6f);
            seq.Append(magicPanel.DOFade(0f, duration / 2f));
            seq.AppendCallback(() => { FindObjectOfType<UIChat>().ReturnFromMagic(); });
        }
        else
        {
            AudioManager.Instance.Play(SE.SE_MAGIC_FAIL);
            counter++;
            count.text = "用错魔法了，再来一次吧";
            CountDown();
        }
    }
}
