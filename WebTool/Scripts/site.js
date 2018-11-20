$.fn.loadData = function (jsonValue) {
    var obj = jsonValue;
    var key, value, tagName, type, arr;
    for (x in obj) {
        key = x;
        value = obj[x];

        $("[name='" + key + "'],[name='" + key + "[]']").each(function () {
            tagName = $(this)[0].tagName;
            type = $(this).attr('type');
            if (tagName == 'INPUT') {
                if (type == 'radio') {
                    $(this).attr('checked', $(this).val() == value);
                } else if (type == 'checkbox') {
                    arr = value.split(',');
                    for (var i = 0; i < arr.length; i++) {
                        if ($(this).val() == arr[i]) {
                            $(this).attr('checked', true);
                            break;
                        }
                    }
                } else {
                    $(this).val(value);
                }
            } else if (tagName == 'SELECT' || tagName == 'TEXTAREA') {
                $(this).val(value);
            }

        });
    }
};

Date.prototype.format = function (fmt) {
    var o = {
        "M+": this.getMonth() + 1,                 //月份 
        "d+": this.getDate(),                    //日 
        "h+": this.getHours(),                   //小时 
        "m+": this.getMinutes(),                 //分 
        "s+": this.getSeconds(),                 //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds()             //毫秒 
    };
    if (/(y+)/.test(fmt)) {
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    }
    for (var k in o) {
        if (new RegExp("(" + k + ")").test(fmt)) {
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
        }
    }
    return fmt;
}
/*16进制颜色转为RGB格式*/
var reg = /^#([0-9a-fA-f]{3}|[0-9a-fA-f]{6})$/;
String.prototype.colorRgba = function () {
    var sColor = this.toLowerCase();
    if (sColor && reg.test(sColor)) {
        if (sColor.length === 4) {
            var sColorNew = "#";
            for (var i = 1; i < 4; i += 1) {
                sColorNew += sColor.slice(i, i + 1).concat(sColor.slice(i, i + 1));
            }
            sColor = sColorNew;
        }
        //处理六位的颜色值
        var sColorChange = [];
        for (var i = 1; i < 7; i += 2) {
            sColorChange.push(parseInt("0x" + sColor.slice(i, i + 2)));
        }
        return "rgba(" + sColorChange.join(",") + ",1)";
    } else {
        return sColor;
    }
};


function myFind(obj, key, val) {
    for (var i in obj) {
        if (obj[i][key] == val) {
            return obj[i];
        }
    }
    return null;
}

function myConfirm(s, f) {
    s = s || "您真的确定要删除吗？";
    noty({
        text: s,
        type: 'warning',
        theme: 'relax',
        dismissQueue: true,
        layout: 'center',
        animation: {
            open: 'animated flipInX',
            close: 'animated flipOutX'
        },
        buttons: [
            {
                addClass: 'btn btn-primary', text: '确定', onClick: function ($noty) {
                    $noty.close();
                    f();
                }
            },
            {
                addClass: 'btn btn-danger', text: '取消', onClick: function ($noty) {
                    $noty.close();
                }
            }
        ]
    });
}
// 将form转为AJAX提交
function ajaxSubmit(frm, fn) {
    var dataPara = $(frm).serialize();
    $.ajax({
        url: frm.attr("action"),
        type: frm.attr("method"),
        data: dataPara,//可能会出现后台接收到的参数值为null的情况，原因是form.js的源码不全，没有data这个参数，需要重新下载官网的源码。
        dataType: "json",
        async: false,//异步
        success: fn
    });
}
function openLock() {
    $('#DivLocker').show();
}

function closeLock() {
    $('#DivLocker').hide();
}

