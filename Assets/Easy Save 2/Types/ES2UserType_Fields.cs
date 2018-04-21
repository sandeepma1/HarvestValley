
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_Fields : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		Fields data = (Fields)obj;
		// Add your writer.Write calls here.
		writer.Write(data.id);
		writer.Write(data.fieldID);
		writer.Write(data.name);
		writer.Write(data.position);
		writer.Write(data.level);
		writer.Write(data.state);
		writer.Write(data.itemID);
		writer.Write(data.dateTime);

	}
	
	public override object Read(ES2Reader reader)
	{
		Fields data = new Fields();
		Read(reader, data);
		return data;
	}
	
	public override void Read(ES2Reader reader, object c)
	{
		Fields data = (Fields)c;
		// Add your reader.Read calls here to read the data into the object.
		data.id = reader.Read<System.Int32>();
		data.fieldID = reader.Read<System.Int32>();
		data.name = reader.Read<System.String>();
		data.position = reader.Read<UnityEngine.Vector2>();
		data.level = reader.Read<System.Int32>();
		data.state = reader.Read<System.Int32>();
		data.itemID = reader.Read<System.Int32>();
		data.dateTime = reader.Read<System.String>();

	}
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_Fields():base(typeof(Fields)){}
}
