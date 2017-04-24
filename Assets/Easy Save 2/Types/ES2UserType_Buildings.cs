
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_Buildings : ES2Type
{
	public override void Write (object obj, ES2Writer writer)
	{
		Buildings data = (Buildings)obj;
		// Add your writer.Write calls here.
		writer.Write (data.id);
		writer.Write (data.name);
		writer.Write (data.pos);
		writer.Write (data.level);
		writer.Write (data.state);
		writer.Write (data.itemID1);
		writer.Write (data.dateTime1);
		writer.Write (data.itemID2);
		writer.Write (data.dateTime2);
		writer.Write (data.itemID3);
		writer.Write (data.dateTime3);

	}

	public override object Read (ES2Reader reader)
	{
		Buildings data = new Buildings ();
		Read (reader, data);
		return data;
	}

	public override void Read (ES2Reader reader, object c)
	{
		Buildings data = (Buildings)c;
		// Add your reader.Read calls here to read the data into the object.
		data.id = reader.Read<System.Int32> ();
		data.name = reader.Read<System.String> ();
		data.pos = reader.Read<UnityEngine.Vector2> ();
		data.level = reader.Read<System.Int32> ();
		data.state = reader.Read<System.Int32> ();
		data.itemID1 = reader.Read<System.Int32> ();
		data.dateTime1 = reader.Read<System.String> ();
		data.itemID2 = reader.Read<System.Int32> ();
		data.dateTime2 = reader.Read<System.String> ();
		data.itemID3 = reader.Read<System.Int32> ();
		data.dateTime3 = reader.Read<System.String> ();

	}
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_Buildings () : base (typeof(Buildings))
	{
	}
}
