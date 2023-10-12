
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class DestroyDelay : UdonSharpBehaviour
{
    void Start()
    {
        // 2秒後にDestroyを実行
        SendCustomEventDelayedSeconds("DestroyObj", 2.0f);
    }
    
    public void DestroyObj()
    {
        // 自身を破棄
        Destroy(gameObject);
    }
}
