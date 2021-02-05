using Godot;
using System;

[Tool]
public class OrbitDebug : Spatial {

    class VirtualBody {

        public Vector3 position, velocity;
        public float mass;

        public VirtualBody(CelestialBody body) {
            position = body.Translation;
            velocity = body.initialVelocity;
            mass = body.mass;
        }
    }

    [Export]
    int numSteps = 1000;

    [Export]
    public float timeStep = 0.1f;

    [Export]
    public bool relativeToBody;

    [Export]
    public CelestialBody centralBody;

    DrawLines linedrawer;

    public override void _Process(float delta) {
        DrawOrbits(delta);
    }

    public override void _Ready() {
        linedrawer = GetNode<DrawLines>("/root/GLineDrawer");
    }

    void DrawOrbits(float delta) {
        var virtualBodies = new VirtualBody[Universe.spaceBodies.Count];
        var drawPoints = new Vector3[Universe.spaceBodies.Count][];
        int referenceFrameIndex = 0;
        Vector3 referenceBodyInitialPosition = new Vector3(0, 0, 0);

        // Initialize virtual bodies (don't want to move the actual bodies)
        for (int i = 0; i < virtualBodies.Length; i++) {
            virtualBodies[i] = new VirtualBody(Universe.spaceBodies[i]);
            drawPoints[i] = new Vector3[numSteps];

            if (Universe.spaceBodies[i] == centralBody && relativeToBody) {
                referenceFrameIndex = i;
                referenceBodyInitialPosition = virtualBodies[i].position;
            }
        }

        // Simulate
        for (int step = 0; step < numSteps; step++) {
            Vector3 referenceBodyPosition = (relativeToBody) ? virtualBodies[referenceFrameIndex].position : new Vector3(0, 0, 0);
            // Update velocities
            for (int i = 0; i < virtualBodies.Length; i++) {
                virtualBodies[i].velocity += CalculateAcceleration(i, virtualBodies) * timeStep;
            }
            // Update positions
            for (int i = 0; i < virtualBodies.Length; i++) {
                Vector3 newPos = virtualBodies[i].position + virtualBodies[i].velocity * timeStep;
                virtualBodies[i].position = newPos;
                if (relativeToBody) {
                    var referenceFrameOffset = referenceBodyPosition - referenceBodyInitialPosition;
                    newPos -= referenceFrameOffset;
                }
                if (relativeToBody && i == referenceFrameIndex) {
                    newPos = referenceBodyInitialPosition;
                }

                drawPoints[i][step] = newPos;
            }
        }

        // Draw paths
        for (int bodyIndex = 0; bodyIndex < virtualBodies.Length; bodyIndex++) {
            var pathColour = new Color(1, 0, 0); //Universe.spaceBodies[bodyIndex].gameObject.GetComponentInChildren<MeshRenderer> ().sharedMaterial.color; //

            for (int i = 0; i < drawPoints[bodyIndex].Length - 1; i++) {
                linedrawer.drawLine(drawPoints[bodyIndex][i], drawPoints[bodyIndex][i + 1], pathColour, delta, false);
            }
        }
    }

    Vector3 CalculateAcceleration(int i, VirtualBody[] virtualBodies) {
        Vector3 acceleration = new Vector3(0, 0, 0);
        for (int j = 0; j < virtualBodies.Length; j++) {
            if (i == j) {
                continue;
            }
            Vector3 forceDir = (virtualBodies[j].position - virtualBodies[i].position).Normalized();
            float sqrDst = (virtualBodies[j].position - virtualBodies[i].position).LengthSquared();
            acceleration += forceDir * Universe.G * virtualBodies[j].mass / sqrDst;
        }
        return acceleration;
    }
}
