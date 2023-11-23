using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public bool canFire;

    [SerializeField] GameObject bullet_prefab, powerful_bullet_prefab;
    [SerializeField] Transform fire_point;
    [SerializeField] float fireRate;
    void Start()
    {
        canFire = true;
        StartCoroutine(Fire());
    }

    void Update()
    {
        
    }

    IEnumerator Fire()
    {
        while (true)
        {
            yield return new WaitForSeconds(100 / fireRate);
            SpawnBullet();
        }
    }

    void SpawnBullet()
    {
        if (canFire)
        {
            if (GameManager.instance.GetBulletDamage() > 40)
            {
                GameObject _bullet = Instantiate(powerful_bullet_prefab, fire_point.position, Quaternion.Euler(90, 0, 0));
            }

            else
            {
                GameObject _bullet1 = Instantiate(bullet_prefab, fire_point.position, Quaternion.Euler(90, 0, 0));
            }
        }
    }

    public void SetFireRate(float _val) { fireRate += _val; }
    public float GetFireRate() { return fireRate; }
}
