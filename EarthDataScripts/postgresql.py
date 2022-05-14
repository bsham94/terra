# File          : postgresql.py
# Project       : TERRA
# Developer     : samuel
# Date          : February 6, 2019
# Description   : This file holds the script to parse the earth data and upload
#                 it to the db for farther

import h5py
import psycopg2
from config import config

# file and dataset shall be specified later on in a config file or another file

# earth data file
file = h5py.File("SMAP_L4_SM_aup_20190110T000000_Vv4030_001.h5", "r")

list (file.keys())

# the specific dataset we wish to utilize
soilMoistureData = file['Forecast_Data/sm_profile_forecast']
shapeData = soilMoistureData

latGrid = []
latCord = []
longGrid = []
longCord = []

count = 0


lowLat = 80
highLat = 0

lowLon = 0
highLon = -100

def GetPotentialCords():
    #        latitude  /  longitude
    ## grid square/polygon:  
    #   NW - 45.252987, -82.453883
    #   NE - 45.700687, -79.899796
    #   SE - 43.293294, -78.896814
    #   SW - 43.082756, -82.342626
    try:
        tempLat = [45.252987, 45.700687, 43.293294, 43.082756]
        tempLong = [-82.453883, -79.899796, -78.896814, -82.342626]

        # specify the dataset we want
        # slice the dataset to get the information that is useful
        #                    latitude, longitude
        latSet = file['cell_lat'][...,0]
        longSet = file['cell_lon'][1623:]
        
        
        global lowLat, highLat, lowLon, highLon, latGrid, latCord, longGrid, longCord

        # set the potential low and high for the latitude
        for low in tempLat:
            if lowLat > low:
                lowLat = low
            if highLat < low:
                highLat = low
        #print(lowLat)


        
        # set the potential low and high for the longitude
        for lon in tempLong:
            if lowLon > lon:
                lowLon = lon
            if highLon < lon:
                highLon = lon
        #print(lowLon)

        count = 0

        # get all of the potential latitudes that are within the data set for future reference
        for lat in latSet:
            if lat > lowLat and lat < highLat:
                # we want the raw number and the count so we know which data value 
                # is associated with it when looking at the data ie. Forecast_Data/sm_profile_forecast
                latGrid.append(count)
                latCord.append(lat)
            count = count + 1
        
        #reset count 
        count = 0
        
        #print (longSet.shape)
        for lonn in longSet:
            for lon in lonn:
                if lon > lowLon and lon < highLon:
                    # we want the raw number and the count so we know which data value 
                # is associated with it when looking at the data ie. Forecast_Data/sm_profile_forecast
                    longGrid.append(count)
                    longCord.append(lon)
                  
                count = count + 1
    except:
        print("Error getting potential cord info \n")
    

    

def connect():

    conn = None
    try:
        params = config()

        

        print('Connecting to the PostgreSQL database...')
        # connect to db using the config class
        conn = psycopg2.connect(**params)

        cur = conn.cursor()

        # debug stuff
        #cur.execute("SELECT * FROM moisture_data")
        #tableResults = cur.fetchone()
        #print (tableResults)


        # run the previous function filling the global variables
        GetPotentialCords()

        gridCount = 0
        
        #writeFile = open("good.txt", "w")
        #print ("3 is the low and high lat set?" + str(lowLat) + ", " + str(highLat))
        
        #unique id for db and file debugging purposes
        uid = 0
        for lon in longGrid:
            print (lon)

            #split off one column at a time so we can cycle through it easily with forloops
            result = soilMoistureData[:3856, [lon]]
            valueCount = 0
            idCount = 0
            for res in result:
                
                #ensure we aren't over reaching a list
                if idCount >= len(latGrid):
                    #break leaves top for loop
                    continue

                if latGrid[idCount] == valueCount:
                #if latCord[valueCount] >= lowLat and latCord[valueCount] <= highLat:
                    cur.execute("INSERT INTO moisture_data (data_id,moisture_value,coordinates) VALUES (" + str(uid) + ", " + str(res[0]) + ", point(" + str(latCord[idCount]) + ", " + str(longCord[gridCount]) + "))")
                    
                    #writeFile.write("count: " + str(uid) + " lon: " + str(longCord[gridCount]) + " lat: " + str(latCord[idCount]) +" value: " + str(res) + "\n")
                    idCount = idCount + 1
                    uid = uid + 1
                valueCount = valueCount + 1
                
            gridCount = gridCount + 1

        # push all the inserts into the db
        conn.commit()
        #writeFile.close()
        

        #print ('Checking Insert\n')

        # check what the db has said
        db_version = cur.fetchone()
        print(db_version)

        cur.close()
    
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
    connect()