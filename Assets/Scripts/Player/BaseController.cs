using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    protected BasePlayerController player_;
    // Start is called before the first frame update
    void Start()
    {
        player_= GetComponent<BasePlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        player_.move(Vector3.forward);
        if (Input.GetButton("Jump"))
        {
            player_.jump();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.gamePause();
        }
    }
}
