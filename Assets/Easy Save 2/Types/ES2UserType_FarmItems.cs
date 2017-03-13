
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_FarmItems : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		FarmItems data = (FarmItems)obj;
		// Add your writer.Write calls here.
		writer.Write(data.id);
		writer.Write(data.count);

	}
	
	public override object Read(ES2Reader reader)
	{
		FarmItems data = new FarmItems();
		Read(reader, data);
		return data;
	}
	
	public override void Read(ES2Reader reader, object c)
	{
		FarmItems data = (FarmItems)c;
		// Add your reader.Read calls here to read the data into the object.
		data.id = reader.Read<System.Int32>();
		data.count = reader.Read<System.Int32>();

	}
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_FarmItems():base(typeof(FarmItems)){}
}
