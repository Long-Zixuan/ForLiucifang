using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BasePlayerController : MonoBehaviour,IGameManagerListener
{
    [SerializeField]
    protected ColorType colorType_ = ColorType.Red;

    public ColorType ColType
    {
        get
        {
            return colorType_;
        }
        set
        {
            colorType_ = value;
        }
    }
    public float speed = 5f;
    public float jumpForce = 6.5f;
    public float jumpTime = 1f;


    public GameObject changeColEffect;
    
    protected bool isOnGround_ = true;
    
    protected Rigidbody rb_;
    protected Animator ani_;

    protected float timer_;

    protected bool canMove_ = true;
    protected bool canJump_ = true;
    protected Material objMat_;
    [SerializeField]
    protected GameObject playerBody_;

    protected GroundData currentGround_;

    protected Dictionary<ColorType, Color> colorDic_ = new Dictionary<ColorType, Color>();
    
    // Start is called before the first frame update
    protected void Start()
    {
        rb_ = GetComponent<Rigidbody>();
        ani_ = GetComponent<Animator>();
        ani_.updateMode = AnimatorUpdateMode.Normal;
        objMat_ = playerBody_.GetComponent<SkinnedMeshRenderer>().material;
        GameManager.Instance.addListener(this);
        colorDic_.Add(ColorType.Red, Color.red);
        colorDic_.Add(ColorType.Blue, Color.blue);
        colorDic_.Add(ColorType.Green, Color.green);
    }

    // Update is called once per frame
    protected void Update()
    {
        resetJumpTimeLogic();
    }

    protected virtual void resetJumpTimeLogic()
    {
        ani_.SetBool("isGround",isOnGround_);
        if (isOnGround_ )
        {
            timer_ = 0;
        }
    }


    public virtual void move(Vector3 dir)
    {
        if (canMove_)
        {
            transform.Translate( dir * speed * Time.deltaTime,Space.World);
        }
    }

    public virtual void jump()
    {
        timer_ += Time.deltaTime;
        if (timer_ < jumpTime && canJump_)
        {
            if (isOnGround_)
            {
                ani_.SetTrigger("Jump");
            }
            rb_.AddForce(Vector3.up * jumpForce, ForceMode.Force);
        } 
    }

    public virtual void changeColor(ColorType colorType)
    {
        print("changeColor:"+colorType.ToString());
        objMat_.SetColor("_Color", colorDic_[colorType]);
        ani_.SetTrigger("ChangeColor");
        Instantiate(changeColEffect, transform.position, Quaternion.identity);
        colorType_ = colorType;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        grounding(collision, false,true);
    }

    private void OnCollisionStay(Collision other)
    {
        grounding(other, false);
    }
    
    private void OnCollisionExit(Collision collision)
    {
        grounding(collision, true);
    }

    void grounding(Collision other, bool exitState,bool isEnter = false)
    {
        if (exitState)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Terrian"))
            {
                isOnGround_ = false;
            }
        }
        else
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Plane"))
            {
                die();
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Terrian") && 
                Vector3.Dot(other.contacts[0].normal ,Vector2.up)>0.5)//夹角小于60度才算是在地面上
            {
                if (isEnter)
                {
                    if (other.gameObject.GetComponent<GroundData>())
                    {
                        currentGround_ = other.gameObject.GetComponent<GroundData>();
                    }
                }

                if (currentGround_.colorType != colorType_)
                {
                    die();
                }

                if (!isOnGround_)
                {
                    isOnGround_ = true;
                }
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Terrian") && !isOnGround_ &&
                     Vector3.Dot(other.contacts[0].normal, Vector2.up) < 0)
            {
                
            }
        }
        
    }
    
    protected void die()
    {
        print("you die");
        GameManager.Instance.gameEnd(false);
    }
    

    public virtual void onGameStart()
    {
        
    }

    public virtual void onGameEnd(bool iswin)
    {
        canJump_ = false;
        if (!iswin)
        {
            canMove_ = false;
        }
    }
    
    public virtual void onGamePause()
    {
        
    }
    
    public virtual void onGameContinue()
    {
        
    }
    
}
