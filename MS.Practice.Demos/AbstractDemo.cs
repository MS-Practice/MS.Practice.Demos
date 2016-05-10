using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MS.Practice.Demos
{
    class AbstractDemo
    {
        static void Main1(string[] arg)
        {
            Animal duck = new Duck("Duck");
            duck.MakeVoice();
            duck.Show();
            Console.ReadLine();
        }
    }
    public abstract class Animal
    {
        protected string _name;
        public abstract string Name
        {
            get;
        }
        public void MakeVoice()
        {
            Console.WriteLine("All animals can make voice!");
        }
        public abstract void Show();
    }
    public interface IAction
    {
        void Move();
    }

    public class Duck : Animal, IAction
    {
        public Duck(string name)
        {
            this._name = name;
        }

        //重载抽象方法
        public override void Show()
        {
            Console.WriteLine(_name + " is showing for you");
        }
        //重载抽象属性
        public override string Name
        {
            get { return _name; }
        }
        //实现接口方法
        public void Move()
        {
            Console.WriteLine("Duck also can swim.");
        }
    }

    public class Dog : Animal, IAction
    {
        public Dog(string name)
        {
            this._name = name;
        }
        public override void Show()
        {
            Console.WriteLine(_name + " is showing for you.");
        }
        public override string Name
        {
            get { return _name; }
        }

        public void Move()
        {
            Console.WriteLine(_name + " also can run.");
        }
    }
}
