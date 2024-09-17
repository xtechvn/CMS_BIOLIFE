let chartRevenu, chartLabel;

let _dashBoard = {
    Init: function () {
        // Get total new client
        _dashBoard.GetTotalNewClientOfDay();

        // Get total order of user
        _dashBoard.GetTotalOrderOfUser();

        // Get total pending payment of user
        _dashBoard.GetTotalPendingPaymentOfUser();

        // get chart department
        _dashBoard.GetOrderStatistic(function (result) {
            $("#total_incomplete_order").text(result.totalrecordErr.toLocaleString());
        });

        // get chart week
        _dashBoard.GetChartRevenuByTime(1);

        // get chart department
        _dashBoard.GetChartRevenuByUnit(2);
    },

    GetOrderStatistic: function (callback) {
        _ajax_caller.post('/DashBoard/GetOrderStatistic', null, function (result) {
            callback(result);
        });
    },

    GetNewClientByDay: function (input, callback) {
        _ajax_caller.post('/DashBoard/GetNewClientByDay', { model: input }, function (result) {
            callback(result);
        });
    },

    GetRevenueOrderByDay: function (input, callback) {
        _ajax_caller.post('/DashBoard/GetRevenueOrderByDay', { model: input }, function (result) {
            callback(result);
        });
    },

    GetRevenueOrderGroupBySale: function (input, callback) {
        _ajax_caller.post('/DashBoard/GetRevenueOrderGroupBySale', { model: input }, function (result) {
            callback(result);
        });
    },

    GetTotalNewClientOfDay: function () {
        _dashBoard.GetNewClientByDay({
            from_date: new Date().toJSON(),
            to_date: new Date().toJSON(),
        }, function (data) {
            let total_new_client = 0;
            if (data != null && data.length > 0) {
                total_new_client = parseInt(data[0].totalClient);
            }
            $("#total_new_client").html(total_new_client.toLocaleString());
        });
    },

    GetTotalOrderOfUser: function () {
        _dashBoard.GetRevenueOrderByDay({
            from_date: new Date().toJSON(),
            to_date: new Date().toJSON(),
            status: 1
        }, function (data) {
            let total_order = 0, total_amount = 0;
            if (data != null && data.length > 0 && data.table.length > 0) {
                total_order = parseInt(data.table[0].totalOrder);
                total_amount = parseInt(data.table[0].totalRevenue);
            }
            $("#total_order_count").html(total_order.toLocaleString());
            $("#total_amount").html(total_amount.toLocaleString());
        });
    },

    GetTotalPendingPaymentOfUser: function () {
        _dashBoard.GetRevenueOrderByDay({
            from_date: new Date().toJSON(),
            to_date: new Date().toJSON(),
            status: 2
        }, function (data) {
            let total_order = 0, total_amount = 0;
            if (data != null && data.length > 0 && data.table.length > 0) {
                total_order = parseInt(data.table[0].totalOrder);
                total_amount = parseInt(data.table[0].totalRevenue);
            }
            $("#pending_payment_order_count").html(total_order.toLocaleString());
            $("#pending_payment_amount").html(total_amount.toLocaleString());
        });
    },

    GetChartRevenuByTime: function (date_type) {
        $('#revenuChart').hide();

        let toDate = new Date();
        let fromDate;

        if (date_type == 1) {
            fromDate = moment().subtract(7, 'd').toDate();
        } else {
            fromDate = moment().startOf('year').toDate();
        }

        _dashBoard.GetRevenueOrderGroupBySale({
            from_date: fromDate.toJSON(),
            to_date: toDate.toJSON(),
            date_type: date_type,
            type: 1
        }, function (result) {
            $('#revenuChart').show();
            chart.LoadChartRevenu(result.label, result.value);
        });
    },

    GetChartRevenuByUnit: function (type) {
        $('#revenuLabelChart').hide();

        let toDate = new Date();
        let fromDate = moment().startOf('month').toDate();

        _dashBoard.GetRevenueOrderGroupBySale({
            from_date: fromDate.toJSON(),
            to_date: toDate.toJSON(),
            type: type
        }, function (result) {
            $('#revenuLabelChart').show();
            chart.LoadChartLabelRevenu(result.label, result.value);
        });
    },
};

var chart = {
    LoadChartRevenu: function (listLabel, listRevenu) {
        if (chartRevenu != null && chartRevenu != undefined) chartRevenu.destroy();
        if (chartRevenu != undefined) chartRevenu.destroy();
        var ctx = document.getElementById("revenuChart").getContext('2d');

        //listLabel = ["day1", "day2", "day3", "day4", "day5", "day6", "day7"];
        //listLabelData = [100000, 200000, 300000, 400000, 500000, 600000, 700000];
        let backgroundColor = '#0059A1', borderColor = '#0059A1';

        var barChartData = {
            labels: listLabel,
            datasets: [
                {
                    type: "bar",
                    fill: false,
                    label: 'Doanh thu thuần',
                    backgroundColor: backgroundColor,
                    borderColor: borderColor,
                    borderWidth: 1,
                    data: listRevenu,
                    stack: 1
                },
            ]
        };

        var ObjectChart = {
            type: 'bar',
            data: barChartData,
            options: {
                responsive: true,
                //legend: {
                //    cursor: "pointer",
                //    position: 'bottom',
                //},
                legend: {
                    display: false
                },
                title: {
                    display: true,
                },
                scales: {
                    yAxes: [{
                        ticks: {
                            fontColor: 'black',
                            beginAtZero: true,
                            callback: function (value, index, values) {
                                return value / 1000000 + " tr";
                            }
                        },
                        stacked: true,
                    }],
                    xAxes: [{
                        barPercentage: 0.5,
                    }]

                },
                tooltips: {
                    enabled: true,
                    mode: 'single',
                    callbacks: {
                        title: function () { },
                        label: function (tooltipItems, data) {
                            var multistringText = [];
                            multistringText.push("Doanh thu : " + tooltipItems.yLabel.toLocaleString());
                            return multistringText;
                        }
                    }
                },
            }
        };

        chartRevenu = new Chart(ctx, ObjectChart);

        if (chartRevenu != undefined) chartRevenu.update();
    },

    LoadChartLabelRevenu: function (listLabel, listRevenu) {
        if (chartLabel != null && chartLabel != undefined) chartLabel.destroy();

        var label = "";
        var ctx = document.getElementById("revenuLabelChart").getContext('2d');

        //var listRevenu = [1000000, 2000000, 300000, 40000, 50000];
        //var listLabel = ["P1", "P2", "P3", "P4", "P5"];
        var backgroundColor = '#0059A1', borderColor = '#0059A1';

        var dataset = {
            label: label,
            data: listRevenu,
            backgroundColor: backgroundColor,
            borderColor: borderColor
        };

        var ObjectChart = {
            type: 'horizontalBar',
            data: {
                labels: listLabel,
                datasets: [dataset]
            },
            options: {
                responsive: true,
                legend: {
                    display: false,
                },
                title: {
                    display: true,
                    text: label
                },
                scales: {
                    yAxes: [{
                        position: 'left',
                        type: "category",
                        barPercentage: 0.5,
                    }],
                    xAxes: [{
                        ticks: {
                            fontColor: 'black',
                            beginAtZero: true,
                            callback: function (value, index, values) {
                                if (value <= 1) {
                                    return 0 + " tr";
                                }
                                if (value <= 100) {
                                    return value;
                                }
                                else {
                                    return value / 1000000 + " tr";
                                }
                            }
                        }
                    }]

                },
                tooltips: {
                    mode: 'label',
                    callbacks: {
                        label: function (tooltipItem, data) {
                            var indice = tooltipItem.index;
                            return data.datasets[0].data[indice].toLocaleString();
                        }
                    }
                },
            }
        };
        chartLabel = new Chart(ctx, ObjectChart);
        if (chartLabel != undefined) chartLabel.update();
    }
}

$('.btn_change_chart').click(function () {
    var seft = $(this);
    seft.addClass('btn-primary');
    seft.siblings().removeClass('btn-primary');
});

$(document).ready(function () {
    _dashBoard.Init();
});

