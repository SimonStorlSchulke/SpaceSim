using Godot;
using System.Collections.Generic;


public class Universe : Node {
    public static List<CelestialBody> spaceBodies = new List<CelestialBody>();

    public static float scaleFactor = 100000;
    public static float timeScale = 1;
    public static float G = 0.6f;

    public static bool running;

	static PackedScene planetSelectionIndicatorScene; //Geht das???
	static Spatial planetSelectionIndicator;

    private static CelestialBody _selectedBody; // field

    public static CelestialBody selectedBody   // property
    {
        get { return _selectedBody; }   // get method
        set {
			if (_selectedBody == null) {
				GD.Print("YO");
				planetSelectionIndicator = (Spatial)planetSelectionIndicatorScene.Instance();

				float r = value.radius / Universe.scaleFactor;
				planetSelectionIndicator.Scale = new Vector3(r, r, r);

				var mainLoop = Godot.Engine.GetMainLoop();
				var sceneTree = mainLoop as SceneTree;
				sceneTree.Root.AddChild(planetSelectionIndicator); //Hackfleisch
			}
			_selectedBody = value;
			planetSelectionIndicator.Translation = _selectedBody.Translation;

			}  // set method
    }

    [Export]
    float SetScaleFactor = 100000, SetTimeScale = 1, SetG = 0.674f;

    public override void _Ready() {
		planetSelectionIndicatorScene = ResourceLoader.Load<PackedScene>("res://Prefabs/UI/PlanetSelection.tscn");
        timeScale = SetTimeScale;
        scaleFactor = SetScaleFactor;
        G = SetG;
    }

    public override void _PhysicsProcess(float delta) {
        if (running) {
            foreach (CelestialBody cBody in spaceBodies) {
                cBody.UpdateVelocity(delta * timeScale);
            }
            foreach (CelestialBody cBody in spaceBodies) {
                cBody.UpdatePosition(delta * timeScale);
            }
        }
    }
}
