using Godot;
using System;

public class SpacebodyDebugDrawer : Spatial
{

    [Export]
    Color pathColor = new Color(1,0,0);

    [Export]
    float persistence = 10, stepsize = 0.1f;

    [Export]
    bool fade = true;

    Vector3 previousPosition;
    DrawLines linedrawer;

    public override void _Ready()
    {
        linedrawer = GetNode<DrawLines>("/root/GLineDrawer");
        SetupPathDrawing();
    }


    void SetupPathDrawing() {
        previousPosition = GlobalTransform.origin;
        Timer t = new Timer();
        AddChild(t);
        t.Connect("timeout", this, nameof(_OnDrawPath));
        t.WaitTime = stepsize;
        t.OneShot = false;
        t.Start();
    }

    void _OnDrawPath() {
        linedrawer.drawLine(previousPosition, GlobalTransform.origin, pathColor, persistence, fade);
        previousPosition = GlobalTransform.origin;
    }
}
