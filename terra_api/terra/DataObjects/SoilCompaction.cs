using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;
using Npgsql;
using GeoJSON.Net.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace terra
{
    public class SoilCompaction : IDatabase
    {

        public int currentCoordinate = 0;
        public int currentSoilCompaction = 0;
        public List<float> soilCompaction;
        public List<NetTopologySuite.Geometries.Point> coordinates;
        public GeoJSON.Net.Geometry.LineString linestring;
        public int field_id;

        public SoilCompaction()
        {
            soilCompaction = new List<float>();
            coordinates = new List<NetTopologySuite.Geometries.Point>();
        }
        [JsonConstructor]
        public SoilCompaction(int field_id,List<float> soilCompaction, GeoJSON.Net.Geometry.LineString linestring)
        {
            this.soilCompaction = soilCompaction;
            this.linestring = linestring;
            if (soilCompaction == null)
            {
                soilCompaction = new List<float>();
            }
            coordinates = new List<NetTopologySuite.Geometries.Point>();
            if (linestring == null)
            {
                linestring = null;
            }
            else
            {
                foreach (IPosition item in linestring.Coordinates)
                {

                    coordinates.Add(new NetTopologySuite.Geometries.Point(item.Latitude, item.Longitude));
                }
            }

            this.field_id = field_id;
        }

        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        //[JsonConstructor]
        //public Field(string user_id, GeoJSON.Net.Geometry.Polygon field_cords, string field_name, int field_id = 0)
        //{
        //    this.user_id = user_id;
        //    this.field_id = field_id;
        //    if (field_cords != null)
        //    {
        //        List<Coordinate> coordinates = new List<Coordinate>();
        //        foreach (var item in field_cords.Coordinates)
        //        {
        //            foreach (var otherItem in item.Coordinates)
        //            {
        //                coordinates.Add(new Coordinate(otherItem.Longitude, otherItem.Latitude));
        //            }
        //        }
        //        this.field_coordinates = new Polygon(new LinearRing(coordinates.ToArray()));
        //    }
        //    else
        //    {
        //        this.field_coordinates = null;
        //    }
        //    this.field_name = field_name;
        //}

        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        public override void Fill(NpgsqlDataReader reader)
        {
            field_id = int.Parse(reader["field_id"].ToString());
            soilCompaction.Add(float.Parse(reader["soil_compaction"].ToString()));
            coordinates.Add((NetTopologySuite.Geometries.Point)(reader["coordinate"]));

        }
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        public override void Init(CommandType type)
        {
            switch (type)
            {
                case CommandType.Create:
                    command = new NpgsqlCommand()
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = "createsoilcompaction"

                    };
                    break;
                case CommandType.Read:
                    command = new NpgsqlCommand()
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = "getsoilcompaction"
                    };
                    break;
                case CommandType.ReadWithParameters:
                    command = new NpgsqlCommand()
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = "getsoilcompaction"
                    };
                    break;
                case CommandType.ReadAll:
                    command = new NpgsqlCommand()
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = "getsoilcompaction"
                    };
                    break;
                case CommandType.Update:
                    command = new NpgsqlCommand()
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = "updatefield"
                    };
                    break;
                case CommandType.Delete:
                    command = new NpgsqlCommand()
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = "DeleteField"
                    };
                    break;
                case CommandType.CheckOwnership:
                    command = new NpgsqlCommand()
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = "checkownership"
                    };
                    break;
                default:
                    break;
            }
        }
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        public override void SetInsertVariables()
        {
            ClearParameters();
            if (command != null)
            {
                command.Parameters.Add(new NpgsqlParameter("field_id", field_id));
                command.Parameters.Add(new NpgsqlParameter("soil_compaction", soilCompaction[currentSoilCompaction]));
                command.Parameters.Add(new NpgsqlParameter("coordinate", coordinates[currentCoordinate]));
            }
        }
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        public void SetSelectVariables(string searchType = null)
        {
            ClearParameters();
            if (command != null)
            {
                switch (searchType)
                {
                    case ("field_id"):
                        command.Parameters.Add(new NpgsqlParameter("field_id", field_id));
                        break;
                        //default:
                        //    command.Parameters.Add(new NpgsqlParameter("field_id", field_id));
                        //break;
                }
            }
        }
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        //public void CheckOwnershipVariables(string type = null)
        //{
        //    ClearParameters();
        //    if (command != null)
        //    {
        //        switch (type)
        //        {
        //            case ("all"):
        //                command.Parameters.Add(new NpgsqlParameter("user_id", user_id));
        //                command.Parameters.Add(new NpgsqlParameter("field_id", field_id));
        //                break;
        //            default:
        //                command.Parameters.Add(new NpgsqlParameter("user_id", user_id));
        //                command.Parameters.Add(new NpgsqlParameter("field_name", field_name));
        //                break;
        //        }

        //    }
        //}
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        private void ClearParameters()
        {
            if (command != null)
            {
                if (command.Parameters.Count != 0)
                {
                    command.Parameters.Clear();
                }
            }
        }
    }
}
