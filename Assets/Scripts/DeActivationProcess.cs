using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeActivationProcess : MonoBehaviour
{
    public GameObject player;
    float offset;
    private void Start()
    {
        offset = Vector3.Distance(player.transform.position, transform.position);
    }
    private void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, player.transform.position.z-offset);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Food") || other.gameObject.CompareTag("Obstacle"))
        {
            other.gameObject.SetActive(false);
        }
    }

}
