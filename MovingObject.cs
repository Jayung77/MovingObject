using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public float speed;

    private Vector3 vector;

    public float runSpeed;
    private float applyRunSpeed;
    private bool applyRunFlag = false; //shift 사용시, 두칸씩 움직임 개선///////////////////

    public int WalkCount;
    private int currentWalkCount;
    //speed = 2.4, walkcount = 20
    //2.4 * 20 = 48
    //while 사용, currentWalkCount += 1, 20 될경우 반복문종료

    private bool canMove = true;

    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    IEnumerator MoveCoroutine()
    {
        while (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                applyRunSpeed = runSpeed;
                applyRunFlag = true;
            }
            else
                applyRunSpeed = 0; //적용이 안됨
            applyRunFlag |= false;

            vector.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), transform.position.z);

            if (vector.x != 0)
                vector.y = 0; //vector.x = 1일때, vector.y = 0

            animator.SetFloat("DirX", vector.x);
            animator.SetFloat("DirY", vector.y);
            animator.SetBool("Walking", true);//standingtree 에서 walkingtree로 상태변환         

            while (currentWalkCount < WalkCount)
            {

                if (vector.x != 0)
                {
                    transform.Translate(vector.x * (speed + applyRunSpeed), 0, 0);
                }
                else if (vector.y != 0)
                {
                    transform.Translate(0, vector.y * (speed + applyRunSpeed), 0);
                }

                if (applyRunFlag)  ////////////////
                    currentWalkCount++;
                currentWalkCount++;
                yield return new WaitForSeconds(0.01f); // 0.01초 동안대기 ex)반복문 20번이면 0.2초
            }

            currentWalkCount = 0; //0으로 초기화, 다시 반복문 돌 수 있게
        }
        animator.SetBool("Walking", false); // 반복문 끝나면 false. walkingtree 에서 standingtree로 change.
        canMove = true; // 방향키처리 가능하게
    }


    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                canMove = false;
                StartCoroutine(MoveCoroutine());
            }
        }

    }
}
