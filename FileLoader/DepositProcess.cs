using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileLoader
{
    public interface IBankProcess
    {
        void Process();
    }
    public interface IClient
    {
        IBankProcess CreateProcess();
    }
    //按银行业务进行分类
    class DepositProcess : IBankProcess
    {
        public void Process()
        {
            //办理取款业务
            Console.WriteLine("取款10000圆");
        }
    }
    class DepositClient : IClient
    {
        IBankProcess IClient.CreateProcess()
        {
            return new DepositProcess();
        }
    }
    class TransferProcess : IBankProcess
    {
        public void Process()
        {
            //办理转账业务
            Console.WriteLine("转账10000圆");
        }
    }
    public class TransferClient : IClient
    {
        IBankProcess IClient.CreateProcess()
        {
            return new TransferProcess();
        }
    }
    public class DrawMoneyClient : IClient
    {
        IBankProcess IClient.CreateProcess()
        {
            return new DrawMoneyProcess();
        }
    }
    public class DrawMoneyProcess : IBankProcess
    {
        public void Process()
        {
            //办理存款业务
            Console.WriteLine("存款10000圆");
        }
    }
    //银行具体操作
    public class EasyBankStaff
    {
        private IBankProcess bankProc = null;
        public void HanleProcess(IClient client)
        {
            //业务处理
            bankProc = client.CreateProcess();
            bankProc.Process();
        }
    }
    //用户
    //public class Client
    //{
    //    private string ClientType;
    //    public Client(string clientType)
    //    {
    //        ClientType = clientType;
    //    }
    //    public IBankProcess CreateProcess()
    //    {
    //        //实际处理
    //        switch (ClientType)
    //        {
    //            case "存款用户":
    //                return new DrawMoneyProcess();
    //                break;
    //            case "转账用户":
    //                return new TransferProcess();
    //                break;
    //            case "取款用户":
    //                return new DepositProcess();
    //                break;
    //        }
    //        return null;
    //    }
    //}
}
