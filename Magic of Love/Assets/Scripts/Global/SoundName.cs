using UnityEngine;
using System.Collections;

public enum BGM
{
    BGM_TITLE,
    BGM_START,
    BGM_ACADEMY,
    BGM_WORLD,
    BGM_CASTLE,
    BGM_HOUSE,
    BGM_FOREST,
    BGM_BOSS,
    BGM_NONE
}

public enum SE
{
    SE_MENU_OPEN,
    SE_MENU_SELECT,
    SE_MENU_ENTER,
    SE_MENU_CANCEL,
    SE_CHAT_SKIP,
    SE_MAGIC_FAIL,
    SE_MAGIC_SUCCESS,
    SE_MAGIC_USE
}

public class SoundName
{
    string[] bgm = { "Title", "Start", "Academy", "World", "Castle", "House", "Forest", "Boss", ""};
    string[] se = { "MenuOpen", "CursorMove", "CursorDecision", "MenuCancel", "ChatSkip", "MagicFail", "MagicSuccess", "MagicUse"};

    public string GetSoundPath(BGM bgmName)
    {
        return "Sound/BGM/" + bgm[(int)bgmName];
    }

    public string GetSoundPath(SE seName)
    {
        return "Sound/SE/" + se[(int)seName];
    }

    public string GetVoicePath(int index)
    {
        return "Sound/Voice/" + Global.currentChat + "-" + index;
    }
}
