using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    bool died = false;
    float health, _temp;

    GameObject player, enemy_canvas;
    public float rot_speed, move_speed;

    private void Start()
    {
        enemy_canvas = transform.GetChild(1).gameObject;
        health = 1000f;
        _temp = health;
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        enemy_canvas.transform.LookAt(Camera.main.transform);
        if (health <= 0 && !died)
        {
            died = true;
            Destroy(transform.GetComponent<BoxCollider>());
            ParticleSystem poof = Instantiate(GameManager.instance.enemy_poof_effect, transform.position, Quaternion.identity);
            poof.Play();

            float duration = poof.main.duration + poof.main.startLifetime.constant;
            Destroy(poof.gameObject, duration);

            FinalWaveManager.instance.killed_enemy++;

            transform.DOScale(.01f, .75f).SetEase(Ease.OutElastic).OnComplete(() =>
            {
                Destroy(this.gameObject);
            });
        }

        RotateToPlayer();
        Move();
    }

    void RotateToPlayer()
    {
        Vector3 pospos = new Vector3(0, 2, 490);
        Quaternion rotrot = Quaternion.LookRotation(-(pospos - transform.position));
        transform.rotation = Quaternion.Slerp(transform.rotation, rotrot, rot_speed * Time.deltaTime);
    }

    void Move()
    {
        transform.position -= transform.forward * move_speed * Time.deltaTime;
        transform.GetChild(0).Rotate(-Vector3.right, rot_speed * 2 * Time.deltaTime);
    }


    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.layer == 6)
        {
            ParticleSystem splash = Instantiate(GameManager.instance.enemy_bullet_splash, coll.transform.position, Quaternion.identity);
            splash.Play();
            
            float duration = splash.main.duration + splash.main.startLifetime.constant;
            Destroy(splash.gameObject, duration);

            transform.DOShakeScale(0.1f, .04f, 0, 1f);

            TakeDamage(GameManager.instance.GetBulletDamage());
            Destroy(coll.gameObject);
        }
    }

    void TakeDamage(int _val)
    {
        if (health > 0)
        {
            health -= _val;
            enemy_canvas.transform.GetChild(0).GetComponent<Image>().fillAmount -= (_val / _temp);
        }
    }
}
