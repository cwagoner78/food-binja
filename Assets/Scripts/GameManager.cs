using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    [Header("Spawn Items")]
    [SerializeField] private List<GameObject> targets;
    [SerializeField] private float _spawnRate = 2;
    [SerializeField] private GameObject _spawnContainer;
    public bool _isGameActive = true;

    //UI
    [Header("UI")]
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _scoreBoard;
    [SerializeField] private GameObject _gameOver;
    [SerializeField] private GameObject _levelUpText;

    //Scoreboard
    [Header("Score Board")]
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _highScoreTextE;
    [SerializeField] private TextMeshProUGUI _highScoreTextM;
    [SerializeField] private TextMeshProUGUI _highScoreTextH;
    [SerializeField] private TextMeshProUGUI _newHighScoreText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private int _scoreToLevel = 50;
    private int _score;
    private int _level = 1;
    private int _difficulty;

    private AudioSource _chompSound;


    void Start()
    {
        SetHighScores();
        _chompSound = GameObject.Find("ChompSource").GetComponent<AudioSource>();
    }

    void Update()
    {
        UpdateLevel();
        if (Input.GetKeyDown(KeyCode.R)) ResetHighScores();
        if (Input.GetMouseButtonDown(0))
        {
            _chompSound.pitch = Random.Range(0.9f, 1.1f);
            _chompSound.Play();
        } 
    }

    void SetHighScores()
    {
        _highScoreTextE.text = "Easy: " + PlayerPrefs.GetInt("High Score Easy");
        _highScoreTextM.text = "Medium: " + PlayerPrefs.GetInt("High Score Medium");
        _highScoreTextH.text = "Hard: " + PlayerPrefs.GetInt("High Score Hard");
    }

     void ResetHighScores()
    {
        PlayerPrefs.DeleteAll();
        _highScoreTextE.text = "Easy: " + 0;
        _highScoreTextM.text = "Medium: " + 0;
        _highScoreTextH.text = "Hard: " + 0;

    }

    public void StartGame(int difficulty)
    {
        _mainMenu.SetActive(false);
        if (difficulty == 0) //easy
        {
            _difficulty = 0;
            _spawnRate = 3;
        }
        else if (difficulty == 1) // medium
        {
            _difficulty = 1;
            _spawnRate = 2;

        }
        else if (difficulty == 2) //hard
        {
            _difficulty = 2;
            _spawnRate = 1;
        }
        
        UpdateScoreBoard(0);
        _scoreBoard.gameObject.SetActive(true);
        StartCoroutine(SpawnTarget());
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

        if (_score <= 0) _score = 0;
        _scoreText.text = "Score: " + _score;

        if (_difficulty == 0)
        {
            if (!_isGameActive)
            {
                if (_score > PlayerPrefs.GetInt("High Score Easy")) StartCoroutine(NewHighScoreFlash());
                if (PlayerPrefs.GetInt("High Score Easy") < _score) PlayerPrefs.SetInt("High Score Easy", _score);
                _highScoreTextE.text = "Easy: " + PlayerPrefs.GetInt("High Score Easy");
            } 
        }
        else if (_difficulty == 1)
        {
            if (!_isGameActive)
            {
                if (_score > PlayerPrefs.GetInt("High Score Medium")) StartCoroutine(NewHighScoreFlash());
                if (PlayerPrefs.GetInt("High Score Medium") < _score) PlayerPrefs.SetInt("High Score Medium", _score);
                _highScoreTextM.text = "Medium: " + PlayerPrefs.GetInt("High Score Medium");
            } 
        }
        else
        {
            if (!_isGameActive)
            {
                if (_score > PlayerPrefs.GetInt("High Score Hard")) StartCoroutine(NewHighScoreFlash());
                if (PlayerPrefs.GetInt("High Score Hard") < _score) PlayerPrefs.SetInt("High Score Hard", _score);
                _highScoreTextH.text = "Hard: " + PlayerPrefs.GetInt("High Score Hard");
            } 
        }
    }

    public void UpdateLevel()
    {
        if (_score >= _scoreToLevel && _score != 0)
        {
            _level++;
            _scoreToLevel += _scoreToLevel;
            _levelText.text = "Level: " + _level;
            _spawnRate *= 0.8f;
            StartCoroutine(LevelTextFlash());
        }
    }

    IEnumerator NewHighScoreFlash()
    {
        for (int i = 0; i < 5; i++)
        {
            _newHighScoreText.gameObject.SetActive(true);
            yield return new WaitForSeconds(.25f);
            _newHighScoreText.gameObject.SetActive(false);
            yield return new WaitForSeconds(.25f);
        }
    } 

    IEnumerator LevelTextFlash()
    {
        for (int i = 0; i < 5; i++)
        {
            _levelUpText.gameObject.SetActive(true);
            yield return new WaitForSeconds(.25f);
            _levelUpText.gameObject.SetActive(false);
            yield return new WaitForSeconds(.25f);
        }
    }
    public void GameOver()
    {
        _gameOver.gameObject.SetActive(true);
        _isGameActive = false;
    }

    public void Restartgame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
