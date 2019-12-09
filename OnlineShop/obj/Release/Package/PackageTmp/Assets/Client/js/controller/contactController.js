var contact = {
    init: function () {
        contact.registerEvents();
    },
    registerEvents: function () {
        $('#btnSend').on('click', function () {
            var name = $('#txtName').val();
            var mobile = $('#txtMobile').val();
            var email = $('#txtEmail').val();
            var address = $('#txtAddress').val();
            var content = $('#txtContent').val();
            console.log("test");
            $.ajax({
                url: '/Contact/Send',
                type: 'POST',
                dataType: 'json',
                data: {
                    name: name,
                    mobile: mobile,
                    email: email,
                    address: address,
                    content: content
                },
                success: function (response) {
                    if (response.status == true) {
                        window.alert('Gửi thành công');
                    }
                }
            });
        });
    }
}
contact.init();