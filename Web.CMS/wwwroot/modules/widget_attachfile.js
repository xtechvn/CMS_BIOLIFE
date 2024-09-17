class AttachmentWidget {
    Trigger = false;
    HTMLImage = `<div class="col-sm-3 col-4 mb10 file" style="max-width:150px;" data-path="@item.Path" data-id="@item.Id" data-ext="@item.Ext">
                        <button type="button" class="delete-file" onclick="$(this).closest('.file').remove()">x</button>
                        <div class="choose-ava lightgallery">
                            <img src="@item.Path" />
                        </div>
                        <p><span>@(file_name)</span> </p>
                    </div>`;
    HTMLVideo = `<div class="col-sm-3 col-4 mb10 file" style="max-width:150px;" data-path="@item.Path" data-id="@item.Id">
                        <button type="button" class="delete-file" onclick="$(this).closest('.file').remove()">x</button>
                        <div class="lg-video-cont lg-has-iframe choose-ava lightgallery"style="width:138px;height:138px;" >
                            <video src="@item.Path" style="width:138px;height:138px;"></video>
                        </div>
                       <p><span>@(file_name)</span> </p>
                    </div>`;
    HTMLOthers = `<div class="col-sm-3 col-4 mb10 file" style="max-width:150px;" data-path="@item.Path" data-id="@item.Id">
                        <button type="button" class="delete-file" onclick="$(this).closest('.file').remove()">x</button>
                        <div class="choose-ava lightgallery">
                            <a href="@item.Path" download style="width:138px;height:138px;">
                                <img src="@default_icon" />
                            </a>
                        </div>
                        <p><span>@(file_name)</span> </p>
                    </div>`;
    HTMLLoading = `<div class="col-sm-3 col-4 mb10 file on-addfile-loading" style="max-width:150px; ">
                <div class="choose-ava lightgallery" style=" vertical-align: center !important;align-content: center;">
                    <img src="/images/icons/loading.gif" style="width: 100%;height: 100%;" class="img_loading_summit coll">
                </div> </div>`;
    ImageCSS = 'choose-ava';
    VideoCSS = 'lg-video-cont lg-has-iframe';
    ImageExtension = ["png", "jpg", "gif", "jpeg", "webp", "PNG", "JPG", "GIF", "JPEG", "WEBP"];
    VideoExtension = ["mp4", "vod", "mkv", "avi", "MP4", "VOD", "MKV", "AVI"];
    DefaultImage = '/images/icons/document.png';
    WidgetOption = {
       
    };
    ElementName= '';
    constructor(element_name, option) {
        this.ElementName = element_name;
        this.WidgetOption = option;
        this.Init()
    };
    Upload() {
        var class_object = this;
        var widget = $('.' + class_object.ElementName)
        widget.find('.attachment-file-gallery').append(class_object.HTMLLoading)
        var formData = new FormData();
        if (!class_object.WidgetOption.allow_edit) {
            _msgalert.error('Bạn không thể chỉnh sửa danh sách tệp đính kèm')
            return;
        }
        var element = $('.' + this.ElementName + ' .attachfile-add')
        $(element[0].files).each(function (index, item) {
            formData.append("files", item);
        });
       
        $.ajax({
            url: "/AttachFile/UploadFile",
            data: formData,
            processData: false,
            contentType: false,
            type: "POST",
            success: function (result) {
                class_object.Trigger = false
                $(result.data).each(function (index, item) {
                    var ext = item.split('.')
                    var split = item.split('/')
                    var file_name = split[split.length - 1]
                    var text_name = split[split.length - 1]
                    if (class_object.VideoExtension.includes(ext[ext.length - 1])) {
                        widget.find('.attachment-file-gallery').append(class_object.HTMLVideo.replaceAll('@item.Id', '0').replaceAll('@item.Path', item).replaceAll('@(file_name)', text_name).replaceAll('@file_name', file_name));

                    }
                    else if (class_object.ImageExtension.includes(ext[ext.length - 1])) {
                        widget.find('.attachment-file-gallery').append(class_object.HTMLImage.replaceAll('@item.Id', '0').replaceAll('@item.Path', item).replaceAll('@(file_name)', text_name));

                    }
                    else {
                        widget.find('.attachment-file-gallery').append(class_object.HTMLOthers.replaceAll('@item.Id', '0').replaceAll('@item.Path', item).replaceAll('@default_icon', class_object.DefaultImage).replaceAll('@item.Path', item).replaceAll('@(file_name)', text_name).replaceAll('@file_name', file_name));
                    }
                });
                widget.find('.on-addfile-loading').remove()
                widget.find(".attachfile-add").val(null);

            }
        });
    };
    Confirm() {
        var class_object = this;

        var widget = $('.' + class_object.ElementName)
        var list = []
        widget.find('.file').each(function (item) {
            var ele = $(this)
            list.push(
                {
                    id: ele.attr('data-id'),
                    path: ele.attr('data-path'),
                    ext: ele.attr('data-ext')
                }
            );
        });
        var object_summit = {
            files: list,
            data_id: widget.attr('data-dataid'),
            service_type: widget.attr('data-type')
        }
        $.ajax({
            url: "/AttachFile/ConfirmFileUpload",
            data: object_summit,
            type: "POST",
            success: function (result) {
                if (class_object.WidgetOption.separate_confirm && result.status == 0) {
                    _msgalert.success('Lưu tệp đính kèm thành công')
                    widget.find(".attachfile-add").val(null);

                }
                else if (class_object.WidgetOption.separate_confirm) {
                    _msgalert.error('Lưu tệp đính kèm thất bại, vui lòng liên hệ IT')

                }
            }
        });
    };
    Init() {
        var class_object = this;
        var widget = $('.' + class_object.ElementName)

        if (!this.WidgetOption.allow_edit) {
            widget.find('.attach-edit').remove()
        }
        else {
            $('body').on('change', '.' + class_object.ElementName + ' .attachfile-add', function () {
                if ($(this)[0].files[0] && !class_object.Trigger) {
                    class_object.Upload()
                }
            })
            $('body').on('click', '.' + class_object.ElementName + ' .confirm-file', function () {
                class_object.Confirm()

            })
        }
        if (this.WidgetOption.allow_preview) {
            var name = this.ElementName
            $('.choose-ava').each(function (index, item) {
                var e = $(this)
                e.attr('data-src', e.find('a').attr('href'))
            });

            $('#'+name).lightGallery({
                selector: '.choose-ava',
            });

        }
        if (this.WidgetOption.separate_confirm) {

            widget.find('.confirm-file').removeClass('mfp-hide')
            widget.find('.confirm-file').show()
            
        }
    }
}