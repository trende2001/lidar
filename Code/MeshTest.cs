using Sandbox;

public sealed class MeshTest : Component
{
	
	private Mesh CreateQuad(float width = 1f, float height = 1f) {
		// Create a quad mesh.
		var mesh = new Mesh(Material.Load("materials/models/editor/axis_helper.vmat"));

		float w = width * .5f;
		float h = height * .5f;
		var vertices = new List<SimpleVertex>()
		{
			new(
				new Vector3(-w, -h, 0),  // Position
				-Vector3.Forward,        // Normal
				Vector3.Right,           // Tangent
				new Vector2(0, 0)        // Texcoord
			),
			new(
				new Vector3(w, -h, 0),
				-Vector3.Forward,
				Vector3.Right,
				new Vector2(1, 0)
			),
			new(
				new Vector3(-w, h, 0),
				-Vector3.Forward,
				Vector3.Right,
				new Vector2(0, 1)
			),
			new(
				new Vector3(w, h, 0),
				-Vector3.Forward,
				Vector3.Right,
				new Vector2(1, 1)
			)
		};

		var tris = new List<int>() {
			// lower left tri.
			0, 2, 1,
			// lower right tri
			2, 3, 1
		};

		mesh.CreateVertexBuffer<SimpleVertex>( vertices.Count, SimpleVertex.Layout, vertices.ToArray() );
		mesh.CreateIndexBuffer( tris.Count, tris.ToArray() );

		return mesh;
	}

	protected override void OnStart()
	{
		base.OnStart();
		
		var mesh = CreateQuad();
		var model = Model.Builder
			.AddMesh( mesh )
			.Create();
		
		Log.Info( mesh );
		
		ParticleSnapshot snapshot = new ParticleSnapshot();
		
		//Graphics.DrawModelInstanced( model, new Span<Transform>(new []{new Transform(WorldPosition, WorldRotation)}) );
	}

	protected override void OnUpdate()
	{

	}
}
