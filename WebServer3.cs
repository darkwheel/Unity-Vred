using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Threading;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class WebServer3 : MonoBehaviour
{
    private List<GameObject> ObjectList;
    private Vector3 rotationData;
    readonly HttpListener listener = new HttpListener();
    readonly List<string> cachedTransform = new List<string>();
    readonly List<string> cachedRotation = new List<string>();
    
    string responseString = String.Empty;
    string responseStringR = String.Empty;
    // Start is called before the first frame update

    void Start()
    { 
        Debug.Log(ObjectList.Count);
        for (int i = 0; i < ObjectList.Count; i++)
        {
            cachedTransform.Add($"{i}");
            cachedRotation.Add($"{i}");
        }
        Debug.Log(cachedRotation);
        foreach (GameObject givenObject in ObjectList)
        {
            transform.position = givenObject.transform.position;

            rotationData = givenObject.transform.rotation.eulerAngles;

            // Create a listener.

            // Add the prefixes.
            string s = "http://localhost:8080/";
            listener.Prefixes.Add(s);

            listener.Start();

            //Translation
            // Note: The GetContext method blocks while waiting for a request. 
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            // Obtain a response givenObject.
            HttpListenerResponse response = context.Response;
            // Construct a response.
            
            string responseString = "translation [" + $"{givenObject.name}" +"] " + $"{transform.position}";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            // Get a response stream and write the response to it.
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            // You must close the output stream.
            output.Close();

            //Rotation
            HttpListenerContext contextR = listener.GetContext();
            HttpListenerRequest requestR = contextR.Request;
            // Obtain a response givenObject.
            HttpListenerResponse responseR = contextR.Response;
            // Construct a response.
            string responseStringR = "rotation [" + $"{givenObject.name}" +"] " + $"{rotationData}";
            byte[] bufferR = System.Text.Encoding.UTF8.GetBytes(responseStringR);
            // Get a response stream and write the response to it.
            responseR.ContentLength64 = bufferR.Length;
            System.IO.Stream outputR = responseR.OutputStream;
            outputR.Write(bufferR, 0, bufferR.Length);
            // You must close the output stream.
            outputR.Close();
            Debug.Log("Here");
        }

    }
    // Update is called once per frame
    void Update()
    {
        //check for every object in the array object list if its coordinates has changed
        for (int i = 0; i < ObjectList.Count; i++)
        {
            //set givenObject to current object of the list
            GameObject givenObject = ObjectList[i];

            //check transformation and rotation and give back a string
            string transformD = UpdateTranslation(givenObject);
            string rotation = UpdateRotation(givenObject);
            
            //if transformation has changed, then send that to the listener
            if (transformD != cachedTransform[i])
            {
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;

                // Obtain a response givenObject.
                HttpListenerResponse response = context.Response;

                string responseString = "translation [" + $"{givenObject.name}" +"] " + $"{transform.position}";
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

                // Get a response stream and write the response to it.
                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();

                //set cachedTransformation to sent transformation
                cachedTransform[i] = transformD;
            }


            //if transformation has changed, then send that to the listener
            if (rotation != cachedRotation[i])
            {
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                // Obtain a response givenObject.
                HttpListenerResponse response = context.Response;
                string responseStringR = "rotation [" + $"{givenObject.name}" +"] " + $"{rotationData}";
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseStringR);
                // Get a response stream and write the response to it.
                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();

                //set cachedRotation to sent rotation
                cachedRotation[i] = rotation; 
            } 
        }
    }

    static string UpdateTranslation(GameObject givenObject)
    {
        transform.position = givenObject.transform.position;
        return "translation [" + $"{givenObject.name}" + "] " + $"{transform.position}";
        
    }
    static string UpdateRotation(GameObject givenObject)
    {
        rotationData = givenObject.transform.rotation.eulerAngles;
        return "rotation [" + $"{givenObject.name}" +"] " + $"{rotationData}";
    }
}



