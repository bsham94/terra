-- Database: terra
-- CREATE DATABASE terra;
-- DROP DATABASE terra;
-- CREATE EXTENSION postgis;

CREATE TABLE account_type(
	account_type_id SERIAL PRIMARY KEY NOT NULL,
	account_type TEXT NOT NULL
);

CREATE TABLE users(
	user_id TEXT PRIMARY KEY NOT NULL,
	user_name TEXT UNIQUE NOT NULL,
	account_type_id INT NOT NULL,
	field_count INT NOT NULL,
	FOREIGN KEY(account_type_id) REFERENCES account_type(account_type_id)
);

--ALTER TABLE users Add COLUMN user_name TEXT UNIQUE;
--DROP TABLE fields;
CREATE TABLE fields(
	field_id SERIAL UNIQUE NOT NULL,
	user_id TEXT NOT NULL,
	field_name TEXT NOT NULL,
	field_cords geometry(polygon, 4326) NOT NULL,
	FOREIGN KEY(user_id) REFERENCES users(user_id),
	PRIMARY KEY(user_id,field_name)
);

--DROP TABLE field_specifics;
CREATE TABLE field_specifics(
	specifics_id SERIAL PRIMARY KEY NOT NULL,
	field_id INT NOT NULL,
	yield FLOAT NOT NULL,
	seedPlanted TEXT NOT NULL,
	fertilizer_use TEXT NOT NULL,
	pesticide_use TEXT NOT NULL,
	entry_date DATE NOT NULL DEFAULT CURRENT_DATE,
	FOREIGN KEY(field_id) REFERENCES fields(field_id)
);


CREATE TABLE device_type(
	device_type_id SERIAL PRIMARY KEY NOT NULL,
	device_name TEXT NOT NULL
);

--DROP TABLE device;
CREATE TABLE device(
	device_id SERIAL PRIMARY KEY NOT NULL, 
	device_type_id INT NOT NULL,
	field_id INT NOT NULL,
	device_data INT NOT NULL,
	FOREIGN KEY(field_id) REFERENCES fields(field_id),
	FOREIGN KEY(device_type_id) REFERENCES device_type(device_type_id)	
);

CREATE TABLE SharedFields(
	field_owner_id TEXT NOT NULL,
	shared_owner_id TEXT NOT NULL,
	FOREIGN KEY(field_owner_id) REFERENCES users(user_id),
	FOREIGN KEY(shared_owner_id) REFERENCES users(user_id),
	PRIMARY KEY(field_owner_id,shared_owner_id)
);

CREATE TABLE SoilCompaction(
	field_id SERIAL UNIQUE NOT NULL,
	soil_compaction float NOT NULL,
	coordinate geometry NOT NULL,
	FOREIGN KEY(field_id) REFERENCES fields(field_id),
	PRIMARY KEY(field_id,coordinate)
);


-- all tables related to earth data
CREATE TABLE earth_data(
	data_type_id SERIAL PRIMARY KEY NOT NULL,
	data_name TEXT,
	dataset_handler TEXT
	--FOREIGN KEY(DataSetID) REFERENCES precipitation_rate(data_id)
);


CREATE TABLE soil_moisture_handler(
	data_set_id SERIAL PRIMARY KEY NOT NULL,
	data_set_name TEXT NOT NULL,
	data_time TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
	data_date DATE NOT NULL DEFAULT CURRENT_DATE
);

INSERT INTO earth_data(data_name, dataset_handler) VALUES('Soil Moisture', 'soil_moisture_handler');

CREATE TABLE precipitation_handler(
	data_set_id SERIAL PRIMARY KEY NOT NULL,
	data_set_name TEXT NOT NULL,
	data_time TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
	data_date DATE NOT NULL DEFAULT CURRENT_DATE
);

INSERT INTO earth_data(data_name, dataset_handler) VALUES('Precipitation', 'precipitation_handler');


CREATE TABLE vegetation_indices_handler(
	data_set_id SERIAL PRIMARY KEY NOT NULL,
	data_set_name TEXT NOT NULL,
	data_time TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
	data_date DATE NOT NULL DEFAULT CURRENT_DATE
);

INSERT INTO earth_data(data_name, dataset_handler) VALUES('Vegetation Indices', 'vegetation_indices_handler');


CREATE TABLE surface_temp_handler(
	data_set_id SERIAL PRIMARY KEY NOT NULL,
	data_set_name TEXT NOT NULL,
	data_time TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
	data_date DATE NOT NULL DEFAULT CURRENT_DATE
);

INSERT INTO earth_data(data_name, dataset_handler) VALUES('Surface Temperature', 'surface_temp_handler');


CREATE TABLE errorlogging(
	logging_id SERIAL PRIMARY KEY NOT NULL,
	description TEXT NOT NULL,
	function_name TEXT NOT NULL,
	class_name TEXT NOT NULL,
	error_time TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE activitylogging(
	logging_id SERIAL PRIMARY KEY NOT NULL,
	description TEXT NOT NULL,
	function_name TEXT NOT NULL,
	class_name TEXT NOT NULL,
	activity_time TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);



CREATE OR REPLACE FUNCTION logerror(description TEXT, function_name TEXT, class_name TEXT) RETURNS void
AS $$
BEGIN
	INSERT INTO errorlogging(description, function_name, class_name) VALUES($1,$2,$3);
END
$$
LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION logactivity(description TEXT, function_name TEXT, class_name TEXT) RETURNS void
AS $$
BEGIN
	INSERT INTO activitylogging(description, function_name, class_name) VALUES($1,$2,$3);
END
$$
LANGUAGE plpgsql;



CREATE OR REPLACE FUNCTION create_dataset(handler_name TEXT, t_name TEXT)
RETURNS VOID AS
$func$
BEGIN
EXECUTE format('INSERT INTO %s(data_set_name, data_time, data_date) VALUES(%L, DEFAULT, DEFAULT);
			   CREATE TABLE IF NOT EXISTS %s (
    data_id serial PRIMARY KEY NOT NULL,
    data_value float NOT NULL,
    coordinates geometry(POINT, 4326) NOT NULL
   )', handler_name, t_name, t_name);
--INSERT INTO %s(data_time, data_date) VALUES(DEFAULT, DEFAULT);
																				   
EXECUTE format('
   CREATE TABLE IF NOT EXISTS %s (
    data_id serial PRIMARY KEY NOT NULL,
    data_value float NOT NULL,
    coordinates geometry(POINT, 4326) NOT NULL
   )', t_name);

END
$func$
LANGUAGE plpgsql;
-- earth data -END-
--SELECT create_dataset('vegetation_indices_handler', 'vegetation2');
																							
-- TYPES
CREATE TYPE earthData AS (dataValue FLOAT, cords geometry(POINT, 4326) );	
CREATE TYPE handlerData AS (dataSetName TEXT, dataDate DATE);	

																				  
-- Functions / Stored Procedures.

CREATE OR REPLACE FUNCTION GetFieldArea(fieldID INT) RETURNS FLOAT
AS $$
BEGIN
    RETURN ST_Area(the_geom::geography) As sqm--/ 1609.34^2 As sqm --*POWER(0.3048,2) As sqm
        FROM (SELECT field_cords FROM fields WHERE field_id = fieldID) As foo(the_geom);
        --acres = m² * 0.00024711
        -- sqm = metres^2
		-- ST_Transform(g.geom, 4326)::geography
		--ST_Transform(geom, 4326)
		-- utm = units i
		-- ST_Area(
		-- ST_Transform(the_geom, utmzone(ST_Centroid(the_geom))
		--	)) 
END
$$
LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetUser(userId TEXT) RETURNS SETOF users 
AS $$
BEGIN
	RETURN QUERY SELECT * FROM users u
	WHERE u.user_id = $1;
END
$$
LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION getuserbyusername(user_name TEXT) RETURNS SETOF users 
AS $$
BEGIN
	RETURN QUERY SELECT * FROM users u
	WHERE u.user_name = $1;
END
$$
LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION CreateUser(userid TEXT,user_name TEXT, account_type_id INT, field_count INT) RETURNS VOID
AS $$
BEGIN
	INSERT INTO users VALUES($1,$2,$3,$4);
END
$$
LANGUAGE plpgsql;	
																							
CREATE OR REPLACE FUNCTION DeleteUser(userid TEXT) RETURNS VOID
AS $$
BEGIN
	DELETE FROM users u WHERE u.user_id = $1;
END
$$
LANGUAGE plpgsql;	
							
CREATE OR REPLACE FUNCTION UpdateUser(userid TEXT, account_type_id INT, field_count INT) RETURNS VOID
AS $$
BEGIN
	UPDATE users 
	SET account_type_id = $2, field_count = $3
	WHERE user_id = $1; 
END
$$
LANGUAGE plpgsql;																								

CREATE OR REPLACE FUNCTION UpdateUser(userid TEXT, field_count INT) RETURNS VOID
AS $$
BEGIN
	UPDATE users u
	SET u.account_type_id = $2
	WHERE u.user_id = $1; 
END
$$
LANGUAGE plpgsql;
		
CREATE OR REPLACE FUNCTION UpdateUser(userid TEXT, columntype TEXT, updateValue INT) RETURNS VOID
AS $$
BEGIN
	IF $2 = 'field_count' THEN
	UPDATE users 
	SET field_count = $3
	WHERE user_id = $1; 
	ELSEIF $2 = 'account_type_id' THEN
	UPDATE users u
	SET account_type_id = $3
	WHERE user_id = $1;																				
	END IF;	
END
$$
LANGUAGE plpgsql;
																							
	
CREATE OR REPLACE FUNCTION getfieldbyname(field_name TEXT) RETURNS SETOF fields
AS $$
BEGIN
	RETURN QUERY SELECT * FROM fields f
	WHERE f.field_name = $1;
END
$$
LANGUAGE plpgsql;	

CREATE OR REPLACE FUNCTION checkownership(user_id TEXT, field_name TEXT) RETURNS BOOLEAN
AS $$
BEGIN
	RETURN EXISTS(SELECT * FROM fields f WHERE f.field_name = $2 AND f.user_id = $1);
END
$$
LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION checkownership(user_id TEXT, field_id INT) RETURNS BOOLEAN
AS $$
BEGIN
	RETURN EXISTS(SELECT * FROM fields f WHERE f.field_id = $2 AND f.user_id = $1);
END
$$
LANGUAGE plpgsql;



--DROP Function GetFieldByFieldId(INT)


CREATE OR REPLACE FUNCTION getfieldscount(user_id TEXT) RETURNS INT
AS $$
BEGIN
	RETURN (SELECT COUNT(*) FROM fields f WHERE f.user_id = $1);
END
$$	
LANGUAGE plpgsql;																						


CREATE OR REPLACE FUNCTION GetFieldByFieldId(field_id INT) RETURNS SETOF fields
AS $$
BEGIN
	RETURN QUERY SELECT * FROM fields f
	WHERE f.field_id = $1;
END
$$
LANGUAGE plpgsql;
	
CREATE OR REPLACE FUNCTION GetAllFields() RETURNS SETOF fields
AS $$
BEGIN
	RETURN QUERY SELECT * FROM fields;
END
$$
LANGUAGE plpgsql;																							

CREATE OR REPLACE FUNCTION GetAllFields(user_id TEXT) RETURNS SETOF fields
AS $$
BEGIN
	RETURN QUERY SELECT * FROM fields f WHERE f.user_id = $1 OR f.user_id = ANY (SELECT field_owner_id FROM sharedfields sf  WHERE sf.shared_owner_id = $1) ORDER BY f.user_id;
END
$$
LANGUAGE plpgsql;	
																			   
--SELECT * FROM fields f WHERE f.user_id = 'ASDFJFWEIOJSD8238GFSDF289YTSDLJ' OR f.user_id = ANY (SELECT field_owner_id FROM sharedfields  WHERE shared_owner_id = 'ASDFJFWEIOJSD8238GFSDF289YTSDLJ')ORDER BY f.user_id;																			   
																			   
																							
CREATE OR REPLACE FUNCTION GetFieldByUserId(user_id TEXT) RETURNS SETOF fields
AS $$
BEGIN
	RETURN QUERY SELECT * FROM fields f
	WHERE f.user_id = $1;
END
$$
LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetFieldByUserId(user_id TEXT, field_name TEXT) RETURNS SETOF fields
AS $$
BEGIN
	RETURN QUERY SELECT * FROM fields f
	WHERE f.user_id = $1 AND f.field_name = $2;
END
$$
LANGUAGE plpgsql;

		
--Remove field_id																						  
CREATE OR REPLACE FUNCTION CreateField(user_id TEXT, field_name TEXT, field_cords geometry) RETURNS VOID
AS $$
BEGIN
	field_cords = ST_SetSRID(field_cords, 4326);
	
	INSERT INTO fields(user_id,field_name,field_cords) VALUES($1,$2,$3);
END
$$
LANGUAGE plpgsql;		

CREATE OR REPLACE FUNCTION UpdateField(user_id TEXT, field_name TEXT, new_field_name TEXT) RETURNS VOID
AS $$
BEGIN
	UPDATE fields f
	SET field_name = $3
	WHERE f.user_id = $1 AND f.field_name = $2; 
END
$$
LANGUAGE plpgsql;	


-- Remove field_id
-- CREATE OR REPLACE FUNCTION UpdateField(field_id INT, userid TEXT, field_cords geometry, field_name TEXT) RETURNS VOID
-- AS $$
-- BEGIN
-- 	UPDATE fields f
-- 	SET f.user_id = $2, f.field_cords = $3, f.field_name = $4
-- 	WHERE f.field_id = $1; 
-- END
-- $$
-- LANGUAGE plpgsql;																								
																							
																							
-- CREATE OR REPLACE FUNCTION UpdateField(field_id INT, columntype INT, updateValue TEXT) RETURNS VOID
-- AS $$
-- BEGIN
-- 	IF $2 = 1 THEN
-- 	UPDATE fields f
-- 	SET user_id = $3
-- 	WHERE f.field_id = $1; 
-- 	ELSEIF $2 = 2 THEN
-- 	UPDATE fields f
-- 	SET field_name = $3
-- 	WHERE f.field_id = $1;
-- 	END IF;	
-- END
-- $$
-- LANGUAGE plpgsql;																							

-- CREATE OR REPLACE FUNCTION UpdateField(field_id INT, columntype TEXT, updateValue TEXT, field_coordinates geometry) RETURNS VOID
-- AS $$
-- BEGIN
-- 	IF $2 = 'user_id' THEN
-- 	UPDATE fields f
-- 	SET user_id = $3
-- 	WHERE f.field_id = $1; 
-- 	ELSEIF $2 = 'field_name' THEN
-- 	UPDATE fields f
-- 	SET field_name = $3
-- 	WHERE f.field_id = $1;
-- 	END IF;	
-- 	UPDATE fields f
-- 	SET field_coordinates = $4
--     WHERE f.field_id = $1;
-- END
-- $$
-- LANGUAGE plpgsql;																							
																							
-- CREATE OR REPLACE FUNCTION UpdateField(field_id INT, updateValue geometry) RETURNS VOID
-- AS $$
-- BEGIN
-- 	UPDATE fields f
-- 	SET field_cords = $3
-- 	WHERE f.field_id = $1;
-- END
-- $$
-- LANGUAGE plpgsql;																							
																							
																							
-- CREATE OR REPLACE FUNCTION UpdateField(field_id INT, user_id TEXT, field_name TEXT) RETURNS VOID
-- AS $$
-- BEGIN
-- 	UPDATE fields f
-- 	SET user_id = $2, field_name = $3
-- 	WHERE f.field_id = $1; 
-- END
-- $$
-- LANGUAGE plpgsql;																								
																							
CREATE OR REPLACE FUNCTION DeleteField(field_id INT) RETURNS VOID
AS $$
BEGIN
	DELETE FROM fields f WHERE f.field_id = $1;
END
$$
LANGUAGE plpgsql;	

CREATE OR REPLACE FUNCTION deletefield(field_name TEXT,user_id TEXT, field_id INT) RETURNS VOID
AS $$
BEGIN
	DELETE FROM field_specifics s WHERE s.field_id = $3;
	DELETE FROM fields f WHERE f.field_name = $1 AND f.user_id = $2;
END
$$
LANGUAGE plpgsql;	

																							
--CREATE OR REPLACE FUNCTION GetMoistureData(data_id int) RETURNS SETOF moisture_data
--AS $$
--BEGIN
--	RETURN QUERY SELECT * FROM moisture_data md
--	WHERE md.data_id = $1;
--END
--$$
--LANGUAGE plpgsql;	
--vegetation_indices4 moisture_data
--DROP FUNCTION GetFieldData(fieldData Geometry);
		
		
--CREATE OR REPLACE FUNCTION GetFieldData(fieldID INT) RETURNS SETOF earthData
--AS $$
--BEGIN
--    RETURN QUERY SELECT moisture_value, coordinates  FROM vegetation_indices4 md
--    INNER JOIN (SELECT field_cords 
--                FROM fields
--                WHERE field_id = fieldID) as f ON 1=1
--    WHERE ST_CONTAINS((f.field_cords), CAST(md.coordinates AS geometry)) AND moisture_value != -9999;
--END
--$$
--LANGUAGE plpgsql; 

--DROP FUNCTION getearthdatabydate(integer,text);


CREATE OR REPLACE FUNCTION GetEarthDataByDate(fieldID INT, type TEXT) RETURNS SETOF earthData
AS $$
BEGIN												
	RETURN QUERY EXECUTE 'SELECT data_value, coordinates  FROM '||type||' md
					INNER JOIN (SELECT field_cords 
					FROM fields
					WHERE field_id = '||fieldID||') as f ON 1=1
    				WHERE ST_CONTAINS((f.field_cords), CAST(md.coordinates AS geometry));';
END;
$$
LANGUAGE plpgsql;

--SELECT GetEarthDataByDate(62, 'surface_temp6');

--https://stackoverflow.com/questions/10723006/return-a-select-from-a-plpgsql-function
--DROP FUNCTION gethandler(text);

CREATE OR REPLACE FUNCTION gethandler(datatype TEXT) RETURNS SETOF handlerData
AS $$
DECLARE 
BEGIN
	RETURN QUERY EXECUTE 'SELECT data_set_name, data_date FROM '||datatype||';';
END;
$$
LANGUAGE plpgsql;

--SELECT gethandler('precipitation_handler');

											
CREATE OR REPLACE FUNCTION GetFieldData(fieldID INT, type TEXT) RETURNS SETOF earthData
AS $$
BEGIN
	if $2 = 'Soil Moisture' THEN
    RETURN QUERY SELECT data_value, coordinates  FROM soil_moisture1 md
	INNER JOIN (SELECT field_cords 
				FROM fields
				WHERE field_id = fieldID) as f ON 1=1
    WHERE ST_CONTAINS((f.field_cords), CAST(md.coordinates AS geometry)) AND data_value != -9999;
	ELSEIF $2 ='Surface Temperature' THEN
	RETURN QUERY SELECT data_value, coordinates  FROM surface_temp1 md
	INNER JOIN (SELECT field_cords 
				FROM fields
				WHERE field_id = fieldID) as f ON 1=1
    WHERE ST_CONTAINS((f.field_cords), CAST(md.coordinates AS geometry)) AND data_value != -9999;										
	ELSEIF $2 = 'Precipitation' THEN
	RETURN QUERY SELECT data_value, coordinates  FROM precipitation1 md
	INNER JOIN (SELECT field_cords 
				FROM fields
				WHERE field_id = fieldID) as f ON 1=1
    WHERE ST_CONTAINS((f.field_cords), CAST(md.coordinates AS geometry)) AND data_value != 0;										
	ELSEIF $2 = 'Vegetation Indices' THEN
	RETURN QUERY SELECT data_value, coordinates  FROM vegetation_indices1 md
	INNER JOIN (SELECT field_cords 
				FROM fields
				WHERE field_id = fieldID) as f ON 1=1
    WHERE ST_CONTAINS((f.field_cords), CAST(md.coordinates AS geometry)) AND data_value != -9999;										
	END IF;
END
$$
LANGUAGE plpgsql;											
											
											
CREATE OR REPLACE FUNCTION GetEarthDataTypes() RETURNS SETOF earth_data
AS $$
BEGIN
   RETURN QUERY SELECT * FROM earth_data; 
END
$$
LANGUAGE plpgsql;
				
											
CREATE OR REPLACE FUNCTION CreateSharedField(field_owner_id TEXT, shared_owner_id TEXT) RETURNS VOID
AS $$
BEGIN
   INSERT INTO sharedfields VALUES($1,$2);
END
$$
LANGUAGE plpgsql;									

											
CREATE OR REPLACE FUNCTION createfieldspecific(field_id INT, yield FLOAT, seedplanted TEXT, fertilizer_use TEXT, pesticide_use TEXT) RETURNS VOID
AS $$
BEGIN
  INSERT INTO field_specifics(field_id,yield,seedplanted,fertilizer_use,pesticide_use) VALUES($1,$2,$3,$4,$5);
END
$$
LANGUAGE plpgsql;
											
									

CREATE OR REPLACE FUNCTION DeleteFieldSpecific(field_id INT) RETURNS void
AS $$
BEGIN
  DELETE FROM field_specifics f WHERE f.field_id = $1;
END
$$
LANGUAGE plpgsql;


CREATE OR REPLACE FUNCTION updatefieldspecific(field_id INT, seedplanted TEXT, columntype TEXT, columnvalue TEXT) RETURNS void
AS $$
BEGIN
	if $3 = 'pesticide_use' THEN
	UPDATE field_specifics f SET pesticide_use=$4 WHERE f.field_id = $1 AND f.seedPlanted = $2;
	ELSEIF $3 = 'fertilizer_use' THEN
	UPDATE field_specifics f SET fertilizer_use=$4 WHERE f.field_id = $1 AND f.seedPlanted = $2;
	END IF;
END
$$
LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION updatefieldspecific(field_id INT, seedplanted TEXT, fertilizer_use TEXT, pesticide_use TEXT, yield FLOAT ) RETURNS void
AS $$
BEGIN
  	UPDATE field_specifics f SET fertilizer_use=$3, pesticide_use=$4, yield=$5 WHERE f.field_id = $1 AND f.seedPlanted = $2;
END
$$
LANGUAGE plpgsql;


CREATE OR REPLACE FUNCTION updatefieldspecific(field_id INT, seedplanted TEXT, columnValue FLOAT) RETURNS void
AS $$
BEGIN
  	UPDATE field_specifics f SET yield=$3 WHERE f.field_id = $1 AND f.seedPlanted = $2;
END
$$
LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetFieldSpecific(field_id INT) RETURNS SETOF field_specifics
AS $$
BEGIN
  RETURN QUERY SELECT * FROM field_specifics f WHERE f.field_id = $1 ORDER BY entry_date DESC; 
END
$$
LANGUAGE plpgsql;
											
											
CREATE OR REPLACE FUNCTION createsoilcompaction(field_id INT, soil_compaction REAL, coordinate GEOMETRY) RETURNS VOID
AS $$
BEGIN
	INSERT INTO SoilCompaction VALUES($1,$2,$3);
END
$$
LANGUAGE plpgsql;	


CREATE OR REPLACE FUNCTION getsoilcompaction(field_id INT) RETURNS SETOF SoilCompaction
AS $$
BEGIN
	RETURN QUERY SELECT * FROM SoilCompaction s WHERE s.field_id = $1;
END
$$
LANGUAGE plpgsql;	

											
-- Sample Data
INSERT INTO account_type(account_type) VALUES('Admin');
INSERT INTO account_type(account_type) VALUES('Farmer');
INSERT INTO users(user_id,user_name,account_type_id,field_count) VALUES('ASDFJFWEIOJSD8238GFSDF289YTSDLJ','testuser1@test.com',1,4);
-- INSERT INTO users(user_id,user_name,account_type_id,field_count) VALUES('GHG9H848F828H2H034209H29H0GH492','testuser2@test.com',1,4);
-- INSERT INTO public.fieldstest(
-- 	user_id, field_name, field_cords)
-- 	VALUES ('bSznI8kKorVztbyyslrr2PTrevO2', 'Testing Area Calculation', ST_GeometryFromText('POLYGON((-81.3397394295207 44.3119328211494,
--   -81.3353276684481 44.3101656596193,
--   -81.3413938399206 44.302461586382,
--   -81.3419453100539 44.3025817021824,
--   -81.3408423697873 44.3041603442054,
--   -81.3424488262625 44.3048638557847,
--   -81.3434798356422 44.3048810145027,
--   -81.3447506146512 44.3054644077801,
--   -81.3397394295207 44.3119328211494))', 4326));
-- SELECT GetFieldArea(58);
INSERT INTO fields(user_id,field_name,field_cords) VALUES('ASDFJFWEIOJSD8238GFSDF289YTSDLJ','usa', st_geomfromtext('POLYGON((-82.5 35.00,-82.5 37.00, -80.0 37.00,  -80.00 35.00, -82.5 35.00))', 4326));
INSERT INTO fields(user_id,field_name,field_cords) VALUES('ASDFJFWEIOJSD8238GFSDF289YTSDLJ','lake huron', st_geomfromtext('POLYGON(( -82.5 45.00 ,-82.5 49.00 ,-80.0 49.00 ,-80.00 45.00 ,-82.5 45.00 ))', 4326));
-- INSERT INTO fields(user_id,field_name,field_cords) VALUES('GHG9H848F828H2H034209H29H0GH492', 'town','POLYGON(( -81.35032653808592 43.23572945423807 ,-81.34517669677734 44.22047656811414 , -80.29831314086914 44.2269964305193 , -80.30191802978516 43.24298548811547 , -81.35032653808592 43.23572945423807 ))');																	  
-- INSERT INTO sharedfields VALUES('ASDFJFWEIOJSD8238GFSDF289YTSDLJ','GHG9H848F828H2H034209H29H0GH492');
-- "70451.0946598053"
-- 82acres for field_id 57
-- table , column		
--SELECT UpdateGeometrySRID('precipitation5','coordinates',4326);
--SELECT ST_AsText( the_geom ) As sqm--/ 1609.34^2 As sqm --*POWER(0.3048,2) As sqm
--    FROM (SELECT field_cords FROM fields WHERE field_id = 57) As foo(the_geom);

--INSERT INTO moisture_data2(moisture_value,coordinates) VALUES(0.3213241, ST_SetSRID(ST_MakePoint(-80.31231, 42.341212), 4326)); -- ST_SetSRID(ST_MakePoint(%s, %s), 4326)
--SELECT COUNT(*) FROM Vegetation_indices_handler;																															  
																														  


