
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_Buildings : ES2Type
{
    public override void Write(object obj, ES2Writer writer)
    {
        Buildings data = (Buildings)obj;
        // Add your writer.Write calls here.
        writer.Write(data.id);
        writer.Write(data.buildingID);
        writer.Write(data.name);
        writer.Write(data.state);
        writer.Write(data.unlockedQueueSlots);
        writer.Write(data.itemID);
        writer.Write(data.dateTime);

    }

    public override object Read(ES2Reader reader)
    {
        Buildings data = new Buildings();
        Read(reader, data);
        return data;
    }

    public override void Read(ES2Reader reader, object c)
    {
        Buildings data = (Buildings)c;
        // Add your reader.Read calls here to read the data into the object.
        data.id = reader.Read<System.Int32>();
        data.buildingID = reader.Read<System.Int32>();
        data.name = reader.Read<System.String>();
        data.state = reader.Read<System.Int32>();
        data.unlockedQueueSlots = reader.Read<System.Int32>();
        data.itemID = reader.ReadArray<System.Int32>();
        data.dateTime = reader.ReadArray<System.String>();

    }

    /* ! Don't modify anything below this line ! */
    public ES2UserType_Buildings() : base(typeof(Buildings)) { }
}
