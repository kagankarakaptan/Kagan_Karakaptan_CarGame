using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementRecorder
{

    //List<Vector3> positions = new List<Vector3>();
    //List<Quaternion> rotations = new List<Quaternion>();

    List<int> actions;

    public MovementRecorder() => actions = new List<int>();

    public void Record(int action)
    {
        //positions.Add(car.position);
        //rotations.Add(car.rotation);

        actions.Add(action);
    }

    public void Send(Clone clone)
    {
        //clone.positions = new Vector3[positions.Count];
        //clone.rotations = new Quaternion[rotations.Count];

        //positions.CopyTo(clone.positions);
        //rotations.CopyTo(clone.rotations);

        clone.actions = new int[actions.Count];
        actions.CopyTo(clone.actions);

    }

    public void Clear()
    {
        //if (positions.Count > 0)
        //    positions.Clear();
        //if (rotations.Count > 0)
        //    rotations.Clear();

        if (actions.Count > 0)
            actions.Clear();
    }
}
