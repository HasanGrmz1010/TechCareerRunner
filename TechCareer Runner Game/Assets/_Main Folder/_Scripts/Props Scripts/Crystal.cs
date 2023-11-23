using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Crystal : MonoBehaviour
{
    float turn_speed = 50f;
    public int health = 200;
    int _temp;
    bool collected = false;
    [SerializeField] Transform coin_spawn_point;

    private void Start()
    {
        _temp = health;
    }

    private void Update()
    {
        if (health <= 0 && !collected)
        {
            collected = true;

            GameObject _coin1 = Instantiate(GameManager.instance.coin, coin_spawn_point.position, Quaternion.Euler(90, 0, 0));
            GameManager.instance.AddCoinToList(_coin1);
            Vector3[] pathPoints = new Vector3[] { coin_spawn_point.position, new Vector3(-8, 3f, _coin1.transform.position.z), new Vector3(-8, 1f, _coin1.transform.position.z) };
            _coin1.transform.DOPath(pathPoints, .5f, PathType.CatmullRom).SetOptions(false).SetEase(Ease.Linear).OnComplete(() =>
            {
                _coin1.transform.SetParent(GameObject.Find("Forwarder").transform);
                _coin1.GetComponent<Coin>().moving = true;
            });

            ParticleSystem poof_effect = Instantiate(GameManager.instance.gem_poof_effect, transform.position, Quaternion.identity);
            poof_effect.Play();

            float duration = poof_effect.main.duration + poof_effect.main.startLifetime.constant;
            Destroy(poof_effect.gameObject, duration);

            this.gameObject.SetActive(false);
        }
        else
        {
            transform.Rotate(Vector3.forward, turn_speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.layer)
        {
            case 6:
                ParticleSystem splash_particle = Instantiate(GameManager.instance.bullet_splash, other.transform.position, Quaternion.Euler(0, -180, 0));
                splash_particle.Play();

                float duration = splash_particle.main.duration + splash_particle.main.startLifetime.constant;
                Destroy(splash_particle.gameObject, duration);

                SetHealth(GameManager.instance.GetBulletDamage());

                Destroy(other.gameObject);
                break;

            default:
                break;
        }
    }

    public void SetHealth(int _val) {
        if (health > 0)
        {
            health -= _val;
            if (health < (_temp * .6f) && health > (_temp * .3f)) { transform.DOScale(1f, .25f).SetEase(Ease.InOutCirc); }
            else if (health < (_temp * .3f) && health > (_temp * .15f)) { transform.DOScale(.75f, .25f).SetEase(Ease.InOutCirc); }
            else if (health <= 0) { transform.DOScale(0f, .25f).SetEase(Ease.InOutCirc); }
        }
    }

    public int GetHealth() { return health; }
}
