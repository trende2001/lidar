public class HandsWeapon : BaseWeapon
{
	[Property]
	public GameObject ScanCluster { get; set; }
	
	GameObject currentCluster;
	
	private void StartScanning()
	{
		if ( !Scanning && !currentCluster.IsValid() )
		{
			// Clone the cluster once at the start
			currentCluster = ScanCluster?.Clone();
			currentCluster.BreakFromPrefab();
			currentCluster.NetworkSpawn();
			Log.Info( "Cluster cloned and scanning started" );
			Scanning = true; // Set scanning state

		}

	}

	private void UpdateScanning()
	{
		if ( Scanning && currentCluster.IsValid() )
		{
			ShootRays( 1000, 0.2f );
		}
	}

	private void StopScanning()
	{
		if ( Scanning )
		{
			Log.Info( "Scanning stopped" );
			Scanning = false; // Reset scanning state
			currentCluster = null;
			currentSnapshot = null;
		}
	}
	
	private ParticleSnapshot currentSnapshot;
	private void ShootRays( int count, float spread )
	{
		var localPlayer = Player.FindLocalPlayer();
		
		currentSnapshot = new ParticleSnapshot();


		var vertices = new List<ParticleSnapshot.Vertex>();
		
			
		for ( int i = 0; i < count; i++ )
		{
			var forward = localPlayer.EyeTransform.Forward;
			forward += Vector3.Random * spread;
			
			var ray = Scene.Trace.Ray( localPlayer.EyeTransform.Position, localPlayer.EyeTransform.Position + (forward * 65565) )
				.IgnoreGameObject( localPlayer.GameObject )
				.Run();

			if ( ray.Hit )
			{
				var vertex = new ParticleSnapshot.Vertex { Position = ray.HitPosition, Normal = ray.Normal, Color = Color.Random, };
				
				vertices.Add( vertex );

			}
			
		}
		currentSnapshot.Update( vertices.ToArray() );

		var particles = currentCluster?.GetComponent<LegacyParticleSystem>();
		particles?.SceneObject.SetControlPoint( 0, currentSnapshot );
		Log.Info( particles.IsValid );
	}

	public bool Scanning;
	
	public override void OnControl( Player player )
	{

		if (IsProxy)
			return;
		//todo:
		// dispatch a prefab with the particle
		// update its snapshot when we're still scanning
		// if we release attack1, then stop everything
		// Handle input states independently

		if ( Input.Pressed( "attack1" ) )
		{
			Log.Info( Scanning );

			StartScanning();
		}

		// Update scanning while "attack1" is held
		if ( Input.Down( "attack1" )  )
		{

			UpdateScanning();
		}

		// Stop scanning when "attack1" is released
		if ( Input.Released( "attack1" ) )
		{
			StopScanning();
			Log.Info( Scanning );
		}
	}
}
