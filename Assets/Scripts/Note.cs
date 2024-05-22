using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    double timeInstantiated;
    public float assignedTime;

    private Transform originalPosition;
    private GameObject noteContainer;

    [SerializeField]
    private Transform destinationPoint;

    private float movementSpeed;
    private Vector2 direction;

    private void Start()
    {
        originalPosition = gameObject.transform;
        timeInstantiated = SongManager.GetAudioSourceTime();
        //Setting Destination Point
        movementSpeed = 10f;
        direction = CalculateDirection();
    }

    private void OnEnable()
    {
        SetDestinationPoint();
    }

    // Update is called once per frame
    private void Update()
    {
        double timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / (SongManager.Instance.noteTime * 2));

        
        if (t > 1)
        {
            //no destruction here but deactivation on sprite renderer
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
        else
        {
            //RotateNote();
            MoveNote(t);

            // this does not belong here should be put with the object pooler alongst enabled or deactivated
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }


    private void MoveNote(float speed)
    {
        if ( gameObject && destinationPoint != null )
        {
            //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            //transform.rotation = Quaternion.Euler(new Vector3(0, 0, -angle));

            transform.position += (Vector3)direction * movementSpeed * Time.deltaTime;
        }
    }

    private Vector2 CalculateDirection()
    {
        if ( gameObject && destinationPoint != null )
        {
            return direction = (destinationPoint.position - originalPosition.position).normalized;
        }
        else
        {
            return Vector2.zero;
        }
    }

    public void SetDestinationPoint()
    {
        DestinationPoint destinationPointParent = FindObjectOfType<DestinationPoint>();
        destinationPoint = destinationPointParent.GetDestinationPoint();
    }

}
