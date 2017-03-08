
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_SaveFarmLand : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		SaveFarmLand data = (SaveFarmLand)obj;
		// Add your writer.Write calls here.
		writer.Write(data.tileIndex);
		writer.Write(data.level);
		writer.Write(data.seedIndex);
		writer.Write(data.state);
		writer.Write(data.dateTime);

	}
	
	public override object Read(ES2Reader reader)
	{
		SaveFarmLand data = new SaveFarmLand();
		Read(reader, data);
		return data;
	}
	
	public override void Read(ES2Reader reader, object c)
	{
		SaveFarmLand data = (SaveFarmLand)c;
		// Add your reader.Read calls here to read the data into the object.
		data.tileIndex = reader.Read<System.SByte>();
		data.level = reader.Read<System.SByte>();
		data.seedIndex = reader.Read<System.SByte>();
		data.state = reader.Read<System.SByte>();
		data.dateTime = reader.Read<System.String>();

	}
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_SaveFarmLand():base(typeof(SaveFarmLand)){}
}
