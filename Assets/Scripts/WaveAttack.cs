using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveAttack : Attack
{
    public float endtime;//碰撞器消失时间
    public float startTime;//碰撞器出现时间
    
    //private Animator anim;
    private PolygonCollider2D collider2D;
    // Start is called before the first frame update
    void Start()
    {
        //anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        collider2D = GetComponent<PolygonCollider2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Attack()
    {
        
            //anim.SetTrigger("Wave_Right");//后续再加入其他方向
            StartCoroutine(startAttack());
    }

    IEnumerator startAttack()
    {
        yield return new WaitForSeconds(startTime);
        collider2D.enabled = true;
        StartCoroutine(disableHitBox());
    }
    IEnumerator disableHitBox()
    {
        yield return new WaitForSeconds(endtime);
        collider2D.enabled = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("玩家对敌人造成2点伤害");
    }
}
