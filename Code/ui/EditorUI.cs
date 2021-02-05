using Godot;
using System;

public class EditorUI : Node {

    PackedScene PlanetSelectionIndicator;

    Vector3 initialVelocityUI = new Vector3();
    Color colorUI = new Color(.5f,.5f,.5f);
    float radiusUI = 2000000;
    float massUI = 10000;

    Spatial planetSpawner = new Spatial();
    Spatial cam;

    public override void _Ready() {
        PlanetSelectionIndicator = GD.Load<PackedScene>("res://Prefabs/UI/PlanetSelection.tscn");
        cam = GetViewport().GetCamera();
        cam.AddChild(planetSpawner);
    }

    void _OnAddPlanet() {
        var planet = CelestialBody.New(massUI, radiusUI, SpawnPlanetAt(), initialVelocityUI, colorUI);
        AddChild(planet);
    }

    public void _OnToggleRunning(bool button_pressed) {
        Universe.running = button_pressed;
    }

    Vector3 SpawnPlanetAt() {
        planetSpawner.Translation = new Vector3(0,0, 5 * -radiusUI / Universe.scaleFactor);
        return planetSpawner.GlobalTransform.origin;
    }

    //---UI Handlers---

    public void _OnRadiusPicked(float value) {
        radiusUI = value;
    }

    public void _OnMassPicked(float value) {
        massUI  =value;
    }

    public void _OnVelocityXPicked(float value) {
        initialVelocityUI.x = value;
    }

    public void _OnVelocityYPicked(float value) {
        initialVelocityUI.y = value;
    }

    public void _OnVelocityZPicked(float value) {
        initialVelocityUI.z = value;
    }

    public void _OnColorPicked(Color color) {
        colorUI = color;
    }
}
