/*<script src="https://www.appelsiini.net/download/jquery.jeditable.js"></script>*/
$(document).ready(function () {
    var oldValue = '';
    $('.edit').editable('/home/saveuser', {
        cssclass: 'jeditForm',
        tooltip: 'click to edit me...',
        width: 'none',
        height: 'none',
        onsubmit: function (settings, original) {
            oldValue = original.revert;
        },
        submitdata: function () {
            return {
                id: $(this).data('id'),
                PropertyName: $(this).data('propertyname')
            }
        },
        callback: function (value, settings) {
            var jsonData = $.parseJSON(value);
            if (jsonData.status) {
                $(this).text(jsonData.value);
            }
            else {
                $(this).text(oldValue);
            }
        }
    })

    $('.editSelect').editable('/home/saveuser', {
        cssclass: 'jeditForm',
        tooltip: 'click to edit me...',
        width: 'none',
        height: 'none',
        type: 'select',
        submit: 'Ok',
        loadurl: '/home/GetUserRoles',
        loaddata: function () {
            return { id: $(this).data('id') }
        },
        onsubmit: function (settings, original) {
            oldValue = original.revert;
        },
        submitdata: function () {
            return {
                id: $(this).data('id'),
                PropertyName: $(this).data('propertyname')
            }
        },
        callback: function (value, settings) {
            var jsonData = $.parseJSON(value);
            if (jsonData.status) {
                $(this).text(jsonData.value);
            }
            else {
                $(this).text(oldValue);
            }
        }
    })

    $('.editSelect').click(function () {
        $('select', this).addClass('form-control pull-left w100');
        $('button', this).addClass('btn btn-sm btn-success margin10')
    })
})