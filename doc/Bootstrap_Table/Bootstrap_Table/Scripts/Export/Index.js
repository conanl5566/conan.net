
$(function () {

    //1.初始化Table
    var oTable = new TableInit();
    oTable.Init();

    //2.初始化select的change事件
    $("#sel_exportoption").change(function () {
        $('#tb_departments').bootstrapTable('refreshOptions', {
            exportDataType: $(this).val()
        });
    });
    

});


var TableInit = function () {
    var oTableInit = new Object();
    //初始化Table
    oTableInit.Init = function () {
        $('#tb_departments').bootstrapTable({
            url: '/Export/GetDepartment',         //请求后台的URL（*）
            method: 'get',                      //请求方式（*）
            toolbar: '#toolbar',                //工具按钮用哪个容器
            striped: true,                      //是否显示行间隔色
            cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: true,                   //是否显示分页（*）
            sortable: false,                     //是否启用排序
            sortOrder: "asc",                   //排序方式
            queryParams: oTableInit.queryParams,//传递参数（*）
            sidePagination: "client",           //分页方式：client客户端分页，server服务端分页（*）
            pageNumber: 1,                       //初始化加载第一页，默认第一页
            pageSize: 10,                       //每页的记录行数（*）
            pageList: [10, 25, 50, 100],        //可供选择的每页的行数（*）
            clickToSelect:true,
            showExport: true,                     //是否显示导出
            exportDataType: "basic",              //basic', 'all', 'selected'.
            columns: [{
                checkbox: true
            }, {
                field: 'Name',
                title: '部门名称'
            }, {
                field: 'ParentName',
                title: '上级部门'
            }, {
                field: 'Level',
                title: '部门级别'
            }, {
                field: 'Desc',
                title: '描述'
            }, ]
        });
    };

    //得到查询的参数
    oTableInit.queryParams = function (params) {
        var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
            limit: params.limit,   //页面大小
            offset: params.offset,  //页码
            departmentname: $("#txt_search_departmentname").val(),
            statu: $("#txt_search_statu").val()
        };
        return temp;
    };
    return oTableInit;
};


var ButtonInit = function () {
    var oInit = new Object();
    var postdata = {};

    oInit.Init = function () {
        //$("#btn_add").click(function () {
        //    $("#myModalLabel").text("新增");
        //    $("#myModal").find(".form-control").val("");
        //    $('#myModal').modal()

        //    postdata.DEPARTMENT_ID = "";
        //});

        //$("#btn_edit").click(function () {
        //    var arrselections = $("#tb_departments").bootstrapTable('getSelections');
        //    if (arrselections.length > 1) {
        //        toastr.warning('只能选择一行进行编辑');

        //        return;
        //    }
        //    if (arrselections.length <= 0) {
        //        toastr.warning('请选择有效数据');

        //        return;
        //    }
        //    $("#myModalLabel").text("编辑");
        //    $("#txt_departmentname").val(arrselections[0].DEPARTMENT_NAME);
        //    $("#txt_parentdepartment").val(arrselections[0].PARENT_ID);
        //    $("#txt_departmentlevel").val(arrselections[0].DEPARTMENT_LEVEL);
        //    $("#txt_statu").val(arrselections[0].STATUS);

        //    postdata.DEPARTMENT_ID = arrselections[0].DEPARTMENT_ID;
        //    $('#myModal').modal();
        //});

        //$("#btn_delete").click(function () {
        //    var arrselections = $("#tb_departments").bootstrapTable('getSelections');
        //    if (arrselections.length <= 0) {
        //        toastr.warning('请选择有效数据');
        //        return;
        //    }

        //    Ewin.confirm({ message: "确认要删除选择的数据吗？" }).on(function (e) {
        //        if (!e) {
        //            return;
        //        }
        //        $.ajax({
        //            type: "post",
        //            url: "/Home/Delete",
        //            data: { "": JSON.stringify(arrselections) },
        //            success: function (data, status) {
        //                if (status == "success") {
        //                    toastr.success('提交数据成功');
        //                    $("#tb_departments").bootstrapTable('refresh');
        //                }
        //            },
        //            error: function () {
        //                toastr.error('Error');
        //            },
        //            complete: function () {

        //            }

        //        });
        //    });
        //});

        //$("#btn_submit").click(function () {
        //    postdata.DEPARTMENT_NAME = $("#txt_departmentname").val();
        //    postdata.PARENT_ID = $("#txt_parentdepartment").val();
        //    postdata.DEPARTMENT_LEVEL = $("#txt_departmentlevel").val();
        //    postdata.STATUS = $("#txt_statu").val();
        //    $.ajax({
        //        type: "post",
        //        url: "/Home/GetEdit",
        //        data: { "": JSON.stringify(postdata) },
        //        success: function (data, status) {
        //            if (status == "success") {
        //                toastr.success('提交数据成功');
        //                $("#tb_departments").bootstrapTable('refresh');
        //            }
        //        },
        //        error: function () {
        //            toastr.error('Error');
        //        },
        //        complete: function () {

        //        }

        //    });
        //});

        //$("#btn_query").click(function () {
        //    $("#tb_departments").bootstrapTable('refresh');
        //});
    };

    return oInit;
};