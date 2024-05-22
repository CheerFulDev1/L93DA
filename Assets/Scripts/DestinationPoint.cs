using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationPoint : MonoBehaviour
{
    private GameObject destinationPoint;
    private Transform destinationPointTransform;


    private void Awake()
    {
        CreateDestinationPoint();
    }

    private void CreateDestinationPoint()
    {

        float numberOfChildren = gameObject.transform.childCount;

        if (numberOfChildren >= 1) {

            Debug.Log("WARNING ! THERE ARE MANY DESTINATION POINTS IN THE HIERARCHY OF THE THREESHOLD BAR !");
            Time.timeScale = 0;
        }

        destinationPoint = new GameObject("DestinationPoint");
        destinationPointTransform = destinationPoint.transform;

        destinationPointTransform.SetParent(gameObject.transform);

        //Doing Transform adjustements
        destinationPointTransform.localPosition = Vector3.zero;
        destinationPointTransform.localRotation = Quaternion.identity;
        destinationPointTransform.localScale = Vector3.one;

    }

    public Transform GetDestinationPoint()
    {
        return destinationPointTransform;
    }
}
