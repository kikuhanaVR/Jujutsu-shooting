
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class RetrySwitch : UdonSharpBehaviour
{
    [SerializeField] private GameObject[] _retryObjects;

    public override void Interact()
    {
        Retry();
    }

    public void Retry()
    {
        // _retryObjectsの子オブジェクトをすべてアクティブにする
        foreach (var obj in _retryObjects)
        {
            foreach (Transform childTransform in obj.transform)
            {
                childTransform.gameObject.SetActive(true);
            }
        }
    }
}
