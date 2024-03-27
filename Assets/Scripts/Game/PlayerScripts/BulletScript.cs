using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;

public class BulletScript : MonoBehaviour
{   
    private Vector3 _direction;
    private Camera _mainCam;
    private Rigidbody2D _rb;
    private Vector3 _initialPosition;

    [SerializeField]
    private float _force;

    [SerializeField]
    private float _maxDistance;

     private void Awake() {
        _mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _initialPosition = transform.position;
        _rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        _rb.velocity = _direction;
    
        float rot = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot + 180f);
    }

    void Update()
    {
        float distanceTraveled = Vector3.Distance(transform.position, _initialPosition);

        if (distanceTraveled >= _maxDistance)
        {
            Destroy(gameObject);
        }
    }

    public void SetDirection(Vector3 direction)
    {
        _direction = direction.normalized * _force;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if  (collision.GetComponent<EnemyMovement>())
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
