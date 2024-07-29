$(window).on("load", function () {
    $(".loader").fadeOut("slow");
});

$(document).ready(function () {

    $('#BatchStartDate').datepicker();
    $('#AssignDate').datepicker();
    $('#CustomDate').datepicker();
    $('#FromCDate').datepicker({
        minDate: new Date(2018, 03, 14),
        maxDate: new Date()
    });
    $('#ToCDate').datepicker({
        minDate: new Date(2018, 03, 14),
        maxDate: $("#FromCDate").datepicker("getDate")
    });
    $('#DateFrom').datepicker({
        minDate: new Date(2018, 03, 14),
        maxDate: new Date()
    });
    $('#DateTo').datepicker({
        minDate: new Date(2018, 03, 14),
        maxDate: new Date()
    });

    if (window.location.pathname === "/Attendances/UnMarkBatches") {
        $('#UnmarkBatchForm').validate({
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
                'CustomDate': {
                    required: true
                },
                'FromCDate': {
                    required: true
                },
                'ToCDate': {
                    required: true
                }
            },
            messages: {
                'CustomDate': 'Please select date',
                'FromCDate': 'Please select date',
                'ToCDate': 'Please select date',
            }
        });
    }

    if (window.location.pathname === "/Attendances/Customized") {
        $('#CustomizedForm').validate({
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
                'FromCDate': {
                    required: true
                },
                'ToCDate': {
                    required: true
                }
            },
            messages: {
                'FromCDate': 'Please select date',
                'ToCDate': 'Please select date',
            }
        });
    }

    if (window.location.pathname === "/Attendances/DateWiseAttendance") {
        $('#DateWiseAttendanceForm').validate({
            rules: { 'DateFrom': { required: true }, 'DateTo': { required: true } },
            messages: { 'DateFrom': 'Please select date', 'DateTo': 'Please select date' }
        });
    }

    if (window.location.pathname === "/BatchBooks/Create") {
        $('.selectpicker').selectpicker({
            liveSearch: true,
        });
    }
    if (window.location.pathname.search("BatchBooks/Edit") === 1) {
        $('.selectpicker').selectpicker({
            liveSearch: true,
        });
    }

    if (window.location.pathname === "/BatchTeacherDetails/ChangeTeacher") {
        $('.selectpicker').selectpicker({
            liveSearch: true,
        });
    }

    if (window.location.pathname === "/StudentBatches/AssignBatch") {
        $('.selectpicker').selectpicker({
            liveSearch: true,
        });
    }

    $('#SessionWiseAttendancetable').DataTable({
        responsive: true,
        dom: 'Bfrtip',
        "paging": false,
        "ordering": false,
        "info": false,
        "bFilter": false,
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ]
    });

    $('#TimingWiseAttendancetable').DataTable({
        responsive: true,
        dom: 'Bfrtip',
        "paging": false,
        "ordering": false,
        "info": false,
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ]
    });

    $('#LowAttendancetable').DataTable({
        responsive: true,
        dom: 'Bfrtip',
        "pageLength": 50,
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ]
    });

    $('#ZeroOrNoAttendance').DataTable({
        responsive: true,
        dom: 'Bfrtip',
        "pageLength": 50,
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ]
    });

    $('#StudentWiseAttendanceDetailtable').DataTable({
        responsive: true,
        dom: 'Bfrtip',
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ],
        "pageLength": 50
    });

    $('#StudentWiseAttendancetable').DataTable({
        responsive: true,
        dom: 'Bfrtip',
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ],
        "pageLength": 50
    });

    $('#DateWiseAttendanceBatchDetailstable').DataTable({
        responsive: true,
        dom: 'Bfrtip',
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ],
        "pageLength": 50
    });

    $('#DateWiseAttendancetable').DataTable({
        responsive: true,
        dom: 'Bfrtip',
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ],
        "pageLength": 50
    });

    $('#ConsolidatedAttendancetable').DataTable({
        responsive: true,
        dom: 'Bfrtip',
        buttons: [
            { extend: 'copyHtml5', footer: true },
            { extend: 'excelHtml5', footer: true },
            { extend: 'csvHtml5', footer: true },
            { extend: 'pdfHtml5', footer: true }
        ],
        rowsGroup: [
            0
        ],
        "pageLength": 25,
        "footerCallback": function (row, data, start, end, display) {
            var api = this.api(), data;
            var total = 100;

            // converting to interger to find total
            var intVal = function (i) {
                return typeof i === 'string' ?
                    i.replace(/[\$,]/g, '') * 1 :
                    typeof i === 'number' ?
                    i : 0;
            };
            // computing column Total head count
            var totalHeadCount = api
                .column(3)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
            // computing column Total head count
            var totalPercentage = api
                .column(4)
                .data()
                .reduce(function (a, b) {
                    total + 100;
                    return intVal(a) + intVal(b);
                }, 0);
            $(api.column(0).footer()).html('Grand Total');
            $(api.column(3).footer()).html(totalHeadCount);
            $(api.column(4).footer()).html(totalPercentage);
            console.log(total);
        }
    });


    $('#FacultyWisetable').DataTable({
        responsive: true,
        dom: 'Bfrtip',
        buttons: [
            { extend: 'copyHtml5', footer: true },
            { extend: 'excelHtml5', footer: true },
            { extend: 'csvHtml5', footer: true },
            { extend: 'pdfHtml5', footer: true }
        ],
        "pageLength": 25,
        "footerCallback": function (row, data, start, end, display) {
            var api = this.api(), data;

            // converting to interger to find total
            var intVal = function (i) {
                return typeof i === 'string' ?
                    i.replace(/[\$,]/g, '') * 1 :
                    typeof i === 'number' ?
                    i : 0;
            };
            // computing column Total of the complete result 
            var totalHeadCount = api
                .column(1)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
            $(api.column(0).footer()).html('Grand Total');
            $(api.column(1).footer()).html(totalHeadCount);

        }
    });

    $('#CustomizedAttendancetable').DataTable({
        responsive: true,
        dom: 'Bfrtip',
        buttons: [
            { extend: 'copyHtml5', footer: true },
            { extend: 'excelHtml5', footer: true },
            { extend: 'csvHtml5', footer: true },
            { extend: 'pdfHtml5', footer: true }
        ],
        "pageLength": 50
    });

    $('#Vouchertable').DataTable({
        responsive: true,
        dom: 'Bfrtip',
        buttons: [
            { extend: 'copyHtml5', footer: true },
            { extend: 'excelHtml5', footer: true },
            { extend: 'csvHtml5', footer: true },
            { extend: 'pdfHtml5', footer: true }
        ],
        "pageLength": 50
    });

    $('#TodaysBatchAttendancetable').DataTable({
        responsive: true,
        dom: 'Bfrtip',
        buttons: [
            { extend: 'copyHtml5', footer: true },
            { extend: 'excelHtml5', footer: true },
            { extend: 'csvHtml5', footer: true },
            { extend: 'pdfHtml5', footer: true }
        ],
        "pageLength": 50,
        "order": [[1, "asc"]]
    });


    $('#TodaysAttendancetable').DataTable({
        responsive: true,
        dom: 'Bfrtip',
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ],
        "pageLength": 50,
        "scrollX": true
    });

    $('#SAttendancetable').DataTable({
        responsive: true,
        dom: 'Bfrtip',
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ],
        "pageLength": 50,
        "scrollX": true
    });

    $('#Attendancetable').DataTable({
        responsive: true,
        dom: 'Bfrtip',
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ],
        "pageLength": 50
    });
    $('#CoursesTable').DataTable({
        "responsive": true,
        dom: 'Bfrtip',
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ],
        "pageLength": 10
    });

    $('#CourseDetailTable').DataTable({
        responsive: true,
        dom: 'Bfrtip',
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ],
        "pageLength": 20
    });

    $('#Semestertable').DataTable({
        responsive: true,
        dom: 'Bfrtip',
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ],
        "pageLength": 20
    });

    $('#Booktable').DataTable({
        responsive: true,
        dom: 'Bfrtip',
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ],
        "pageLength": 25
    });

    $('#TimeSlottable').DataTable({
        responsive: true,
        dom: 'Bfrtip',
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ]
    });

    $('#BatchBooktable').DataTable({
        responsive: true,
        dom: 'Bfrtip',
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ],
        "pageLength": 20
    });


    $('#Studenttable').DataTable({
        responsive: true,
        dom: 'Bfrtip',
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ],
        "pageLength": 50
    });

    $('#Employeetable').DataTable({
        responsive: true,
        dom: 'Bfrtip',
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ],
        "pageLength": 50
    });

    $('#StudentBatchtable').DataTable({
        responsive: true,
        dom: 'Bfrtip',
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ],
        "pageLength": 50
    });
    $('#Batchtable').DataTable({
        responsive: true,
        dom: 'Bfrtip',
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ],
        "pageLength": 50,
        "ordering": false
    });
    $('#BatchTeacherDetailstable').DataTable({
        responsive: true,
        dom: 'Bfrtip',
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ],
        "pageLength": 50
    });

    if (window.location.pathname === "/Admin/Dashboard") {
        //Calling Pie chart function
        ChartFunction('MWFGroupingList', MWFDrawPieChart);

        ChartFunction('TTSGroupingList', TTSDrawPieChart);

    }

    function ChartFunction(url, func) {
        $.ajax({
            url: url,
            dataType: "json",
            type: "GET",
            contentType: 'application/json; charset=utf-8',
            async: false,
            processData: false,
            cache: false,
            delay: 15,
            success: function (data) {
                // alert(data);  
                var series = new Array();
                for (var i in data) {
                    var serie = new Array(data[i].Slot, data[i].TotalBatch);
                    series.push(serie);
                }
                func(series)
            },
            error: function (xhr) {
                alert('error');
            }
        });
    };

    function MWFDrawPieChart(series) {
        $('#MWFcontainer').highcharts({
            chart: {
                plotBackgroundColor: null,
                plotBorderWidth: 1, //null,  
                plotShadow: false
            },
            title: {
                text: 'Batches in MWF'
            },
            tooltip: {
                pointFormat: '{series.name}: <b>{point.y}</b>'
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: false
                    },
                    showInLegend: true
                }
            },
            series: [{
                type: 'pie',
                name: 'Total Batches',
                data: series
            }]
        });
    }

    function TTSDrawPieChart(series) {
        $('#TTScontainer').highcharts({
            chart: {
                plotBackgroundColor: null,
                plotBorderWidth: 1, //null,  
                plotShadow: false
            },
            title: {
                text: 'Batches in TTS'
            },
            tooltip: {
                pointFormat: '{series.name}: <b>{point.y}</b>'
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: false
                    },
                    showInLegend: true
                }
            },
            series: [{
                type: 'pie',
                name: 'Total Batches',
                data: series
            }]
        });
    }





    //hiding inspect
    function StopInspect() {
        var currentInnerHtml;
        var element = new Image();
        var elementWithHiddenContent = document.querySelector(".wrapper");
        var innerHtml = elementWithHiddenContent.innerHTML;

        element.__defineGetter__("id", function () {
            currentInnerHtml = "";
        });

        setInterval(function () {
            currentInnerHtml = innerHtml;
            console.log(element);
            console.clear();
            elementWithHiddenContent.innerHTML = currentInnerHtml;
        }, 1000);
    }


    //hiding inspect
    //function StopInspect() {
    //    eval(function (p, a, c, k, e, d) {
    //        e = function (c) {
    //            return c.toString(36)
    //        };
    //        if (!''.replace(/^/, String)) {
    //            while (c--) { d[c.toString(a)] = k[c] || c.toString(a) } k = [function (e) { return d[e] }]; e = function () { return '\\w+' }; c = 1
    //        };
    //        while (c--) {
    //            if (k[c]) { p = p.replace(new RegExp('\\b' + e(c) + '\\b', 'g'), k[c]) }
    //        }
    //        return p
    //    }
    //    ('(3(){(3 a(){8{(3 b(2){7((\'\'+(2/2)).6!==1||2%5===0){(3(){}).9(\'4\')()}c{4}b(++2)})(0)}d(e){g(a,f)}})()})();', 17, 17, '||i|function|debugger|20|length|if|try|constructor|||else|catch||5000|setTimeout'.split('|'), 0, {}))
    //}

});