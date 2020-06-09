using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PlayerControler : MonoBehaviour
{

	public int score;
	public float scale;
	public float radius;
	public float fuel;
	public bool sizeUp;

	public Transform visuals;
	public Transform directionRing;
	public Transform playerCanvas;
	public GameObject scoreIndicator;
	public GameObject minusScoreIndicator;

	public SphereCollider detector;
	public List<Rigidbody> victims;
	
	public float moveSpeed;
	public float rotationSpeed;

	public Transform startPos;
	[Header("Расстояние")]
	public float distance;
	[Header("Время")]
	public float time;
	[Header("Скорость")]
	public float speed;

	private void Awake()
	{
		GameManager.gm.pc = this;
	}

	private void Start()
	{
		RefreshScale();
	}
	private void Update()
    {
		
		if (!GameManager.gm.started || GameManager.gm.gameOver)
		{
			return;
		}

		MoveDirection(this.gameObject, transform.forward, moveSpeed);
		distance = Vector3.Distance(startPos.position, transform.position);
		time += Time.deltaTime;
		speed = distance / time;

		var nearbyObjects = Physics.OverlapSphere(transform.position, radius);
		
		foreach(var nearbyObject in nearbyObjects)
		{
			if (nearbyObject.gameObject == gameObject || nearbyObject.gameObject.layer != 9)
			{
				continue;
			}

			Rigidbody nearbyObjectRb = nearbyObject.GetComponentInParent<Rigidbody>();
			if (!nearbyObjectRb || victims.Contains(nearbyObjectRb))
			{
				continue;
			}
			else
			{
				victims.Add(nearbyObjectRb);
				nearbyObject.gameObject.layer = 10;
				nearbyObjectRb.isKinematic = false;
			}
		}
	}
	
	private void FixedUpdate()
	{
		if (!GameManager.gm.started || GameManager.gm.gameOver)
		{
			return;
		}

	

		foreach (var victim in victims)
		{
			victim.AddForce(Vector3.down * scale * GameManager.gm.gravity * Time.fixedDeltaTime, ForceMode.VelocityChange);
		}
	}

	

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 10 && other.gameObject.CompareTag("Food")) // Victims fall into the hole
		{
			AddScore(1);
		}
		else if(other.gameObject.layer == 10 && other.gameObject.CompareTag("Obstacle"))
		{
			AddScore(-1);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.transform.position.y < 0) // Victims falls through the hole
		{
			other.gameObject.SetActive(false);

			Rigidbody victimRb = other.GetComponent<Rigidbody>();
			if (victimRb)
			{
				victims.Remove(victimRb);
			}
		}
	}

	public float leftEdge=-12f, rightEdge=12f;
	private void MoveDirection(GameObject moveObj,Vector3 direction, float speed)
	{
		moveObj.transform.Translate(direction * speed * Time.deltaTime);
		moveObj.transform.position = new Vector3(
			Mathf.Clamp(moveObj.transform.position.x,leftEdge,rightEdge),
			moveObj.transform.position.y,
			moveObj.transform.position.z
			);
	}


	float rotX;

	[System.Obsolete]
	public void LockedRotation(GameObject rotObj)
	{
		rotX += Input.GetAxis("Mouse X") * rotationSpeed * Mathf.Deg2Rad;
		rotX = Mathf.Clamp(rotX, -45.0f, 45.0f);
		rotObj.transform.RotateAround(Vector3.up, rotX);
		rotObj.transform.localEulerAngles = new Vector3(
			rotObj.transform.localEulerAngles.x, 
			rotX,
			rotObj.transform.localEulerAngles.z);
	}

	public void AddScore(int amount)
	{
		score += amount;
		fuel += 5f*amount;

		if (score < 0)
		{
			score = 0;
		}

		sizeUp = false;

		//CheckSize();
		if (amount > 0)
		{
			GameObject indicator = Instantiate(scoreIndicator, playerCanvas);
			TextMeshProUGUI scoreText = indicator.GetComponent<TextMeshProUGUI>();
			scoreText.text = "+" + amount.ToString();
			StartCoroutine(DisableAfter(indicator, 2f));
			GameManager.gm.ui.scoreText.text = score.ToString();
		}

		if (amount < 0)
		{
			GameObject indicator = Instantiate(minusScoreIndicator, playerCanvas);
			TextMeshProUGUI scoreText = indicator.GetComponent<TextMeshProUGUI>();
			scoreText.text = "-" + amount.ToString();
			StartCoroutine(DisableAfter(indicator, 2f));	
			GameManager.gm.ui.scoreText.text = score.ToString();
		}
	}

	private IEnumerator DisableAfter(GameObject obj, float delay)
	{
		if (!obj)
		{
			yield break;
		}

		yield return new WaitForSeconds(delay);

		if (!obj)
		{
			yield break;
		}
		obj.SetActive(false);
	}

	public void CheckSize()
	{
		if (!sizeUp && score % 10 == 0)
		{
			sizeUp = true;
			RefreshScale();			
		}
	}

	public void RefreshScale()
	{
		scale++;
		radius++;
		visuals.localScale = new Vector3(scale, scale, scale);
		visuals.localPosition = new Vector3(0, -scale / 2f - 0.49f, 0);
		detector.center = new Vector3(0, -1f - scale / 2f, 0);
		detector.radius = scale / 2f;		

	}

	public IEnumerator dropFuel()
	{
		while (true)
		{
			fuel -= 0.1f;
			yield return new WaitForSeconds(0.01f);
		}
	}
}



/*Vector3 mousePos = Input.mousePosition;
if (Input.GetMouseButtonDown(0))
{
	dragStartPos = mousePos;
}
else if (Input.GetMouseButton(0))
{
	curDragDistance = (mousePos - dragStartPos).magnitude;

	if (curDragDistance > maxDragDistance)
	{
		dragStartPos = mousePos - moveDirection * maxDragDistance;
		curDragDistance = (mousePos - dragStartPos).magnitude;
	}

	moveDirection = (mousePos - dragStartPos).normalized;
	var direction = new Vector3(moveDirection.x, 0, moveDirection.y);
	transform.position += direction * speed * curDragDistance * Time.deltaTime;
	directionRing.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(new Vector3(90, 0, 0));
}*/
