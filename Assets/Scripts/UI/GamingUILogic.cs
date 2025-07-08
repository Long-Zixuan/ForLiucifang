using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamingUILogic : MonoBehaviour,IGameManagerListener
{
    public GameObject againBotton;
    public GameObject nextBotton;
    public GameObject continueBotton;
    
    public GameObject winText;
    public GameObject loseText;
    public GameObject background;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.addListener(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onGameStart()
    {
        
    }
    
    public void onGamePause()
    {
        continueBotton.SetActive(true);
    }

    public void onGameContinue()
    {
        continueBotton.SetActive(false);
    }

    public void onGameEnd(bool isWin)
    {
        StartCoroutine(gameendLogic(isWin));
    }

  
    
    public void onClickAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void onClickNext()
    {
       SceneManager.LoadScene(GameManager.Instance.nextSceneName); 
    }
    
    public void onClickContinue()
    {
        GameManager.Instance.gameContinue();
    }
    
    IEnumerator gameendLogic(bool isWin)
    {
        yield return new WaitForSeconds(0.2f);
        if (!isWin)
        {
            loseText.SetActive(true);
        }
        else
        {
            winText.SetActive(true);
            nextBotton.SetActive(true);
        }
        againBotton.SetActive(true);
        background.SetActive(true);
    }
    
}
