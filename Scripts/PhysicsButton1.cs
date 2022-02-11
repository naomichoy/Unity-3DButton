using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class PhysicsButton1 : MonoBehaviour
{ 

    [SerializeField] private float threshold = 0.1f;
    [SerializeField] private float deadZone = 0.025f;

    public UnityEvent onPressed, onReleased;

    private bool _isPressed;
    private bool prevPressedState;
    private Vector3 _startPos;
    private ConfigurableJoint _joint;

    public AudioSource pressedSound;
    public AudioSource releasedSound;

    public Light LightObj;

    // Start is called before the first frame update
    void Start()
    {
        _startPos = transform.localPosition;
        _joint = GetComponent<ConfigurableJoint>();
        LightObj = LightObj.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isPressed && GetValue() + threshold >= 1)
            _isPressed = true;
        if (_isPressed && GetValue() - threshold <= 0)
            _isPressed = false;

        // Debug.Log(_isPressed);

        if (_isPressed && prevPressedState != _isPressed)
            Pressed();
        if (!_isPressed && prevPressedState != _isPressed)
            Released();
    }

    private float GetValue()
    {
        var value = Vector3.Distance(_startPos, transform.localPosition) / _joint.linearLimit.limit;

        if (Math.Abs(value) < deadZone)
            value = 0;

        // Debug.Log("value " + value);

        return Mathf.Clamp(value, -1f, 1f);
    }

    private void Pressed()
    {
        prevPressedState = _isPressed;
        pressedSound.pitch = 1;
        pressedSound.Play();
        Debug.Log(LightObj);
        LightObj.enabled = !LightObj.enabled;
        onPressed.Invoke();
        Debug.Log("Pressed");
    }

    private void Released()
    {
        prevPressedState = _isPressed;
        releasedSound.pitch = UnityEngine.Random.Range(1.1f, 1.2f);
        releasedSound.Play();
        onReleased.Invoke();
        Debug.Log("Released");
    }
}
