let chartRevenu, chartLabel;
let amount = 0;
let amount2 = 0;

var dashBoard_html = {
    Log_TransferSms_Today: ` <li><div class="info"><span style="width: 200px !important;"><img src="{Logo}" alt=""></span> <div class="content"> <div class="mb8">{BankName} - {AccountNumber}</div> {MessageContent}</div> </div> <div class="status"><div class="money"><span>{Amount} đ</span></div><div class="time">{ReceiveTimeStr}</div></div></li>`,
}
let _dashBoard = {
    Init: function () {

        // get chart week
        //_dashBoard.GetChartRevenuByTime(2);
        _dashBoard.GetTotalAmountTransactionSMs();
        _dashBoard.GetTotalAmountPayment();
        _dashBoard.GetListTransferSms_dashBoard();
        
        _dashBoard.GetCountPaymentThang();
        _dashBoard.GetPaymentVoucherByDate();
        _dashBoard.GettotalAmountBankingAccountTransferSms();
    },

    GetSumAmountTransactionSMs: function (input, callback) {
        _ajax_caller.post('/DashBoard/SumAmountTransactionSMs', { searchModel: input }, function (result) {
            callback(result);
        });
    },
    GetAmountPayment: function (input, callback) {
        _ajax_caller.post('/DashBoard/GetTotalAmountPaymentVoucherByDate', { searchModel: input }, function (result) {
            callback(result);
        });
    },
    GetListTransferSms: function (input, callback) {
        _ajax_caller.post('/DashBoard/GetListTransferSms', { searchModel: input }, function (result) {
            callback(result);
        });
    },
    GetCountPayment: function (input, callback) {
        _ajax_caller.post('/DashBoard/GetCountAmountPaymentVoucherByDate', { searchModel: input }, function (result) {
            callback(result);
        });
    },
    GetListPaymentVoucherByDate: function (input, callback) {
        _ajax_caller.post('/DashBoard/GetListTransferSmsGroupByDate', { searchModel: input }, function (result) {
            callback(result);
        });
    },
    GetRevenueOrderGroupBySale: function (input, callback) {
        _ajax_caller.post('/DashBoard/GetRevenueOrderGroupBySale', { model: input }, function (result) {
            callback(result);
        });
    },
    GettotalAmountBankingAccountTransferSmsGroupByDate: function (input, callback) {
        _ajax_caller.post('/DashBoard/GettotalAmountBankingAccountTransferSmsGroupByDate', { model: input }, function (result) {
            callback(result);
        });
    },
    GetTotalAmountTransactionSMs: function () {
        var searchModel = {
            FromDateStr: new Date().toLocaleDateString("en-GB"),
            ToDateStr: new Date().toLocaleDateString("en-GB"),
            type: 1,
        };
        _dashBoard.GetSumAmountTransactionSMs(searchModel, function (data) {
            $("#total_Amount_TransactionSMs").html(data.amount.toLocaleString());

        });
    },
    GetTotalAmountPayment: function () {
        var searchModel = {
            FromDateStr: new Date().toLocaleDateString("en-GB"),
            ToDateStr: new Date().toLocaleDateString("en-GB"),
            type: 1,
        };
        _dashBoard.GetAmountPayment(searchModel, function (data) {
            $("#total_Amount").html(Math.round(data.totalAmountTransactionSMs.amount - data.totalAmountPayment).toLocaleString());
            $("#total_Amount_Payment").html(data.totalAmountPayment.toLocaleString());

        });
    },

    GettotalAmountBankingAccountTransferSms: function () {
        var newdate = new Date();
        newdate.setDate(01);
        var searchModel = {
            FromDateStr: newdate.toLocaleDateString("en-GB"),
            ToDateStr: new Date().toLocaleDateString("en-GB"),
        };
        _dashBoard.GettotalAmountBankingAccountTransferSmsGroupByDate(searchModel, function (data) {

            $("#totalAmount_BankingAccount_TransferSms").html(data.toLocaleString());

        });
      
    },
    GetCountPaymentThang: function () {
        var newdate = new Date();
        newdate.setDate(01);
        var searchModel = {
            FromDateStr: newdate.toLocaleDateString("en-GB"),
            ToDateStr: new Date().toLocaleDateString("en-GB"),
            type: 1,
        };
        _dashBoard.GetCountPayment(searchModel, function (data) {
            $("#count_Payment_thang").html(data.toLocaleString());
            var countpm = data;
            _dashBoard.GetSumAmountTransactionSMs(searchModel, function (data) {

                $("#total_Amount_Payment_thang").html(data.amount.toLocaleString());
                $("#TB_total_Amount_Payment_thang").html(Math.round(data.amount / data.total).toLocaleString());
                $("#count_TransactionSMs_thang").html(data.total.toLocaleString());
                $("#Sum_total").html((data.total + countpm).toLocaleString());
            });
            

        });
       
    },
    GetPaymentVoucherByDate: function () {
        $('#revenuChart').hide();
        var newdate = new Date();
        newdate.setDate(01);
        newdate.setMonth(00);
        var searchModel = {
            FromDateStr: newdate.toLocaleDateString("en-GB"),
            ToDateStr: new Date().toLocaleDateString("en-GB"),
        };
        _dashBoard.GetListPaymentVoucherByDate(searchModel, function (data) {

            $('#revenuChart').show();
            var date = [];
            var amount = [];
            var Balance = [];
            var amountTransaction = [];
            data.forEach(function (item) {
                date.push('Tháng '+ item.month);
                amount.push(item.amount);
                amountTransaction.push(item.amountTransaction);
                Balance.push(item.balance);
            });
            chart.LoadChartRevenu(date, amountTransaction, amount, Balance);

        });
    },

    GetListTransferSms_dashBoard: function () {
        var searchModel = {
            FromDateStr: new Date().toLocaleDateString("en-GB"),
            ToDateStr: new Date().toLocaleDateString("en-GB"),
            Amount: -1,
        };
        var rows = ''; 
        _dashBoard.GetListTransferSms(searchModel, function (data) {
            if (data != undefined && data != undefined && data.length > 0) {
                data.forEach(function (item) {
                    rows += dashBoard_html.Log_TransferSms_Today.replace('{Logo}', item.logo).replace('{BankName}', item.bankName).replace('{AccountNumber}', item.accountNumber).replace('{Amount}', item.amount > 0 ? '+' + item.amount.toLocaleString() : item.amount.toLocaleString()).replace('{ReceiveTimeStr}', item.receiveTimeStr).replace('{MessageContent}', item.messageContent.replaceAll('|', '</br>'));
                 
                });
            }
            $("#list_TransactionSms").html(rows);
        });
    },
    getList: function () {
        var type = $('input[name=optradio]:checked').val();
        $('#form_down_check_radio').attr('style', 'display: none');
        $('#btn_check_radio').removeClass('active');

        var searchModel = {
            FromDateStr: null,
            ToDateStr: null,
            type: 1,
        };
        switch (type) {
            case "1": {
                searchModel.FromDateStr = new Date().toLocaleDateString("en-GB");
                searchModel.ToDateStr = new Date().toLocaleDateString("en-GB");
                $('#check_radio_name').text("Hôm nay")
            }
                break;
            case "2": {
                var newDate = new Date(_global_function.ParseDateTostring(new Date().toLocaleDateString("en-GB")));

                newDate.setDate(newDate.getDate() - 1);
                searchModel.FromDateStr = newDate.toLocaleDateString("en-GB");
                searchModel.ToDateStr = newDate.toLocaleDateString("en-GB");
                $('#check_radio_name').text("Hôm qua")
            }
                break;
            case "3": {
                var newDate = new Date(_global_function.ParseDateTostring(new Date().toLocaleDateString("en-GB")));

                newDate.setDate(newDate.getDate() - 7);
                searchModel.FromDateStr = newDate.toLocaleDateString("en-GB");
                searchModel.ToDateStr = new Date().toLocaleDateString("en-GB");
                $('#check_radio_name').text("Tuần trước")
            }
                break;
            case "4": {
                var newdate = new Date();
                newdate.setDate(01);
                searchModel.FromDateStr = newdate.toLocaleDateString("en-GB");
                searchModel.ToDateStr = new Date().toLocaleDateString("en-GB");
                $('#check_radio_name').text("Tháng này")
            }
                break;
            case "5": {
                var newDate = new Date(_global_function.ParseDateTostring(new Date().toLocaleDateString("en-GB")));
                newDate.setDate(01);
                newDate.setMonth(newDate.getMonth() - 1);
                var newDate2 = new Date(_global_function.ParseDateTostring(new Date().toLocaleDateString("en-GB")));
                newDate2.setDate(30);
                newDate2.setMonth(newDate2.getMonth() - 1);

                searchModel.FromDateStr = newDate.toLocaleDateString("en-GB");
                searchModel.ToDateStr = newDate2.toLocaleDateString("en-GB");
                $('#check_radio_name').text("Tháng trước")
            }
                break;
            case "6": {
                $('#check_radio_name').text("Quý này")
                var newDate = new Date(_global_function.ParseDateTostring(new Date().toLocaleDateString("en-GB")));
                var month = newDate.getMonth() + 1;
                var quarter = Math.ceil(month / 3);
                switch (quarter) {
                    case 1: {
                        var newDate = new Date(_global_function.ParseDateTostring(new Date().toLocaleDateString("en-GB")));
                        newDate.setDate(01);
                        newDate.setMonth(00);
                        var newDate2 = new Date(_global_function.ParseDateTostring(new Date().toLocaleDateString("en-GB")));
                        newDate2.setDate(31);
                        newDate2.setMonth(02);

                        searchModel.FromDateStr = newDate.toLocaleDateString("en-GB");
                        searchModel.ToDateStr = newDate2.toLocaleDateString("en-GB");
                    } break;
                    case 2: {
                        var newDate = new Date(_global_function.ParseDateTostring(new Date().toLocaleDateString("en-GB")));
                        newDate.setDate(01);
                        newDate.setMonth(03);
                        var newDate2 = new Date(_global_function.ParseDateTostring(new Date().toLocaleDateString("en-GB")));
                        newDate2.setDate(30);
                        newDate2.setMonth(05);

                        searchModel.FromDateStr = newDate.toLocaleDateString("en-GB");
                        searchModel.ToDateStr = newDate2.toLocaleDateString("en-GB");
                    } break;
                    case 3: {
                        var newDate = new Date(_global_function.ParseDateTostring(new Date().toLocaleDateString("en-GB")));
                        newDate.setDate(01);
                        newDate.setMonth(06);
                        var newDate2 = new Date(_global_function.ParseDateTostring(new Date().toLocaleDateString("en-GB")));
                        newDate2.setDate(30);
                        newDate2.setMonth(08);

                        searchModel.FromDateStr = newDate.toLocaleDateString("en-GB");
                        searchModel.ToDateStr = newDate2.toLocaleDateString("en-GB");
                    } break;
                    case 4: {
                        var newDate = new Date(_global_function.ParseDateTostring(new Date().toLocaleDateString("en-GB")));
                        newDate.setDate(01);
                        newDate.setMonth(09);
                        var newDate2 = new Date(_global_function.ParseDateTostring(new Date().toLocaleDateString("en-GB")));
                        newDate2.setDate(31);
                        newDate2.setMonth(11);

                        searchModel.FromDateStr = newDate.toLocaleDateString("en-GB");
                        searchModel.ToDateStr = newDate2.toLocaleDateString("en-GB");
                    } break;
                }
            }
                break;
            case "7": {
                $('#check_radio_name').text("Quý trước")
                var newDate = new Date(_global_function.ParseDateTostring(new Date().toLocaleDateString("en-GB")));
                var month = newDate.getMonth() + 1;
                var quarter = Math.ceil(month / 3) - 1;
                switch (quarter) {
                    case 1: {
                        var newDate = new Date(_global_function.ParseDateTostring(new Date().toLocaleDateString("en-GB")));
                        newDate.setDate(01);
                        newDate.setMonth(00);
                        var newDate2 = new Date(_global_function.ParseDateTostring(new Date().toLocaleDateString("en-GB")));
                        newDate2.setDate(31);
                        newDate2.setMonth(02);

                        searchModel.FromDateStr = newDate.toLocaleDateString("en-GB");
                        searchModel.ToDateStr = newDate2.toLocaleDateString("en-GB");
                    } break;
                    case 2: {
                        var newDate = new Date(_global_function.ParseDateTostring(new Date().toLocaleDateString("en-GB")));
                        newDate.setDate(01);
                        newDate.setMonth(03);
                        var newDate2 = new Date(_global_function.ParseDateTostring(new Date().toLocaleDateString("en-GB")));
                        newDate2.setDate(30);
                        newDate2.setMonth(05);

                        searchModel.FromDateStr = newDate.toLocaleDateString("en-GB");
                        searchModel.ToDateStr = newDate2.toLocaleDateString("en-GB");
                    } break;
                    case 3: {
                        var newDate = new Date(_global_function.ParseDateTostring(new Date().toLocaleDateString("en-GB")));
                        newDate.setDate(01);
                        newDate.setMonth(06);
                        var newDate2 = new Date(_global_function.ParseDateTostring(new Date().toLocaleDateString("en-GB")));
                        newDate2.setDate(30);
                        newDate2.setMonth(08);

                        searchModel.FromDateStr = newDate.toLocaleDateString("en-GB");
                        searchModel.ToDateStr = newDate2.toLocaleDateString("en-GB");
                    } break;
                    case 4: {
                        var newDate = new Date(_global_function.ParseDateTostring(new Date().toLocaleDateString("en-GB")));
                        newDate.setDate(01);
                        newDate.setMonth(09);
                        var newDate2 = new Date(_global_function.ParseDateTostring(new Date().toLocaleDateString("en-GB")));
                        newDate2.setDate(31);
                        newDate2.setMonth(11);

                        searchModel.FromDateStr = newDate.toLocaleDateString("en-GB");
                        searchModel.ToDateStr = newDate2.toLocaleDateString("en-GB");
                    } break;
                }
            }
                break;
        }
      
        _dashBoard.GetAmountPayment(searchModel, function (data) {
            $("#total_Amount").html(Math.round(data.totalAmountTransactionSMs.amount - data.totalAmountPayment).toLocaleString());
            $("#total_Amount_Payment").html(data.totalAmountPayment.toLocaleString());

        });
        _dashBoard.GetSumAmountTransactionSMs(searchModel, function (data) {
            $("#total_Amount_TransactionSMs").html(data.amount.toLocaleString());

        });
        //_dashBoard.GetListPaymentVoucherByDate(searchModel, function (data) {

        //    $('#revenuChart').show();
        //    var date = [];
        //    var amount = [];
        //    var Balance = [];
        //    var amountTransaction = [];
        //    data.forEach(function (item) {
        //        date.push('Tháng ' + item.month);
        //        amount.push(item.amount);
        //        amountTransaction.push(item.amountTransaction);
        //        Balance.push(item.balance);
        //    });
        //    chart.LoadChartRevenu(date, amountTransaction, amount, Balance);

        //});
    }

};

var chart = {
    LoadChartRevenu: function (listLabel, listRevenu, listRevenu2, listRevenu3) {
        if (chartRevenu != null && chartRevenu != undefined) chartRevenu.destroy();
        if (chartRevenu != undefined) chartRevenu.destroy();
        var densityCanvas = document.getElementById("revenuChart");

        var densityData = {
            label: 'Tiền vào',
            data: listRevenu,
            backgroundColor: 'blue',
            stack: 1,

      
        };

        var gravityData = {
            label: 'Tiền ra',
            data: listRevenu2,
            backgroundColor: 'aqua',
            stack: 2,
         
           
        };
        var BalanceData = {
            label: 'Balance',
            data: listRevenu3,
            backgroundColor: 'yellow',
            stack: 3,
      
          
        };
        var barChartData = {
            labels: listLabel,
            datasets: [densityData,gravityData,BalanceData]
        };

        var chartOptions = {
            scales: {
                yAxes: [{
                    ticks: {
                        fontColor: 'black',
                        beginAtZero: true,
                        callback: function (value, index, values) {
                            return (value / 1000000000).toLocaleString() + " Tỷ";
                        }
                    },
                    stacked: true,
                    
                }],
                xAxes: [{
                   
                    categoryPercentage: 0.6
                }],

            },

            tooltips: {
                responsive: false,
                enabled: true,
                mode: 'single',
                callbacks: {
                    title: function () { },
                    label: function (tooltipItems, data) {
                        var multistringText = [];
                        multistringText.push(" "+tooltipItems.yLabel.toLocaleString() +" đ");
                        return multistringText;
                    }
                }
            },
        };
        chartRevenu = new Chart(densityCanvas, {
            type: 'bar',
            data: barChartData,
            options: chartOptions
        });
        if (chartRevenu != undefined) chartRevenu.update();
    },

   
}

$('.btn_change_chart').click(function () {
    var seft = $(this);
    seft.addClass('btn-primary');
    seft.siblings().removeClass('btn-primary');
});

$(document).ready(function () {
    _dashBoard.Init();
});
