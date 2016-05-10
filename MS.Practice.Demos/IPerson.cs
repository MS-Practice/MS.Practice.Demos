using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MS.Practice.Demos
{
    public interface IPerson
    {
        void GetSex();
    }
    public class Person
    {
        public Person() { }

        public Person(string name, int age)
        {
            _name = name;
            _age = age;
        }

        public string _name;
        public int _age;

        //属性
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
            }
        }
        public int Age
        {
            get { return _age; }
            set { _age = value; }
        }
    }

    //定义结构体 不可继承类和结构  但是接受接口的继承
    public struct Family : IPerson
    {
        public string name;
        public int age;
        public bool sex;
        public string country;
        public Person person;

        //不可以包含显式的无参构造函数和析构函数
        public Family(string name, int age, bool sex, string country, Person person)
        {
            this.name = name;
            this.age = age;
            this.sex = sex;
            this.country = country;
            this.person = person;
        }
        //不可以实现protected、virtual、sealed和override成员
        public void GetSex()
        {
            if (sex)
                Console.WriteLine(person.Name + " is a boy.");
            else
                Console.WriteLine(person.Name + " is a girl.");
        }

        public void ShowPerson()
        {
            Console.WriteLine("This is {0} from {1}", new Person(name, 22).Name, country);
        }

        //可以重载ToString()
        public override string ToString()
        {
            return string.Format("{0} is {1}, {2} from {3}", person.Name, age, sex ? "Boy" : "Girl", country);
        }

    }

    public class MyTest
    {
        static void Main1(string[] arg)
        {
            //不使用new来实例化结构，其内部成员初始化为0
            Family newFamily;
            newFamily.name = "Marson Family";   //name = "Marson Family"
            newFamily.sex = true;                           //sex = true,age=0,con
            Console.WriteLine(newFamily.name);  // Marson Family
            //用new生成结构 内部成员初始化调用构造函数
            Family myFamily = new Family("Marson Family", 23, true, "China", new Person("Marson", 23)); //调用构造函数 name=Marson Family,sex=23
            Person person = new Person();
            person.Name = "Marson"; //Person类中的Name=Marson
            //按值传递参数
            ShowFamily(myFamily);
            //按引用传递参数
            ShowPerson(person);
            myFamily.GetSex();
            myFamily.ShowPerson();
        }

        public static void ShowPerson(Person person)
        {
            person.Name = "Emma";
            Console.WriteLine("this is {0}", person.Name);
        }
        public static void ShowFamily(Family family)
        {
            family.name = "Aeor";   //name="Aeor"
            Console.WriteLine("this is {0}", family.name);
        }
    }
}
