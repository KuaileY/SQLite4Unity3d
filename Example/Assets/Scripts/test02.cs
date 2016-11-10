using UniRx;
using System.Xml.Linq;
using UnityEngine;

public class test02 : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    var textAsset = Resources.Load<TextAsset>("aa");
	    var xDoc = XDocument.Parse(textAsset.text);

        var users = xDoc.Element("Root").Elements();
	    users.ToObservable()
	        .Select(t => new
	        {
	            userId = t.Element("UserID").Value,
	            userName = t.Element("UserName").Value
	        })
	        .Do(t =>
	        {
	            var output = string.Format("useId:{0},useName:{1}", t.userId, t.userName);
	            output.print();
	        })
	        .Subscribe();
	}
	
}
public static class StringExtension
{
    public static void print(this string ss)
    {
        Debug.Log(ss);
    }

    public static void print(this int ii)
    {
        print(ii.ToString());
    }
}