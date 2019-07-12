$(function () {
 
    var dataJson = [
    ];

    $.ajax({
        url: "/SysUser/GetClientsDataJson",
        type: "get",
        dataType: "json",
        async: false,
        success: function (r) {
         
            if (r.isSucceeded == true) {

            }
            else
                if (r.isSucceeded == false) {

                    $('[authorize=yes]').hide();

                    if (r.message != null) {
                        dataJson = r.message;
                        if (dataJson != undefined) {

                            $.each(dataJson, function (i) {
                                $("#authorize" + dataJson[i].itemId).show();

                            });
                        }
                    }
                }
        }
    });
  
});