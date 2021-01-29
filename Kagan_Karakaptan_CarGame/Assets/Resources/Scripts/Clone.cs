using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone : MonoBehaviour
{
    //public Vector3[] positions;
    //public Quaternion[] rotations;

    private Car car;

    [HideInInspector]
    public int[] actions;

    private int frame;

    Vector3 initialPos;
    Quaternion initialRot;

    private void Awake()
    {
        car = GameObject.Find("car").GetComponent<Car>();
        initialPos = transform.position;
        initialRot = transform.rotation;
    }

    public void Start()
    {
        transform.position = initialPos;
        transform.rotation = initialRot;
        frame = 0;
    }

    void FixedUpdate()
    {

        if (car.canMove && frame < actions.Length)
        {
            transform.Rotate(0, 0, actions[frame] * car.turnSpeed * Time.deltaTime);

            transform.position += transform.up * car.speed * Time.deltaTime;

            frame++;
        }
    }


}
