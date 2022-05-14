from geojson import Point, Feature, FeatureCollection, dump
import psycopg2
from config import config
import re


def genFile ():

    conn = None
    try:
        params = config()

        

        print('Connecting to the PostgreSQL database...')
        # connect to db using the config class
        conn = psycopg2.connect(**params)

        cur = conn.cursor()
        # select all datapoints within the polygon aslong with the moisture value
        cur.execute("SELECT moisture_value, coordinates FROM Moisture_data md WHERE ST_CONTAINS(st_geomfromtext('POLYGON((44.273713 -81.532176,44.270130 -80.418622,43.575866 -80.323896,43.452944 -81.420064,44.273713 -81.532176))'), CAST(md.coordinates AS geometry));")
        #grab all of the rows selected
        rows = cur.fetchall()
        features = []
        
        for row in rows:
            # create list of features while adding in the features
            
            # split the cord string i.e (43.243242, -82.42342)
            cords = re.findall(r"[-+]?\d*\.\d+|\d+", row[1])
            point = Point((float(cords[1]), float(cords[0])))
            features.append(Feature(properties={"sm":str(row[0])}, geometry=point))
            #for col in row:
                # moisture -> cords
            print (" ") , row[0], row[1]

        #create feature collection
        featur_coll = FeatureCollection(features)

        # dump the coll to a file
        # ../../../websites/mysite/www/
        with open('SoilMoisture.geojson', 'w') as f:
            dump(featur_coll, f)
        
        print("SoilMoisture GeoJson File Generated")

    # ensure there are no errors that cause the script to crash
    except (Exception, psycopg2.DatabaseError) as error:
        print(error)
    finally:
        # safe shutdown
        if conn is not None:
            conn.close()
            print('Database connection closed.')


#start the right func
if __name__ == '__main__':
    genFile()