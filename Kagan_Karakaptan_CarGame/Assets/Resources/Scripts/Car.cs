using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    private MovementRecorder recorder;

    public Transform[] entrances;
    public Transform[] exits;

    public bool canMove;

    public float speed;
    public float turnSpeed;
    private int input;

    private int phase;

    private void Awake()
    {
        input = 0;
        phase = 0;
        recorder = new MovementRecorder();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] clones = GameObject.FindGameObjectsWithTag("clone");
        foreach (GameObject clone in clones)
            clone.GetComponent<Clone>().Start();

        canMove = false;


        transform.position = entrances[phase].position;
        transform.rotation = entrances[phase].rotation;

        foreach (Transform entrance in entrances)
            entrance.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        entrances[phase].GetChild(0).GetComponent<MeshRenderer>().enabled = true;

        foreach (Transform exit in exits)
        {
            exit.GetComponent<BoxCollider2D>().enabled = false;
            exit.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        }
        exits[phase].GetComponent<BoxCollider2D>().enabled = true;
        exits[phase].GetChild(0).GetComponent<MeshRenderer>().enabled = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            canMove = true;

        //if (canMove)
        //{
        //    if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        //        transform.Rotate(0, 0, turnSpeed * Time.deltaTime);

        //    else if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        //        transform.Rotate(0, 0, -turnSpeed * Time.deltaTime);

        //    transform.position += transform.up * speed * Time.deltaTime;
        //}

        if (canMove)
        {
            if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
                input = 1;
            else if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
                input = -1;
            else
                input = 0;
        }


    }

    private void FixedUpdate()
    {
        //if (canMove)
        //{
        //    recorder.Record(transform);
        //}

        if (canMove)
        {
            //if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            //    recorder.Record(1);
            //else if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            //    recorder.Record(-1);
            //else
            //    recorder.Record(0);

            Move(input);
        }


    }

    public void Move(int input)
    {
        transform.Rotate(0, 0, input * turnSpeed * Time.deltaTime);
        transform.position += transform.up * speed * Time.deltaTime;
        recorder.Record(input);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("exit"))
        {
            phase++;
            GameObject clone = Instantiate(Resources.Load("Prefabs/clone"), entrances[phase - 1].position, entrances[phase - 1].rotation) as GameObject;
            //send the recorded path and rotation values to this instance to follow
            recorder.Send(clone.GetComponent<Clone>());

            //for (int i = 0; i < clone.GetComponent<Clone>().actions.Length; i++)
            //    Debug.Log(clone.GetComponent<Clone>().actions[i]);

        }

        recorder.Clear();
        Start();

    }
}
