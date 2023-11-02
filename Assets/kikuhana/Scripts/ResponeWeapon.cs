
using UdonSharp;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using VRC.SDKBase;
using VRC.Udon;

public class ResponeWeapon : UdonSharpBehaviour
{
    public GameObject[] Weapon;
    public Vector3[] _iniPos;
    public Vector3[] _iniRot;

    public override void Interact()
    {
        if (!Networking.IsOwner(Networking.LocalPlayer, this.gameObject))
        {
            Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
        }
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(Respone));
    }

    public void Respone()
    {
        //　初期位置にWeaponを戻す
        for (int i = 0; i < Weapon.Length; i++)
        {
            Weapon[i].transform.position = _iniPos[i];
            Weapon[i].transform.rotation = Quaternion.Euler(_iniRot[i]);
        }
    }
}
