using UnityEngine;

public class SFXFields_Menu : SFXSystem
{
    public void MenuMusic()
    {
        ClipsName _finalBatleMusic = (ClipsName)Random.Range((float)ClipsName.MenuMusic1, (float)ClipsName.MenuMusic4 + 1);
        PlayMusic(sounds[(int)_finalBatleMusic], 0.17f);
    }

    public enum ClipsName
    {
        MenuMusic1 = 0,
        MenuMusic2 = 1,
        MenuMusic3 = 2,
        MenuMusic4 = 3
    }
}
