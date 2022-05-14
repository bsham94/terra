/************************************************************************************************|
|   Project: TERRA                                                                               |
|   File: DAL.cs                                                                                 |
|   Date: March 8, 2019                                                                          |
|   Author: Ben Shamas, Sam Guta, Kevin Park, Zaid Omar                                          |
|   Description: This file contains the methods for database functionality                       |
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
        
        // Function   : Init
        // Description: Initializes the connection string.
        // Paramaters : none
        // Returns    : bool: Whether initializing the data access layer was successful.
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
        // Function   : NonQuery
        // Description: This function performs all non-query calls to the database.
        // Paramaters : IDatabase: the data object.
        // Returns    : bool: Whether the database call was successful.
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
            catch (Exception e)
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
        // Function   : Read
        // Description: Reads a single row from the database results
        // Paramaters : IDatabase: The data object.
        // Returns    : IDatabase: The dataobject containing the result data.
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
                    default:
                        item.command.Connection.Close();
                        break;
                }
            }
            if (item.command.Connection != null && item.command.Connection.State.Equals(ConnectionState.Open))
            {
                item.command.Connection.Close();
            }
            //Nothing was returned by the database.
            return null;
        }
        // Function   : ReadMore
        // Description: Reads all result rows from the database
        // Paramaters : IDatabase: The dataobject
        // Returns    : List<IDatabase>: The list of result dataobjects.
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
                //Fill the correct dataobjects with the results and add to the result list.  
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
                        item.command.Connection.Close();
                        break;
                }
            }
            if (item.command.Connection != null && item.command.Connection.State.Equals(ConnectionState.Open))
            {
                item.command.Connection.Close();
            }
            //Nothing was returned by the database.
            return null;
        }

        // Function   : CheckByOwnership
        // Description: Checks if the user owns the specific field
        // Paramaters : field: The field to check ownership
        // Returns    : bool: Whether the field is owner or not.
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

        // Function   : Count
        // Description: Gets the amount of fields owned by a user.
        // Paramaters : Field: The field object
        // Returns    : int: The amount of fields.
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
        // Function   : GetArea
        // Description: Gets the size of the field
        // Paramaters : Field: The field object
        // Returns    : float: The size of the field.
        public float GetArea(Field field)
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
                float result;
                float.TryParse(reader[0].ToString(), out result);
                field.command.Connection.Close();
                return result;
            }
            field.command.Connection.Close();
            return 0;
        }
    }

}
