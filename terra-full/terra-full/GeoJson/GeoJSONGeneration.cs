/************************************************************************************************|
|   Project: Terra                                                                               |
|   File:  GeoJsonGeneration.cs                                                                  |
|   Date: March 8, 2019                                                                          |
|   Author: Ben Shamas, Sam Guta, Kevin Park, Zaid Omar                                          |
|   Description: This file contains the functionality for generating geojson                     |
*************************************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;

namespace terra
{
    class GeoJSONGeneration
    {
        // Function   : FieldDataGeneration
        // Description: Generates the geojson from field data
        // Paramaters : List<earthdata>: The list of earthdata
        //              string: The data type
        // Returns    : The list of earthdata in geojson format.
        public static string FieldDataGeneration( List<EarthData> dataset, string dataType)
        {

			// this is a const as this is a name that goes along with the value in this case sm == soil moisture
			// will be changed dependent on the calling filter
			const string valueName = "sm";

			//dependent on the data we are retrieving e.g. soilMoisture
			string fileName = dataType;
			var points = new List<Point>();
			List<Dictionary<string, object>> valueList = new List<Dictionary<string, object>>();


			foreach (EarthData data in dataset)
			{
				// cast it to earthdata so we can access the data properly 
				//EarthData earth = (EarthData)data;
				//									might need to flip which is X and Y
				points.Add( new Point(new Position(data.coordinates.Y, data.coordinates.X)));

				// create a list of dictionaries as we need to get the values through indexing.
				// upon run time these values won't be known as it is from the db which is taken from the earth data
				valueList.Add(new Dictionary<string, object> { { valueName, data.dataValue } });
				//could make a class specific for this properties but we need the 'valuename' to be dynamic...

			};

            List<Feature> features = new List<Feature>();
            int positionCount = 0;

            try
            {
                foreach (var point in points)
                {
                    // example feature 
                    // {"type":"Feature","geometry":{"type":"Point","coordinates":[4.8892593383789063,52.370725881211314]},"properties":{"sm":10.0}}
                    //Feature feature = new Feature(point, Dic);
                    // create a new feature 
                    Feature feature = new Feature(point, valueList[positionCount]);
                    // add the feature into the collection
                    features.Add(feature);
                    positionCount++;
                }
            }
            catch (IOException)
            {
                //log the error, potential value mis-count
            }

            // create the collection
            var collection = new FeatureCollection(features);


            // get my docs path 
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // serialize the data into a json object which when given the geojson file type is now geojson data.
            var jsonObj = JsonConvert.SerializeObject(collection);
            return jsonObj;
        }

        // Function   : FieldGeneration
        // Description: Generates the geojson from field data
        // Paramaters : List<earthdata>: The list of earthdata
        //              string: The data type
        // Returns    : The list of earthdata in geojson format.
        public static string FieldGeneration(List<Field> dataSet,DAL dal = null, Field userField = null )
        {

            //string fileName = "fieldsTest";
            List<Polygon> fieldPolys = new List<Polygon>();
            List<FieldProperties> fieldProperties = new List<FieldProperties>();
            List<Feature> fieldFeatures = new List<Feature>();
            if (dal != null & userField != null)
            {
                userField.Init(IDatabase.CommandType.CheckOwnership);
            }

            foreach (Field data in dataSet)
            {
                //Generate coordinates from string.
                //NetTopologySuite.Geometries.Polygon tempGeom = (NetTopologySuite.Geometries.Polygon) data.field_cords;
                NetTopologySuite.Geometries.Polygon tempGeom = data.field_coordinates;

                GeoAPI.Geometries.Coordinate[] cordList = tempGeom.Coordinates;

                List<IPosition> temp = new List<IPosition>();

                int maxCordCount = cordList.Length;

                for (int cordCount = 0; cordCount < maxCordCount; cordCount++)
                {
					//temp.Add(new Position(cordList[cordCount].X, cordList[cordCount].Y));
					//Must use format with SRID: 4326
					temp.Add(new Position(cordList[cordCount].Y, cordList[cordCount].X));
				}

                fieldPolys.Add(new Polygon(new List<LineString>
                {new LineString(temp)

                }));

                if (dal != null && userField != null)
                {
                    userField.field_id = data.field_id;
                    userField.CheckOwnershipVariables("all");
                    if (dal.CheckOwnership(userField))
                    {
                        fieldProperties.Add(new FieldProperties
                        {
                            id = data.field_id.ToString(),
                            field_name = data.field_name,
                            //temperature = 0.0f,
                            field_ownership = FieldProperties.OwnerShipType.OwnedByUser
                        });
                    }
                    else
                    {
                        fieldProperties.Add(new FieldProperties
                        {
                            id = data.field_id.ToString(),
                            field_name = data.field_name,
                            //temperature = 0.0f,
                            field_ownership = FieldProperties.OwnerShipType.OwnedByOther
                        });
                    }



                }
                else
                {
                    fieldProperties.Add(new FieldProperties
                    {
                        id = data.field_id.ToString(),
                        field_name = data.field_name,
                        //temperature = 0.0f,
                        field_ownership = FieldProperties.OwnerShipType.NotOwned
                    });
                }

                //fieldPolys.Add(data.field_cords);
            }

            int fieldCount = 0;
            foreach (Polygon poll in fieldPolys)
            {

                fieldFeatures.Add(new Feature(poll, fieldProperties[fieldCount]));
                fieldCount++;
            }


            // create the collection
            var collection = new FeatureCollection(fieldFeatures);


            // get my docs path 
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // serialize the data into a json object which when given the geojson file type is now geojson data.
            var jsonObj = JsonConvert.SerializeObject(collection);

            return jsonObj;

        }
    }
}

// Function   : 
// Description: 
// Paramaters : none
// Returns    : void
//public static string SoilCompactionGeneration(SoilCompaction soilCompaction)
//{

//    // this is a const as this is a name that goes along with the value in this case sm == soil moisture
//    // will be changed dependent on the calling filter
//    const string valueName = "sc";

//    //dependent on the data we are retrieving e.g. soilMoisture
//    //string fileName = dataType;
//    var points = new List<Point>();
//    List<Dictionary<string, object>> valueList = new List<Dictionary<string, object>>();

//    int count = 0;
//    foreach (var data in soilCompaction.coordinates)
//    {
//        // cast it to earthdata so we can access the data properly 
//        //EarthData earth = (EarthData)data;
//        //									might need to flip which is X and Y
//        points.Add(new Point(new Position(data.Coordinate.Y, data.Coordinate.X)));

//        // create a list of dictionaries as we need to get the values through indexing.
//        // upon run time these values won't be known as it is from the db which is taken from the earth data
//        valueList.Add(new Dictionary<string, object> { { valueName, soilCompaction.soilCompaction[count] } });
//        count++;
//        //could make a class specific for this properties but we need the 'valuename' to be dynamic...
//    };

//    List<Feature> features = new List<Feature>();
//    int positionCount = 0;

//    try
//    {
//        foreach (var point in points)
//        {
//            // example feature 
//            // {"type":"Feature","geometry":{"type":"Point","coordinates":[4.8892593383789063,52.370725881211314]},"properties":{"sm":10.0}}
//            //Feature feature = new Feature(point, Dic);
//            // create a new feature 
//            Feature feature = new Feature(point, valueList[positionCount]);
//            // add the feature into the collection
//            features.Add(feature);
//            positionCount++;
//        }
//    }
//    catch (IOException e)
//    {
//        //log the error, potential value mis-count
//    }

//    // create the collection
//    var collection = new FeatureCollection(features);


//    // get my docs path 
//    string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

//    // serialize the data into a json object which when given the geojson file type is now geojson data.
//    var jsonObj = JsonConvert.SerializeObject(collection);
//    return jsonObj;
//    // -- dev line
//    //File.WriteAllText(Path.Combine(docPath, fileName + ".geojson"), jsonObj);

//    //// -- dev line
//    //Console.WriteLine($"GeoJson Geometry Written To: {Path.Combine(docPath, fileName + ".geojson")}");
//}
