using System.Collections;
using DG.Tweening;
using UnityEngine;


public class CameraLogic : MonoBehaviour,IGameManagerListener

{

    public Transform player = null;            

    private Vector3 tagetPostion;       

    private Vector3 ve3;                

    Quaternion angel;                   

    public float speed;                 

    public float upFloat;               

    public float backFloat;             

    public float yOffect = 0;

    public string[] unColiTags;
    
    private Vector3 followPos;

    private bool follow_ = true;

    public bool Follow
    {
        set { follow_ = value; } 
    }
    
    void Start()
    {
        followPos = player.position + new Vector3(0, yOffect, 0);
        GameManager.Instance.addListener(this);
    }
    
    void LateUpdate()
    {
        if (follow_)
        {
            followLogic();
        }

    }
    
    void followLogic()
    {
        followPos = player.position + new Vector3(0, yOffect, 0);
        tagetPostion = followPos + player.up * upFloat - 
                       player.gameObject.transform.forward * backFloat;
        
        tagetPostion = hitOffect(tagetPostion);
        
        transform.position = Vector3.SmoothDamp(transform.position, tagetPostion, ref ve3, 0);

        angel = Quaternion.LookRotation(followPos - tagetPostion);

        transform.rotation = Quaternion.Slerp(transform.rotation, angel, speed);
    }
    
    Vector3 hitOffect(Vector3 v3)
    {

        RaycastHit hit;

        if (Physics.Raycast(player.position, v3 - player.position, out hit, 5.0f))

        {

            if (hit.collider.tag != "MainCamera" && isColi(hit.collider.tag))
            {

                v3 = hit.point + transform.forward * 0.5f;

            }

        }

        return v3;

    }


    bool isColi(string tag)
    {
        foreach(var t in unColiTags)
        {
            if( t == tag)
            {
                return false;
            }
        }
        return true;
    }

    void winLogic()
    {
        DOTween.To(
                () =>backFloat,
                x => backFloat = x,
                backFloat - 5,
                0.1f
            ).SetEase(ease: Ease.OutQuad) //缓动类型
            .SetUpdate(true); 
    }

    void loseLogic()
    {
        follow_ = false;
        transform.DORotate(new Vector3(0, 0, 560), 0.1f);
    }


    public void onGameStart()
    {
        
    }

    public void onGameEnd(bool iswin)
    {
        if (iswin)
        {
            winLogic();
        }
        else
        {
            loseLogic();
        }
    }

    public void onGamePause()
    {
        
    }

    public void onGameContinue()
    {
        
    }
}
