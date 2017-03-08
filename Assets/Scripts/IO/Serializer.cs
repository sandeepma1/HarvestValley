using System;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

public class Serializer
{
	/// <summary>
	/// ExampleClass exampleClass = Serializer.Load<ExampleClass>(filenameWithExtension));
	/// </summary>
	/// <param name="filename">Filename.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static T Load<T> (string filename) where T: class
	{
		if (File.Exists (filename)) {
			try {
				using (Stream stream = File.OpenRead (Application.persistentDataPath + "/" + filename)) {
					BinaryFormatter formatter = new BinaryFormatter ();
					return formatter.Deserialize (stream) as T;
				}
			} catch (Exception e) {
				Debug.Log (e.Message);
			}
		}
		return default(T);
	}

	/// <summary>
	/// Serializer.Save<ExampleClass>(filenameWithExtension, exampleClass);
	/// </summary>
	/// <param name="filename">Filename.</param>
	/// <param name="data">Data.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static void Save<T> (string filename, T data) where T: class
	{
		using (Stream stream = File.OpenWrite (Application.persistentDataPath + "/" + filename)) {    
			BinaryFormatter formatter = new BinaryFormatter ();
			formatter.Serialize (stream, data);
		}
	}
}

//Serializer.Save<ExampleClass>(filenameWithExtension, exampleClass);
//ExampleClass exampleClass = Serializer.Load<ExampleClass>(filenameWithExtension));