using UnityEngine;
using System.Collections;
using System;

public class MyDragAndDropable : DragAndDropable {

    protected override bool Use2DPhysics
    {
        get
        {
            return false;
        }
    }

    protected override void OnDrop(DropContainer container)
    {
        Debug.Log("I was dropped into " + container.name);
    }

    protected override void Awake() {
        base.Awake();
	}

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    // Update is called once per frame
    void Update () {
	
	}

    protected override void OnStartHovering(DropContainer container)
    {
        Debug.Log("I started hovering " + container.name);

    }

    protected override void OnHovering(DropContainer container)
    {

    }

    protected override void OnEndHovering(DropContainer container)
    {
        Debug.Log("I stopped hovering " + container.name);

    }
}
