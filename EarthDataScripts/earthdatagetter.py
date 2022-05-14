#!/usr/bin/python
from cookielib import CookieJar
from urllib import urlencode
 
import urllib2
from datetime import datetime


# The user credentials that will be used to authenticate access to the data
 
username = "samdabullet"
password = "RPD9MhNKTeVKCBzuanjx"
  
 
currentYear = "2019"
currentMonth = "02"
currentDay = "04"
nextDay = "05"

#smap file
smapFile = "SMAP_L4_SM_aup_" + currentYear + currentMonth + nextDay + "T000000_Vv4030_001.h5"

#merra2 file
merraFile = "MERRA2_400.tavg1_2d_lnd_Nx." + currentYear + currentMonth + currentDay + ".nc4"

# The url of the file we wish to retrieve
#                                                                             year  month                     yyymmdd
merra = "https://goldsmr4.gesdisc.eosdis.nasa.gov/data/MERRA2/M2T1NXLND.5.12.4/" + currentYear + "/" + currentMonth + "/" + merraFile

#                                                                yyyy.mm.dd                                        yyymmdd                            hhmm??
smap= "https://n5eil01u.ecs.nsidc.org/DP6/SMAP/SPL4SMAU.004/" + currentYear + "." + currentMonth + "." + currentDay + "/" + smapFile
#smap example 3hr interval
example = "https://n5eil01u.ecs.nsidc.org/opendap/DP6/SMAP/SPL4SMAU.004/2019.02.04/SMAP_L4_SM_aup_20190205T000000_Vv4030_001.h5"

url = smap

# Create a password manager to deal with the 401 reponse that is returned from
# Earthdata Login
 
password_manager = urllib2.HTTPPasswordMgrWithDefaultRealm()
password_manager.add_password(None, "https://urs.earthdata.nasa.gov", username, password)
 
 
# Create a cookie jar for storing cookies. This is used to store and return
# the session cookie given to use by the data server (otherwise it will just
# keep sending us back to Earthdata Login to authenticate).  Ideally, we
# should use a file based cookie jar to preserve cookies between runs. This
# will make it much more efficient.
 
cookie_jar = CookieJar()
  
 
# Install all the handlers.
 
opener = urllib2.build_opener(
    urllib2.HTTPBasicAuthHandler(password_manager),
    #urllib2.HTTPHandler(debuglevel=1),    # Uncomment these two lines to see
    #urllib2.HTTPSHandler(debuglevel=1),   # details of the requests/responses
    urllib2.HTTPCookieProcessor(cookie_jar))
urllib2.install_opener(opener)
 
 
# Create and submit the request. There are a wide range of exceptions that
# can be thrown here, including HTTPError and URLError. These should be
# caught and handled.
 
request = urllib2.Request(url)
response = urllib2.urlopen(request)
 
#create todays file
writeFile = open( smapFile, "w")

#write the binary to todays file and save it
writeFile.write(response.read())
writeFile.close()

# Print out the result (not a good idea with binary data!)
#body = response.read()
#print body
