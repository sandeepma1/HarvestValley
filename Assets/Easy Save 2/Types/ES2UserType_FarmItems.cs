
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_FarmItems : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		InventoryItems data = (InventoryItems)obj;
		// Add your writer.Write calls here.
		writer.Write(data.itemId);
		writer.Write(data.itemCount);

	}
	
	public override object Read(ES2Reader reader)
	{
		InventoryItems data = new InventoryItems();
		Read(reader, data);
		return data;
	}
	
	public override void Read(ES2Reader reader, object c)
	{
		InventoryItems data = (InventoryItems)c;
		// Add your reader.Read calls here to read the data into the object.
		data.itemId = reader.Read<System.Int32>();
		data.itemCount = reader.Read<System.Int32>();

	}
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_FarmItems():base(typeof(InventoryItems)){}
}
