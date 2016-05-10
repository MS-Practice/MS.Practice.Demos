using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileLoader
{
    public interface IDriveable
    {
        void Drive();
    }
    public class TractorDrive : IDriveable
    {
        public int Name { get; set; }
        public void Drive() {
            Console.WriteLine("拖拉机司机开拖拉机");
        }
    }
    public class CarDrive : IDriveable
    {
        public int Name { get; set; }
        public void Drive()
        {
            Console.WriteLine("汽车司机开汽车");
        }
    }
}
