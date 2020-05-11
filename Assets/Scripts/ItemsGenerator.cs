using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsGenerator : MonoBehaviour
{

    public GameObject player;

    public GameObject[] foods;
    public GameObject[] obstacles;

    float offset;

    public void Awake()
    {
        offset = Vector3.Distance(player.transform.position, transform.position);
        GameManager.gm.ig=this;
    }
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, player.transform.position.z+offset);
    }
    
    public IEnumerator FoodsGenerator()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.5f, 1f));
            Instantiate(
                foods[Random.Range(0, foods.Length-1)],
                transform.position+new Vector3(Random.Range(-13,13), 0,0),
                Quaternion.identity);
        }
    }

    public IEnumerator ObstaclesGenerator()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.5f, 1f));
            GameObject temp = Instantiate(
                obstacles[Random.Range(0, obstacles.Length - 1)],
                transform.position + new Vector3(Random.Range(-13, 13), 0, 0),
                Quaternion.identity);
            temp.transform.Rotate(new Vector3(0, 90f, 0));
        }
    }
}
