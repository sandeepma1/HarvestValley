
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_GrassTypes : ES2Type
{
    public override void Write(object obj, ES2Writer writer)
    {
        GrassTypes data = (GrassTypes)obj;
        // Add your writer.Write calls here.
        writer.Write(data.id);
        writer.Write(data.state);
        writer.Write(data.grassTypeID);
        writer.Write(data.dateTime);

    }

    public override object Read(ES2Reader reader)
    {
        GrassTypes data = new GrassTypes();
        Read(reader, data);
        return data;
    }

    public override void Read(ES2Reader reader, object c)
    {
        GrassTypes data = (GrassTypes)c;
        // Add your reader.Read calls here to read the data into the object.
        data.id = reader.Read<System.Int32>();
        data.state = reader.Read<System.Int32>();
        data.grassTypeID = reader.Read<System.Int32>();
        data.dateTime = reader.Read<System.String>();

    }

    /* ! Don't modify anything below this line ! */
    public ES2UserType_GrassTypes() : base(typeof(GrassTypes)) { }
}
