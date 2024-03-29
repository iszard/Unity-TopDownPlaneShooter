﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ZenvaVR;

public class GameController : MonoBehaviour
{
    public Player player;
    public GameObject gameCamera;

    public ObjectPool enemyPool;
    public float enemySpawnInterval = 1f;
    public float horizontalLimit = 2.8f;

    public GameObject fuelPrefab;
    public float fuelSpawnInterval = 9f;
    public float fuelDecreaseSpeed = 5f;


    public Text scoreText;
    public Text fuelText;

    private float enemySpawnTimer;
    private float fuelSpawnTimer;
    private float restartTimer = 3f;

    private int score;
    private float fuel = 100f;

    // Start is called before the first frame update
    void Start()
    {
        enemySpawnTimer = enemySpawnInterval;
        player.OnFuel += OnFuel;

        fuelSpawnTimer = Random.Range(0f, fuelSpawnInterval);
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            // Add Enemies
            enemySpawnTimer -= Time.deltaTime;
            if (enemySpawnTimer <= 0)
            {
                enemySpawnTimer = enemySpawnInterval;

                GameObject enemyInstance = enemyPool.GetObj();
                enemyInstance.transform.SetParent(transform);
                enemyInstance.transform.position = new Vector2(
                    Random.Range(-horizontalLimit, horizontalLimit),
                    player.transform.position.y + Screen.height / 100f
                    );
                enemyInstance.GetComponent<Enemy>().OnKill += OnEmenyKill;
            }

            fuelSpawnTimer -= Time.deltaTime;
            if (fuelSpawnTimer <= 0)
            {
                fuelSpawnTimer = fuelSpawnInterval;

                GameObject fuelInstance = Instantiate(fuelPrefab);
                fuelInstance.transform.SetParent(transform);
                fuelInstance.transform.position = new Vector2(
                    Random.Range(-horizontalLimit, horizontalLimit),
                    player.transform.position.y + Screen.height / 100f
                    );
            }

            // Alter Fuel
            fuel -= Time.deltaTime * fuelDecreaseSpeed;
            fuelText.text = "Fuel: " + (int)fuel;
            if (fuel <= 0)
            {
                fuelText.text = "Fuel: 0";
                Destroy(player.gameObject);
            }
        }
        else
        {
            restartTimer -= Time.deltaTime;
            if (restartTimer <= 0f)
            {
                SceneManager.LoadScene("Game");
            }
        }

        // Delete Enemies
        foreach(Enemy enemy in GetComponentsInChildren<Enemy>())
        {
            if(gameCamera.transform.position.y - enemy.transform.position.y > Screen.height / 100f)
            {
                enemy.gameObject.SetActive(false);
            }
        }
    }

    void OnEmenyKill()
    {
        score += 25;
        scoreText.text = "Score: " + score;
    }

    void OnFuel()
    {
        fuel = 100f;
    }
}
