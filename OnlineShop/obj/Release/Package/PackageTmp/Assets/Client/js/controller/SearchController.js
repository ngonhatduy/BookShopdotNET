$(function () {
    $("#txtKeyword").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "/Product/ListName",
                dataType: "json",
                data: {
                    q: request.term
                },
                success: function (res) {
                    //console.log(res.data);
                    response(res.data);
                }
            });
        },
        focus: function (event, ui) {
            //console.log(ui.item);
            $("#txtKeyword").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtKeyword").val(ui.item.label);
            return false;
        }
    })
    .autocomplete("instance")._renderItem = function (ul, item) {
        console.log(item.label);
        return $("<li>")
            .append("<a>" + item.label + "</a>")
            .appendTo(ul);
    };
});