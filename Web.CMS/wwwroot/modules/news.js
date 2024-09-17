$(document).ready(function () {
    _news.Init();
});

$('.btn-toggle-cate').click(function () {
    var seft = $(this);
    if (seft.hasClass("plus")) {
        seft.siblings('ul.lever2').show();
        seft.removeClass('plus').addClass('minus');
    } else {
        seft.siblings('ul.lever2').hide();
        seft.removeClass('minus').addClass('plus');
    }
});

$('.ckb-news-cate').click(function () {
    var seft = $(this);
    var ul_lever2 = seft.parent().siblings('ul.lever2');
    if (ul_lever2.length > 0) {
        var btn_toggle = seft.parent().siblings('a.btn-toggle-cate');
        if (seft.is(":checked")) {
            ul_lever2.find('.ckb-news-cate').prop('checked', true);
            if (btn_toggle.hasClass('plus')) {
                btn_toggle.trigger('click');
            }
        } else {
            if (btn_toggle.hasClass('minus')) {
                btn_toggle.trigger('click');
            }
            ul_lever2.find('.ckb-news-cate').prop('checked', false);
        }
    }
});

var _news = {
    Init: function () {
        let _searchModel = {
            Title: null,
            ArticleId: -1,
            FromDate: null,
            ToDate: null,
            AuthorId: -1,
            ArticleStatus: -1,
            ArrCategoryId: null,
            SearchType: 0
        };

        let objSearch = {
            searchModel: _searchModel,
            currentPage: 1,
            pageSize: 20
        };

        this.SearchParam = objSearch;
        this.Search(objSearch);
    },

    Search: function (input) {
        $.ajax({
            url: "/news/search",
            type: "post",
            data: input,
            success: function (result) {
                $('#grid-data').html(result);
                var total = $('#data-total-record').val();
                $('#total-article-filter').text(total);
            }
        });
    },

    OnPaging: function (value) {
        var objSearch = this.SearchParam;
        objSearch.currentPage = value;
        this.Search(objSearch);
    },

    BasicSearch: function () {
        var objSearch = this.SearchParam;
        objSearch.searchModel.Title = $('#BasicTitle').val().trim();
        objSearch.searchModel.AuthorId = -1;
        objSearch.currentPage = 1;
        this.Search(objSearch);
    },

    AdvanceSearch: function () {
        var objSearch = this.SearchParam;

        var _arrCateGory = [];
        $('.ckb-news-cate').each(function () {
            if ($(this).is(":checked")) {
                _arrCateGory.push(parseInt($(this).val()));
            }
        });

        objSearch.searchModel.Title = $('#AdvanceTitle').val().trim();
        objSearch.searchModel.ArticleId = $('#ArticleId').val() <= 0 ? -1 : $('#ArticleId').val();
        objSearch.searchModel.FromDate = $('#FromDate').val();
        objSearch.searchModel.ToDate = $('#ToDate').val();
        objSearch.searchModel.AuthorId = $('#AuthorId').val();
        objSearch.searchModel.ArticleStatus = $('#ArticleStatus').val();
        objSearch.searchModel.ArrCategoryId = _arrCateGory;
        objSearch.searchModel.SearchType = 1;

        objSearch.currentPage = 1;
        this.Search(objSearch);
    },

    OnOpenAdvanceSearch: function () {
        $('.fillter-search').slideDown();
        $('.dynamic-dom').hide();
    },

    OnCloseAdvanceSearch: function () {

        this.SearchParam.searchModel.Title = null;
        this.SearchParam.searchModel.ArticleId = -1;
        this.SearchParam.searchModel.FromDate = null;
        this.SearchParam.searchModel.ToDate = null;
        this.SearchParam.searchModel.AuthorId = -1;
        this.SearchParam.searchModel.ArticleStatus = -1;
        this.SearchParam.searchModel.ArrCategoryId = null;
        this.SearchParam.searchModel.SearchType = 0;

        $('.fillter-search').slideUp();
        $('#form-advance-search').trigger("reset");
        $('.dynamic-dom').show();
    },

    OpenCategoryPanel: function () {
        $('#panel-category-filter ul.lever2').show();
        $('#panel-category-filter .btn-toggle-cate').removeClass('plus').addClass('minus');
    },

    CloseCategoryPanel: function () {
        $('#panel-category-filter ul.lever2').hide();
        $('#panel-category-filter .btn-toggle-cate').removeClass('minus').addClass('plus');
    },
    LoadPageView: function (list_id) {
        if (list_id != undefined && list_id.length>0) {
            $.ajax({
                url: '/News/GetPageViewByList',
                type: 'POST',
                data: {
                    article_id: list_id,
                },
                success: function (result) {
                    if (result != undefined && result.length > 0) {
                        
                        result.forEach(function (item) {
                            $('#pv_' + item.articleID).html(item.pageview);
                        });
                    }
                },
                error: function (jqXHR) {
                },
            });
        }
       
    }
};