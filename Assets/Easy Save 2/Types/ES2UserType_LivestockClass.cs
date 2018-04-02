
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_LivestockClass : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		LivestockClass data = (LivestockClass)obj;
		// Add your writer.Write calls here.
		writer.Write(data.livestockId);
		writer.Write(data.canProduceItemId);
		writer.Write(data.biteCount);
		writer.Write(data.hatched);
		writer.Write(data.maxHatchCount);
		writer.Write(data.livestockType);
		writer.Write(data.dateTime);

	}
	
	public override object Read(ES2Reader reader)
	{
		LivestockClass data = new LivestockClass();
		Read(reader, data);
		return data;
	}
	
	public override void Read(ES2Reader reader, object c)
	{
		LivestockClass data = (LivestockClass)c;
		// Add your reader.Read calls here to read the data into the object.
		data.livestockId = reader.Read<System.Int32>();
		data.canProduceItemId = reader.Read<System.Int32>();
		data.biteCount = reader.Read<System.Int32>();
		data.hatched = reader.Read<System.Int32>();
		data.maxHatchCount = reader.Read<System.Int32>();
		data.livestockType = reader.Read<LivestockType>();
		data.dateTime = reader.Read<System.String>();

	}
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_LivestockClass():base(typeof(LivestockClass)){}
}
