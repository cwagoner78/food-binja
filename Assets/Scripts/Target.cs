using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Rigidbody _targetRB;
    private GameManager _gameManager;
    private AudioManager _audioManager;
    [SerializeField] int _pointValue = 1;

    [Header("Effects")]
    [SerializeField] ParticleSystem _explosionParticles;
    [SerializeField] GameObject _floatingPoints;
    [SerializeField] GameObject _sensor;

    public float _minSpeed = 12;
    public float _maxSpeed = 16;
    public float _maxTorque = 10;
    public float _xRange = 4;
    public float _ySpawnPos = 6;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>(); 
        _audioManager = FindObjectOfType<AudioManager>();
        _targetRB = GetComponent<Rigidbody>();
        _targetRB.AddForce(RandomForce(), ForceMode.Impulse);
        _targetRB.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);
        transform.position = RandomSpawnPos();
    }

    private void Update()
    {
        if (!_gameManager._isGameActive) Destroy(gameObject);
    }

    private void OnMouseDown()
    {
        if (tag == "Good")
        {
            _floatingPoints.GetComponentInChildren<TextMesh>().text = _pointValue.ToString();
            _floatingPoints.GetComponentInChildren<TextMesh>().color = Color.green;
            GameObject instance = Instantiate(_floatingPoints, transform.position, Quaternion.identity);
            _audioManager.PlayBiteSound();
            Destroy(instance, 3);
        }
        
        if (gameObject.CompareTag("Skull"))
        {
            _audioManager.PlaySkullSound();
            _floatingPoints.GetComponentInChildren<TextMesh>().text = _pointValue.ToString();
            _floatingPoints.GetComponentInChildren<TextMesh>().color = Color.red;
            GameObject instance = Instantiate(_floatingPoints, transform.position, Quaternion.identity);
            Destroy(instance, 3);
            //_gameManager.GameOver();
        }

        if (gameObject.CompareTag("Bad"))
        {
            _audioManager.PlayBombSound();
            _gameManager.GameOver();
        }

        Destroy(gameObject);
        _gameManager.UpdateScoreBoard(_pointValue);
        Instantiate(_explosionParticles, transform.position, _explosionParticles.transform.rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (tag == "Good")
        {
            _floatingPoints.GetComponentInChildren<TextMesh>().text = (-_pointValue).ToString();
            _floatingPoints.GetComponentInChildren<TextMesh>().color = Color.red;
            GameObject instance = Instantiate(_floatingPoints, _sensor.transform.position + new Vector3(transform.position.x, 4, 0), Quaternion.identity);
            _audioManager.PlayFartSound();
            _gameManager.UpdateScoreBoard(-_pointValue);
            Destroy(instance, 3);
        }

        Destroy(gameObject);
    }

    Vector3 RandomForce()
    {
        return Vector3.up * Random.Range(_minSpeed, _maxSpeed);
    }

    float RandomTorque() 
    {
        return Random.Range(-_maxTorque, _maxTorque);
    }

    Vector3 RandomSpawnPos()
    {
        if (gameObject.CompareTag("Bad")) return new Vector3(Random.Range(-_xRange, _xRange), _ySpawnPos, -1);
        else return new Vector3(Random.Range(-_xRange, _xRange), _ySpawnPos);
    }
}
