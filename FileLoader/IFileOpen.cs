using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileLoader.ENUM;
using System.IO;

namespace FileLoader
{
    public interface IFileOpen
    {
        void Open();
    }
    //所有文件的基类
    public abstract class Files : IFileOpen
    {
        private FileType fileType = FileType.doc;
        private FileType FileType
        {
            get { return fileType; }
        }
        public abstract void Open();
    }
    public abstract class DocFile : Files
    {
        public int GetPageCount()
        {
            return 2;//计算文档页数
        }
    }
    public abstract class MediaFile : Files
    {
        public int FileSize()
        {
            return 1024;
        }
    }
    public class MPEGFile : MediaFile
    {
        public override void Open()
        {
            Console.WriteLine("Open the MPEG file.");
        }
    }

    abstract class ImageFIle : Files
    {
        public void ZoomIn()
        {
            //方法比例
        }
        public void ZoomOut()
        {
            //缩小比例
        }
    }
    //对Word文档进行具体操作
    public class WordFile : DocFile
    {
        public override void Open()
        {
            Console.WriteLine("Open the Word file;");
        }
    }

    public class LoadManager
    {
        private IList<Files> files = new List<Files>();
        public IList<Files> Files
        {
            get
            {
                return files;
            }
        }
        public void LoadFiles(Files file)
        {
            files.Add(file);
        }
        //打开所有资料
        public void OpenAllFiles()
        {
            foreach (IFileOpen file in files)
            {
                file.Open();
            }
        }
        //打开单个资料
        public void OpenFile(IFileOpen file)
        {
            file.Open();
        }
        //获取文件类型
        public FileType GetFileType(string fileName)
        { 
            //根据指定路径文件返回类型
            FileInfo fi = new FileInfo(fileName);
            return (FileType)Enum.Parse(typeof(FileType), fi.Extension);
        }
    }

    class FileClient
    {
        public static void Main()
        { 
            //首先启动文件加载
            LoadManager lm = new LoadManager();
            //添加要处理的文件
            lm.LoadFiles(new WordFile());
            foreach (Files file in lm.Files)
            {
                if (file is DocFile)
                {
                    lm.OpenFile(file);
                }
            }
        }
    }
}
