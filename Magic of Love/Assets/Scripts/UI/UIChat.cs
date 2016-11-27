using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class UIChat : MonoBehaviour {

    public CanvasGroup chatPanel;
    public Text word, tip;
    public Image block, background, avatar, frame1, frame2;
    Image frame;
    float duration = 0.3f, waitTime = 0f;
    float voiceLength = 0f;

    private bool isChatting = false;

    // Use this for initialization
    void Start ()
    {        
    }

    // Update is called once per frame
    void Update()
    {
        waitTime = waitTime - Time.deltaTime;
        if (voiceLength > 0f && waitTime < -voiceLength)
            Global.skip = true;
    }

    public bool Chat(string speaker, string bg, string content, int index)
    { 
        if (waitTime > 0f)
            return false;

        Sequence seq = DOTween.Sequence();

        if (!isChatting)
        {
            waitTime = duration * 9f;

            if (speaker != "旁白-1")
                frame = frame1;
            else
                frame = frame2;

            background.sprite = Resources.Load<Sprite>("BackGround/" + bg);
            tip.text = bg;
            word = frame.GetComponentInChildren<Text>();
            word.text = "";
            isChatting = true;
            seq.AppendInterval(duration);
            seq.Append(chatPanel.DOFade(1f, duration * 2f));
            seq.AppendInterval(duration);
            seq.Append(frame.transform.DOScale(Vector3.one, duration * 2f));
        }
        else        
            waitTime = duration;

       if (content.Contains("Magic"))
        {
            waitTime = 36000f;
            isChatting = false;
            seq.Append(chatPanel.DOFade(0f, duration * 2f));
            seq.AppendInterval(duration);
            seq.AppendCallback(()=> { FindObjectOfType<UIMagic>().StartMagic(content); });            
        }
        else
        {
            avatar.sprite = Resources.Load<Sprite>("Character/" + speaker);
            seq.Append(word.DOFade(0f, duration / 2));
            seq.AppendCallback(() => 
            {                
                AudioManager.Instance.Play(SE.SE_CHAT_SKIP);
                word.text = content;
                //voiceLength = AudioManager.Instance.PlayVoice(index);
                word.DOFade(1f, duration / 2);
            });
        }                  
    
        return true;
    }    

    public void EndChat()
    {
        if (isChatting && waitTime <= 0f)
        {
            isChatting = false;
            AudioManager.Instance.StopVoice();
            AudioManager.Instance.Play(SE.SE_MENU_CANCEL);

            Global.currentChat++;
            Global.currentStory++;

            Sequence seq = DOTween.Sequence();
            seq.Append(chatPanel.DOFade(0f, 0.7f));
            seq.Append(block.DOFade(1f, 0.5f));
            seq.AppendCallback(() => { SceneManager.Instance.Load(); });
        }
    }

    public void ReturnFromMagic()
    {
        waitTime = 0f;
        Global.skip = true;
    }
}
