using UnityEngine;
using System.Collections;
using PureMVC.Patterns;
using PureMVC.Interfaces;
using System.Collections.Generic;
using SimpleFramework;

public class EnterWorldCommand:SimpleCommand
{
    public override void Execute(INotification notification)
    {
        object body = notification.Body;
        if (body == null) return;

        Application.LoadLevel("world");
    }
}