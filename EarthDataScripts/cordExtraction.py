import h5py
file = h5py.File("MERRA2_400.tavg1_2d_lnd_Nx.20181130.nc4", "r")

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

latSet = file['lat']
longSet = file['lon']

latGrid = []
longGrid = []

lastGrid = 0
count = 0


#data is store for MERRA like 24*361*576
# 360 coulmns
# 24 coulmns
# / chunk
# this means we can go right to that specific chunk!

#MERRA GwetTop = soil moisture - is chunked in 24

for temp in tempLat:
    for lat in latSet:
        if temp > lastGrid and temp < lat:
            latGrid.append(count)
            print (count)
            break
        lastGrid = lat
        count = count + 1
    print (count)
    count = 0
    lastGrid = 0

lastGrid = 0
count = 0

for temp in tempLong:
    for lon in longSet:
        if temp > lastGrid and temp < lon:
            longGrid.append(count)
            break
        lastGrid = lon
        count = count + 1
    count = 0
    lastGrid = 0

soilMoistureData = file['GWETTOP']
results = soilMoistureData[24:361:576, [157, 161, 162]]

writeFile = open("tempWhole.txt", "w")

count = 0

print (results.shape)
for result in results:
    
    for item in result:
        print (float(item))
        if count == (271) or count == (272) or count == (267):
            writeFile.write(str(item) + ", ")
        #full line?
        #if ((count % 24) == 0)
        #    writeFile.write("\n")
        count = count + 1
    print ("Final count after chunk: " + str(count))
    count = 0
    print (result)
    #writeFile.write(str(result) + "\n")

writeFile.close()

#print(results.shape)
#print (results)



print (soilMoistureData)



## we now have our grid square which information is useful

print (latGrid)
print (longGrid)



