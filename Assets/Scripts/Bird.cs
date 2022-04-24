using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField] float _launchForce = 500;
    [SerializeField] float _maxDragDistance = 5;

    Vector2 _startPosition;
    Rigidbody2D _rigidbody2D;
    SpriteRenderer _spriteRenderer;

    public GameObject PointPrefab;

    public GameObject[] Points;

    public int numberOfPoints;


    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _startPosition = _rigidbody2D.position;
        _rigidbody2D.isKinematic = true;
        _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void OnMouseDown()
    {
        _spriteRenderer.color = Color.red;

        Points = new GameObject[numberOfPoints];

        for (int i = 0; i < numberOfPoints; i++)
        {
            Points[i] = Instantiate(PointPrefab, transform.position, Quaternion.identity);
        }

    }

    void OnMouseUp()
    {
        Vector2 currentPosition = _rigidbody2D.position;
        Vector2 direction = _startPosition - currentPosition;
        direction.Normalize();

        _rigidbody2D.isKinematic = false;
        //_rigidbody2D.AddForce(direction * _launchForce);

        _rigidbody2D.velocity = direction * _launchForce;

        _spriteRenderer.color = Color.white;

        for (int i = 0; i < numberOfPoints; i++)
        {
            Points[i].SetActive(false);
        }
    }

    void OnMouseDrag()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 desiredPosition = mousePosition;

        float distance = Vector2.Distance(desiredPosition, _startPosition);
        if(distance > _maxDragDistance)
        {
            Vector2 direction = desiredPosition - _startPosition;
            direction.Normalize();
            desiredPosition = _startPosition + (direction * _maxDragDistance);
        }

        if (desiredPosition.x > _startPosition.x)
        {
            desiredPosition.x = _startPosition.x;
        }

        _rigidbody2D.position = desiredPosition;
    }

    void Update()
    {
        for (int i = 0; i < Points.Length; i++)
        {
            Points[i].transform.position = PointPosition(i * 0.1f);
        }
    }

    IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(2);
        _rigidbody2D.position = _startPosition;
        _rigidbody2D.isKinematic = true;
        _rigidbody2D.velocity = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(ResetAfterDelay());
    }

    Vector2 PointPosition(float t)
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 desiredPosition = mousePosition;
        Vector2 direction = -desiredPosition + _startPosition;


        Vector2 currentPointPosition = (Vector2)transform.position + (direction.normalized * _launchForce * t) + 0.5f * Physics2D.gravity * (t*t);

        return currentPointPosition;
    }

}
