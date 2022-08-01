using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DemoControl : MonoBehaviour
{
    public GameObject reference;
    private bool start = false;
    private string dataP;

    [System.Serializable]
    public class Test
    {
        public List<Vector3> positionsAR = new List<Vector3>();
        public List<Vector3> positionsReference = new List<Vector3>();
        public List<Vector3> rotationsAR = new List<Vector3>();
        public List<Vector3> rotationsReference = new List<Vector3>();
    }

    private Test test = new Test();

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            start = !start;
            if (start) print("Test started!");
        }

        if (start)
        {
            test.positionsAR.Add(transform.position);
            test.positionsReference.Add(reference.transform.position);
            test.rotationsAR.Add(transform.rotation.eulerAngles);
            test.rotationsReference.Add(reference.transform.rotation.eulerAngles);
        }else if (test.positionsAR.Count != 0)
        {
            print("Test finished!");
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(Application.dataPath + "/Tests Data");
            int count = DirCount(dir);

            dataP = Path.Combine(Application.dataPath + "/Tests Data", "test" + count.ToString() + ".json");
            string json = JsonUtility.ToJson(test, true);
            File.WriteAllText(dataP, json);
            test.positionsAR.Clear();
            test.positionsReference.Clear();
            test.rotationsAR.Clear();
            test.rotationsReference.Clear();
        }

    }

    public static int DirCount(DirectoryInfo d)  // Counts the number of json files in a folder
    {
        int count = 0;
        // Add file sizes.
        FileInfo[] fis = d.GetFiles();
        foreach (FileInfo fi in fis)
        {
            if (fi.Extension.Contains("json"))
                count++;
        }
        return count;
    }
}
