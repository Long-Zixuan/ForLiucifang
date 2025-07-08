using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorObjLogic : MonoBehaviour
{
    public ColorType colorType;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<BasePlayerController>().changeColor(colorType);
        }
    }
    
}
