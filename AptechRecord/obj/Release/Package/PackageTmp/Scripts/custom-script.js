$(document).ready(function () {

    if (window.location.pathname === "/RecoveryOfficer/Task") {
        $('.selectpicker').selectpicker({
            liveSearch: true,
        });
    }

    $('#AbsenteesDate').datepicker({
        minDate: new Date(2018, 03, 14),
        maxDate: new Date()
    });


    if (window.location.pathname === "/") {
        $('form').validate({
            onError: function () {
                $('.input-group.error-class').find('.help-block.form-error').each(function () {
                    $(this).closest('.form-group').addClass('error-class').append($(this));
                });
            },
            rules: {
                Username: { required: true },
                Password: { required: true }
            },
            messages: {
                Username: { required: "Username is required" },
                Password: { required: "Password is required" }
            }
        });

        var $toolbar = $('.wrap-input100 > label');
        $toolbar.parent().before($toolbar);
    }

    if (window.location.pathname === "/RecoveryOfficer/AbsenteesFollowUp") {
        $('#AbsenteesFollowUpForm').validate({
            errorClass: 'help-block animation-slideDown', // You can change the animation class for a different entrance animation - check animations page  
            errorElement: 'div',
            errorPlacement: function (error, e) {
                e.parents('.form-group > div').append(error);
            },
            highlight: function (e) {

                $(e).closest('.form-group').removeClass('has-success has-error').addClass('has-error');
                $(e).closest('.help-block').remove();
            },
            success: function (e) {
                e.closest('.form-group').removeClass('has-success has-error');
                e.closest('.help-block').remove();
            },
            rules: {
                'AbsenteesDate': {
                    required: true
                }
            },
            messages: {
                'AbsenteesDate': 'Please select date'
            }
        });
    }


    $('#TodaysBatchMarked').DataTable({
        "bLengthChange": false,
        "pageLength": 7,
        responsive: true
    });

    $('#StudentDropoutListTable').DataTable({
        responsive: true
    });

    $('#AbsenteesFollowUp').DataTable({
        oLanguage: {
            sProcessing: "<img src='~/images/ajax-loader.gif'>"
        },
        processing: true,
        "bLengthChange": false,
        "pageLength": 30,
        responsive: true,
        dom: 'Bfrtip',
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ]
    });





})