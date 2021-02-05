using Godot;
using System;

public class selector : RayCast {
    public override void _UnhandledInput(InputEvent inputEvent) {
        if (inputEvent.IsActionPressed("MouseDown")) {
            if (IsColliding()) {
                //Area a = (Area)GetCollider();
                //GD.Print(a.GetParent());
            }
        }
    }
}
