
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_AAA : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		AAA data = (AAA)obj;
		// Add your writer.Write calls here.
		writer.Write(data.id);
		writer.Write(data.name);
		writer.Write(data.pos);
		writer.Write(data.aa);

	}
	
	public override object Read(ES2Reader reader)
	{
		AAA data = new AAA();
		Read(reader, data);
		return data;
	}
	
	public override void Read(ES2Reader reader, object c)
	{
		AAA data = (AAA)c;
		// Add your reader.Read calls here to read the data into the object.
		data.id = reader.Read<System.Int32>();
		data.name = reader.Read<System.String>();
		data.pos = reader.ReadArray<System.Int32>();
		data.aa = reader.ReadQueue<System.Int32>();

	}
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_AAA():base(typeof(AAA)){}
}
