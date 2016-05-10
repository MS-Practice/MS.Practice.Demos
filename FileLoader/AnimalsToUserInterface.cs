using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileLoader.InterfaceClass;

namespace FileLoader
{
    class AnimalsToUserInterface
    {
    }
    public class Dog : Animal
    {

        public override void Show()
        {
            Console.WriteLine("This is Dog");
        }
    }
    public class Cat : Animal
    {

        public override void Show()
        {
            Console.WriteLine("This is Cat");
        }
    }

    //泛型工厂方法
    public class AnimalFactory<TAnimalBase, TAnimal> : IAnimalFactory<TAnimalBase> where TAnimal : TAnimalBase, new()
    {
        //显示现实接口
        //TAnimalBase IAnimalFactory<TAnimalBase>.Create()
        //{
        //    throw new NotImplementedException();
        //}
        public TAnimalBase Create()
        {
            return new TAnimal();
        }
    }
    //基于对工厂方法的封装
    public class FactoryBuilder
    {
        public static IAnimalFactory<Animal> Build(string type)
        {
            if (type == "Dog")
                return new AnimalFactory<Animal, Dog>();
            else if (type == "Cat")
                return new AnimalFactory<Animal, Cat>();
            return null;
        }
    }
}
