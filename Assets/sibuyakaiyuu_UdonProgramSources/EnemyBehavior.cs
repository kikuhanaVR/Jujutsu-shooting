
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class EnemyBehavior : UdonSharpBehaviour
{
    public bool isHeightSwing;
    public bool isWidthSwingX;
    public bool isWidthSwingZ;
    public bool isJump;
    public float swingHeight;
    public float swingWidth;
    public float swingSpeed = 1.0f;
    //やられたときのエフェクト
    public GameObject ExplodeEffect;
    Vector3 OriginalPosition;
    GameObject explode;
    void Start()
    {
        OriginalPosition = transform.position;
    }

    void FixedUpdate()
    {
        Vector3 pos = transform.position;

        if (isHeightSwing)
        {
            //元の位置からswingHeight分上下に動かす
            pos.y = OriginalPosition.y + Mathf.Sin(Time.time * swingSpeed) * swingHeight;
        }
        if (isWidthSwingX)
        {
            //元の位置からswingWidth分左右に動かす
            pos.x = OriginalPosition.x + Mathf.Sin(Time.time * swingSpeed) * swingWidth;
        }
        if (isWidthSwingZ)
        {
            //元の位置からswingWidth分前後に動かす
            pos.z = OriginalPosition.z + Mathf.Cos(Time.time * swingSpeed) * swingWidth;
        }
        if (isJump)
        {
            //元の位置から跳ねるように上下に動かす
            pos.y = OriginalPosition.y + Mathf.Abs(Mathf.Sin(Time.time * swingSpeed)) * swingHeight;
        }

        transform.position = pos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "murasaki")
        {
            Explode();
        }
    }

    //やられたときの処理
    public void Explode()
    {
        //やられたときのエフェクトを生成, 2秒後に消す
        explode = Instantiate(ExplodeEffect, transform.position, transform.rotation);
        explode.transform.localScale = transform.localScale * 0.75f;
        Destroy(explode, 2.0f);
    }
}
