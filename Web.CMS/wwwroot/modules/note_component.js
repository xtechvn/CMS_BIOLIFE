var _note = {
    OnEditComment: function (id) {
        var comment = $('.note-comment[data-id="' + id + '"]').text();
        $("#ip-note-id").val(id);
        $("#ip-note-comment").val(comment);
        $("#ip-note-comment").focus();
        $("#btn-reset-comment").removeClass('mfp-hide');
    },

    ResetComment: function () {
        $("#ip-note-id").val(0);
        $("#ip-note-comment").val('');
        $("#btn-reset-comment").addClass('mfp-hide');
    },

    SendComment: function () {
        let valid = true;
        let objmodel = {
            Id: parseFloat($("#ip-note-id").val()),
            Comment: $("#ip-note-comment").val(),
            DataId: parseFloat($("#ip-note-data-id").val()),
            Type: parseInt($("#ip-note-type").val())
        };

        if (objmodel.Comment.trim() == "") {
            valid = false;
            _msgalert.error("Bạn phải nhập nội dung bình luận");
        }

        if (valid) {
            $.ajax({
                url: "/Note/UpSert",
                data: objmodel,
                type: "POST",
                success: function (data) {
                    if (data.isSuccess) {
                        _msgalert.success(data.message);
                        _note.ReLoadComponent(objmodel.DataId, objmodel.Type);
                        _note.ResetComment();
                    } else {
                        _msgalert.error(data.message);
                    }
                }
            });
        }
    },

    DeleteComment: function (id) {
        var _DataId = parseFloat($("#ip-note-data-id").val());
        var _Type = parseInt($("#ip-note-type").val());
        _msgconfirm.openDialog("Xác nhận xóa bình luận", "Bạn có chắc muốn xóa bình luận này?", function () {
            $.ajax({
                url: "/Note/Delete",
                data: { NoteId: id },
                type: "POST",
                success: function (data) {
                    if (data.isSuccess) {
                        _msgalert.success(data.message);
                        _note.ReLoadComponent(_DataId, _Type);
                    } else {
                        _msgalert.error(data.message);
                    }
                }
            });
        });
    },

    ReLoadComponent: function (dataid, type) {
        $.ajax({
            url: "/Note/CommentList",
            data: { DataId: dataid, Type: type },
            type: "POST",
            success: function (data) {
                $('#grid-comment-data').html(data);
            }
        });
    }
};