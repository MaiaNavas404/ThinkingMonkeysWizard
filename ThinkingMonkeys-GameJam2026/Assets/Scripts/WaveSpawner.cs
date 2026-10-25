using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] private GameObject _enemyPrefab;

    [Header("Waves")]
    [SerializeField] private int _wavesAmount = 5;
    [SerializeField] private int _enemiesInFirstWave = 3;
    [SerializeField] private int _extraEnemiesPerWave = 2;
    [SerializeField] private float _delayBetweenWaves = 5f;

    [Header("Spawn Area")]
    [SerializeField] private Vector2 _spawnAreaSize = new Vector2(75f, 75f);
    [SerializeField] private float _spawnHeight = 50f;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private int _spawnAttemptsPerEnemy = 20;


    [Header("UI")]
    [SerializeField] private TMP_Text _waveText;
    [SerializeField] private string _winSceneName = "WinScene";
    private readonly List<GameObject> _enemies = new List<GameObject>();

    private int _currentWave = 0;
    private Transform _player;

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject == null)
        {
            Debug.LogError("WaveSpawner: No object with the Player tag was found.");
            enabled = false;
            return;
        }

        _player = playerObject.transform;

        if (_enemyPrefab == null)
        {
            Debug.LogError("WaveSpawner: Please assign an enemy prefab.");
            enabled = false;
            return;
        }

        StartCoroutine(WaveRoutine());
    }

    private IEnumerator WaveRoutine()
    {
        while (_currentWave < _wavesAmount)
        {
            _currentWave++;

            int enemiesToSpawn =
                _enemiesInFirstWave +
                (_currentWave - 1) * _extraEnemiesPerWave;

            // Update the wave counter.
            if (_waveText != null)
            {
                _waveText.text = $"Wave {_currentWave} / {_wavesAmount}";
            }

            Debug.Log(
                $"Wave {_currentWave}/{_wavesAmount} starting! " +
                $"Enemies: {enemiesToSpawn}"
            );

            _enemies.Clear();

            // Spawn every enemy in this wave immediately.
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                SpawnEnemy();
            }

            // Wait until all enemies from this wave are dead.
            while (HasLivingEnemies())
            {
                yield return null;
            }

            Debug.Log($"Wave {_currentWave} completed!");

            // Wait after each wave, including the final wave.
            yield return new WaitForSeconds(_delayBetweenWaves);
        }

        // All waves are completed. Load the win screen.
        SceneManager.LoadScene(_winSceneName);
    }

    private void SpawnEnemy()
    {
        for (int attempt = 0; attempt < _spawnAttemptsPerEnemy; attempt++)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-_spawnAreaSize.x / 2f, _spawnAreaSize.x / 2f),
                0f,
                Random.Range(-_spawnAreaSize.y / 2f, _spawnAreaSize.y / 2f)
            );

            Vector3 rayOrigin =
                transform.position + randomOffset + Vector3.up * _spawnHeight;

            if (Physics.Raycast(
                rayOrigin,
                Vector3.down,
                out RaycastHit hit,
                _spawnHeight * 2f,
                _groundMask))
            {
                GameObject enemy = Instantiate(
                    _enemyPrefab,
                    hit.point,
                    Quaternion.identity
                );

                // Choose a random type before EnemyMovement.Start runs.
                EnemyMovement movement = enemy.GetComponent<EnemyMovement>();

                if (movement != null)
                {
                    movement.enemyType = GetRandomEnemyType();
                }
                else
                {
                    Debug.LogWarning(
                        "The enemy prefab has no EnemyMovement component."
                    );
                }

                _enemies.Add(enemy);
                return;
            }
        }

        Debug.LogWarning(
            "Could not find ground for an enemy spawn. " +
            "Check the Ground Mask and Spawn Area."
        );
    }

    private EnemyMovement.EnemyType GetRandomEnemyType()
    {
        int randomIndex = Random.Range(0, 3);

        return (EnemyMovement.EnemyType)randomIndex;
    }

    private bool HasLivingEnemies()
    {
        // Remove enemies that have been destroyed.
        _enemies.RemoveAll(enemy => enemy == null);

        return _enemies.Count > 0;
    }
}
