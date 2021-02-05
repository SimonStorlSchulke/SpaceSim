using Godot;
using System.Collections.Generic;

public class CelestialBody : Spatial {
    [Export]
    public float mass = 10000, radius = 2000000;

    [Export]
    public Vector3 initialVelocity = new Vector3(0, 0, 0);

    [Export]
    public Color color = new Color(0.7f,0.7f,0.7f);

    Vector3 currentVelocity, force;

    Vector3 acceleration;
    DrawLines linedrawer;

    Vector3 previousPosition;
    Vector3 Zero = new Vector3(0,0,0);

    
    public static CelestialBody New(float _mass, float _radius, Vector3 position, Vector3 _initialVelocity, Color _color) {
        var body = new CelestialBody();
        body.mass = _mass;
        body.radius = _radius;
        body.color = _color;
        body.initialVelocity = _initialVelocity;
        body.Translation = position;
        return body;
    }

    public override void _Ready() {
        currentVelocity = initialVelocity;
        Universe.spaceBodies.Add(this);

        float r = radius / Universe.scaleFactor;
        MeshInstance meshInstance = new MeshInstance();
        meshInstance.Mesh = new SphereMesh();   //just once!
        meshInstance.Scale = new Vector3(r,r,r);

        SpatialMaterial mat = new SpatialMaterial();
        mat.AlbedoColor = color;
        meshInstance.Mesh.SurfaceSetMaterial(0, mat);

        AddChild(meshInstance);

        var area = new Area();
        var colissionShape = new CollisionShape();
        var SphereShape = new SphereShape();
        SphereShape.Radius = r;
        colissionShape.Shape = SphereShape;
        area.AddChild(colissionShape);
        AddChild(area);

        area.Connect("input_event", this, nameof(_OnSelect));
    }

    void _OnSelect(Node camera, InputEvent @event, Vector3 click_position, Vector3 click_normal, int shape_idx) {
        
        if (@event.IsActionPressed("MouseDown")) {
            GD.Print(this.Name);
            Universe.selectedBody = this;
        }
    }

    public override void _Process(float delta) {
        //linedrawer.drawLine(Translation, Translation + (currentVelocity * 4), new Color(0,1,0), delta);
    }

    public void UpdateVelocity(float timeStep) {
        foreach (CelestialBody body in Universe.spaceBodies) {
            if (body != this) {
                float sqrDist = (body.Translation - this.Translation).LengthSquared();
                Vector3 forceDir = (body.Translation - this.Translation).Normalized();
                Vector3 force = forceDir * Universe.G * this.mass * body.mass / sqrDist;
                Vector3 acceleration = force / mass;
                currentVelocity += acceleration * timeStep;
            }
        }
    }

    public void UpdatePosition(float timeStep) {
        this.Translation += currentVelocity * timeStep;
    }
}