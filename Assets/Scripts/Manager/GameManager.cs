using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public string nextSceneName;
    
    private static GameManager instance_s;
    
    private bool isGameEnd = false;

    public static GameManager Instance
    {
        get
        {
            if (instance_s == null)
            {
                Debug.LogError("GameManager is null");
            }
            return instance_s;
        }
    }
    
    private List<IGameManagerListener> listeners_ = new List<IGameManagerListener>();
    
    private void Awake()
    {
        if (instance_s != null)
        {
            Debug.LogError("GameManager is already exist at object:"+instance_s.gameObject.name+". Destroy this object:"+gameObject.name);
            Destroy(gameObject);
            return;
        }
        instance_s = this;
        Time.timeScale = 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addListener(IGameManagerListener listener)
    {
        listeners_.Add(listener);
    }

    public void removeListener(IGameManagerListener listener)
    {
        listeners_.Remove(listener);
    }
    
    public void gameEnd(bool iswin)
    {
        if (isGameEnd)
        {
            return;
        }
        isGameEnd = true;
        Time.timeScale = 0.1f;
        
        foreach (var listener in listeners_)
        {
            listener.onGameEnd(iswin);
        }
    }

    public void gamePause()
    {
        Time.timeScale = 0;
        foreach (var listener in listeners_)
        {
            listener.onGamePause();
        }
    }
    
    public void gameContinue()
    {
        Time.timeScale = 1;
        foreach (var listener in listeners_)
        {
            listener.onGameContinue();
        }
    }
    
    
}
