using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SQLite4Unity3d;
using UnityEngine;

public class test01 : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        var dbPath = string.Format(@"Assets/StreamingAssets/OneToMany.db");
        var db = new SQLiteConnection(dbPath);

        var customersFromDb = db.Table<Customer>().ToList();
        var ordersFromDb = db.Table<Order>().ToList();

        PrintInfo(customersFromDb, "customers");
        PrintInfo(ordersFromDb, "orders");

        var map = new TableMapping(typeof(Customer));
        var q = string.Format("update {0} set Id = 999 where Id = 1", map.TableName);
        db.Query(map, q, null);

        customersFromDb = db.Table<Customer>().Where(x => x.Id == 999).ToList();
        ordersFromDb = db.Table<Order>().Where(x => x.CustomerId == 999).ToList();

        PrintInfo(customersFromDb, "customers");
        PrintInfo(ordersFromDb, "orders");
    }

    private static void PrintInfo(IList objects, string objType)
    {
        if (objects.Count == 0)
        {
            Debug.Log(string.Format("\nThere are no {0}!", objType));
        }

        foreach (var o in objects)
        {
            Debug.Log(o.ToString());
        }
    }
}

public class Customer
{
    public Customer(string name, List<Order> orders)
    {
        this.Name = name;
        this.Orders = orders;
    }
    public Customer()
    {

    }

    [PrimaryKey]
    public int Id { get; set; }

    public string Name { get; set; }

    [One2Many(typeof(Order))]
    public List<Order> Orders { get; set; }

    public override string ToString()
    {
        return string.Format("CustomerId: {0} \t Name: {1}", Id, Name);
    }
}

public class Order
{
    public Order(string orderName, int customerId)
    {
        this.OrderName = orderName;
        this.CustomerId = customerId;
    }

    public Order()
    {

    }

    [PrimaryKey]
    public int Id { get; set; }

    public string OrderName { get; set; }

    [References(typeof(Customer))]
    [OnUpdateCascade]
    [OnDeleteCascade]
    public int CustomerId { get; set; }

    public override string ToString()
    {
        return string.Format("OrderId: {0} \t CustomerId: {1}", Id, CustomerId);
    }
}