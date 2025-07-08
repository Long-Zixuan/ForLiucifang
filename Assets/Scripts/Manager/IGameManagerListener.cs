using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameManagerListener
{
    void onGameStart();
    void onGameEnd(bool iswin);

    void onGamePause();
    
    void onGameContinue();
}
