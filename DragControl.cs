using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragControl : MonoBehaviour, IDragHandler
{
    public GameObject player;
    public GameObject directionRing;

    public void Awake()
    {
        GameManager.gm.ControlPanel = this;
    }
    public void OnDrag(PointerEventData eventData)
    {
        GameManager.gm.pc.LockedRotation(player);
        GameManager.gm.pc.LockedRotation(directionRing);

    }

}
