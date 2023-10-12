using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class LocalMirrorController : UdonSharpBehaviour
{

    [SerializeField] private GameObject toggleObject;
    [SerializeField] private GameObject offObject;

    public override void Interact()
    {
        toggleObject.SetActive(!toggleObject.activeSelf);
        offObject.SetActive(false);
    }
}