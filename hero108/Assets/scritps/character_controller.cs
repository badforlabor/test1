using UnityEngine;
using System.Collections;

/**
 * 
 * 控制行走的
 * 
 * **/

public class character_controller : MonoBehaviour {

    enum ERoleState
    {
        IDLE_Invalid = -1,
        IDLE_Front,
        IDLE_Left,
        IDLE_Right,
        IDLE_Back,
        IDLE_Left_Front,
        IDLE_Right_Front,
        IDLE_Left_Back,
        IDLE_Right_Back,


        RUN_Invalid = -1,
        RUN_Front,
        RUN_Left,
        RUN_Right,
        RUN_Back,
        RUN_Left_Front,
        RUN_Right_Front,
        RUN_Left_Back,
        RUN_Right_Back,
    }

    Animator animator = null;

	// Use this for initialization
	void Start () {
        animator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        ProcessMove();
	}

    int dirX = 0;
    int dirY = 0;
    public int Rotation = 0;
    public float Speed = 7;
    void ProcessMove()
    {
        if (animator == null)
        {
            return;
        }

        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        hor = hor > 0 ? 1 : (hor < 0 ? -1 : 0);
        ver = ver > 0 ? 1 : (ver < 0 ? -1 : 0);
        ERoleState[] dirs = { 
                            ERoleState.IDLE_Invalid, ERoleState.IDLE_Front, ERoleState.IDLE_Back,   /* x=0, y=0, 1, 2*/
                            ERoleState.IDLE_Left, ERoleState.IDLE_Left_Front, ERoleState.IDLE_Left_Back,   /* x=1, y=0, 1, 2*/
                            ERoleState.IDLE_Right, ERoleState.IDLE_Right_Front, ERoleState.IDLE_Right_Back,   /* x=2, y=0, 1, 2*/
                         };
        int x = hor == 0 ? 0 : (hor > 0 ? 2 : 1);   // 0表示没有按键盘，2表示向右(x轴正方向），1表示向左（x轴负方向）
        int y = ver == 0 ? 0 : (ver > 0 ? 2 : 1);
        ERoleState pending_dir = dirs[x * 3 + y];
        if (pending_dir != ERoleState.IDLE_Invalid)
        {
            Rotation = (int)pending_dir;
        }

        if (hor != 0 || ver != 0)
        {
            animator.SetInteger("run_dir", Rotation);
            animator.SetInteger("idle_dir", (int)ERoleState.IDLE_Invalid);
        }
        else
        {
            animator.SetInteger("run_dir", (int)ERoleState.RUN_Invalid);
            animator.SetInteger("idle_dir", Rotation);
        }

        // 处理移动
        Vector3 move_dir = new Vector3(hor, ver, 0);

        this.transform.Translate(move_dir.normalized * Speed * Time.deltaTime);
    }
    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 300,50), "" + Input.GetAxis("Horizontal"));
    }
}
