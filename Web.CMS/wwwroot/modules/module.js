var _modulecommon = {
    Init: function (data) {
        this.GridViewData = data.GridViewData;
        this.ControllerName = data.ControllerName;
        this.SearchAction = data.SearchAction;

        this.CreateAction = data.CreateAction;
        this.CreateTitle = data.CreateTitle;

        this.EditAction = data.EditAction;
        this.EditTitle = data.EditTitle;

        this.DeleteAction = data.DeleteAction;

        this.PagingAction = data.PagingAction;
        this.SearchParam = data.SearchParam;

        this.OnSearch(this.SearchParam);
    },

    RenderUrl: function (controllerName, actionName) {
        return '/' + controllerName + '/' + actionName;
    },

    OnSearch: (model) => {
        axios.get(this.RenderUrl(this.ControllerName, this.SearchAction), model).then(function (response) {
            $(_modulecommon.GridData).html(response.data);
        }).catch(function (error) {
            _alertmsg.error("Lỗi kết nối đến server");
        });
    }
};