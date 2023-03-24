using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> targets;
    [SerializeField] private float _spawnRate = 2;
    [SerializeField] private GameObject _spawnContainer;
    private bool _isGameActive = true;

    //Scoreboard
    [Header("Score Board")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private int _scoreToLevel = 50;
    private int _score;
    private int _level = 1;

    //UI
    [Header("UI")]
    [SerializeField] private GameObject _gameOverText;
    [SerializeField] private GameObject _levelUpText;

    [Header("Sound")]
    [SerializeField] private AudioClip[] _biteSounds;
    [SerializeField] private AudioClip _bombSound;
    private AudioSource _source;

    // Start is called before the first frame update
    void Start()
    {
        _source = GetComponent<AudioSource>();
        StartCoroutine(SpawnTarget());
        UpdateScoreBoard(0);
    }

    IEnumerator SpawnTarget()
    {
        while (_isGameActive)
        {
            yield return new WaitForSeconds(_spawnRate);
            int index = Random.Range(0, targets.Count);
            GameObject newSpawn = Instantiate(targets[index]);
            newSpawn.transform.parent = _spawnContainer.transform;
        }
    }

    public void UpdateScoreBoard(int scoreToAdd)
    {
        _score += scoreToAdd;
        scoreText.text = "Score: " + _score;

        if (_score >= _scoreToLevel && _score != 0)
        {
            _level++;
            _scoreToLevel += _scoreToLevel;
            levelText.text = "Level: " + _level;
            _spawnRate *= 0.8f;
            StartCoroutine(LevelTextFlash());
        }
    }

    IEnumerator LevelTextFlash()
    {
        for (int i = 0; i < 5; i++)
        {
            _levelUpText.gameObject.SetActive(true);
            yield return new WaitForSeconds(.5f);
            _levelUpText.gameObject.SetActive(false);
            yield return new WaitForSeconds(.5f);
        }
    }

    public void GameOver()
    {

        _gameOverText.gameObject.SetActive(true);
        _isGameActive = false;
    }

    public void PlayBiteSound()
    {
        _source.pitch = Random.Range(0.9f, 1.1f);
        _source.volume = 1;
        int index = Random.Range(0, _biteSounds.Length);
        _source.PlayOneShot(_biteSounds[index]);
    }

    public void PlayBombSound()
    {
        _source.pitch = Random.Range(0.9f, 1.1f);
        _source.volume = 0.4f;
        _source.PlayOneShot(_bombSound);
    }
}
