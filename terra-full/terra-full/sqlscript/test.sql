-- Test stuff																				  
-- CREATE TABLE sample(
-- 	id int primary key,
-- 	name text, 	
-- 	location geography(point,4326)
-- );

-- INSERT INTO sample(id, name, location) values (1,'town','SRID=4326;POINT(-110 30)');
-- INSERT INTO sample(id, name, location) values (2,'forest','SRID=4326;POINT(-109 29)');
-- INSERT INTO sample(id, name, location) values (3,'london','SRID=4326;POINT(0 49)');
-- INSERT INTO sample(id, name, location) values (4,'london','SRID=4326;POINT(0 55)');
-- INSERT INTO sample(id, name, location) values (5,'london','SRID=4326;POINT(1 49)');
-- INSERT INTO sample(id, name, location) values (6,'london','SRID=4326;POINT(1 55)');																													   
																													   
																													   
-- CREATE TABLE sample2(
-- 	id int primary key,
-- 	name text, 	
-- 	location geography(polygon,4326)
-- );				
																											  																													   																													   
-- INSERT INTO sample2(id, name, location) values (1,'town','SRID=4326;POLYGON((45.00 -82.5,45.00 -80.00, 47.00 -82.5, 47.00 -80.00, 45.00 -82.5))');																													   
-- SELECT * FROM sample2;																															   
																													   
-- CREATE TABLE sample3(
-- 	id int primary key,
-- 	name text, 	
-- 	location geography(geometry,4326)
-- );	

-- INSERT INTO sample3(id, name, location) values (1,'town','SRID=4326;POLYGON((45.00 -82.5,45.00 -80.00, 47.00 -82.5, 47.00 -80.00, 45.00 -82.5))');
-- INSERT INTO sample3(id, name, location) values (2,'town','SRID=4326;POINT(-110 30)');
-- SELECT * FROM sample3;																																		   
																													   
-- SELECT ST_AsGeoJSON('POLYGON((45.00 -82.5,45.00 -80.00, 47.00 -82.5, 47.00 -80.00, 45.00 -82.5))');																													   
																													   
-- CREATE table test1(
-- 	id int primary key,
-- 	col1 point,
-- 	col3 geometry,
-- 	col4 POLYGON
-- );


--SELECT GetFieldData(1);
											   
--SELECT GetFieldData(ST_Polygon(ST_GeomFromText('LINESTRING(75.15 29.53,77 29,77.6 29.5, 75.15 29.53)'), 4326));																				  
																				  
--SELECT moisture_value FROM Moisture_data md
--WHERE ST_CONTAINS(st_geomfromtext('POLYGON((45.00 -82.5,45.00 -80.00, 47.00 -82.5, 47.00 -80.00, 45.00 -82.5))'), CAST(md.coordinates AS geometry));																				  
																				  
--SELECT FROM GetFieldByFieldId(1);		