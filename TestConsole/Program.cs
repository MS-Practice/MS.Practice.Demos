using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileLoader;
using FileLoader.InterfaceClass;
using Microsoft.Practices.Unity;
using System.Collections;

namespace TestConsole
{
    class Program
    {
        delegate void MessageDelegate();
        delegate void MessageDelegate1(string value);
        static void Main(string[] args)
        {
            LoadManager lm = new LoadManager();
            //添加要处理的文件
            lm.LoadFiles(new WordFile());
            lm.LoadFiles(new MPEGFile());
            foreach (Files file in lm.Files)
            {
                if (file is DocFile || file is MediaFile)
                {
                    lm.OpenFile(file);
                }
            }
            Files myFifles = new WordFile();
            myFifles.Open();

            //IList<IDriveable> drivers = new List<IDriveable>();
            //drivers.Add(new CarDrive());
            //drivers.Add(new TractorDrive());
            //foreach (IDriveable driver in drivers)
            //{
            //    driver.Drive();
            //}

            EasyBankStaff bankStaff = new EasyBankStaff();
            bankStaff.HanleProcess(new TransferClient());
            //bankStaff.HanleProcess(new Client("取款用户"));

            //Animal am = new Dog();
            //am.Show();
            ////初始化泛型接口
            IAnimalFactory<Animal> factory = FactoryBuilder.Build("Cat");
            Animal dog = factory.Create();
            dog.Show();

            ////微软Unity容器实现依赖注入
            //IUnityContainer container = new UnityContainer();
            ////注册 mapping(映射)
            //container.RegisterType<Animal, Dog>();
            //Animal dog1 = container.Resolve<Animal>();
            //dog1.Show();

            //通过匿名委托形成闭包
            //string value = "Hello,World";
            //MessageDelegate message = delegate()
            //{
            //    Show(value);
            //};
            //MessageDelegate1 message1 = Show;
            //message();
            //message1(value);
            //ShowValues();

            //int i = 100;
            //string str = "One";
            //ChangeByValue(ref i);   // 值传递参数  指针指向的地址转向200 所以输出200
            //ChangeByRef(ref str);   //引用传递  str指向引用的地址 One代表的地址 经过ChangeByRef 地址转向One More 输出OneMore
            //Console.WriteLine(i);
            //Console.WriteLine(str);
        }
        private static void ChangeByValue(ref int IValue) {
            IValue = 200;
        }
        private static void ChangeByRef(ref string sValue)
        {
            sValue = "One More";
        }
        private static void ChangeStr(string aStr)
        {
            aStr = "Changing String";
            Console.WriteLine(aStr);
        }
        private static void Show(string value)
        {
            int val = 100;
            IList<Func<int>> funcs = new List<Func<int>>();
            funcs.Add(() =>
            {
                return val + 1;
            });
            funcs.Add(() => val - 2);
            foreach (var f in funcs)
            {
                val = f();
                Console.WriteLine(val);
            }
            Tax tx = new Tax();
            decimal toll = tx.Calculate(()=>
            {
                return 55 * 62;
            },55);
            Console.WriteLine(toll);
            Console.WriteLine(value);
        }
        private static void ShowValues()
        {
            List<Action> actions = new List<Action>();
            for (int i = 0; i < 5; i++)
            {
                actions.Add(() => Console.WriteLine(i));
            }
            actions.ForEach(x => x());
        }
    }
}
