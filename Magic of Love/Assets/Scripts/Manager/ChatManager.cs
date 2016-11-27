using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class ChatManager : MonoBehaviour {
    
    TextAsset content;
    private string[] word;
    int current = 0;

    public Image block;

    string speaker, state;

    float delta = 0f;
    Vector2 lastMouse;
    
    // Use this for initialization
    void Start ()
    {
        block.color = Color.white;
        block.DOFade(0f, 0.6f);
        lastMouse = Global.GetPosition();
        StartChat();
    }
	
	// Update is called once per frame
	void Update ()
    {
        delta += Global.Distance(Global.GetPosition(), lastMouse);

        if (delta > 2f * Screen.height || Global.skip)
        {
            if (current < word.Length - 2)
                NextChat();
            else
            {
                FindObjectOfType<UIChat>().EndChat();
            }

            Global.skip = false;
            delta = 0;
        }

        lastMouse = Global.GetPosition();
    }

    void StartChat()
    {
        content = Resources.Load<TextAsset>("ChatContent/" + Global.currentChat.ToString());

        word = content.text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        speaker = word[0];
        state = word[1];
        AudioManager.Instance.Play(GetBGM(state));
        

        if (word.Length >= 2)
        {
            NextChat();
        }
    }

    void NextChat()
    {
        string content = word[current + 2];
        int emotion = content[content.Length - 1] - 48;
        if (!content.Contains("Magic"))
            content = content.Substring(0, content.Length - 1);

        if (FindObjectOfType<UIChat>().Chat(speaker + "-" + emotion, state, content, current))
            current++;
    }

    BGM GetBGM(string background)
    {
        switch(background)
        {
            case "学院外": return BGM.BGM_ACADEMY;
            case "学院内": return BGM.BGM_ACADEMY;
            case "宫殿":  return BGM.BGM_CASTLE;
            case "木屋外": return BGM.BGM_HOUSE;
            case "木屋": return BGM.BGM_HOUSE;
            case "鸟巢": return BGM.BGM_BOSS;
            case "森林入口": return BGM.BGM_FOREST;
            case "森林深处": return BGM.BGM_BOSS;
            case "放鸟": return BGM.BGM_FOREST;
        }

        return BGM.BGM_NONE;
    }
}
