using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;
using DG.Tweening;

public class Gate : MonoBehaviour
{

    public enum Mode
    {
        firerate,
        firepower
    }
    [SerializeField] Mode mode = new Mode();


    bool collected;


    [SerializeField] Image healthBar;
    [SerializeField] private float health;
    [SerializeField] private int effect;

    [SerializeField] Transform coin_spawn_point;



    private void Start()
    {

    }

    private void Update()
    {
        if (healthBar.fillAmount <= 0f && !collected)
        {
            switch (mode)
            {
                case Mode.firerate:
                    collected = true;
                    GameObject _coin1 = Instantiate(GameManager.instance.coin, coin_spawn_point.position, Quaternion.Euler(90, 0, 0));
                    GameManager.instance.AddCoinToList(_coin1);
                    GameObject.Find("Player").GetComponent<PlayerMovement>().rifle.GetComponent<Gun>().SetFireRate(effect * 5);

                    Vector3[] pathPoints = new Vector3[] { coin_spawn_point.position, new Vector3(-8, 3f, _coin1.transform.position.z), new Vector3(-8, 1f, _coin1.transform.position.z) };
                    _coin1.transform.DOPath(pathPoints, .5f, PathType.CatmullRom).SetOptions(false).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        _coin1.transform.SetParent(GameObject.Find("Forwarder").transform);
                        _coin1.GetComponent<Coin>().moving = true;
                    });
                    break;

                case Mode.firepower:
                    collected = true;
                    GameObject _coin2 = Instantiate(GameManager.instance.coin, coin_spawn_point.position, Quaternion.Euler(90, 0, 0));
                    GameManager.instance.AddCoinToList(_coin2);
                    GameManager.instance.SetBulletDamage(GameManager.instance.GetBulletDamage() + effect);

                    Vector3[] pathPoints1 = new Vector3[] { coin_spawn_point.position, new Vector3(-8, 3f, _coin2.transform.position.z), new Vector3(-8, 1f, _coin2.transform.position.z) };
                    _coin2.transform.DOPath(pathPoints1, .5f, PathType.CatmullRom).SetOptions(false).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        _coin2.transform.SetParent(GameObject.Find("Forwarder").transform);
                        _coin2.GetComponent<Coin>().moving = true;
                    });
                    break;

                default:
                    break;
            }

            if (collected)
            {
                transform.DOMoveY(-1f, 3f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    gameObject.SetActive(this.gameObject);
                });
                transform.DORotate(new Vector3(90, 0, 0), 0.5f).SetEase(Ease.Linear);
            }

        }
    }

    public void SetHealth(float _val)
    {
        if (health > 0)
            health -= _val;
        healthBar.fillAmount -= (_val / health);
    }

    public float GetHealth(){ return health; }

    private void OnTriggerEnter(Collider col)
    {
        switch (col.gameObject.layer)
        {
            case 6:
                if (!collected)
                {
                    ParticleSystem splash_particle = Instantiate(GameManager.instance.bullet_splash, col.transform.position, Quaternion.Euler(0, -180, 0));
                    splash_particle.Play();

                    float duration = splash_particle.main.duration + splash_particle.main.startLifetime.constant;
                    Destroy(splash_particle.gameObject, duration);

                    SetHealth(GameManager.instance.GetBulletDamage());

                    Destroy(col.gameObject);
                }
                break;



            default:
                break;
        }
    }
}
