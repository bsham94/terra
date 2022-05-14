import h5py
#file = h5py.File("SMAP_L3_SM_P_E_20190115_R16022_001.h5", "r")
file = h5py.File("SMAP_L4_SM_aup_20190110T000000_Vv4030_001.h5", "r")

list (file.keys())

#HDFVIEW
#Panoply

## get the latitude and longitude   MERRA2 = lat & long

## grid square/polygon:  
#   NW - 45.252987, -82.453883
#   NE - 45.700687, -79.899796
#   SE - 43.293294, -78.896814
#   SW - 43.082756, -82.342626

tempLat = [45.252987, 45.700687, 43.293294, 43.082756]
tempLong = [-82.453883, -79.899796, -78.896814, -82.342626]

# need to fix spots that have default data - (-9999.0)
#latSet = file['/Soil_Moisture_Retrieval_Data_AM/latitude'][...,0]
latSet = file['cell_lat'][...,0]
print(str(latSet))
grossFile = open("gross.txt", 'w')
for latt in latSet:
    grossFile.write(str(latt) + ", ")

grossFile.close()

#longSet = file['/Soil_Moisture_Retrieval_Data_AM/longitude'][1623:]
longSet = file['cell_lon'][1623:]

latGrid = []
latCord = []
longGrid = []
longCord = []

lastGrid = 0
count = 0


#data is store for MERRA like 24*361*576
# 360 coulmns
# 24 coulmns
# / chunk
# this means we can go right to that specific chunk!

#MERRA GwetTop = soil moisture - is chunked in 24
lowLat = 0
highLat = 0
for low in tempLat:
    if lowLat > low:
        lowLat = low
    if highLat < low:
        highLat = low

lowLon = 0
highLon = 0

for lon in tempLong:
    if lowLon > lon:
        lowLon = lon
    if highLon < lon:
        highLon = lon

print ("highLon: " + str(highLon) + "lowLon: " + str(lowLon))

# inside a 0.625 degree we have 24 more chunks longitude way
#   aka we have 0.026041667 deg per block
print (latSet.shape)
for temp in tempLat:
    for lat in latSet:
        if temp < lastGrid and temp > lat:
        #if lat > lowLat and lat < highLat:
            #latGrid.append(count)
            #latCord.append(lat)

            #print ("latitude: " + str(lat) + ", specified: " + str(temp) + ", " + str(count))
            break
        lastGrid = lat
        count = count + 1
    count = 0
    lastGrid = 0

count = 0
lastGrid = 0

for lat in latSet:
    if lat > lowLat and lat < highLat:
        latGrid.append(count)
        latCord.append(lat)
        #print ("latitude: " + str(lat) + ", specified: " + str(temp) + ", " + str(count))
    count = count + 1

count = 0
lastGrid = 0
print (longSet.shape)
for lonn in longSet:
    for lon in lonn:
        if lon > lowLon and lon < highLon:
            longGrid.append(count)
            longCord.append(lon)
            #print ("longitude: " + str(lon) + ", specified: " + str(temp) + ", " + str(count))
        count = count + 1


lastGrid = 0
count = 0

for temp in tempLong:
    for lonn in longSet:
        for lon in lonn:
            if temp > lastGrid and temp < lon:
                #longGrid.append(count)
                #longCord.append(lon)
                #print ("longitude: " + str(lon) + ", specified: " + str(temp) + ", " + str(count))
                break
            lastGrid = lon
            count = count + 1
    count = 0
    lastGrid = 0

#print ( "array:  " + str(longCord))
soilMoistureData = file['Forecast_Data/sm_profile_forecast']
shapeData = soilMoistureData
# specify latitude
#results = soilMoistureData[0, [1]]
#print(results.shape)
#results = soilMoistureData[0:3856, [230, 234, 254, 257]]

# currently it appears that we are getting the proper rows selected through this method
# we can later on select the proper columns in the for loop
results = soilMoistureData[:3856, [1045, 1046, 1047, 1048, 1049, 1050, 1072, 1083, 1926, 1927]]
# soilMoistureData[longitude:latitude]
# soilMoistureData[y:x]
countg = 0
gridCount = 0
writeFile = open("good.txt", "w")
for lon in longGrid:
    print (lon)
    result = soilMoistureData[:3856, [lon]]
    j = 0
    
    for res in result:
        
        if j >= lowLat and j <= highLat:
            writeFile.write("count: " + str(gridCount) + " lon: " + str(longCord[j]) + " lat: " + str(latCord[j]) +" value: " + str(res) + "\n")
        j = j + 1
    gridCount = gridCount + 1

writeFile.close()

# this is a super inefficient way to pick specific plots out, atleast i think its inefficent might work...
possible = []
possibleTemp = soilMoistureData[...,1045]
possible.append(possibleTemp[234,...])
possibleTemp = soilMoistureData[...,1072]
possible.append(possibleTemp[230,...])
print (str(possible))
# 1623 = latitude

writeFile = open("tempWhole.txt", "w")

count = 0
#print ("result: " + str(results))
#print (results.shape)
for result in results:
    #for item in result:
        #print (float(item))
    if count >= (230) and count <= (257):
        # fix scientific notation - '%f' % your_var
        cordCount = 0
        for writen in result:
            print (writen)
            writeFile.write("count: " + str(count) + " lon: " + str(longCord[cordCount]) + " lat: " + str(latCord[cordCount]) +" value: " + str(writen) + "\n")
            cordCount = cordCount + 1
            print ("cordCount: " + str(cordCount))
        print (result)
        #full line?
        #if ((count % 24) == 0)
        #    writeFile.write("\n")
    count = count + 1
    #print ("Final count after chunk: " + str(count))
print(count)
count = 0
    #print (result)
    #writeFile.write(str(result) + "\n")

writeFile.close()

#print(results.shape)
#print (results)



print (soilMoistureData)



## we now have our grid square which information is useful

print (latGrid)
print (longGrid)



