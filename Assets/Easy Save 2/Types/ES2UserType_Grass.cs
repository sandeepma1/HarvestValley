
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_Grass : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		Grass data = (Grass)obj;
		// Add your writer.Write calls here.
		writer.Write(data.grassId);
		writer.Write(data.itemId);
		writer.Write(data.position);

	}
	
	public override object Read(ES2Reader reader)
	{
		Grass data = new Grass();
		Read(reader, data);
		return data;
	}
	
	public override void Read(ES2Reader reader, object c)
	{
		Grass data = (Grass)c;
		// Add your reader.Read calls here to read the data into the object.
		data.grassId = reader.Read<System.Int32>();
		data.itemId = reader.Read<System.Int32>();
		data.position = reader.Read<UnityEngine.Vector2>();

	}
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_Grass():base(typeof(Grass)){}
}
