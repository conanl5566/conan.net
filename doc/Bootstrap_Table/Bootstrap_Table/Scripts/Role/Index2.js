
$(function () {

    $('#tb_roles').bootstrapTable({
        url: '/Role/GetRole',
        method: 'get',
        toolbar: '#toolbar',
        striped: true,
        cache: false,
        striped: true,
        pagination: true,
        sortable: true,
        queryParams: function (param) {
            return param;
        },
        queryParamsType: "limit",
        detailView: false,//父子表
        sidePagination: "server",
        pageSize: 10,
        pageList: [10, 25, 50, 100],
        search: true,
        showColumns: true,
        showRefresh: true,
        minimumCountColumns: 2,
        clickToSelect: true,
        
    });

});


