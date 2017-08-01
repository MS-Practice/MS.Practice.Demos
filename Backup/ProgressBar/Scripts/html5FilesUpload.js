/*
参考文献：https://www.w3.org/TR/file-upload/#dfn-onload
*/

var ZXXFILE = {
    fileInput: null,        //html file控件
    dragDrop: null,     //拖拽敏感区域
    upButton: null,     //提交按钮
    url: "", 					//ajax地址
    fileFilter: [], 				//过滤后的文件数组
    filter: function (files) {		//选择文件组的过滤方法
        return files;
    },
    onSelect: function () { }, 	//文件选择后
    onDelete: function () { }, 	//文件删除后
    onDragOver: function () { }, 	//文件拖拽到敏感区域时
    onDragLeave: function () { }, //文件离开到敏感区域时
    onProgress: function () { }, 	//文件上传进度
    onSuccess: function () { }, 	//文件上传成功时
    onFailure: function () { }, 	//文件上传失败时,
    onComplete: function () { }, 	//文件全部上传完毕时
    /* 开发参数和内置方法分界线 */
    funDragHover: function (e) {
        e.stopPropagation();
        e.preventDefault();
        this[e.type === "dragover" ? "onDragOver" : "onDragLeave"].call(e.target);
        return this;
    },
    //获取选择文件，file控件或拖拽
    funGetFiles: function (e) {
        //取消鼠标经过样式
        this.funDragHover(e);
        //获取文件对象
        var files = e.target.files || e.dataTransfer.files;
        //继续添加文件
        this.fileFilter = this.fileFilter.concat(this.filter(files));
        this.funDealFiles();
        return this;
    },
    //选中文件的处理与回调
    funDealFiles: function () {
        for (var i = 0, file; file = this.fileFilter[i]; i++) {
            //增加唯一索引值
            file.index = i;
        }
        //执行选择回调
        this.onSelect(this.fileFilter);
        return this;
    },
    //删除对应的文件
    funDeleteFile: function (fileDelete) {
        var arrFile = [];
        for (var i = 0, file; file = this.fileFilter[i]; i++) {
            if (file != fileDelete) {
                arrFile.push(file);
            } else {
                this.onDelete(fileDelete);
            }
        }
        this.fileFilter = arrFile;
        return this;
    },
    //文件上传
    funUploadFile: function () {
        var self = this;
        if (location.host.indexOf("sitepointstatic") >= 0) return;  //非站点服务器不允许运行
        for (var i = 0, file; file = this.fileFilter[i]; i++) {
            (function (file) {
                var xhr = new XMLHttpRequest();
                //html5 XMLHttpRequest Level2新增的对象  如果支持这个对象
                if (xhr.upload) {
                    //上传
                    //prograss 是返回上传进度信息
                    xhr.upload.addEventListener("prograss", function (e) {
                        //event.total是需要传输的总字节，event.loaded是已经传输的字节
                        self.onProgress(file, e.loaded, e.total);
                    }, false);
                    //文件上传成功还是失败
                    xhr.onreadystatechange = function (e) {
                        if (xhr.readyState == 4) {
                            if (xhr.status == 200) {
                                self.onSuccess(file, xhr.responseText);
                                self.funDeleteFile(file);
                                if (!self.fileFilter.length) {
                                    //全部上传完毕
                                    self.onComplete();
                                } else {
                                    self.onFailure(file, xhr.responseText);
                                }
                            }
                        };
                        //开始上传  Ajax
                        xhr.open("POST", self.url, true);
                        xhr.setRequestHeader("X_FILENAME", file.name);      //设置请求头部信息
                        xhr.send(file);     //发送数据
                    }
                }
            })(file)
        }

    },
    init: function () {
        var self = this;
        if (this.dragDrop) {        //是否支持dragDrop拖拽对象
            this.dragDrop.addEventListener("dragover", function (e) { self.funDragHover(e); }, false);      //当被拖拽对象拖至另一对象范围内触发此函数
            this.dragDrop.addEventListener("dragleave", function (e) { self.funDragHover(e); }, false);     //当被拖拽对象移开该范围时 触发
            this.dragDrop.addEventListener("drop", function (e) { self.funGetFiles(e); }, false);       //在拖拽过程中，当鼠标松开时触发此函数
        }

        //文件选择控件选择
        if (this.fileInput) {
            this.fileInput.addEventListener("change", function (e) { self.funGetFiles(e); }, false);
        }

        //上传按钮提交
        if (this.upButton) {
            this.upButton.addEventListener("click", function (e) { self.funUploadFile(e); }, false);
        }
    }
};