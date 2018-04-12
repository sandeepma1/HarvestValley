
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_Fishes : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		Fishes data = (Fishes)obj;
		// Add your writer.Write calls here.
		writer.Write(data.itemId);
		writer.Write(data.fishLength);
		writer.Write(data.nextSpawnDateTime);

	}
	
	public override object Read(ES2Reader reader)
	{
		Fishes data = new Fishes();
		Read(reader, data);
		return data;
	}
	
	public override void Read(ES2Reader reader, object c)
	{
		Fishes data = (Fishes)c;
		// Add your reader.Read calls here to read the data into the object.
		data.itemId = reader.Read<System.Int32>();
		data.fishLength = reader.Read<System.Single>();
		data.nextSpawnDateTime = reader.Read<System.String>();

	}
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_Fishes():base(typeof(Fishes)){}
}
