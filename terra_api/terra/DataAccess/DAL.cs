/************************************************************************************************|
|   Project:                                                                             |
|   File:  .cs                                                                    |
|   Date: March 8, 2019                                                                          |
|   Author: Ben Shamas, Sam Guta, Kevin Park, Zaid Omar                                          |
|   Description:                   |
*************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using Npgsql;
using System.Data;
using System.Collections.Specialized;
using GeoAPI;
using NetTopologySuite;
using NetTopologySuite.Geometries.Implementation;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;

namespace terra
{
    public class DAL
    {
        private string connectionString;
        
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        public bool Init()
        {
            NpgsqlConnection.GlobalTypeMapper.UseNetTopologySuite();
            GeometryServiceProvider.Instance = new NtsGeometryServices(
            new DotSpatialAffineCoordinateSequenceFactory(Ordinates.XYM),
            new PrecisionModel(PrecisionModels.Floating),
            -1);
            connectionString = Startup.ConnectionString;

            if (String.IsNullOrEmpty(connectionString))
            {
                return false;
            }
            return true;
        }
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        public bool NonQuery(IDatabase item)
        {
            item.command.Connection = new NpgsqlConnection(connectionString);
            try
            {
                item.command.Connection.Open();
                item.command.Transaction = item.command.Connection.BeginTransaction();
                item.command.ExecuteNonQuery();
                item.command.Transaction.Commit();
            }
            catch (Exception)
            {
                if (item.command.Transaction != null)
                {
                    item.command.Transaction.Rollback();
                }
                item.command.Connection.Close();
                throw;
            }
            item.command.Connection.Close();

            return true;
        }
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        public IDatabase Read(IDatabase item)
        {
            NpgsqlDataReader reader = null;
            item.command.Connection = new NpgsqlConnection(connectionString);
            try
            {
                item.command.Connection.Open();
                reader = item.command.ExecuteReader();
            }
            catch (Exception)
            {
                item.command.Connection.Close();
                throw;
            }
            if (reader != null && reader.Read())
            {
                switch (item.GetType().Name)
                {
                    case ("User"):
                        User newUser = new User();
                        newUser.Fill(reader);
                        item.command.Connection.Close();
                        return newUser;
                    case ("Field"):
                        Field newField = new Field();
                        newField.Fill(reader);
                        item.command.Connection.Close();
                        return newField;
                    case ("Device"):
                        Device newDevice = new Device();
                        newDevice.Fill(reader);
                        item.command.Connection.Close();
                        return newDevice;
                    //case ("MoistureData"):
                    //    MoistureData moistureData = (MoistureData)temp;
                    //    moistureData.Fill(reader);
                    //    item.command.Connection.Close();
                    //    return moistureData;
                    default:
                        break;
                }
            }
            return null;
        }
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        public List<IDatabase> ReadMore(IDatabase item)
        {
            NpgsqlDataReader reader = null;
            item.command.Connection = new NpgsqlConnection(connectionString);
            try
            {
                item.command.Connection.Open();
                reader = item.command.ExecuteReader();
            }
            catch (Exception)
            {
                item.command.Connection.Close();
                throw;
            }

            if (reader != null && reader.Read())
            {
                List<IDatabase> list = new List<IDatabase>();
                switch (item.GetType().Name)
                {
                    case ("User"):
                        do
                        {
                            User newUser = new User();
                            newUser.Fill(reader);
                            list.Add(newUser);
                        } while (reader.Read());
                        item.command.Connection.Close();
                        return list;
                    case ("Field"):
                        do
                        {
                            Field newField = new Field();
                            newField.Fill(reader);
                            list.Add(newField);
                        } while (reader.Read());
                        item.command.Connection.Close();
                        return list;
                    case ("Device"):
                        do
                        {
                            Device newDevice = new Device();
                            newDevice.Fill(reader);
                            list.Add(newDevice);
                        } while (reader.Read());
                        item.command.Connection.Close();
                        return list;
                    case ("SoilCompaction"):
                        SoilCompaction soilCompaction = new SoilCompaction();
                        do
                        {                           
                            soilCompaction.Fill(reader);                           
                        } while (reader.Read());
                        list.Add(soilCompaction);
                        item.command.Connection.Close();
                        return list;
                    case ("EarthData"):
                        do
                        {
                            EarthData earthData = new EarthData();
                            earthData.Fill(reader);
                            list.Add(earthData);
                        } while (reader.Read());
                        item.command.Connection.Close();
                        return list;
                    case ("EarthDataTypes"):
                        do
                        {
                            EarthDataTypes earthDataTypes = new EarthDataTypes();
                            earthDataTypes.Fill(reader);
                            list.Add(earthDataTypes);
                        } while (reader.Read());
                        item.command.Connection.Close();
                        return list;
                    case ("FieldSpecific"):
                        do
                        {
                            FieldSpecific fieldSpecific = new FieldSpecific();
                            fieldSpecific.Fill(reader);
                            list.Add(fieldSpecific);
                        } while (reader.Read());
                        item.command.Connection.Close();
                        return list;
                    case ("DataHandler"):
                        do
                        {
                            DataHandler dataHandler = new DataHandler();
                            dataHandler.Fill(reader);
                            list.Add(dataHandler);
                        } while (reader.Read());
                        item.command.Connection.Close();
                        return list;
                    default:
                        break;

                }
            }
            return null;
        }

        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        public bool CheckOwnership(Field field)
        {
            NpgsqlDataReader reader = null;
            field.command.Connection = new NpgsqlConnection(connectionString);
            try
            {
                field.command.Connection.Open();
                reader = field.command.ExecuteReader();
            }
            catch (Exception)
            {
                field.command.Connection.Close();
                throw;
            }
            
            if (reader.Read())
            {
                bool result;
                bool.TryParse(reader[0].ToString(),out result);
                field.command.Connection.Close();
                return result;
            }
            field.command.Connection.Close();
            return false;
        }

        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        public int Count(Field field)
        {
            NpgsqlDataReader reader = null;
            field.command.Connection = new NpgsqlConnection(connectionString);
            try
            {
                field.command.Connection.Open();
                reader = field.command.ExecuteReader();
            }
            catch (Exception)
            {
                field.command.Connection.Close();
                throw;
            }

            if (reader.Read())
            {
                int result;
                int.TryParse(reader[0].ToString(), out result);
                field.command.Connection.Close();
                return result;
            }
            field.command.Connection.Close();
            return 0;
        }
    }

}
