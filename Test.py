import cv2
import numpy as np
import datetime


def Rec():
    
        print("Recording...")
        text = False

def VideoWrite():
    print("Camera on")
    print("Recording =  press 1")
    print("Stop Recording = press 2")
    print("Camera off = press 3")
    
    cap = cv2.VideoCapture(0)
    fourcc = cv2.VideoWriter_fourcc(*'XVID')
    record = False
    width = int(cap.get(3))
    height = int(cap.get(4))
            
    while(True):
        ret, frame = cap.read()
        now = datetime.datetime.now().strftime("%m_%d_%H-%M")
        if not ret:
            print("camera fail")
            break
        cv2.imshow("video",frame)
        k = cv2.waitKey(1)
        
        if k == 49 and record == False:
            print("Record start")
            record = True
            text = True
            out = cv2.VideoWriter(str(now) + ".avi", fourcc, 20.0, (width,height))
            
        elif k == 50 and record == True:
            print("Recording stop")
            record = False
            out.release()
        elif k == 51:
            print("Camera off")
            break
        
        if record == True:
            out.write(frame)
            print("Recording...")
        
        
    cap.release()
    cv2.destroyAllWindows()
    
VideoWrite()