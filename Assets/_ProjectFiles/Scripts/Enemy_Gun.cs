using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Gun : MonoBehaviour {

    public bool ShootPistol = false;
    public bool _Shooting { get { return isShooting; } }

    Animator anim;

    [SerializeField]
    bool isShooting;
    bool isAiming;
    bool outOfAmmo;

    float lastFired;

    //Ammo left
    public int currentAmmo;

    [System.Serializable]
    public class shootSettings
    {
        [Header("Ammo")]
        //Total ammo
        public int ammo;

        [Header("Fire Rate & Bullet Settings")]
        public bool automaticFire;
        public float fireRate;

        [Space(10)]

        //How far the raycast will reach
        public float bulletDistance = 500.0f;
        //How much force will be applied to rigidbodies 
        //by the bullet raycast
        public float bulletForce = 500.0f;

        [Header("Shotgun Settings")]
        public bool useShotgunSpread;
        //How big the pellet spread area will be
        public float spreadSize = 2.0f;
        //How many pellets to shoot
        public int pellets = 30;
    }
    public shootSettings ShootSettings;

    [System.Serializable]
    public class reloadSettings
    {
        [Header("Reload Settings")]
        public bool casingOnReload;
        public float casingDelay;

        [Header("Bullet In Mag")]
        public bool hasBulletInMag;
        public Transform[] bulletInMag;
        public float enableBulletTimer = 1.0f;

        [Header("Bullet Or Shell Insert")]
        //If the weapon uses a bullet/shell insert style reload
        //Used for the bolt action sniper and pump shotgun for example
        public bool usesInsert;

    }
    public reloadSettings ReloadSettings;

    //All Components
    [System.Serializable]
    public class components
    {
        [Header("Muzzleflash Holders")]
        public bool useMuzzleflash = false;
        public GameObject sideMuzzle;
        public GameObject topMuzzle;
        public GameObject frontMuzzle;
        //Array of muzzleflash sprites
        public Sprite[] muzzleflashSideSprites;

        [Header("Light Front")]
        public bool useLightFlash = false;
        public Light lightFlash;

        [Header("Particle System")]
        public bool playSmoke = false;
        public ParticleSystem smokeParticles;
        public bool playSparks = false;
        public ParticleSystem sparkParticles;
        public bool playTracers = false;
        public ParticleSystem bulletTracerParticles;
    }
    public components Components;

    //All weapon types
    [System.Serializable]
    public class prefabs
    {
        [Header("Prefabs")]
        public Transform casingPrefab;

    }
    public prefabs Prefabs;

    [System.Serializable]
    public class spawnpoints
    {
        [Header("Spawnpoints")]
        //Array holding casing spawn points 
        //(some weapons use more than one casing spawn)
        public Transform[] casingSpawnPoints;
        //Bullet raycast start point
        public Transform bulletSpawnPoint;
    }
    public spawnpoints Spawnpoints;

    [System.Serializable]
    public class audioClips
    {
        [Header("Audio Source")]

        public AudioSource mainAudioSource;

        [Header("Audio Clips")]

        //All audio clips
        public AudioClip shootSound;
        public AudioClip reloadSound;
    }
    public audioClips AudioClips;

    public bool noSwitch = false;

    void Awake()
    {
        ShootSettings.ammo = Random.Range(3, 8);

        //Set the animator component
        anim = GetComponent<Animator>();

        //Set the ammo count
        RefillAmmo();
    }

    private void Start()
    {
        //Hide the muzzleflashes
        Components.sideMuzzle.GetComponent<SpriteRenderer>().enabled = false;
        Components.topMuzzle.GetComponent<SpriteRenderer>().enabled = false;
        Components.frontMuzzle.GetComponent<SpriteRenderer>().enabled = false;

        //Disable the light flash, disable for melee weapons and grenade
        Components.lightFlash.GetComponent<Light>().enabled = false;
    }

    void Update()
    {
        //Check which animation 
        //is currently playing
        AnimationCheck();

        //------------------------------------------------마우스 좌클릭 시작---------------------------------------------------------

        //Left click (if automatic fire is false)
        if (ShootPistol && !ShootSettings.automaticFire
            //Disable shooting while running and jumping
            && !outOfAmmo && !isShooting)
        {

            //If shotgun shoot is true
            if (ShootSettings.useShotgunSpread == true)
            {
                ShotgunShoot();
            }
            //If projectile weapon, grenade and melee weapons is false
            else if (!ShootSettings.useShotgunSpread)
            {
                Shoot();
                //If projectile weapon is true
            }
        }

        //Left click hold (if automatic fire is true)
        if (ShootPistol && ShootSettings.automaticFire == true
            //Disable shooting while running and jumping
            && !outOfAmmo && !isShooting )
        {
            //Shoot automatic
            if (Time.time - lastFired > 1 / ShootSettings.fireRate)
            {
                Shoot();
                lastFired = Time.time;
            }
        }


        //------------------------------------------------마우스 좌클릭 끝---------------------------------------------------------

        //If out of ammo
        if (currentAmmo == 0)
        {
            outOfAmmo = true;
            //if ammo is higher than 0
        }
        else if (currentAmmo > 0)
        {
            outOfAmmo = false;
        }
    }

    //Muzzleflash
    IEnumerator MuzzleFlash()
    {
        //Show muzzleflash if useMuzzleFlash is true
        if (Components.useMuzzleflash == true)
        {
            //Show a random muzzleflash from the array
            Components.sideMuzzle.GetComponent<SpriteRenderer>().sprite = Components.muzzleflashSideSprites
                [Random.Range(0, Components.muzzleflashSideSprites.Length)];
            Components.topMuzzle.GetComponent<SpriteRenderer>().sprite = Components.muzzleflashSideSprites
                [Random.Range(0, Components.muzzleflashSideSprites.Length)];

            //Show the muzzleflashes
            Components.sideMuzzle.GetComponent<SpriteRenderer>().enabled = true;
            Components.topMuzzle.GetComponent<SpriteRenderer>().enabled = true;
            Components.frontMuzzle.GetComponent<SpriteRenderer>().enabled = true;
        }

        //Enable the light flash if true
        if (Components.useLightFlash == true)
        {
            Components.lightFlash.GetComponent<Light>().enabled = true;
        }

        //Play smoke particles if true
        if (Components.playSmoke == true)
        {
            Components.smokeParticles.Play();
        }
        //Play spark particles if true
        if (Components.playSparks == true)
        {
            Components.sparkParticles.Play();
        }
        //Play bullet tracer particles if true
        if (Components.playTracers == true)
        {
            Components.bulletTracerParticles.Play();
        }

        //Show the muzzleflash for 0.02 seconds
        yield return new WaitForSeconds(0.02f);

        if (Components.useMuzzleflash == true)
        {
            //Hide the muzzleflashes
            Components.sideMuzzle.GetComponent<SpriteRenderer>().enabled = false;
            Components.topMuzzle.GetComponent<SpriteRenderer>().enabled = false;
            Components.frontMuzzle.GetComponent<SpriteRenderer>().enabled = false;
        }

        //Disable the light flash if true
        if (Components.useLightFlash == true)
        {
            Components.lightFlash.GetComponent<Light>().enabled = false;
        }


    }

    //Shotgun shoot
    void ShotgunShoot()
    {
        anim.SetTrigger("Shoot");

        //Remove 1 bullet
        currentAmmo -= 1;

        //Play shoot sound
        AudioClips.mainAudioSource.clip = AudioClips.shootSound;
        AudioClips.mainAudioSource.Play();

        //Start casing instantiate
        if (!ReloadSettings.casingOnReload)
        {
            StartCoroutine(CasingDelay());
        }

        //Show the muzzleflash
        StartCoroutine(MuzzleFlash());

        //Send out shotgun raycast with set amount of pellets
        for (int i = 0; i < ShootSettings.pellets; ++i)
        {

            float randomRadius = Random.Range
                (0, ShootSettings.spreadSize);
            float randomAngle = Random.Range
                (0, 2 * Mathf.PI);

            //Raycast direction
            Vector3 direction = new Vector3(
                randomRadius * Mathf.Cos(randomAngle),
                randomRadius * Mathf.Sin(randomAngle),
                15);

            direction = transform.TransformDirection(direction.normalized);

            RaycastHit hit;
            if (Physics.Raycast(Spawnpoints.bulletSpawnPoint.transform.position, direction,
                                 out hit, ShootSettings.bulletDistance))
            {

                //If a rigibody is hit, add bullet force to it
                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddTorque(direction * ShootSettings.bulletForce, ForceMode.Impulse);
                    hit.rigidbody.AddForce(direction * ShootSettings.bulletForce, ForceMode.Impulse);
                }

            }
        }
    }

    //Shoot
    void Shoot()
    {


        anim.SetTrigger("Shoot");
        

        //Remove 1 bullet
        currentAmmo -= 1;

        //Play shoot sound
        AudioClips.mainAudioSource.clip = AudioClips.shootSound;
        AudioClips.mainAudioSource.Play();

        //Start casing instantiate
        if (!ReloadSettings.casingOnReload)
        {
            StartCoroutine(CasingDelay());
        }

        //Show the muzzleflash
        StartCoroutine(MuzzleFlash());

        float randomRadius = Random.Range
                (0, ShootSettings.spreadSize);
        float randomAngle = Random.Range
            (0, 2 * Mathf.PI);

        //Raycast direction
        Vector3 direction = new Vector3(
            randomRadius * Mathf.Cos(randomAngle),
            randomRadius * Mathf.Sin(randomAngle),
            15);

        direction = transform.TransformDirection(direction.normalized);

        RaycastHit hit;
        if (Physics.Raycast(Spawnpoints.bulletSpawnPoint.transform.position, direction,
                             out hit, ShootSettings.bulletDistance))
        {

            //If a rigibody is hit, add bullet force to it
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddTorque(direction * ShootSettings.bulletForce, ForceMode.Impulse);
                hit.rigidbody.AddForce(direction * ShootSettings.bulletForce, ForceMode.Impulse);
            }
        }
    }

    //Refill ammo
    void RefillAmmo()
    {

        currentAmmo = ShootSettings.ammo;
    }

    IEnumerator CasingDelay()
    {

        //Wait set amount of time for casing to spawn
        yield return new WaitForSeconds(ReloadSettings.casingDelay);
        //Spawn a casing at every casing spawnpoint
        for (int i = 0; i < Spawnpoints.casingSpawnPoints.Length; i++)
        {
            Instantiate(Prefabs.casingPrefab,
                         Spawnpoints.casingSpawnPoints[i].transform.position,
                         Spawnpoints.casingSpawnPoints[i].transform.rotation);
        }
    }

    //Check current animation playing
    void AnimationCheck()
    {

        //Check if shooting
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Fire"))
        {
            isShooting = true;
        }
        else
        {
            isShooting = false;
        }

    }
}
