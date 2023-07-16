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
    private bool applyRunFlag = false; //shift ����, ��ĭ�� ������ ����///////////////////

    public int WalkCount;
    private int currentWalkCount;
    //speed = 2.4, walkcount = 20
    //2.4 * 20 = 48
    //while ���, currentWalkCount += 1, 20 �ɰ�� �ݺ�������

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
                applyRunSpeed = 0; //������ �ȵ�
            applyRunFlag |= false;

            vector.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), transform.position.z);

            if (vector.x != 0)
                vector.y = 0; //vector.x = 1�϶�, vector.y = 0

            animator.SetFloat("DirX", vector.x);
            animator.SetFloat("DirY", vector.y);
            animator.SetBool("Walking", true);//standingtree ���� walkingtree�� ���º�ȯ         

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
                yield return new WaitForSeconds(0.01f); // 0.01�� ���ȴ�� ex)�ݺ��� 20���̸� 0.2��
            }

            currentWalkCount = 0; //0���� �ʱ�ȭ, �ٽ� �ݺ��� �� �� �ְ�
        }
        animator.SetBool("Walking", false); // �ݺ��� ������ false. walkingtree ���� standingtree�� change.
        canMove = true; // ����Űó�� �����ϰ�
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
