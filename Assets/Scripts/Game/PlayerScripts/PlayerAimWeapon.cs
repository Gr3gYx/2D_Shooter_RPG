using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.InputSystem;

public class PlayerAimWeapon : MonoBehaviour
{
    private Camera _mainCam;
    private Vector3 _mousePos;
    private Vector3 aimDirection;
    private float _timer;
    private bool _fireContinously;
    
    [SerializeField]
    private GameObject _bullet;

    [SerializeField]
    private Transform _bulletTransform;

    [SerializeField]
    private bool _canFire;

    [SerializeField]
    private float _timeBetweenFiring;

    private Vector2 pointerInput;
    public Vector2 PointerInput => pointerInput;
    [SerializeField]
    private InputActionReference _pointerPosition;
    
    private void Awake() {
        _mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    private Vector2 GetPointerInput()
    {
        Vector3 mousePosi = _pointerPosition.action.ReadValue<Vector2>();
        mousePosi.z = _mainCam.nearClipPlane;
        return _mainCam.ScreenToWorldPoint(new Vector3(mousePosi.x, mousePosi.y, -_mainCam.transform.position.z));
    }

    private void Update() {
        //Vector3 mouseScreenPosition = Input.mousePosition;
        
        _mousePos = GetPointerInput();
        //_mousePos = _mainCam.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, _mainCam.transform.position.z));
        //_mousePos.z = 0f;

        aimDirection = _mousePos - transform.position;

        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        Vector2 scale = transform.localScale;
        if(aimDirection.x < 0)
        {
            scale.y = -1;
        }
        else if (aimDirection.x > 0)
        {
            scale.y = 1;
        }
        transform.localScale = scale;

        if (_canFire && _fireContinously)
        {
            FireBullet();
        }

        if(!_canFire)
        {
            _timer += Time.deltaTime;
            if(_timer >= _timeBetweenFiring)
            {
                _canFire = true;
                _timer = 0;
            }
        }        
    }

    public void OnFire(InputValue inputValue)
    {        
        _fireContinously = inputValue.isPressed;    
    }

    private void FireBullet()
    {
        _canFire = false;

        GameObject bullet = Instantiate(_bullet, _bulletTransform.position, Quaternion.identity);
        BulletScript bulletScript = bullet.GetComponent<BulletScript>();
        
        // Pass the mouse position to the bullet script
        bulletScript.SetDirection(aimDirection);
    }

    // Handle application focus change
    private void OnApplicationFocus(bool hasFocus)
    {
        // Reset the continuous fire flag if the game loses focus
        if (!hasFocus)
        {
            _fireContinously = false;
        }
    }

    // Handle application pause
    private void OnApplicationPause(bool pauseStatus)
    {
        // Reset the continuous fire flag if the game pauses
        if (pauseStatus)
        {
            _fireContinously = false;
        }
    }
}
