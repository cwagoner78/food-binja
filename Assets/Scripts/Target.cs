using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Rigidbody _targetRB;
    private GameManager _gameManager;
    [SerializeField] int _pointValue = 1;

    [Header("Effects")]
    [SerializeField] ParticleSystem _explosionParticles;

    public float _minSpeed = 12;
    public float _maxSpeed = 16;
    public float _maxTorque = 10;
    public float _xRange = 4;
    public float _ySpawnPos = 6;

    private bool _inputEnabled = true;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>(); 
        _targetRB = GetComponent<Rigidbody>();
        _targetRB.AddForce(RandomForce(), ForceMode.Impulse);
        _targetRB.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);
        transform.position = RandomSpawnPos();
    }

    private void OnMouseDown()
    {
        Destroy(gameObject);
        _gameManager.UpdateScoreBoard(_pointValue);
        Instantiate(_explosionParticles, transform.position, _explosionParticles.transform.rotation);
        if (gameObject.CompareTag("Bad"))
        {
            _gameManager.PlayBombSound();
            _gameManager.GameOver();
            _inputEnabled = false;
        }
        if (gameObject.CompareTag("Good")) _gameManager.PlayBiteSound();

    }

    private void OnTriggerEnter(Collider other)
    {
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
