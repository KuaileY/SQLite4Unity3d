using SQLite4Unity3d;
using UnityEngine;
using UniRx;

public class test : MonoBehaviour
{
    SQLiteConnection _connection;

    void Start()
    {
        var dbPath = string.Format(@"Assets/StreamingAssets/tempDB.sqlite3");
        _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        createPerson();
        createPersonType();

        var cmd = new SQLiteCommand(_connection);
        cmd.CommandText =

        //"select Person.Name as name,PersonType.TypeName as type from Person,PersonType where Person.typeId = PersonType.id limit 10;";
        //左外连接
        //"select Person.Name as name,PersonType.TypeName as type from Person left outer join PersonType on Person.typeId = PersonType.id limit 10;";
        //内连接
        "select Person.Name as name,PersonType.TypeName as type from Person join PersonType on Person.typeId = PersonType.id limit 10;";
        //自然连接
        //"select Person.Name as name,PersonType.TypeName as type from Person natural join PersonType limit 10;";

        var result = cmd.ExecuteQuery<val>();
        result.ToObservable()
            .Do(t =>
            {
                var s = string.Format("name:{0},type:{1}", t.name, t.type);
                s.print();
            })
            .Subscribe();
    }

    public class val
    {
        public string name { get; set; }
        public string type { get; set; }
    }


    void createPerson()
    {
        //删除table
        _connection.DropTable<Person>();
        //创建table
        _connection.CreateTable<Person>();
        //插入数据
        _connection.Insert(new Person {typeId = 1, Name = "Tom"});
        _connection.Insert(new Person {typeId = 2, Name = "Dom"});
        _connection.Insert(new Person {typeId = 1, Name = "Jake"});
        _connection.Insert(new Person {typeId = 2, Name = "Rose"});
        //整体插入
        _connection.InsertAll(
            new[]
            {
                new Person {typeId = 3, Name = "Ben"},
                new Person {typeId = 3, Name = "Lily"},
            });
    }

    void createPersonType()
    {
        _connection.DropTable<PersonType>();
        _connection.CreateTable<PersonType>();

        _connection.Insert(new PersonType {TypeName = "hehe"});
        _connection.Insert(new PersonType {TypeName = "lala"});
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

public class Person
{
    [PrimaryKey,AutoIncrement]
    public int Id { get; set; }
    public int typeId { get; set; }
    public string Name { get; set; }
}

public class PersonType
{
    [PrimaryKey,AutoIncrement]
    public int Id { get; set; }
    public string TypeName { get; set; }
}
