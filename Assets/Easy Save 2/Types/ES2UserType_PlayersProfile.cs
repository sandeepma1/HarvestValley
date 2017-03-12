
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_PlayersProfile : ES2Type
{
	public override void Write (object obj, ES2Writer writer)
	{
		PlayersProfile data = (PlayersProfile)obj;
		// Add your writer.Write calls here.
		writer.Write (data.name);
		writer.Write (data.farmName);
		writer.Write (data.level);
		writer.Write (data.XPPoints);
		writer.Write (data.gold);
		writer.Write (data.gems);
		writer.Write (data.stamina);
		writer.Write (data.staminaMaxDateTime);

	}

	public override object Read (ES2Reader reader)
	{
		PlayersProfile data = new PlayersProfile ();
		Read (reader, data);
		return data;
	}

	public override void Read (ES2Reader reader, object c)
	{
		PlayersProfile data = (PlayersProfile)c;
		// Add your reader.Read calls here to read the data into the object.
		data.name = reader.Read<System.String> ();
		data.farmName = reader.Read<System.String> ();
		data.level = reader.Read<System.Int32> ();
		data.XPPoints = reader.Read<System.Int32> ();
		data.gold = reader.Read<System.Int32> ();
		data.gems = reader.Read<System.Int32> ();
		data.stamina = reader.Read<System.Int32> ();
		data.staminaMaxDateTime = reader.Read<System.String> ();

	}
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_PlayersProfile () : base (typeof(PlayersProfile))
	{
	}
}
