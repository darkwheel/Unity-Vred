import urllib.request
import re

#Define the variables of the URL you want to call later (Python API)
#"start" contains the localhostIP and the Port number of VRED
start = "http://127.0.0.1:8888/pythonapi?value="

#now the two functions my programm should execute in VRED, you will find these functions, if you execute them by yourself in the webapi and copy the URL
translation = "setTransformNodeTranslation%28findNode%28%22"
rotation = "setTransformNodeRotation%28findNode%28%22"
signNodeData = "%22%29%2C"
signDataData = "%2C"
signEndData = "%29%0D%0A"
worldSpace = "1"

print("Starting python server...")
print("Python server... \nListen to Unity server now...")
bool = True
lastContent = []
while bool == True:
    
    #Should be the same URL like in the C# script of unity
    req = urllib.request.Request('http://127.0.0.1:80/')
    with urllib.request.urlopen(req) as response:
        the_page = response.read()
        the_page = the_page.decode()
        #print(the_page)
        #find every information in the sent data of unity
        isTranslation = re.findall(r"translation", the_page)
        #MESSAGE = re.findall(r"\-?\d+\.\d+", the_page)
        MESSAGE = re.findall(r"\-?\d+\.?\d*", the_page)
        nodeWithBrackets = re.findall(r"\[\w+\]", the_page)
        nodeList = re.findall(r"\w+", nodeWithBrackets[0])   
        node = nodeList[0] 

        #If you find the word translation
        if isTranslation:
            #print("translation")
            #print(MESSAGE[1])

            #Values of Translation times 1000 because then you will see more in VRED
            xTranslation = float(MESSAGE[0]) * 1000
            yTranslation = float(MESSAGE[1]) * 1000
            zTranslation = float(MESSAGE[2]) * 1000

            MESSAGE[0] = str(xTranslation)
            MESSAGE[1] = str(yTranslation)
            MESSAGE[2] = str(zTranslation)
            urlToExe = start + translation + node + signNodeData + MESSAGE[0] + signDataData + MESSAGE[1] + signDataData + MESSAGE[2] + signDataData + worldSpace + signEndData
            
        else:
            #print("rotation")
            #print(MESSAGE[0])
            
            
            xRotation = float(MESSAGE[0]) 
            yRotation = float(MESSAGE[1])
            zRotation = float(MESSAGE[2])

            MESSAGE[0] = str(xRotation)
            MESSAGE[1] = str(yRotation)
            MESSAGE[2] = str(zRotation)
            
            urlToExe = start + rotation + node + signNodeData + MESSAGE[0] + signDataData + MESSAGE[1] + signDataData + MESSAGE[2] + signEndData
        
        #Open the created URL
        x = urllib.request.urlopen(urlToExe)
        x.close()
        