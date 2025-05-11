using Godot;
using System;


public partial class NodeHelpers : Node
{
    public static NodeHelpers Instance;

    public override void _Ready()
    {
        Instance = this;
    }

    public Vector2 GetViewportUpperLeftPosition()
    {
        var viewportRect = GetViewport().GetVisibleRect();
        var camera = GetViewport().GetCamera2D();

        if (camera != null)
        {
            var cameraPosition = camera.GlobalPosition;
            return cameraPosition - new Vector2(viewportRect.Size.X / 2, viewportRect.Size.Y / 2);
        }

        return new Vector2();
    }


    public Tween FadeToBlack(Node SceneNode, float SecondsDuration)
    {
        var Viewport = GetViewport();
        var ScreenResolution = new Vector2(Viewport.GetVisibleRect().Size.X, Viewport.GetVisibleRect().Size.Y);

        // Start the fade to black
        using(
            var fadeRect = new ColorRect
            {
                Visible = true,
                Size = ScreenResolution,
                ZIndex = 10,    // Make sure the fade is on top
                Position = GetViewportUpperLeftPosition()
            }
        )
        {
            // Load the shader from the.gdshader file
            var shader = (Shader)ResourceLoader.Load("res://Graphics/Shaders/fade_to_black.gdshader");
            var shaderMaterial = new ShaderMaterial { Shader = shader };

            fadeRect.Material = shaderMaterial;

            // This has to be added to the canvas, because it's rendered after (over top of) the main battle scene
            SceneNode.AddChild(fadeRect);
            fadeRect.Material.Set("shader_parameter/fade_progress", 0.254); // Set to initial value to make partially visible

            var FadeTween = CreateTween();
            FadeTween.TweenProperty(fadeRect.Material, "shader_parameter/fade_progress", 1.0f, SecondsDuration);

            return FadeTween;
        }
    }
}
