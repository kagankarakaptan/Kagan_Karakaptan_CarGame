using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        //wait for next input
        canMove = false;

        //resetting the initial state of this object
        transform.position = entrances[phase].position;
        transform.rotation = entrances[phase].rotation;

        //re-starting all clones to set the initial states of them
        GameObject[] clones = GameObject.FindGameObjectsWithTag("clone");
        foreach (GameObject clone in clones)
            clone.GetComponent<Clone>().Start();

        //Determining entrances according to the phase
        foreach (Transform entrance in entrances)
            entrance.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        entrances[phase].GetChild(0).GetComponent<MeshRenderer>().enabled = true;

        //Determining exits according to the phase
        foreach (Transform exit in exits)
        {
            exit.GetComponent<BoxCollider2D>().enabled = false;
            exit.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        }
        exits[phase].GetComponent<BoxCollider2D>().enabled = true;
        exits[phase].GetChild(0).GetComponent<MeshRenderer>().enabled = true;

    }

    void Update()
    {
        //starting the movement of the environment
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetMouseButtonDown(0))
            canMove = true;

        //setting the input value according to the input itself
        if (canMove)
        {
            if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) || Input.GetMouseButton(0) && Input.mousePosition.x < Screen.width / 2)
                input = 1;
            else if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A) || Input.GetMouseButton(0) && Input.mousePosition.x > Screen.width / 2)
                input = -1;
            else
                input = 0;
        }


    }

    private void FixedUpdate()
    {
        if (canMove)
            Move(input);

    }

    //basic movement function with recorder object to save all movement info for the next clone will invoke
    public void Move(int input)
    {
        transform.Rotate(0, 0, input * turnSpeed * Time.deltaTime);
        transform.position += transform.up * speed * Time.deltaTime;
        recorder.Record(input); //add the current frame input value to the list which keeps the actions inside the recorder object
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("exit"))
        {
            //loads the new level
            if (phase == exits.Length - 1) //means we are in the last phase of this level
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

            else
            {
                phase++; //increasing the state of the game for this level(scene)
                GameObject clone = Instantiate(Resources.Load("Prefabs/clone"), entrances[phase - 1].position, entrances[phase - 1].rotation) as GameObject;
                recorder.Send(clone.GetComponent<Clone>()); //sending the actions to this instance to follow
            }

        }

        recorder.Clear(); //clears the list which keeps the action inside the recorder object
        Start(); //re-starting for the next phase

    }
}
