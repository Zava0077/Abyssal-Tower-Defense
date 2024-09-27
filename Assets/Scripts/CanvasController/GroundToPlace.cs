using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GroundToPlace : MonoBehaviour
{
    public Entity entity;
    public CanvasController controller;

    private void Start()
    {
        controller = Player.instance.canvasController;
    }

    public void ClickOnGround()
    {
        controller.Show(entity,this);
    }
}
