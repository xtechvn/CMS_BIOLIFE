var _data_monitor = {
    OnExcuteQuerry: function () {
        var _dataQuery = $('#text-query-data').val();
        $.ajax({
            url: "/home/executequery",
            type: "post",
            data: { dataQuery: _dataQuery },
            success: function (data) {
                if (data.isSuccess) {
                    var dataResult = JSON.parse(data.dataSet);
                    var tableCount = data.tableCount;

                    var divElement = document.createElement("div");
                    divElement.className = "table-responsive";
                    var pElemet = document.createElement("p");
                    pElemet.style.fontSize = "20px";

                    var TableExecuteName = (tableCount - 1 > 0) ? "Table" + (tableCount - 1) : "Table";
                    var ExecuteResult = dataResult[TableExecuteName][0]["ResultValue"];

                    if (ExecuteResult == 1) {
                        pElemet.className = "success";
                        pElemet.textContent = "Thực hiện truy vấn thành công";
                    } else {
                        pElemet.className = "error";
                        pElemet.textContent = "Sai cú pháp hoặc đã xảy ra lỗi khi truy vấn";
                    }

                    divElement.appendChild(pElemet);

                    if (tableCount > 1) {
                        for (var i = 0; i < tableCount - 1; i++) {
                            var TableName = (i == 0 ? "Table" : "Table" + i);
                            console.log(dataResult[TableName]);
                            var table = _data_monitor.RenderDataToTable(dataResult[TableName]);
                            divElement.appendChild(table);
                        }
                    }

                    $("#grid-table-data").empty().append(divElement);

                    _msgalert.success(data.message);
                } else {
                    _msgalert.error(data.message);
                }
            }
        });
    },

    RenderDataToTable: function (JsonArray) {
        var col = [];
        for (var i = 0; i < JsonArray.length; i++) {
            for (var key in JsonArray[i]) {
                if (col.indexOf(key) === -1) {
                    col.push(key);
                }
            }
        }

        // Create a table.
        var table = document.createElement("table");
        table.className = "table table-bordered";

        // Create table header row using the extracted headers above.
        var tr = table.insertRow(-1);

        // table header
        for (var i = 0; i < col.length; i++) {
            var th = document.createElement("th");
            th.innerHTML = col[i];
            tr.appendChild(th);
        }

        // add json data to the table as rows.
        for (var i = 0; i < JsonArray.length; i++) {
            tr = table.insertRow(-1);
            for (var j = 0; j < col.length; j++) {
                var tabCell = tr.insertCell(-1);
                tabCell.innerHTML = JsonArray[i][col[j]];
            }
        }

        return table;
    },

    OnGetTemplate: function (type) {
        var value = null;

        switch (type) {
            case 1:
                value = "select * from [Table Name] where [Field Name] =  [Your Value]"
                break;
            case 2:
                value = "insert into  [Table Name] ([Field 1],[Field 2]) values ([Value 1],[Value 2])"
                break;
            case 3:
                value = "update [Table Name] set [Field 1] = [Value 1],[Field 2] = [Value 2]  where [Field Name] =  [Your Value]"
                break;
            case 4:
                value = "delete from [Table Name] where [Field Name] = [Your Value]"
                break;
        }
        $('#text-query-data').val(value);
    }
};