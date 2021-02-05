using Godot;
using System.Collections.Generic;

public class DrawLines : Node2D {

    struct Line {
        public Vector3 Start, End;
        public Color LineColor;
        public float Time, cTime;
        public bool Fade;

        public Line(Vector3 start, Vector3 end, Color lineColor, float time, bool fade) {
            Start = start;
            End = end;
            LineColor = lineColor;
            Time = time;
            cTime = time;
            Fade = fade;
        }
    }

    List<Line> Lines = new List<Line>();

    bool RemovedLine = false;

    public override void _Process(float delta) {

        for (int i = 0; i < Lines.Count; i++) {
            Line newLine = Lines[i];
            newLine.cTime -= delta;
            Lines[i] = newLine;
        }

        if (Lines.Count > 0 || RemovedLine) {
            Update(); //Calls _draw
            RemovedLine = false;
        }
    }


    public void drawLine(Vector3 Start, Vector3 End, Color LineColor, float Time, bool fade) {
        Lines.Add(new Line(Start, End, LineColor, Time, fade));
    }

    public override void _Draw() {
        Camera cam = GetViewport().GetCamera();
        foreach (Line cLine in Lines) {
            Vector2 ScreenPointStart = cam.UnprojectPosition(cLine.Start);
            Vector2 ScreenPointEnd = cam.UnprojectPosition(cLine.End);

            if (cam.IsPositionBehind(cLine.Start) || cam.IsPositionBehind(cLine.End)) continue;

            Color c = cLine.LineColor;
            if (cLine.Fade) c.a = cLine.cTime / cLine.Time;

            DrawLine(ScreenPointStart, ScreenPointEnd, c);
        }


        int i = Lines.Count - 1;
        while (i >= 0) {

            if (Lines[i].cTime < 0.0) {

                Lines.RemoveAt(i);
                RemovedLine = true;
            }
            i -= 1;
        }
    }
}

