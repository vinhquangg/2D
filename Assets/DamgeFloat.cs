using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamgeFloat : MonoBehaviour
{
    public TextMeshProUGUI floatDamge;
    public Transform target;
    public Vector3 offset = new Vector3(0, 1.5f, 0);
    public float speed;
    public float duration;

    public void SetFloat(int damge, Transform enemyTransform)
    {
        floatDamge.text = damge.ToString();
        Color randomColor = new Color(Random.value, Random.value, Random.value);
        floatDamge.color = randomColor;
        target = enemyTransform; 
        Destroy(gameObject,duration);
    }

    private void Update()
    {
        if (target != null)
        {
            transform.position = target.position + offset * speed * Time.deltaTime;
        }
    }
}
