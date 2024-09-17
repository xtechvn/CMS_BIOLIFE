var _newsRelation = {
    ArrRelation: [],

    CreateSearchModel: function () {
        var _arrCateGory = [];
        var _title = $('#ip-relation-title').val();
        var _authorid = $('#ip-relation-authorid').val();
        var _searchType = 0;

        $('#panel-relation-category .ckb-news-cate').each(function () {
            if ($(this).is(":checked")) {
                _arrCateGory.push(parseInt($(this).val()));
            }
        });

        if (_arrCateGory.length > 0) {
            _searchType = 1;
        }

        let _searchModel = {
            Title: _title,
            ArticleId: -1,
            FromDate: null,
            ToDate: null,
            AuthorId: _authorid,
            ArticleStatus: -1,
            ArrCategoryId: _arrCateGory,
            SearchType: _searchType
        };

        let objSearch = {
            searchModel: _searchModel,
            currentPage: 1,
            pageSize: 10
        };
        return objSearch;
    },

    Search: function (input) {
        $.ajax({
            url: "/news/RelationSearch",
            type: "post",
            data: input,
            success: function (result) {
                $('#grid-relation-data').html(result);

                if (_newsRelation.ArrRelation.length > 0) {
                    _newsRelation.ArrRelation.map(function (item) {
                        $('#grid-relation-data .ckb-select-relation[value="' + item.id + '"]').prop('checked', true);
                    });
                }
            }
        });
    },

    OnPaging: function (pageIndex) {
        var objsearch = this.CreateSearchModel();
        objsearch.currentPage = pageIndex;
        this.Search(objsearch);
    },

    OnSearch: function () {
        var objsearch = this.CreateSearchModel();
        this.Search(objsearch);
    },

    OnAddRelation: function () {
        console.log(this.ArrRelation);
        if (this.ArrRelation.length > 0) {
            var _strHtml = "";
            this.ArrRelation.map(function (item) {
                _strHtml += '<div class="item-related-article col-md-6 col-xs-12" data-id="' + item.id + '">'
                    + '<img class="image" src="' + item.image + '" />'
                    + '<div class="related-title">'
                    + '<span class="title">' + item.title + '</span>'
                    + '<br />'
                    + '<span class="author">' + item.author + '</span>'
                    + '</div>'
                    + '<a class="cur-pointer" title="Xóa tin liên quan" onclick="$(this).closest(' + "'" + '.item-related-article' + "'" + ').remove()"><i class="fa fa-times red"></i></a>'
                    + '</div>';
            });

            $('#panel-related-article').html(_strHtml);

            $.magnificPopup.close();
        }
        else {
            _msgalert.error('Bạn chưa chọn bài viết');
        }
    }
}

$(document).ready(function () {
    $('.select2').select2({
        dropdownParent: "#magnific-popup-large"
    });

    $('.item-related-article').each(function () {
        var seft = $(this);
        _newsRelation.ArrRelation.push({
            id: seft.data('id'),
            title: seft.find('.title').text().trim(),
            image: seft.find('.image').attr('src') != undefined ? seft.find('.image').attr('src') : '',
            author: seft.find('.author').text().trim()
        });
    });
});

$('#grid-relation-data').on('click', '.ckb-select-relation', function () {
    var seft = $(this);
    var seftParent = seft.closest('tr');
    var article_id = seft.val();

    var exist_value = _newsRelation.ArrRelation.find(x => x.id == article_id);
    if (exist_value == null || exist_value == undefined) {
        _newsRelation.ArrRelation.push({
            id: seft.val(),
            title: seftParent.find('.article-title').text().trim(),
            image: seftParent.find('.article-image').val(),
            author: seftParent.find('.article-author').text()
        });

    } else {
        _newsRelation.ArrRelation = _newsRelation.ArrRelation.filter(x => x.id != article_id);
    }
});

$('#panel-relation-category .btn-toggle-cate').click(function () {
    var seft = $(this);
    if (seft.hasClass("plus")) {
        seft.siblings('ul.lever2').show();
        seft.removeClass('plus').addClass('minus');
    } else {
        seft.siblings('ul.lever2').hide();
        seft.removeClass('minus').addClass('plus');
    }
});
