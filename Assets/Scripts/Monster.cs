using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Monster : MonoBehaviour
{

    [SerializeField] Sprite _dead;
    [SerializeField] ParticleSystem _particleSystem;

    bool _hasDied;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (ShouldDieFromCollision(collision))
        {
            StartCoroutine(Die());
        }
    }

    bool ShouldDieFromCollision(Collision2D collision)
    {
        if (_hasDied)
        {
            return false;
        }

        Bird bird = collision.gameObject.GetComponent<Bird>();
        if (bird != null)
        {
            return true;
        }else if (collision.contacts[0].normal.y < -0.5)
        {
            return true;
        }

        return false;

    }

    IEnumerator Die()
    {
        _hasDied = true;
        GetComponent<SpriteRenderer>().sprite = _dead;
        _particleSystem.Play();
        FindObjectOfType<AudioManager>().Play("MonsterHit");
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}
