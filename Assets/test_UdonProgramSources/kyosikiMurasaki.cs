
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;

public class kyosikiMurasaki : UdonSharpBehaviour
{
    public GameObject aka;
    public GameObject murasakiPrefab;
    public Transform firepoint;
    public AudioClip ChargeSound;
    public AudioClip FireSound;
    public float speed;
    public float destroyTime;

    GameObject murasaki;
    bool isFire;
    void Start()
    {
        murasaki = null;
    }

    void Update()
    {
        if (murasaki != null && isFire == true)
        {
            murasaki.transform.Translate(new Vector3(speed, 0.0f, 0.0f) * Time.deltaTime);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "aka" && murasaki == null && isFire == false)
        {
            murasaki = Instantiate(murasakiPrefab, firepoint.position, firepoint.rotation);
            //チャージ音再生
            GetComponent<AudioSource>().PlayOneShot(ChargeSound);
        }
        GetComponent<VRCPickup>().GenerateHapticEvent();
        aka.GetComponent<VRCPickup>().GenerateHapticEvent();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "aka" && isFire == false)
        {
            isFire = true;
            SendCustomEventDelayedSeconds("DestroyMurasaki", destroyTime);

            //チャージ音を停止し、発射音再生
            GetComponent<AudioSource>().Stop();
            GetComponent<AudioSource>().PlayOneShot(FireSound);
        }
    }

    public void DestroyMurasaki()
    {
        Destroy(murasaki);
        murasaki = null;
        isFire = false;
    }
}
