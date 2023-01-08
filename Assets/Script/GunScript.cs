using UnityEngine;
using System.Collections;
using TMPro;
using Unity.Netcode;

[RequireComponent(typeof(AudioSource))]
public class GunScript : NetworkBehaviour
{
    public float damage = 10f;
    public float fireRate = 15f;
    public bool Automatic = false;
    public int magCapacity = 30;
    public int magCounter = 30;
    public int ammo;
    public float timeReload = 1f;
    private bool isReloading = false;
    public Animator animator;
    public int classSelected = 5;
    public bool FMJ;

    public PlayerMovement player;
    public PlayerManager kills;

    private const string BodyTag = "Body";
    private const string HeadTag = "Head";

    public float walkPaceSet = 10f;
    public float dropForceSet = -20f;
    public float thrustPowerSet = 3f;
    public TextMeshProUGUI currentMag;
    public TextMeshProUGUI ammoLeft;

    [SerializeField]
    PlayerManager playerManager;
    public Camera mainCamera;
    public ParticleSystem muzzleFlash;
    public GameObject objHit;
    public GameObject AR;
    public GameObject SMG;
    public GameObject Snotgun;
    public GameObject ARScript;
    public GameObject SMGScript;
    public GameObject SnotgunScript;
    public GameObject DotCH;
    public GameObject CrossCH;
    public GameObject PlusCH;
    public GameObject dotOverlay;
    public GameObject crossOverlay;
    public GameObject plusOverlay;
    public GameObject AROverlay;
    public GameObject SMGOverlay;
    public GameObject SnotGunOverlay;
    public GameObject ARCharacter;
    public GameObject SMGCharacter;
    public GameObject SnotGunCharacter;
    public GameObject TeaBlock;
    public GameObject SeaTeaBlock;

    public LayerMask fmjMask;
    public LayerMask normalMask;
    public AudioSource audioSource;
    public AudioSource reloadSource;
    public AudioClip shootSound;
    public AudioClip equipSound;
    public AudioClip reloadSound;
    private float FireInterval;
    [SerializeField]
    private float shotVolume = 1f;

    private void OnEnable()
    {
        isReloading = false;
        animator.SetBool("CurrentlyReloading", false);
        animator.SetBool("Healing", false);
        FireInterval = Time.time + 1f / fireRate;
        audioSource.PlayOneShot(equipSound, 0.5f);
        player.walkPace = walkPaceSet;
        player.dropForce = dropForceSet;
        player.thrustPower = thrustPowerSet;
    }

    void Update()
    {
            currentMag.text = magCounter + "";
            ammoLeft.text = ammo + "";

            if (Time.time >= FireInterval)
            {
                animator.SetInteger("fire", -1);
            }
            if (isReloading == true)
            {
                return;
            }
            if (magCounter <= 0)
            {
                StartCoroutine(Reload());
                return;
            }
            if (Automatic == true)
            {
                if (Input.GetButton("Fire1") && Time.time >= FireInterval)
                {
                    animator.SetInteger("fire", 2);
                    FireInterval = Time.time + 1f / fireRate;
                    Shoot();
                }
            }
            if (Automatic == false)
            {
                if (Input.GetButtonDown("Fire1") && Time.time >= FireInterval)
                {
                    animator.SetInteger("fire", 2);
                    FireInterval = Time.time + 1f / fireRate;
                    Shoot();
                }
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(Reload());
                return;
            }
    }
    IEnumerator Reload()
    {
        reloadSource.PlayOneShot(reloadSound, 0.4f);
        isReloading = true;

        Debug.Log("Reloading");

        animator.SetBool("CurrentlyReloading", true);

        yield return new WaitForSeconds(timeReload);

        animator.SetBool("CurrentlyReloading", false);

        if (ammo >= 1)
        {
            if (ammo >= (magCapacity - magCounter))
            {
                ammo -= (magCapacity - magCounter);
                magCounter = magCapacity;
            }
            else
            {
                magCounter += ammo;
                ammo = 0;

            }
        }
        else
        {
            animator.Play("GunReload");
        }

        isReloading = false;
    }
    void Shoot()
    {
        audioSource.PlayOneShot(shootSound, shotVolume);
        muzzleFlash.Play();
        magCounter--;
        RaycastHit hit;
        if (FMJ == true)
        {
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, (Mathf.Infinity), ~fmjMask))
            {
                if (hit.collider.tag == BodyTag)
                {
                    PlayerHitServerRpc(hit.collider.name, damage);
                }
                else if (hit.collider.tag == HeadTag)
                {
                    damage = damage * 2f;
                    PlayerHitServerRpc(hit.collider.name, damage);
                }

                //takeDamage target = hit.transform.GetComponent<takeDamage>();
                //if (target != null)
                //{
                //    target.TakeDamage(damage);
                //}
                //bullet contact on animation
                GameObject hitobj = Instantiate(objHit, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(hitobj, 2f);


                EquipAR(hit.transform.name);
                EquipSMG(hit.transform.name);
                EquipSnotgun(hit.transform.name);
                EquipDotCH(hit.transform.name);
                EquipCrossCH(hit.transform.name);
                EquipPlusCH(hit.transform.name);
            }
        }
        else if (FMJ == false)
        {
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, (Mathf.Infinity), ~normalMask))
            {
                if (hit.collider.tag == BodyTag)
                {
                    PlayerHitServerRpc(hit.collider.name, damage);
                }
                else if (hit.collider.tag == HeadTag)
                {
                    damage = damage * 2f;
                    PlayerHitServerRpc(hit.collider.name, damage);
                }

                //bullet contact on animation
                GameObject hitobj = Instantiate(objHit, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(hitobj, 2f);

                SelectTeaServerRpc(hit.transform.name);
                SelectSeaTeaServerRpc(hit.transform.name);

                EquipAR(hit.transform.name);
                EquipSMG(hit.transform.name);
                EquipSnotgun(hit.transform.name);
                EquipDotCH(hit.transform.name);
                EquipCrossCH(hit.transform.name);
                EquipPlusCH(hit.transform.name);
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.GetComponent<PlayerManager>().Health.Value <= 0f)
        {
            kills.AddKill();
        }
    }
    [ServerRpc(RequireOwnership = false)]
    void PlayerHitServerRpc(string userID, float damage)
    {

        Debug.Log(userID + "has been hit");

        PlayerManager user = GameManager.GetUser(userID);
        user.RegisterDamageClientRpc(damage);
    }

    [ServerRpc]
    private void SelectTeaServerRpc(string name)
    {
        if (name == TeaBlock.name)
        {
            playerManager.playerTeam.Value = 0;
            playerManager.SpawnPoint(0);
            GoSpawnPlayerClientRpc(0);
        }
    }

    [ServerRpc]
    private void SelectSeaTeaServerRpc(string name)
    {
        if (name == SeaTeaBlock.name)
        {
            playerManager.playerTeam.Value = 1;
            GoSpawnPlayerClientRpc(1);
        }
    }

    [ClientRpc]
    private void GoSpawnPlayerClientRpc(int team)
    {
        playerManager.SpawnPoint(team);
    }
    private void EquipAR(string name)
    {
        if (name == AR.name)
        {
            classSelected = 0;
            Debug.Log("AR equiped");
            SnotGunOverlay.SetActive(false);
            SnotGunCharacter.SetActive(false);
            SMGOverlay.SetActive(false);
            SMGCharacter.SetActive(false);
            AROverlay.SetActive(true);
            ARCharacter.SetActive(true);
            ARScript.SetActive(true);
            SMGScript.SetActive(false);
            SnotgunScript.SetActive(false);
        }
    }

    private void EquipSMG(string name)
    {
        if (name == SMG.name)
        {
            classSelected = 1;
            Debug.Log("SMG equiped");
            SnotGunOverlay.SetActive(false);
            SnotGunCharacter.SetActive(false);
            SMGOverlay.SetActive(true);
            SMGCharacter.SetActive(true);
            AROverlay.SetActive(false);
            ARCharacter.SetActive(false);
            ARScript.SetActive(false);
            SMGScript.SetActive(true);
            SnotgunScript.SetActive(false);
        }
    }

    private void EquipSnotgun(string name)
    {
        if (name == Snotgun.name)
        {
            classSelected = 2;
            Debug.Log("Snotgun equiped");
            SnotGunOverlay.SetActive(true);
            SnotGunCharacter.SetActive(true);
            SMGOverlay.SetActive(false);
            SMGCharacter.SetActive(false);
            AROverlay.SetActive(false);
            ARCharacter.SetActive(false);
            ARScript.SetActive(false);
            SMGScript.SetActive(false);
            SnotgunScript.SetActive(true);
        }
    }

    private void EquipDotCH(string name)
    {
        if (name == DotCH.name)
        {
            dotOverlay.SetActive(true);
            crossOverlay.SetActive(false);
            plusOverlay.SetActive(false);
        }
    }

    private void EquipCrossCH(string name)
    {
        if (name == CrossCH.name)
        {
            crossOverlay.SetActive(true);
            dotOverlay.SetActive(false);
            plusOverlay.SetActive(false);
        }
    }

    private void EquipPlusCH(string name)
    {
        if (name == PlusCH.name)
        {
            plusOverlay.SetActive(true);
            dotOverlay.SetActive(false);
            crossOverlay.SetActive(false);
        }
    }
}
