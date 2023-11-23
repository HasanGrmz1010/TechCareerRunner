using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    List<GameObject> collectedCoins = new List<GameObject>();

    public ParticleSystem bullet_splash;
    public ParticleSystem gem_poof_effect;
    public ParticleSystem coin_poof_effect;
    public ParticleSystem enemy_bullet_splash;
    public ParticleSystem enemy_poof_effect;
    public GameObject coin;

    public static GameManager instance = new GameManager();

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);

        else
            instance = this;
    }

    public enum GameState
    {
        Running,
        GameOver,
        Finished,
        Win
    }
    public GameState g_state = new GameState();

    private int bullet_damage = 20;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

 
    public void AddCoinToList(GameObject _coin) { collectedCoins.Add(_coin); }
    public List<GameObject> GetCoinList() { return collectedCoins; }
    public void SetBulletDamage(int _val) { bullet_damage = _val; }
    public int GetBulletDamage() { return bullet_damage; }
}
